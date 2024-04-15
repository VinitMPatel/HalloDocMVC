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

        Task<AdminProfile> AdminProfileData(int adminId);

        Task UpdateAdminInfo(int adminId, AdminInfo obj);

        Task UpdateBillingInfo(int adminId, BillingInfo obj);

        Task<RoleAccess> CreateAccessRole(int accountType , int roleid);

        Task AddNewRole(List<int> menus, short accountType, string roleName, int adminId);

        RoleAccess AddedRoles();

        RoleAccess EditRole(int roleId);

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
        Scheduling Scheduling();
        List<Physician> GetPhysicians();
        DayWiseScheduling Daywise(int regionid, DateTime currentDate, List<Physician> physician);
        WeekWiseScheduling Weekwise(int regionid, DateTime currentDate, List<Physician> physician);
        MonthWiseScheduling Monthwise(int regionid, DateTime currentDate, List<Physician> physician);
        bool AddShift(Scheduling model, int admin, List<string> chk);
        Scheduling viewshift(int shiftdetailid);
        void ViewShiftreturn(int shiftdetailid, string adminname);
        bool ViewShiftedit(Scheduling modal, string adminname);
        Scheduling ProvidersOnCall(Scheduling modal);
        Scheduling ProvidersOnCallbyRegion(int regionid, List<int> oncall, List<int> offcall);
        ShiftforReviewModal ShiftForReview();
        ShiftforReviewModal ShiftReviewTable(int currentPage, int regionid);
        void ApproveSelected(int[] shiftchk, string adminname);
        void DeleteSelected(int[] shiftchk, string adminname);
        void DeleteShift(int shiftdetailid, string adminname);
    }
}