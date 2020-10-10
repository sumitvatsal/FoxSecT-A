using System;

namespace FoxSec.DomainModel.DomainObjects
{
    public class TimeZoneProperty : Entity
    {
        public virtual int TimeZoneId { get; set; }

	    public virtual int OrderInGroup { get; set; }

        public virtual DateTime? ValidFrom { get; set; }

        public virtual DateTime? ValidTo { get; set; }

	    public virtual bool IsMonday { get; set; }

	    public virtual bool IsTuesday { get; set; }

	    public virtual bool IsWednesday { get; set; }

	    public virtual bool IsThursday { get; set; }

	    public virtual bool IsFriday { get; set; }

	    public virtual bool IsSaturday { get; set; }

        public virtual bool IsSunday { get; set; }

        // relations:

        public virtual TimeZone TimeZone { get; set; }
    }
}