using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class UserBuildingRepository : RepositoryBase<UserBuilding>, IUserBuildingRepository
	{
		public UserBuildingRepository(IDatabaseFactory factory) : base(factory) { }

		protected override IQueryable<UserBuilding> All()
		{
			return (base.All() as ObjectSet<UserBuilding>).Include("Building.Location.Country").Include("BuildingObject");
        }

        public ICollection<UserBuilding> FindByUserId(int id)
        {
            return All().Where(ubo => ubo.UserId == id && !ubo.IsDeleted).ToList();
        }
	}
}