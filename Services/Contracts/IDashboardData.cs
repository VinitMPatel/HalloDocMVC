﻿using Data.Entity;
using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboardData
    {
        Task<AdminDashboard> AllStateData(AdminDashboard obj);

        Task<AdminDashboard> AllData();

        Task<CaseActionDetails> ViewCaseData(int requestId);

        Task<List<Physician>> PhysicianList(int regionid);

        Task<CaseActionDetails> ViewUploads(int requestId);

        Task UplodingDocument(List<IFormFile> myfile, int reqid);

        Task uploadFile(List<IFormFile> upload, int id);

        Task SingleDelete(int reqfileid);

        byte[] DownloadExcle(AdminDashboard model);

        Task<AdminProfile> AdminProfileData(int adminId);

        Task UpdateAdminInfo(int adminId, AdminInfo obj);

        Task UpdateBillingInfo(int adminId, BillingInfo obj);

        ProviderViewModel ProviderData(int regionId);

        Task ToStopNotification(List<int> notifications , List<int> toNotification);

        EditProviderViewModel EditProvider(int physicianId);

        Task UpdatePhysicianInfo(EditProviderViewModel obj, List<int> selectedRegion);
       
       
        Task UpdateProfile(EditProviderViewModel obj);

        Task UpdateBillingInfo(EditProviderViewModel obj);
        RoleAccess CreateAccessRole(int roleid);
        Task AddNewRole(List<int> menus, short accountType, string roleName, int adminId);
        RoleAccess AddedRoles();
        RoleAccess EditRole(int roleId);
        Task<List<Healthprofessionaltype>> GetProfessions();
        Task<List<Healthprofessional>> GetBusinesses(int professionId);
        Task<Healthprofessional> GetBusinessesDetails(int businessid);
        Task<PartnerViewModel> Partners();
        Task<PartnerViewModel> PartnerData(int professionType);
    }
}