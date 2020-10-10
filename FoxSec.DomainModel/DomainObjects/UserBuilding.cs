using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class UserBuilding : Entity
    {
        public virtual int UserId { get; set; }

        public virtual int BuildingId { get; set; }

		public virtual int? BuildingObjectId { get; set; }
        
        public virtual bool IsDeleted { get; set; }

        // relations:

        public virtual User User { get; set; }

        public virtual Building Building { get; set; }

		public virtual BuildingObject BuildingObject { get; set; }
    }
}