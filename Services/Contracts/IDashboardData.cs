using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboardData
    {
        AdminDashboard NewStateData();

        AdminDashboard PendingStateData();

        AdminDashboard ActiveStateData();

        AdminDashboard ConcludeStateData();

        AdminDashboard ToCloseStateData();

        AdminDashboard UnpaidStateData();

        AdminDashboard AllData();
    }
}