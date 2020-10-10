using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class UserPermissionGroupTimeZoneRepository : RepositoryBase<UserPermissionGroupTimeZone>, IUserPermissionGroupTimeZoneRepository
    {
        public UserPermissionGroupTimeZoneRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<UserPermissionGroupTimeZone> All()
        {
            return (base.All() as ObjectSet<UserPermissionGroupTimeZone>).Include("UserTimeZone").Include("BuildingObject.BuildingObjectType").Include("UserPermissionGroup.User");
        }

        public IEnumerable<UserPermissionGroupTimeZone> FindByPGId(int Id)
        {

            return base.All().Where(x => x.Active == true && x.UserPermissionGroupId == Id && x.IsDeleted == false);/*.Include("UserTimeZone").Include("BuildingObject.BuildingObjectType").Include("UserPermissionGroup.User")*/

        }
        public IEnumerable<UserPermissionGroupTimeZone> FindByPGIdDel(int Id)
        {

            return base.All().Where(x => x.UserPermissionGroupId == Id && x.IsDeleted == false);/*.Include("UserTimeZone").Include("BuildingObject.BuildingObjectType").Include("UserPermissionGroup.User")*/

        }
        public IEnumerable<UserPermissionGroupTimeZone> FindActiveTZ(int Id)
        {
            return base.All().Where(x => !x.IsDeleted && x.UserTimeZoneId == Id);
        }
        public UserPermissionGroupTimeZone FindGpTZbyBuilding(int Pg, int Id)
        {
            return base.All().Where(x => /*!x.IsDeleted &&*/ x.UserPermissionGroupId == Pg && x.BuildingObjectId == Id && x.IsDeleted == false).FirstOrDefault();

        }
        public IEnumerable<UserPermissionGroupTimeZone> FindGpTZbyBuilding(int Id)
        {
            return base.All().Where(x => !x.IsDeleted && x.BuildingObjectId == Id && x.Active).ToList();

        }

    }
}