using Data.Entity;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IProviderServices
    {
        Task<bool> CheckPhysician(string email);

        Task<Task> ContactProvider(string email, string note , string aspNetUserId);

        Task<EditProviderViewModel> CreateProvider();

        Task CreateProviderAccount(EditProviderViewModel obj, List<int> selectedRegion, string aspNetUserId);

        Task DeleteAccount(int providerId);

        Task<EditProviderViewModel> EditProvider(string aspNetUserId);

        Task<string> GetLocations();

        Task<PayrateViewModel> GetPayrateData(int physicianId);

        Task<List<Region>> GetRegions();

        Task<ProviderViewModel> ProviderData(int regionId);

        Task SavePayrate(int physicianId, string fieldName, int payRate, string aspNetUserId);

        Task ToStopNotification(List<int> toStopNotifications, List<int> toNotification);

        Task UpdateBillingInfo(EditProviderViewModel obj);

        Task UpdatePhysicianInfo(EditProviderViewModel obj, List<int> selectedRegion);

        Task UpdateProfile(EditProviderViewModel obj);

        Task UploadNewDocument(EditProviderViewModel obj);
    }
}
