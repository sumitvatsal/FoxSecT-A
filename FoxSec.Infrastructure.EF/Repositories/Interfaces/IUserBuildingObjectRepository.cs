using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
	public interface IUserBuildingObjectRepository : IRepository<UserBuildingObject>
    {
        ICollection<UserBuildingObject> FindByUserId(int id);
    }
}