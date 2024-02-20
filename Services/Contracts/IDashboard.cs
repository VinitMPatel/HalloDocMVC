using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboard
    {
        patient_dashboard PatientDashboard(int id);

        public String editing(patient_dashboard r, int id);

        public patient_dashboard ViewDocuments(int reqId, int userId);
    }
}