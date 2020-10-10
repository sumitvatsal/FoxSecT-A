using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;


namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class UserBuildingObjectRepository : RepositoryBase<UserBuildingObject>, IUserBuildingObjectRepository
	{
		public UserBuildingObjectRepository(IDatabaseFactory factory) : base(factory) { }

		protected override IQueryable<UserBuildingObject> All()
		{
			return (base.All() as ObjectSet<UserBuildingObject>).Include("BuildingObject.BuildingObjectType").Include("BuildingObject.Building.Location.Country");
        }

        public ICollection<UserBuildingObject> FindByUserId(int id)
        {
            return All().Where(ubo => ubo.UserId == id && !ubo.IsDeleted).ToList();
        }
	}
}