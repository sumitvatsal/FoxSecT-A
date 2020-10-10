using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class PartnerEditViewModel : ViewModelBase
	{
        public PartnerEditViewModel()
        {
            Partner = new PartnerItem();
        	Managers = new List<SelectListItem>();
        }

        public PartnerItem Partner { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
	}
}