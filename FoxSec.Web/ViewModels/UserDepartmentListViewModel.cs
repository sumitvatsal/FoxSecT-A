using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class UserDepartmentListViewModel : ViewModelBase
	{
        public UserDepartmentListViewModel()
		{
            Departments = new List<DepartmentItem>();
            UserDepartments = new List<UserDepartmentItem>();
		}

        public IEnumerable<DepartmentItem> Departments { get; set; }
        public IEnumerable<UserDepartmentItem> UserDepartments { get; set; }
        public int UserId { get; set; }
	}

    public class UserDepartmentItem
	{
        public virtual int Id { get; set; }

        public virtual int DepartmentId { get; set; }
        public virtual int UserId { get; set; }

		public virtual string DepartmentName { get; set; }

        public virtual String ValidFrom { get; set; }

        public virtual String ValidTo { get; set; }

        public virtual bool CurrentDep { get; set; }
        
        public virtual string Manager { get; set; }

        public virtual bool IsForDelete { get; set; }
	}
}