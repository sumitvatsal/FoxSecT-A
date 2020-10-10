using System;
using System.Collections.Generic;
using FoxSec.Common.Extensions;

namespace FoxSec.DomainModel.DomainObjects
{
	public class Role : LookupEntity
	{
        public virtual int StaticId { get; set; }

        public virtual bool Active { get; set; } 

		public virtual byte[] Permissions { get; set; }

        public virtual byte[] Menues { get; set; }

        public virtual int Priority { get; set; }

		public virtual int? RoleTypeId { get; set; }

		public virtual int? UserId { get; set; }

		private IPermissionSet _permissionSet;

		public IPermissionSet GetPermissionSet()
		{
			return _permissionSet ?? (_permissionSet = new PermissionSet(Permissions));
		}

	    private IMenuSet _menuSet;

        public IMenuSet GetMenuSet()
        {
            return _menuSet ?? (_menuSet = new MenuSet(Menues));
        }

		public void AssignPermissions(IEnumerable<int> permissionIndexes)
		{
            if (Permissions != null)
            {
                Array.Clear(Permissions, 0, Permissions.Length);
            }

		    IPermissionSet ps = GetPermissionSet();

			permissionIndexes.ForEach(index => ps[index.AsEnum<Permission>()] = true);
			ApplyPermissions();
		}

        public void TogglePermission(int index)
        {
            IPermissionSet ps = GetPermissionSet();
            ps[index.AsEnum<Permission>()] = !ps[index.AsEnum<Permission>()];
            ApplyPermissions();
        }

		public void ApplyPermissions()
		{
			Permissions = Permissions;
			_permissionSet = null;
		}

        public void AssignMenues(IEnumerable<int> menuIndexes)
        {
            if (Menues != null)
            {
                Array.Clear(Menues, 0, Menues.Length);
            }

            IMenuSet ms = GetMenuSet();

            menuIndexes.ForEach(index => ms[index.AsEnum<Menu>()] = true);
            ApplyMenues();
        }

        public void ToggleMenu(int index)
        {
            IMenuSet ms = GetMenuSet();
            ms[index.AsEnum<Menu>()] = !ms[index.AsEnum<Menu>()];
            ApplyMenues();
        }

        public void ApplyMenues()
        {
            Menues = Menues;
            _menuSet = null;
        }

        // realtions:

        public virtual ICollection<UserRole> UserRoles { get; set; }

		public virtual ICollection<RoleBuilding> RoleBuildings { get; set; }

		public virtual RoleType RoleType { get; set; }

		public virtual User User { get; set; }
	}
}