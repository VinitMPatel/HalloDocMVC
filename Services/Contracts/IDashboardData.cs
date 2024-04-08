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

        Task SingleDelete(int reqfileid);

        byte[] DownloadExcle(AdminDashboard model);

        AdminProfile AdminProfileData(int adminId);

        Task UpdateAdminInfo(int adminId, AdminInfo obj);

        Task UpdateBillingInfo(int adminId, BillingInfo obj);

        ProviderViewModel ProviderData(int regionId);

        Task ToStopNotification(List<int> notifications , List<int> toNotification);

        EditProviderViewModel EditProvider(int physicianId);

        Task UpdatePhysicianInfo(EditProviderViewModel obj, List<int> selectedRegion);
       
       
        Task UpdateProfile(EditProviderViewModel obj);

        Task UpdateBillingInfo(EditProviderViewModel obj);
    }
}