using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class RoleType : EntityName
    {
       // relations:
		public virtual byte[] Menues { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }

	public enum RoleTypeEnum
	{
		SA = 1,

		BA = 2,

		CM = 3,

		DM = 4,

		U  = 5
	}
}