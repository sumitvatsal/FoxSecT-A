using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class TitleListViewModel : ViewModelBase
	{
        public TitleListViewModel()
		{
            Titles = new List<TitleItem>();
		}

        public IEnumerable<TitleItem> Titles { get; set; }
	}

    public class TitleItem
	{
		public int? Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
		public string Name { get; set; }

		[StringLength(250, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError250")]
		public string Description { get; set; }

        public string ModifiedBy { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }
	}
}