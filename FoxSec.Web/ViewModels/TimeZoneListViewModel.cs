using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class TimeZoneListViewModel : PaginatorViewModelBase
    {
        public TimeZoneListViewModel()
        {
            TimeZones = new List<TimeZoneItem>();
        	IsModelReadOnly = false;
        }

        public List<TimeZoneItem> TimeZones { get; set; }

        public string SearchStartTime { get; set; }

		public bool IsModelReadOnly { get; set; }
    }

    public class TimeZoneItem
    {
        public int Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
		public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsInUse { get; set; }

        //public object TimeZoneId { get; set; }

        public int TimeZoneId { get; set; }

        public int CompanyId { get; set; }
    }
}