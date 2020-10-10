using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Country : EntityName
    {
        public virtual short? ISONumber { get; set; }

        // relations:

        public virtual ICollection<Location> Locations { get; set; }
    }
}