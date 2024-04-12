using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class RecordsViewModel
    {
        public List<Requesttype> requestTypeList {  get; set; }


    }

    public class SearchRecordsData
    {
        public int selectedStatus { get; set; }

        public int selectedType { get; set; }

        public string searchedPatient { get; set; }

        public string searchedEmail { get; set; }

        public string searchedProvider { get; set; }

        public string searchedPhone { get; set; }

        public List<Requestclient> requestclients { get; set; }

        public int totalPages { get; set; }

        public int currentpage { get; set; }

        public int totalEntity { get; set; }

        public int requestedPage { get; set; }
    }


    public class PatientHistory
    {
        public List<User> userList { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

    }

    public class ExplorePatientHistory
    {
        public List<Requestclient> reqcList { get; set; }

    }

}
