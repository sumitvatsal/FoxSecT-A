using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class CompanyEditViewModel : ViewModelBase
	{
        public CompanyEditViewModel()
		{
            Company = new CompanyItem();
        	BuildingItems = new List<SelectListItem>();
            FoxSecUser = new UserItem();

		}

        public CompanyItem Company { get; set; }

        public IEnumerable<Building> Buildings { get; set; }

		public IEnumerable<SelectListItem> BuildingItems { get; set; }

        public IEnumerable<Company> CompanyItems { get; set; }

        public List<int> SelCompanyItems { get; set; }

        public UserItem FoxSecUser { get; set; } // added by manoranjan
    }
}