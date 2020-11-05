using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class TaUserGroupShifts
    {
     
        public int Id { get; set; }
        public string Name { get; set; }
        public int RepeatAfterWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public IEnumerable<TaWeekShift> TaWeekShifts { get; set; }
        public IEnumerable<ShiftSchedulerDisplay> ShiftSchedulerDisplays { get; set; }
        public IEnumerable<TaWeekShift> AllTaWeekShifts { get; set; }
        public IEnumerable<BreakSchedulerDisplays> BreakSchedulerDisplays { get; set; }
    }
}