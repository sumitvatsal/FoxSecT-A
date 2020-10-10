using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class UserPermissionGroupTimeZone : Entity
    {
        public virtual int UserPermissionGroupId { get; set; }

        public virtual int UserTimeZoneId { get; set; }

        public virtual int BuildingObjectId { get; set; }

        public virtual bool IsArming { get; set; }

        public virtual bool IsDefaultArming { get; set; }

        public virtual bool IsDisarming { get; set; }

        public virtual bool IsDefaultDisarming { get; set; }

        public virtual bool Active { get; set; }

        public virtual bool IsDeleted { get; set; }

        // relations:

        public virtual UserPermissionGroup UserPermissionGroup { get; set; }

        public virtual UserTimeZone UserTimeZone { get; set; }

        public virtual BuildingObject BuildingObject { get; set; }
    }
}