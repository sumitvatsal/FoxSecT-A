using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Title : EntityName
    {
        public virtual string Description { get; set; }

        public virtual int CompanyId { get; set; }

        // relations:

        public virtual Company Company { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}