using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FoxSec.Common.Enums;

namespace FoxSec.DomainModel.DomainObjects
{
	public class User : Entity
	{
       /* [Key]
        public new virtual int Id { get; set; } //Added newly */
		public virtual string LoginName { get; set; }

		public virtual string Password { get; set; }

		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }

		public virtual string Email { get; set; }

		public virtual string PersonalId { get; set; }

        public virtual bool Active { get; set; }

        public virtual string Comment { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime ModifiedLast { get; set; }

        public virtual string OccupationName { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual Int16? WorkHours { get; set; }

        public virtual int? GroupId { get; set; }

        public virtual byte [] Image { get; set; }

        public virtual DateTime? Birthday { get; set; }

        public virtual string Birthplace { get; set; }

        public virtual string FamilyState { get; set; }

        public virtual string Citizenship { get; set; }

        public virtual string Residence { get; set; }

        public virtual string Nation { get; set; }

        public virtual int? TitleId { get; set; }

        public virtual int? CompanyId { get; set; }

        public virtual string ContractNum { get; set; }

        public virtual DateTime? ContractStartDate { get; set; }

        public virtual DateTime? ContractEndDate { get; set; }

        public virtual DateTime? PermitOfWork { get; set; }

        public virtual bool? PermissionCallGuests { get; set; }

        public virtual bool? MillitaryAssignment { get; set; }

        public virtual String PersonalCode { get; set; }

        public virtual String ExternalPersonalCode { get; set; }

        public virtual int? LanguageId { get; set; }

        public virtual DateTime RegistredStartDate { get; set; }

        public virtual DateTime RegistredEndDate { get; set; }

        public virtual int? TableNumber { get; set; }

        public virtual bool? WorkTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string PIN1 { get; set; }

        public virtual string PIN2 { get; set; }

		public virtual bool? EServiceAllowed { get; set; }

		public virtual int? ClassificatorValueId { get; set; }

        public virtual bool? IsVisitor { get; set; }

        public virtual bool? CardAlarm { get; set; }

        public virtual bool? IsShortTermVisitor { get; set; }

        public virtual bool? ApproveTerminals { get; set; }

        public virtual bool? ApproveVisitor { get; set; }
        //public virtual int? CMPermisionGroups { get; set; }

        // relations:

        public virtual Company Company { get; set; }

        public virtual Title Title { get; set; }

        public virtual ICollection <Visitor> Visitors { get; set; }


        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<UsersAccessUnit> UsersAccessUnits { get; set; }

        public virtual ICollection<UserDepartment> UserDepartments { get; set; }

        public virtual ICollection<UserPermissionGroup> UserPermissionGroups { get; set; }

        public virtual ICollection<CompanyManager> CompanyManagers { get; set; }

		public virtual ICollection<Log> Logs { get; set; }

		public virtual ICollection<LogFilter> LogFilters { get; set; }

		public virtual ClassificatorValue ClassificatorValue { get; set; }

        public virtual ICollection<UserTimeZone> UserTimeZones { get; set; }

		public virtual ICollection<UserBuilding> UserBuildings { get; set; }

		public virtual ICollection<Role> Roles { get; set; }

        //public virtual ICollection<LogEntity> Logs { get; set; }

        public virtual ICollection<TAReport> TAReports { get; set; }
        public virtual ICollection<TAMove> TAMoves { get; set; }

        public int? buildingID { get; set; }
        public string buildingName { get; set; }

        public string DepartmentName { get; set; }

        public string UserCompanyName { get; set; }

        public IPermissionSet GetPermissions()
        {
        	var perm_set = (from user_role in UserRoles select user_role.Role.GetPermissionSet());
			if (perm_set == null || perm_set.Count() == 0)
			{
				return null;
			}

            return perm_set.Aggregate(PermissionSet.Merge);
        }
        public class TabAccess
        {
            
            public int CompanyId { get; set; }
            public int RoleID { get; set; }
        }
        public class TabAccess1
        {
            public int CameraID { get; set; }
            public int CompanyID { get; set; }
            
        }
        public IMenuSet GetMenues()
        {
        	var menu_set = (from user_role in UserRoles.Where(x=>!x.IsDeleted && x.ValidFrom <DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now ) select user_role.Role.GetMenuSet());
			if (menu_set == null || menu_set.Count() == 0)
			{
				return null;
			}

			return menu_set.Aggregate(MenuSet.Merge);
        }

		public int RolePriority()
		{
			var userRole = UserRoles.Where(node => !node.IsDeleted).FirstOrDefault();
            if (userRole == null)
            {
                return int.MaxValue;
            }

			return userRole.Role.RoleTypeId.HasValue ? userRole.Role.RoleTypeId.Value : (int)FixedRoleType.User;
		}
	}
}