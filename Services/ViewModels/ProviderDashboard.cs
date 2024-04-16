using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class ProviderDashboard
    {
        public List<Requestclient> requestclients { get; set; }

        public int totalPages { get; set; }

        public int currentpage { get; set; }

        public List<Region> regionlist { get; set; }

        public int requeststatus { get; set; }

        public int requestType { get; set; }

        public int requestedPage { get; set; }

        public string searchKey { get; set; }

        public int totalEntity { get; set; }

        public string aspNetUserId { get; set; }
    }

    public class ProviderCaseAction
    {
        public int requestId { get; set; }
    }
}
