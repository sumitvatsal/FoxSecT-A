using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class UserAccessUnitEntity : LogEntity
	{
		public string UserName { get; set; }

		public string TypeName { get; set; }

		public string CompanyName { get; set; }

		public string Serial { get; set; }

		public string Code { get; set; }

		public bool Active { get; set; }

		public bool Free { get; set; }

		public string Opened { get; set; }

		public string Closed { get; set; }

		public string ValidFrom { get; set; }

		public string ValidTo { get; set; }

		public string Dk { get; set; }

		public virtual int CreatedBy { get; set; }

		public virtual bool IsDeleted { get; set; }
		
	}
}
