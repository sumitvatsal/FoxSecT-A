using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface IUserPermissionGroupTimeZoneRepository : IRepository<UserPermissionGroupTimeZone>
    {

        IEnumerable<UserPermissionGroupTimeZone> FindByPGId(int Id);
        IEnumerable<UserPermissionGroupTimeZone> FindByPGIdDel(int Id);
        IEnumerable<UserPermissionGroupTimeZone> FindActiveTZ(int Id);
        UserPermissionGroupTimeZone FindGpTZbyBuilding(int Pg, int Id);
        IEnumerable<UserPermissionGroupTimeZone> FindGpTZbyBuilding(int Id);
    }
}