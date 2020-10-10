using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class CompanyBuildingObject : Entity
    {
        public virtual int CompanyId { get; set; }

        public virtual int BuildingObjectId { get; set; }

        public virtual DateTime ValidFrom { get; set; }

        public virtual DateTime ValidTo { get; set; }

        public virtual bool IsDeleted { get; set; }

        // relations:

        public virtual Company Company { get; set; }

        public virtual BuildingObject BuildingObject { get; set; }
    }
}