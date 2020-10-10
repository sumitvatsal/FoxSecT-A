using System;
using System.Collections.Generic;

namespace FoxSec.Web.ViewModels
{
    public class CompanyFloorViewModel : ViewModelBase
    {
        public int BuildingId { get; set; }

        public int FloorsCount { get; set; }
    }
}