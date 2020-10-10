using FoxSec.Common.Enums;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;
using FoxSec.Infrastructure.EF.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.Infrastructure.EF.Repositories
{
   internal class VisitorRepository: RepositoryBase<Visitor>, IVisitorRepository
    {
        public VisitorRepository(IDatabaseFactory factory) : base(factory) { }

        public Visitor FindByUserId(int userId)
        {
            return All().Where(entity => entity.UserId == userId).SingleOrDefault();
        }
        public bool IsSuperAdmin(int id)
        {
            var activeRoles = FindById(id).VisitorRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

            if (activeRoles.Count != 0)
            {
                int? roleTypeId = activeRoles.First().Role.RoleTypeId;

                if (roleTypeId.HasValue)
                {
                    return roleTypeId.Value == (int)FixedRoleType.SuperAdmin;
                }
            }

            return false;
        }
       
        public bool IsBuildingAdmin(int id)
        {
            var activeRoles = FindById(id).VisitorRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

            if (activeRoles.Count != 0)
            {
                int? roleTypeId = activeRoles.First().Role.RoleTypeId;

                if (roleTypeId.HasValue)
                {
                    return roleTypeId.Value == (int)FixedRoleType.Administrator;
                }
            }

            return false;
        }
        public bool IsCompanyManager(int id)
        {
            var activeRoles = FindById(id).VisitorRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

            if (activeRoles.Count != 0)
            {
                int? roleTypeId = activeRoles.First().Role.RoleTypeId;

                if (roleTypeId.HasValue)
                {
                    return roleTypeId.Value == (int)FixedRoleType.CompanyManager;
                }
            }

            return false;
        }
        public bool IsCommonUser(int id)
        {
            var activeRoles = FindById(id).VisitorRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

            if (activeRoles.Count != 0)
            {
                int? roleTypeId = activeRoles.First().Role.RoleTypeId;

                if (roleTypeId.HasValue)
                {
                    return roleTypeId.Value == (int)FixedRoleType.User;
                }
            }

            return false;
        }

        /*  protected override IQueryable<Visitor> All()
          {
              return (base.All() as ObjectSet<Visitor>).Include("User.UserBuildings.Building").Include("User.UserBuildings.BuildingObject").Include("Company").Include("UserAccessUnitType").Include("Building").Include("ClassificatorValue");
          }
          */
    }
}
