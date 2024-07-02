using Core.Data.Models.AccountViewModels;
using Core.Data.Models.CommonViewModel;
using Core.Data.Pages;

namespace Core.Data.Models.DashboardViewModel
{
    public class SharedUIDataViewModel
    {
        public UserProfile UserProfile { get; set; }
        public ApplicationInfo ApplicationInfo { get; set; }
        public MainMenuViewModel MainMenuViewModel { get; set; }
    }
}
