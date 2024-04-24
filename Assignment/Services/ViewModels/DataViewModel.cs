using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class DataViewModel
    {
        public List<Student> Students { get; set; }

        public int totalPages { get; set; }

        public int currentpage { get; set; }

        public int totalEntity { get; set; }

        public int requestedPage { get; set; }

        public string searchKey { get; set; }
    }
}
