using API.APIStarterKit.Extensions;
using API.APIStarterKit.GenericRepo;
using API.APIStarterKit.Helper;
using API.APIStarterKit.Services;
using Core.Data.Context;
using Core.Data.Helper;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICommonService, CommonService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IFunctional, Functional>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ApplicationDbContext>();
var _ApplicationInfo = builder.Configuration.GetSection("ApplicationInfo").Get<ApplicationInfo>();
string _GetConnStringName = ControllerExtensions.GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_GetConnStringName));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_GetConnStringName));

//Get Super Admin Default options
builder.Services.Configure<SuperAdminDefaultOptions>(builder.Configuration.GetSection("SuperAdminDefaultOptions"));
builder.Services.Configure<ApplicationInfo>(builder.Configuration.GetSection("ApplicationInfo"));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});

//For Identity  
IConfigurationSection _IConfigurationSection = builder.Configuration.GetSection("IdentityDefaultOptions");
builder.Services.Configure<DefaultIdentityOptions>(_IConfigurationSection);

var _DefaultIdentityOptions = _IConfigurationSection.Get<DefaultIdentityOptions>();
AddIdentityOptions.SetOptions(builder.Services, _DefaultIdentityOptions);

//Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
            ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
        };
    });



//builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseCors("Open");

ProgramTaskExtension.SeedingData(app);
app.Run();

