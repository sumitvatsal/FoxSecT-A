using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class AuditEventUser
	{
		public AuditEventUser()
		{
			UserRoles = new List<string>();
		}
		public string LoginName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public IEnumerable<string> UserRoles { get; set; }
	}
}
