using System.Collections.Generic;
using System.Linq;
using FoxSec.Authentication;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;

namespace FoxSec.Web.Controllers
{
	public abstract class BusinessCaseController : AuthorizeControllerBase
	{
		protected BusinessCaseController(ICurrentUser currentUser, ILogger logger) : base(currentUser, logger) {}

		public string HostName
		{
			get { return Request.UserHostAddress; }
		}

		protected List<int> GetRoleBuildings(IBuildingRepository buildingRepository, IRoleRepository roleRepository)
		{
			var roleBuildings = buildingRepository.FindAll(x => !x.IsDeleted);
			if (!CurrentUser.Get().IsSuperAdmin)
            {
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId == null) { return (from rb in roleBuildings select rb.Id).ToList(); }
				var userRoleBuildings = roleRepository.FindById(CurrentUser.Get().RoleId).RoleBuildings.Where(rb => !rb.IsDeleted);
				roleBuildings = roleBuildings.Where(rb => userRoleBuildings.Any(urb => urb.BuildingId == rb.Id && !urb.IsDeleted));
			}
		    return (from rb in roleBuildings select rb.Id).ToList();
		}

        protected List<int> GetUserBuildings(IBuildingRepository buildingRepository, IUserRepository userRepository, int? userId = null)
        {
            var roleBuildings = buildingRepository.FindAll(x => !x.IsDeleted);
            /*if (!CurrentUser.Get().IsSuperAdmin)//<--!CurrentUser.Get().IsSuperAdmin
            {
                if (userId.HasValue)
                {
                    var userBuildings = userRepository.FindById(userId.HasValue ? userId.Value : CurrentUser.Get().Id).UserBuildings.Where(x => !x.IsDeleted);
                    roleBuildings = roleBuildings.Where(rb => userBuildings.Any(ub => ub.BuildingId == rb.Id));
                }
            }
            */
            return (from rb in roleBuildings select rb.Id).ToList();
        }

		protected List<User> GetUsersByBuildingInRole(List<User> users, IBuildingRepository buildingRepository, IUserRepository userRepository)
		{
			if (CurrentUser.Get().IsSuperAdmin)
			{
				return users;
			}
			var roleBuildingIds = GetUserBuildings(buildingRepository, userRepository);
			return
				users.Where(
					x => (CurrentUser.Get().IsCompanyManager && (x.UserBuildings.Count == 0 || !x.UserBuildings.Any(ub=>!ub.IsDeleted)) ) ||
					(x.UserBuildings.Count > 0 && x.UserBuildings.Any(ub => !ub.IsDeleted && roleBuildingIds.Contains(ub.BuildingId)))
					).ToList();
		}
	}
}