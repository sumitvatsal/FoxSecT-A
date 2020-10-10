using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class DepartmentListViewModel : PaginatorViewModelBase
	{
        public DepartmentListViewModel()
		{
            Departments = new List<DepartmentItem>();
		}

        public IEnumerable<DepartmentItem> Departments { get; set; }
	}

    public class DepartmentItem
	{
        public DepartmentItem()
		{
            UserDepartments = new List<UserDepartment>();

            DepartmentManagersList = new Dictionary<int, string>();
		}

		public int Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
		public string Number { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
        public string Name { get; set; }

        public string ModifiedBy { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public bool IsDepartmentManager { get; set; }

        public IEnumerable<UserDepartment> UserDepartments { get; set; }

        public IDictionary<int, string> DepartmentManagersList { get; set; }

        public string Manger { get; set; }
	}

    public class DepartmentManager
    {
        public int UserDepartmentId { get; set; }

        public int? UserId { get; set; }

        public String ValidFrom { get; set; }
        public String ValidTo{ get; set; }
    }
}