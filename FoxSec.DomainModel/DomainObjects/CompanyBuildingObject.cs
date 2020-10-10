using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class CompanyManager : Entity
    {
        public virtual int CompanyId { get; set; }

        public virtual int UserId { get; set; }
        public virtual bool IsDeleted { get; set; }

        // relations:

        public virtual Company Company { get; set; }

        public virtual User User { get; set; }
    }
}