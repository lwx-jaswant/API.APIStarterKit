using Core.Data.Models;
using Core.Data.ViewModel.JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.APIStarterKit.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRolesService _iRolesService;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IRolesService iRolesService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _iRolesService = iRolesService;
        }
        public async Task<(int, string)> Registeration(RegistrationModel model, string role)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return (0, "User already exists");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                //FirstName = model.FirstName,
                //LastName = model.LastName,
            };
            var createUserResult = await userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
                return (0, "User creation failed! Please check user details and try again.");

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            if (await roleManager.RoleExistsAsync(role))
                await userManager.AddToRoleAsync(user, role);

            return (1, "User created successfully!");
        }

        public async Task<TokenViewModel> Login(LoginModel model)
        {
            TokenViewModel _TokenViewModel = new();
            try
            {
                var _ApplicationUser = await userManager.FindByNameAsync(model.Username);
                if (_ApplicationUser == null)
                {
                    _TokenViewModel.StatusCode = 1;
                    _TokenViewModel.StatusMessage = "Invalid username";
                    return _TokenViewModel;
                }
                if (!await userManager.CheckPasswordAsync(_ApplicationUser, model.Password))
                {
                    _TokenViewModel.StatusCode = 1;
                    _TokenViewModel.StatusMessage = "Invalid password";
                    return _TokenViewModel;
                }

                var _PasswordSignInAsync = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: true);
                var userRoles = await userManager.GetRolesAsync(_ApplicationUser);
                var authClaims = new List<Claim>
                {
                   new Claim(ClaimTypes.Name, _ApplicationUser.UserName),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                _TokenViewModel = GenerateToken(authClaims);
                _TokenViewModel.RefreshToken = GenerateRefreshToken();
                _TokenViewModel.StatusCode = 1;
                _TokenViewModel.StatusMessage = "Success";

                _ApplicationUser.RefreshToken = _TokenViewModel.RefreshToken;
                _ApplicationUser.RefreshTokenExpiryTime = _TokenViewModel.RefreshTokenExpiryTime;
                await userManager.UpdateAsync(_ApplicationUser);

                _TokenViewModel.MainMenuViewModel = await _iRolesService.RolebaseMenuLoad(_ApplicationUser);

                return _TokenViewModel;
            }
            catch (Exception ex)
            {
                _TokenViewModel.StatusCode = 0;
                _TokenViewModel.StatusMessage = ex.Message;
                return _TokenViewModel;
            }
        }

        public async Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model)
        {
            TokenViewModel _TokenViewModel = new();
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            string username = principal.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _TokenViewModel.StatusCode = 0;
                _TokenViewModel.StatusMessage = "Invalid access token or refresh token";
                return _TokenViewModel;
            }

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            _TokenViewModel = GenerateToken(authClaims);
            _TokenViewModel.StatusCode = 1;
            _TokenViewModel.StatusMessage = "Success";
            _TokenViewModel.RefreshToken = GenerateRefreshToken();

            user.RefreshToken = _TokenViewModel.RefreshToken;
            user.RefreshTokenExpiryTime = _TokenViewModel.RefreshTokenExpiryTime;
            await userManager.UpdateAsync(user);

            return _TokenViewModel;
        }


        private TokenViewModel GenerateToken(IEnumerable<Claim> claims)
        {
            TokenViewModel _TokenViewModel = new();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var _TokenExpiryTimeInMinutes = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInMinutes"]);
            var _RefreshTokenExpiryTimeInMinutes = Convert.ToInt64(_configuration["JWTKey:RefreshTokenExpiryTimeInMinutes"]);
            _TokenViewModel.TokenExpiryTime = DateTime.Now.AddMinutes(_TokenExpiryTimeInMinutes);
            _TokenViewModel.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_RefreshTokenExpiryTimeInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                Expires = _TokenViewModel.TokenExpiryTime,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var _TokenHandler = new JwtSecurityTokenHandler();
            var _CreateToken = _TokenHandler.CreateToken(tokenDescriptor);
            _TokenViewModel.AccessToken = _TokenHandler.WriteToken(_CreateToken);
            return _TokenViewModel;
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
