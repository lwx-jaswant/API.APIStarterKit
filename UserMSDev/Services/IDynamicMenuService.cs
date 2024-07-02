using Core.Data.Models.DashboardViewModel;

namespace UserMSDev.Services
{
    public interface IDynamicMenuService
    {
        Task<SharedUIDataViewModel> GetRequiredAppData();
    }
}
