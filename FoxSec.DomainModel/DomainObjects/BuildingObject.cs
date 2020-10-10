using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
	public class BuildingObject : EntityName
	{
		public virtual int TypeId { get; set; }

		public virtual int BuildingId { get; set; }

		public virtual int? ParentObjectId { get; set; }

        public virtual int? FloorNr { get; set; }

		public virtual string Description { get; set; }

        public virtual int? StatusIconId { get; set; }

		public virtual int? ObjectNr { get; set; }

		public virtual string Comment { get; set; }

        public virtual int? FSControllerNodeId { get; set; }

        public virtual int? FSControllerObjectNr { get; set; }

        public virtual int? FSTypeId { get; set; }

        public virtual int? BOOrder { get; set; }

        public virtual int? GlobalBuilding { get; set; }

        public virtual int? ParentArea { get; set; }

        // relations:

        public virtual Building Building { get; set; }

		public virtual BuildingObjectType BuildingObjectType { get; set; }

		public virtual ICollection<UserBuilding> UserBuildings { get; set; }

		public virtual ICollection<CompanyBuildingObject> CompanyBuildingObjects { get; set; }

        public virtual ICollection<UserPermissionGroupTimeZone> UserPermissionGroupTimeZones { get; set; }
	}
}