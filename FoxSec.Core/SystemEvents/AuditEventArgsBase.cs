using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents
{
	public class AuditEventArgsBase
	{
		public AuditEventArgsBase(string loginName, string firstName, string lastName, DateTime eventTime)
		{
			LoginName = loginName;
			FirstName = firstName;
			LastName = lastName;
			EventTime = eventTime;
		}

		public string LoginName { get; private set; }

		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		public DateTime EventTime { get; private set; }
	}
}
