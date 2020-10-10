using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Location : EntityName
    {
        public virtual int CountryId { get; set; }

        // relations:

        public virtual Country Country { get; set; }

        public virtual ICollection<Building> Buildings { get; set; }
    }
}