using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class ClassificatorValueListViewModel : ViewModelBase
	{
        public ClassificatorValueListViewModel()
		{
            ClassificatorValues = new List<ClassificatorValueItem>();
		}

        public IEnumerable<ClassificatorValueItem> ClassificatorValues { get; set; }
	}

    public class ClassificatorValueItem
	{
        public int? Id { get; set; }

		public int? ClassificatorId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(500, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
        public string Value { get; set; }
        public int? Legal { get; set; }
        public int? Remaining { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Comments { get; set; }
        public string ToDateTime { get; set; }
    }
}