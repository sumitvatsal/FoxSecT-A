using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface IUsersAccessUnitRepository : IRepository<UsersAccessUnit>
	{
        UsersAccessUnit FindByUserId(int userId);
       List<UsersAccessUnit> FindByUserIdList(int userId);



    }

}