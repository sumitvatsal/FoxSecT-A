using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
	public interface IUserRoleRepository : IRepository<UserRole>
	{
        IEnumerable<UserRole> FindByUserId(int userId);
	}
}