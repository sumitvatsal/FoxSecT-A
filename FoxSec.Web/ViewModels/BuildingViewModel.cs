using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class BuildingViewModel : ViewModelBase
	{
        public BuildingViewModel()
		{
            
		}

		public string Address { get; set; }

		public string BuildingName { get; set; }

		public string AdminName { get; set; }
	}

    
}