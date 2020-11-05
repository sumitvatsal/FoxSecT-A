using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class BreakSchedulerDisplays
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}