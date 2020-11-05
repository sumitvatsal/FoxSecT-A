using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class TaWeekShiftsModel
    {
        public string Name { get; set; }
        public int? MondayShift { get; set; }
        public int? TuesdayShift { get; set; }
        public int? WednesdayShift { get; set; }
        public int? ThursdayShift { get; set; }
        public int? FridayShift { get; set; }
        public int? SaturdayShift { get; set; }
        public int? SundayShift { get; set; }
        public IEnumerable<TAShifts> TaShiftsModelsList { get; set; }
        public IEnumerable<SelectListItem> DropDownItems { get; set; }
       
    }
}