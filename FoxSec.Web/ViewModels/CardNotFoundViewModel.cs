using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class CardNotFoundViewModel : ViewModelBase
    {
        public CardNotFoundViewModel()
        {
        	CanCreateCard = true;
        	Buildings = new List<SelectListItem>();
        	CardTypes = new List<SelectListItem>();
            ValidFrom = DateTime.Now.ToString("dd.MM.yyyy");

            ValidTo = DateTime.Now.AddYears(2).ToString("dd.MM.yyyy");
        }

        public bool IsHideThisDialog { get; set; }

		public bool CanCreateCard { get; set; }

		public IEnumerable<SelectListItem> Buildings { get; set; }

		public IEnumerable<SelectListItem> CardTypes { get; set; }

        public string ValidFrom { get; set; }

        public string ValidTo { get; set; }

		public int BuildingId { get; set; }

		public int TypeId { get; set; }
    }
}