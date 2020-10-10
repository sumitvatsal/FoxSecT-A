using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface IUserDepartmentRepository : IRepository<UserDepartment>
    {
        IEnumerable<UserDepartment> FindByUserId(int userId);
    }
}