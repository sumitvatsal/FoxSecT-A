using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class UserAccessUnitTypeListViewModel : ViewModelBase
    {
        public UserAccessUnitTypeListViewModel()
        {
            CardTypes = new List<CardTypeItem>();
        }

        public IEnumerable<CardTypeItem> CardTypes { get; set; }
    }

    public class CardTypeItem
    {
        public int? Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
		public string Name { get; set; }

		[StringLength(250, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError250")]
        public string Description { get; set; }

        public bool IsCardCode { get; set; }

        public bool IsSerDK { get; set; }


    }
}