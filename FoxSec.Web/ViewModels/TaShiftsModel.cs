using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class TaShiftsModel
    {
        public List<TAShifts> TAShifts { get; set; }
        public IEnumerable<Companies> CompaniesList { get; set; }
        public IEnumerable<TAReportLabels> TAReportLabels { get; set; }
        public IEnumerable<SchedulerData> schedulerData { get; set; }
        public IEnumerable<SelectListItem> repeatWeeksList { get; set; }
    }
}