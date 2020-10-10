using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface IUserPermissionGroupRepository : IRepository<UserPermissionGroup>
    {
        IEnumerable<UserPermissionGroup> FindByUserId(int Id);
        IEnumerable<UserPermissionGroup> FindByPupgHasValue();
    }
}