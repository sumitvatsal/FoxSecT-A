using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class RoleBuilding : Entity
    {
        public virtual int RoleId { get; set; }

        public virtual int BuildingId { get; set; }
        
        public virtual bool IsDeleted { get; set; }

        // relations:

        public virtual Role Role { get; set; }

        public virtual Building Building { get; set; }
    }
}