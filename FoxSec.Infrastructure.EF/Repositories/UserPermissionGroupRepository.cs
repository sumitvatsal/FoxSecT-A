using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class UserPermissionGroupRepository : RepositoryBase<UserPermissionGroup>, IUserPermissionGroupRepository
    {
        public UserPermissionGroupRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<UserPermissionGroup> All()
        {
            return (base.All() as ObjectSet<UserPermissionGroup>).Include("User").Include("UserTimeZone")./*Include("UserPermissionGroupTimeZones").*/Where(x => !x.IsDeleted);
        }
        public IEnumerable<UserPermissionGroup> All(bool Include)
        {
            return (base.All() as ObjectSet<UserPermissionGroup>).Include("User").Include("UserTimeZone").Include("UserPermissionGroupTimeZones").Where(x => !x.IsDeleted);
        }

        public IEnumerable<UserPermissionGroup> FindByUserId(int id)
        {
            return (base.All() as ObjectSet<UserPermissionGroup>).Include("User").Include("UserTimeZone").Include("UserPermissionGroupTimeZones")
                .Where(x => x.PermissionIsActive && !x.IsDeleted && x.UserId == id);
        }
        public IEnumerable<UserPermissionGroup> FindByPupgHasValue()
        {
            return (base.All() as ObjectSet<UserPermissionGroup>).Where(x => !x.IsDeleted && x.ParentUserPermissionGroupId.HasValue);
        }
    }
}