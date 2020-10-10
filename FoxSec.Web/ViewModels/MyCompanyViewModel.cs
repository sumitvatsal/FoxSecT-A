using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class MyCompanyViewModel : ViewModelBase
	{
        public MyCompanyViewModel()
		{
            Company = new CompanyItem();
		}

        public CompanyItem Company { get; set; }
	}
}