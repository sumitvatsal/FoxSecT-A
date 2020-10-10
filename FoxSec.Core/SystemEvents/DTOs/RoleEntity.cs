using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class RoleEntity : LogEntity
	{
		public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Menues { get; set; }

		public byte[] Permissions { get; set; }

		public string RoleType { get; set; }

		public IEnumerable<RoleBuildingEntity> RoleBuildings { get; set; }
		
	}

	public class RoleBuildingEntity
	{
		public int RoleId{ get; set;}

		public int BuildingId { get; set; }

		public bool IsDeleted { get; set; }

		public string BuildingName { get; set; }
	}
}
