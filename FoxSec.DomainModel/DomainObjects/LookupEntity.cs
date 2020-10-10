using System;

namespace FoxSec.DomainModel.DomainObjects
{
    public class LookupEntity : EntityName
	{
		protected internal LookupEntity() {}

		public virtual string Description { get; set; }

        public virtual DateTime ModifiedLast { get; set; }

        public virtual string ModifiedBy { get; set; }
	}
}