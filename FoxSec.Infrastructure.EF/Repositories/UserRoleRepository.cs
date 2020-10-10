using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
	{
		public UserRoleRepository(IDatabaseFactory factory) : base(factory) {}

	    public IEnumerable<UserRole> FindByUserId(int userId)
	    {
            return All().Where(ur => ur.UserId == userId);
	    }
	}
}