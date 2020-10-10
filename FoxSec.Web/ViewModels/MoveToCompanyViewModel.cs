using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class MoveToCompanyViewModel : ViewModelBase
	{
        public SelectList Companies { get; set; }
	}
}