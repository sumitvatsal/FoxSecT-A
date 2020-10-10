using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxSec.Common.Enums;

namespace FoxSec.DomainModel.DomainObjects
{
    public class FSSubLocation : Entity
    {
        public virtual string Sublocation { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime TimeStamp { get; set; }
        public virtual int? CompanyId { get; set; }

       
    } 
}
