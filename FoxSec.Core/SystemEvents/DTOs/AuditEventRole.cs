using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class AuditEventRole
	{
		public AuditEventRole()
		{
			Permissions = new List<string>();
		}

		public string Description { get; set; }

		public IEnumerable<string> Permissions { get; set; }
	}
}
