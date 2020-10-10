using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
	public interface IUserRepository : IRepository<User>
	{
		User FindByLoginName(string loginName);
        bool IsSuperAdmin(int id);
        bool IsBuildingAdmin(int id);
        bool IsCompanyManager(int id);
        bool IsCommonUser(int id);
	}
}