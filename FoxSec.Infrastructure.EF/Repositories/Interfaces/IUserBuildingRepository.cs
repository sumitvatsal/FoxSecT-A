using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
	public interface IUserBuildingRepository : IRepository<UserBuilding>
    {
        ICollection<UserBuilding> FindByUserId(int id);
    }
}