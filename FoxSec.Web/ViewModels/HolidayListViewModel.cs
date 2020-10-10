using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class HolidayListViewModel : ViewModelBase
	{
        public HolidayListViewModel()
		{
            Holidays = new List<HolidayItem>();
		}

        public IEnumerable<HolidayItem> Holidays { get; set; }
	}

	public class HolidayItem
	{
		public int? Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
		public string Name { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventEnd { get; set; }

        public string ModifiedBy { get; set; }

        public bool MovingHoliday { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[RegularExpression("(0?[1-9]|[12][0-9]|3[01]).(0?[1-9]|1[0-2]).(20[0-9]{2}|[2][0-9][0-9]{2})", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "CommonDateFormat")]
        public string EventStartStr { get; set; }
	}
}