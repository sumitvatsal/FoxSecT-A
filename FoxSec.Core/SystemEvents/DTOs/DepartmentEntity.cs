using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class DepartmentEntity : LogEntity
	{
		public string Name { get; set; }
		public string Number { get; set; }		
		public virtual string CompanyName { get; set; }
		public virtual string Activity { get; set; }		
	}
}
