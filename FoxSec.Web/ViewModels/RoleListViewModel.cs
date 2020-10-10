using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
	public class RoleListViewModel : PaginatorViewModelBase
	{
		 public RoleListViewModel()
		{
			Roles = new List<RoleItem>();
		}

		public IEnumerable<RoleItem> Roles { get; set; }
	}

	public class RoleItem
	{
		public int? Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
		[StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
		public string Name { get; set; }

        public byte[] Permissions { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
        //[StringLength(250, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError250")]
		public string Description { get; set; }

		public string DescriptionShort { get; set; }

        public DateTime ModifiedLast { get; set; }

        public string ModifiedBy { get; set; }

        public bool Active { get; set; }

        public byte[] Menues { get; set; }

		public IEnumerable<RoleMenuFoxsecAccessItem> PermissionItems { get; set; }

        public IEnumerable<RoleMenuFoxsecAccessItem> MenuItems { get; set; }

		public RoleTypeEnum RoleTypeId { get; set; }

		public List<RoleBuildingItem> RoleBuildingItems { get; set; }

		public string RoleBuildings { get; set; }

		public string BuildingsNames { get; set; }

		public bool IsReadOnly { get; set; }

        //public bool CanEdit { get; set; }

        //public bool CanDelete { get; set; }
	}

	public class RoleBuildingItem
	{
		public RoleBuildingItem()
		{
			IsChecked = false;
			IsAvailable = true;
			Id = null;
		}

		public int? Id { get; set; }

		public int? RoleId { get; set; }

		public int? BuildingId { get; set; }

		public bool IsChecked { get; set; }

		public string BuildingName { get; set; }

		public bool IsAvailable { get; set; }
	}

	public class RoleMenuFoxsecAccessItem
	{
		public int Value { get; set; }

		public string Text { get; set; }

		public bool IsSelected { get; set; }

		public bool IsItemAvailable { get; set; }
	}



}