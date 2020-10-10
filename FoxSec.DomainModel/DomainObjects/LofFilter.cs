using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
	public class LogFilter : Entity
	{
		public virtual int? CompanyId { get; set; }

		public virtual int UserId { get; set; }

		public virtual DateTime? FromDate { get; set; }

		public virtual DateTime? ToDate { get; set; }

		public virtual string Name { get; set; }

		public virtual string UserName { get; set; }

		public virtual string Building { get; set; }

		public virtual string Node { get; set; }

		public virtual bool IsDeleted { get; set; }

		public virtual bool? IsShowDefaultLog { get; set; }

		public virtual string Activity { get; set; }

        //relations

        public virtual Company Company { get; set; }

		public virtual User User { get; set; }




	}
}