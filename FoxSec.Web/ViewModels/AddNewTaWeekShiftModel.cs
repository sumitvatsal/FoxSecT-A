using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class AddNewTaWeekShiftModel
    {
        public string Name { get; set; }
        public int? MondayShift { get; set; }
        public int? TuesdayShift { get; set; }
        public int? WednesdayShift { get; set; }
        public int? ThursdayShift { get; set; }
        public int? FridayShift { get; set; }
        public int? SaturdayShift { get; set; }
        public int? SundayShift { get; set; }
    }
}