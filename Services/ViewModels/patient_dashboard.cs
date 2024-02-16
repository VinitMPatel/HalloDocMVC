using Data.Entity;

namespace Services.ViewModels;

public class patient_dashboard
{
    public User user {  get; set; }

    public List<Request> request { get; set; }

    public List<Requestwisefile> requestwisefile { get; set; }

    enum statusName
    {
        january,
        Unassigned,
        Cancelled,
        MdEnRoute,
        MdOnSite,
        Closed,
        Clear,
        Unpaid
    }

    public string StatusFind(int id)
    {
        string sName = ((statusName)id).ToString();
        return sName;
    }

}
