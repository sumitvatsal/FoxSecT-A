using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class UserEntity : LogEntity
	{
		public string LoginName { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string PersonalId { get; set; }

		public bool Active { get; set; }

		public string Comment { get; set; }

		public string ModifiedBy { get; set; }

		public string OccupationName { get; set; }

		public string CreatedBy { get; set; }

		public string PhoneNumber { get; set; }

		public Int16? WorkHours { get; set; }

		public string Birthday { get; set; }

		public string Birthplace { get; set; }

		public string FamilyState { get; set; }

		public string Citizenship { get; set; }

		public string Residence { get; set; }

		public string Nation { get; set; }

		public string ContractNum { get; set; }

		public string ContractStartDate { get; set; }

		public string ContractEndDate { get; set; }

		public string PermitOfWork { get; set; }

		public bool? PermissionCallGuests { get; set; }

		public bool? MillitaryAssignment { get; set; }

		public String PersonalCode { get; set; }

		public String ExternalPersonalCode { get; set; }

		public string RegistredStartDate { get; set; }

		public virtual DateTime RegistredEndDate { get; set; }

		public int? TableNumber { get; set; }

		public bool? WorkTime { get; set; }

		public virtual string PIN1 { get; set; }

		public virtual string PIN2 { get; set; }

		public virtual bool? EServiceAllowed { get; set; } 

		// relations:

		public string CompanyName { get; set; }

		public string TitleName { get; set; }

	}
}
