using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class UserAccessUnitEditViewModel : ViewModelBase
    {
        public UserAccessUnitEditViewModel()
        {
            Card = new UserAccessUnitItem();
        }

        public UserAccessUnitItem Card { get; set; }
        public bool isCompanyManager { get; set; }
    }

    public class UserAccessUnitItem
	{
        public UserAccessUnitItem()
        {
        	Buildings = new List<SelectListItem>();
        }

		public int Id { get; set; }

        public int? UserId { get; set; }

        public int? TypeId { get; set; }

        public int? CompanyId { get; set; }

		[RegularExpression(@"\d*", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "NumericValidationMessage")]
		public string Serial { get; set; }

		[RegularExpression(@"\d*", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "NumericValidationMessage")]
        public string Dk { get; set; }

		//[RegularExpression(@"\d*", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "NumericValidationMessage")]
        public string Code { get; set; }

        public bool Free { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

		public int BuildingId { get; set; }

        public String DeactivationReason { get; set; }
		// custom:

        public SelectList CardTypes { get; set; }

        public SelectList Companies { get; set; }

		public IEnumerable<SelectListItem> Buildings { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		public string FirstName { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
        public string PersonalCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
        [RegularExpression("(0?[1-9]|[12][0-9]|3[01]).(0?[1-9]|1[0-2]).(20[0-9]{2}|[2][0-9][0-9]{2})", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "CommonDateFormat")]
        public string ValidFromStr { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
        [RegularExpression("(0?[1-9]|[12][0-9]|3[01]).(0?[1-9]|1[0-2]).(20[0-9]{2}|[2][0-9][0-9]{2})", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "CommonDateFormat")]
        public string ValidToStr { get; set; }

        public string ExternalPersonalCode { get; set; }
        //added
        public string Comment { get; set; }

        public bool? IsMainUnit { get; set; }

        public DateTime? Opened { get; set; }
        public DateTime? Closed { get; set; }
    }
}