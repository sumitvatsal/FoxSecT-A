using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class UserTimeZone : Entity
    {
        public virtual int UserId { get; set; }

        public virtual int? TimeZoneId { get; set; }

        public virtual int? ParentUserTimeZoneId { get; set; }

        public virtual string Name { get; set; }

        public virtual Guid Uid { get; set; }

        public virtual bool IsOriginal { get; set; }

        public virtual bool IsDeleted { get; set; }

		public virtual bool IsCompanySpecific { get; set; }

        public virtual int? CompanyId { get; set; }

        // relations:

        public virtual User User { get; set; }

//      public virtual TimeZone TimeZone { get; set; }  //6.07.2012 timezone relation

        public virtual ICollection<UserTimeZoneProperty> UserTimeZoneProperties { get; set; }

        public virtual ICollection<UserPermissionGroup> UserPermissionGroups { get; set; }

        public virtual ICollection<UserPermissionGroupTimeZone> UserPermissionGroupTimeZones { get; set; }
    }
}