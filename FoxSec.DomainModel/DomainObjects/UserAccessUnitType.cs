using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class UserAccessUnitType : EntityName
    {
        public virtual string Description { get; set; }
        public virtual bool IsCardCode { get; set; }
        public virtual bool IsSerDK { get; set; }

        // relations

        public virtual ICollection<UsersAccessUnit> UsersAccessUnits { get; set; }
    }
}