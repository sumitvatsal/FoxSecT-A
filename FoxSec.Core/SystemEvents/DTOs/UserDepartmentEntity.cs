using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class UserDepartmentEntity : LogEntity
	{
		public string ValidFrom { get; set; }

		public string ValidTo { get; set; }

		public string UserName { get; set; }

		public string DepartmentName { get; set; }

		public bool IsDepartmentManager { get; set; }

		public bool CurrentDep { get; set; }



	}
}
