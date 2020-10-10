using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class ClassificatorListViewModel : ViewModelBase
	{
        public ClassificatorListViewModel()
		{
            Classifiers = new List<ClassificatorItem>();
		}

        public IEnumerable<ClassificatorItem> Classifiers { get; set; }
	}

    public class ClassificatorItem
	{
		public int? Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
		public string Description { get; set; }

		[StringLength(250, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError250")]
		public string Comments { get; set; }
        public string Value { get; set; }
        public int? Legal { get; set; }
        public int? Remaining { get; set; }
        public DateTime? ValidTo { get; set; }

    }
}