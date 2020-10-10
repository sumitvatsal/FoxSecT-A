using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.Audit.Data;

namespace FoxSec.Web.ViewModels
{
	public class AuditLogViewModel : ViewModelBase
	{
		public bool CanClearLog { get; set; }

		public IEnumerable<AuditLog> LogItems { get; set; }
	}
}