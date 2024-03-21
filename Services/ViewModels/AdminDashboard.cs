using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class AdminDashboard
    {

        public List<Requestclient> requestclients { get; set; }

        public int totalPages { get; set; }

        public int currentpage { get; set; }

        public List<Region> regionlist { get; set; }

        public int requeststatus { get; set; }

        public int requestType { get; set; }

        public int requestedPage { get; set; }

        public int regionId { get; set; }

        public string searchKey { get; set; }
    }


}
