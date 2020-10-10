using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class BuildingObjectType : EntityName
    {
        public virtual string Description { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime ModifiedLast { get; set; }

        // relations:

        public virtual ICollection<BuildingObject> BuildingObjects { get; set; }
    }
}