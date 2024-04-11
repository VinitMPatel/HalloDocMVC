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
        public List<Requestclient> requestclients { get; set; }
    }
}
