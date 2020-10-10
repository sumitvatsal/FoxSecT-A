using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class BuildingObjectEditViewModel : ViewModelBase
	{
		public BuildingObjectEditViewModel()
		{
			BuildingObject = new BuildingObjectItem();
		}

		public BuildingObjectItem BuildingObject { get; set; }

    }

	public class BuildingObjectItem
	{
		public int? Id { get; set; }
		
		[StringLength(250, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthErrorComment")]
		public string Comment { get; set; }
	}
}