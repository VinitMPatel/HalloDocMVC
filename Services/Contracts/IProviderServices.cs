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
        Task<EditProviderViewModel> CreateProvider();
        Task CreateProviderAccount(EditProviderViewModel obj, List<int> selectedRegion, int adminId);
        EditProviderViewModel EditProvider(int physicianId);
        Task<string> GetLocations();
        Task<List<Region>> GetRegions();
        Task<ProviderViewModel> ProviderData(int regionId);
        Task ToStopNotification(List<int> toStopNotifications, List<int> toNotification);
        Task UpdateBillingInfo(EditProviderViewModel obj);
        Task UpdatePhysicianInfo(EditProviderViewModel obj, List<int> selectedRegion);
        Task UpdateProfile(EditProviderViewModel obj);
        Task UploadNewDocument(EditProviderViewModel obj);
    }
}
