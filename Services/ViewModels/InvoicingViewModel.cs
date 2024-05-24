using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class InvoicingViewModel
    {
        public int timeSheetId { get; set; }

        public string startDate {  get; set; }

        public DateOnly endDate { get; set; }

        public List<Shiftdetail> shiftdetails { get; set; }

        public List<TimeSheetData> timeSheetData { get; set; }
    }

    public class TimeSheetData
    {
        public DateOnly crrDate {  get; set; }

        public bool holidays { get; set; }

        public int houseCall { get; set; }

        public int consult { get; set; }

        public float totalHour { get; set; }

    }
}
