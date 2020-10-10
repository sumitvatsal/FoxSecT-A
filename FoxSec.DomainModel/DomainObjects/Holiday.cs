using System;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Holiday : EntityName
    {
        public virtual DateTime EventStart { get; set; }

        public virtual DateTime EventEnd { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime ModifiedLast { get; set; }

        public virtual bool MovingHoliday { get; set; }
    }
}