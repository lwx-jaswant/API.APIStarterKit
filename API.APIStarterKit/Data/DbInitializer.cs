using API.APIStarterKit.Services;
using Core.Data.Context;

namespace API.APIStarterKit.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, IFunctional functional)
        {
            context.Database.EnsureCreated();
            await functional.GetDefaultIdentitySettings();
            //await functional.CreateSingleRole("Test");

            if (context.ApplicationUser.Any())
            {
                return;
            }
            else
            {
                await functional.CreateDefaultSuperAdmin();
                await functional.CreateDefaultEmailSettings();
                await functional.CreateDefaultIdentitySettings();
                await functional.InitAppData();
                await functional.GenerateUserUserRole();
                await functional.CreateDefaultOtherUser();
            }
        }
    }
}
