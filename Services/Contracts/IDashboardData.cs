using Data.Entity;
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

        Task<AdminProfile> AdminProfileData(string aspNetUserId);

        Task UpdateAdminInfo(string aspNetUserId, AdminInfo obj);

        Task UpdateBillingInfo(string aspNetUserId, BillingInfo obj);

        Task<RoleAccess> CreateAccessRole(int accountType , int roleid);

        Task AddNewRole(List<int> menus, short accountType, string roleName, string aspNetUserId);

        Task<RoleAccess> AddedRoles();

        Task<RoleAccess> EditRole(int roleId);

        Task<List<Healthprofessionaltype>> GetProfessions();

        Task<List<Healthprofessional>> GetBusinesses(int professionId);

        Task<Healthprofessional> GetBusinessesDetails(int businessid);

        Task<PartnerViewModel> Partners();

        Task<PartnerViewModel> PartnerData(int professionType , string searchKey);

        Task<BusinessData> GetProfessionsTypes();

        Task AddNewBusiness(BusinessData obj);

        Task<BusinessData> ExistingBusinessData(int professionId);

        Task DeleteBusiness(int profesionId);

        Task UpdateBusiness(BusinessData obj);

        Task<RecordsViewModel> SearchRecordsService();

        Task<SearchRecordsData> GetSearchRecordData(SearchRecordsData obj);

        Task<PatientHistory> GetPatientHistoryData(PatientHistory obj);

        Task<ExplorePatientHistory> ExplorePatientHistory(int userId);

        Task<BlockedHistory> GetBlockHistoryData(BlockedHistory obj);

        Task UnblockPatient(int requestId);

        Task<CaseActionDetails> ViewNotes(int requestId);

        Task UpdateAdminPassword(string aspNetUserId, string password);

        Scheduling Scheduling();

        List<Physician> GetPhysicians();

        DayWiseScheduling Daywise(int regionid, DateTime currentDate, List<Physician> physician);

        WeekWiseScheduling Weekwise(int regionid, DateTime currentDate, List<Physician> physician);

        MonthWiseScheduling Monthwise(int regionid, DateTime currentDate, List<Physician> physician);

        Task<bool> AddShift(Scheduling model, string aspNetUserId, List<string> chk);

        Task<Scheduling> viewshift(int shiftdetailid);

        Task ViewShiftreturn(int shiftdetailid, string aspNetUserId);

        Task<bool> ViewShiftedit(Scheduling modal, string aspNetUserId);

        Task<Scheduling> ProvidersOnCall(Scheduling modal);

        Task<Scheduling> ProvidersOnCallbyRegion(int regionid, List<int> oncall, List<int> offcall);

        Task<ShiftforReviewModal> ShiftForReview();

        Task<ShiftforReviewModal> ShiftReviewTable(int currentPage, int regionid);

        Task ApproveSelected(int[] shiftchk, string aspNetUserId);

        Task DeleteSelected(int[] shiftchk, string aspNetUserId);

        Task DeleteShift(int shiftdetailid, string aspNetUserId);

        byte[] DownloadSearchRecordExcle(SearchRecordsData model);

        Task<bool> CheckEmail(string Email);

        Task<(string, string)> FindUser(LoginPerson model);

        Task UpdatePassword(LoginPerson model);

        Task<List<Role>> GetAdminRoles();

        Task CreateAdmin(CreateAdminModel obj, List<int> selectedRegion);

        string GetFilesNames(int reqwiseid);

        Task DeleteRequest(int requestId);

        Task SaveRoleChanges(int roleid, List<int> selectedRole);

        Task<EmailLogViewModel> GetEmailLogData(EmailLogViewModel obj);

        Task DeleteRole(int roleId);

        Task<bool> CheckRole(string roleName);

        Task<bool> CheckAdmin(string adminEmail);
        Task EmailLogEntry(string emailTemplate, string subject, string aspNetUserId, string email);
    }
}