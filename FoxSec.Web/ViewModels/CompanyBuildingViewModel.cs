using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class CompanyBuildingViewModel : ViewModelBase
    {
       public int Id { get; set; }

        public SelectList Buildings { get; set; }

		public int? BuildingId { get; set; }

	}
	
}