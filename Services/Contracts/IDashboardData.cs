using Data.Entity;
using Microsoft.AspNetCore.Http;
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

        CaseDetails ViewCaseData(int requestId);

        List<Physician> PhysicianList(int regionid);

        CaseDetails ViewUploads(int requestId);
        void UplodingDocument(List<IFormFile> myfile, int reqid);
        void uploadFile(List<IFormFile> upload, int id);
        void SingleDelete(int reqfileid);
    }
}