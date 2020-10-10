using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class UserPermissionGroup : Entity
    {
        public virtual int UserId { get; set; }

        public virtual int DefaultUserTimeZoneId { get; set; }

        public virtual int? ParentUserPermissionGroupId { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsOriginal { get; set; }

        public virtual bool PermissionIsActive { get; set; }

        public virtual bool IsDeleted { get; set; }

        // relations:

        public virtual User User { get; set; }

        public virtual UserTimeZone UserTimeZone { get; set; }

        public virtual ICollection<UserPermissionGroupTimeZone> UserPermissionGroupTimeZones { get; set; }
    }
}