using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class TimeZonePropertiesViewModel : ViewModelBase
    {
        public int Id { get; set; }

        public int TimeZoneId { get; set; }

        public int OrderInGroup { get; set; }

        public bool IsMonday { get; set; }

        public bool IsTuesday { get; set; }

        public bool IsWednesday { get; set; }

        public bool IsThursday { get; set; }

        public bool IsFriday { get; set; }

        public bool IsSaturday { get; set; }

        public bool IsSunday { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string ValidFromStr { get; set; }

        public string ValidToStr { get; set; }
    }
}