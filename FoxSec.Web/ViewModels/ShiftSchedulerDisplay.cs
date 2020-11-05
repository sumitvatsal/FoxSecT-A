using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class ShiftSchedulerDisplay
    {
        public string Text { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Color { get; set; } = "#168da8";
    }
}