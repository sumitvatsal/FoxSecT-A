using System;

namespace FoxSec.DomainModel.DomainObjects
{
	public class UserRole : Entity
	{
		public virtual int UserId { get; set; }

		public virtual int RoleId { get; set; }

        public virtual int? CompanyId { get; set; }

        public virtual int? BuildingId { get; set; }

        public virtual DateTime ValidFrom { get; set; }

        public virtual DateTime ValidTo { get; set; }

        public virtual bool IsDeleted { get; set; }

        // relations:

		public virtual User User { get; set; }

		public virtual Role Role { get; set; }
        
        public virtual Company Company { get; set; }
	}
}