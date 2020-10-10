using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoxSec.DomainModel.DomainObjects
{
   
	public class Log : Entity
	{
		public virtual int? CompanyId { get; set; }

		public virtual int? UserId { get; set; }

        public virtual int? BuildingObjectId { get; set; }

        public virtual int LogTypeId { get; set; }

		public virtual DateTime EventTime { get; set; }

		public virtual string Action { get; set; }

		public virtual string Building { get; set; }

		public virtual string Node { get; set; }

		public virtual string EventKey { get; set; }
        //public virtual int TAReportLabelId { get; set; }
        public virtual int? TAReportLabelId { get; set; }
        //relations

        public virtual LogType LogType { get; set; }

		public virtual Company Company { get; set; }

		public virtual User User { get; set; }
	}
}