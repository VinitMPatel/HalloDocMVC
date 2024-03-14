using Data.Entity;
using Microsoft.AspNetCore.Http;

namespace Services.ViewModels;

public class patient_dashboard
{
    public User user {  get; set; }

    public ICollection<Request> request { get; set; }

    public ICollection<Requestwisefile> requestwisefile { get; set; }

    public int reqId { get; set; }

    public List<IFormFile> Upload { get; set; }

    public DateTime DOB {  get; set; }
}
