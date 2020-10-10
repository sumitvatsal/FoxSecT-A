using System;
using System.ComponentModel.DataAnnotations;

namespace FoxSec.Audit.Data
{
	public class AuditLog
	{
		public int Id { get; set; }
		public int EventTypeId { get; set; }
		public DateTime EventTime { get; set; }
		public string DateTimeFormatString { get; set; }
		public string UserName { get;set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public string DataChangeRendererName { get; set; }
	}

	public enum AuditEventType
	{
		[Display(Name = "User creation")]
		UserCreated = 0,
		[Display(Name = "User deletion")]
		UserDeleted = 1,
		[Display(Name = "User edit")]
		UserEdited = 2,
		[Display(Name = "Role creation")]
		RoleCreated = 3,
		[Display(Name = "Role deletion")]
		RoleDeleted = 4,
		[Display(Name = "Role edit")]
		RoleEdited = 5
	}
}
