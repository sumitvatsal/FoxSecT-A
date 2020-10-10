using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class UsersAccessUnitRepository : RepositoryBase<UsersAccessUnit>, IUsersAccessUnitRepository
	{
        public UsersAccessUnitRepository(IDatabaseFactory factory) : base(factory) {}

        public UsersAccessUnit FindByUserId(int userId)
        {
            return All().Where(entity => entity.UserId == userId).SingleOrDefault();
        }

        public List<UsersAccessUnit> FindByUserIdList(int userId)
        {
            return All().Where(entity => entity.UserId == userId && entity.IsDeleted==false).ToList();
        }

        protected override IQueryable<UsersAccessUnit> All()
        {
			return (base.All() as ObjectSet<UsersAccessUnit>).Include("User.UserBuildings.Building").Include("User.UserBuildings.BuildingObject").Include("Company").Include("UserAccessUnitType").Include("Building").Include("ClassificatorValue");
        }
	}
}