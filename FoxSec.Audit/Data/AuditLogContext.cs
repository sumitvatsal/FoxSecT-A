using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using System.Text;

namespace FoxSec.Audit.Data
{
	public class AuditLogContext : ObjectContext
	{
		public const string CONTAINER_NAME = "TerisDBAudit";
		public const string CONNECTION_STRING = "name=TerisDBAudit";
		private ObjectSet<AuditLog> _auditLogs;

		public AuditLogContext() : base(CONNECTION_STRING, CONTAINER_NAME)
		{
		}

		public ObjectSet<AuditLog> AuditLogs
		{
			get { return _auditLogs ?? (_auditLogs = CreateObjectSet<AuditLog>("AuditLogs")); }
		}
	}
}
