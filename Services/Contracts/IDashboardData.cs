using Data.Entity;
using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboardData
    {
        AdminDashboard AllStateData(AdminDashboard obj);

        AdminDashboard AllData();

        CaseActionDetails ViewCaseData(int requestId);

        List<Physician> PhysicianList(int regionid);

        CaseActionDetails ViewUploads(int requestId);
        void UplodingDocument(List<IFormFile> myfile, int reqid);
        void uploadFile(List<IFormFile> upload, int id);
        void SingleDelete(int reqfileid);
        byte[] DownloadExcle(AdminDashboard model);
        AdminProfile AdminProfileData(int adminId);
        void UpdateAdminInfo(int adminId, AdminInfo obj);
        void UpdateBillingInfo(int adminId, BillingInfo obj);
        ProviderViewModel ProviderData(int regionId);
        void ToStopNotification(List<int> notifications , List<int> toNotification);
        EditProviderViewModel EditProvider(int physicianId);
        void UpdatePhysicianInfo(EditProviderViewModel obj, List<int> selectedRegion);
        void UpdateBillingInfo(EditProviderViewModel obj);
        void UpdateProfile(EditProviderViewModel obj);
    }
}