using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;
using FoxSec.Common.Enums;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class UserRepository : RepositoryBase<User>, IUserRepository
	{
		public UserRepository(IDatabaseFactory factory) : base(factory) { }

		protected override IQueryable<User> All()
		{
			return (base.All() as ObjectSet<User>).Include("Title").Include("UserRoles.Role").Include("UserRoles.User").Include("UserBuildings.Building").Include("UserDepartments.Department").Include("UsersAccessUnits").Include("Company");
        }
        
		public User FindByLoginName(string loginName)
		{
            try
            { 
            return All().Where(user => user.LoginName == loginName).ToList().SingleOrDefault(user => string.Compare(user.LoginName, loginName, false) == 0);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool IsSuperAdmin(int id)
        {
            var activeRoles = FindById(id).UserRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

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
            var activeRoles = FindById(id).UserRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

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
            var activeRoles = FindById(id).UserRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

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
            var activeRoles = FindById(id).UserRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

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
	}
}