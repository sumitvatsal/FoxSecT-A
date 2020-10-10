using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
	public class Company : EntityName
	{
		public virtual string ModifiedBy { get; set; }
		public virtual DateTime ModifiedLast { get; set; }
		public virtual string Comment { get; set; }
		public virtual bool Active { get; set; }
		public virtual int? ParentId { get; set; }
		public virtual bool IsCanUseOwnCards { get; set; }
		public virtual int? ClassificatorValueId { get; set; }
        public String BuidingNames { get; set; }
        public String Floors { get; set; }
		// relations:

		public virtual ICollection<Department> Departments { get; set; }
		public virtual ICollection<Title> Titles { get; set; }
		public virtual ICollection<UserRole> UserRoles { get; set; }
		public virtual ICollection<CompanyBuildingObject> CompanyBuildingObjects { get; set; }
		public virtual ICollection<UsersAccessUnit> UsersAccessUnits { get; set; }
		public virtual ICollection<CompanyManager> CompanyManagers { get; set; }
		public virtual ICollection<Log> Logs { get; set; }
		public virtual ICollection<LogFilter> LogFilters { get; set; }
		public virtual ICollection<User> Users { get; set; }
		public virtual ClassificatorValue ClassificatorValue { get; set; }
	}
}