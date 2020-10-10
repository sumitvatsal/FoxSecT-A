using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class TimeZone : EntityName
    {
        public virtual bool IsActive { get; set; }

		public virtual bool IsDefault { get; set; }

        // relations:

        public virtual ICollection<UserTimeZone> UserTimeZones { get; set; }

        public virtual ICollection<TimeZoneProperty> TimeZoneProperties { get; set; }

        public virtual ICollection<UserTimeZoneProperty> UserTimeZoneProperties { get; set; }
    }
}