using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class RoleRepository : LookupRepositoryBase<Role>, IRoleRepository
	{
		public RoleRepository(IDatabaseFactory factory) : base(factory) {}

		protected override IQueryable<Role> All()
		{
			return (base.All() as ObjectSet<Role>).Include("RoleBuildings.Building").Include("RoleType").Include("UserRoles.User").Include("User");
		}
	}
}