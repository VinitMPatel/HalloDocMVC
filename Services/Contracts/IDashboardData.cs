using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboardData
    {
        AdminDashboard NewStateData(String a , String b);

        AdminDashboard PendingStateData();

        AdminDashboard ActiveStateData();

        AdminDashboard ConcludeStateData();

        AdminDashboard ToCloseStateData();

        AdminDashboard UnpaidStateData();

        AdminDashboard AllData();

        ViewCase ViewCaseData(int requestId);
    }
}