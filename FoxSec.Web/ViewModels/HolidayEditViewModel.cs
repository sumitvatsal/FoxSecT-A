using FoxSec.DomainModel.DomainObjects;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class HolidayEditViewModel : ViewModelBase
    {
        public HolidayEditViewModel()
        {
            Holiday = new HolidayItem();

            BuildingItems = new List<SelectListItem>();

        }

        public HolidayItem Holiday { get; set; }

        public Boolean AllBuildings { get; set; }

        public IEnumerable<SelectListItem> BuildingItems { get; set; }
    }
}