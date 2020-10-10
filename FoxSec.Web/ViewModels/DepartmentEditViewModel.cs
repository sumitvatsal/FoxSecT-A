using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class DepartmentEditViewModel : ViewModelBase
	{
        public DepartmentEditViewModel()
		{
            Department = new DepartmentItem();
            Managers = new List<DepartmentManager>();
		}

        public DepartmentItem Department { get; set; }

        public int SelectedUserId { get; set; }
        public int SelectedDepartmentManagerId { get; set; }

        public SelectList CompanyList { get; set; }
        public List<DepartmentManager> Managers { get; set; }
        public int ManagerId { get; set; }
	}
}