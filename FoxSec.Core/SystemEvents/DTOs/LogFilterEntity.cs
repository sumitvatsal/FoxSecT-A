using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class LogFilterEntity : LogEntity
	{
		public string FromDate { get; set; }

		public string ToDate { get; set; }

		public string Name { get; set; }

		public string UserName { get; set; }

		public string Building { get; set; }

		public string Node { get; set; }

		public bool IsDeleted { get; set; }

		public string Company { get; set; }

		public string Activity { get; set; }

		public bool? IsShowDefaultLog { get; set; }
		
	}
}
