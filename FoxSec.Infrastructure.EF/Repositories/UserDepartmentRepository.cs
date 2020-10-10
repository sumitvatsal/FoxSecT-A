using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class UserDepartmentRepository : RepositoryBase<UserDepartment>, IUserDepartmentRepository
	{
        public UserDepartmentRepository(IDatabaseFactory factory) : base(factory) { }
        public IEnumerable<UserDepartment> FindByUserId(int userId)
        {
            return All().Where(entity => entity.UserId == userId).ToList();
        }


        protected override IQueryable<UserDepartment> All()
        {
            return (base.All() as ObjectSet<UserDepartment>).Include("User").Include("Department").Include("User.UserRoles.Role");
        }
	}
}