using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.Web.ViewModels;

namespace FoxSec.Web.Controllers
{
    public class GlobalSearchController : PaginatorControllerBase<GlobalSearchItem>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public GlobalSearchController(
							    IBuildingRepository buildingRepository,
							    IRoleRepository roleRepository,
                                IUserRepository userRepository,
							    ICurrentUser currentUser,
                                ILogger logger) : base(currentUser, logger)
        {
            _buildingRepository = buildingRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            RegisterSearchUserRelatedMaps();
        }

        [HttpGet]
        public ActionResult Index(string searchCriteria, int? nav_page, int? rows)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            var gsvm = CreateViewModel<GlobalSearchViewModel>();
            gsvm.SearchCriteria = searchCriteria;
            var list = Search(searchCriteria);
            gsvm.Paginator = SetupPaginator(ref list, nav_page, rows);
            gsvm.Paginator.DivToRefresh = "GlobalSearchResult";
            gsvm.Paginator.Prefix = "GlobalSearch";
            gsvm.Items = list;
            return View("Index", gsvm);
        }

        [HttpGet]
        public ActionResult List(string searchCriteria, int? nav_page, int? rows, int? sort_field, int? sort_direction)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            var gsvm = CreateViewModel<GlobalSearchViewModel>();
            var list = Search(searchCriteria);
            gsvm.SearchCriteria = searchCriteria;
            if (sort_field.HasValue && sort_direction.HasValue)
            {
                switch (sort_field)
                {
                    case 1:
                        if (sort_direction.Value == 0)
                            list = list.OrderBy(x=>x.Name);
                        else
                            list = list.OrderByDescending(x=>x.Name);
                        break;
                    case 2:
                        if (sort_direction.Value == 0)
                            list =list.OrderBy(x=>x.TypeDescription);
                        else
                            list=list.OrderByDescending(x=>x.TypeDescription);
                        break;
                }
            }
            gsvm.Paginator = SetupPaginator(ref list, nav_page, rows);
            gsvm.Paginator.DivToRefresh = "GlobalSearchResult";
            gsvm.Paginator.Prefix = "GlobalSearch";
            gsvm.Items = list;
            return View("List", gsvm);
        }

        private IEnumerable<GlobalSearchItem> Search(string searchCriteria)
        {
            var result = new List<GlobalSearchItem>();
            result.AddRange(SearchUsers(searchCriteria));
            return result;
        }

        private IEnumerable<GlobalSearchItem> SearchUsers(string searchCriteria)
        {
            var result = new List<GlobalSearchItem>();
            var user_priority = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_priority <= x.RolePriority()).ToList();
            if (!CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin)
            {
                users = users.Where(us => us.CompanyId != null).ToList();
            }
            users = GetUsersByBuildingInRole(users);
            var filtered_users = new List<User>();
            //By user name
            string[] split = searchCriteria.ToLower().Trim().Split(' ');
            if (split.Count() == 1)
            {
                filtered_users.AddRange(users.Where(x => (x.FirstName.ToLower().Contains(split[0]) || (x.PersonalId != null && x.PersonalId.ToLower().Contains(split[0])))).ToList());
            }
            else if (split.Count() == 2)
            {
                filtered_users.AddRange(users.Where(x => x.FirstName.ToLower().Contains(split[0]) && x.LastName.ToLower().Contains(split[1])).ToList());
            }
            Mapper.Map(filtered_users, result);
            return result;
        }

        private List<User> GetUsersByBuildingInRole(List<User> users)
        {
            if (CurrentUser.Get().IsSuperAdmin)
            {
                return users;
            }
            var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
            var new_users =
                    users.Where(
                        x =>
                        x.UserBuildings.Count > 0 &&
                        x.UserBuildings.Any(ub => !ub.IsDeleted && role_building_ids.Contains(ub.BuildingId))).ToList();
            return new_users;
        }

        private void RegisterSearchUserRelatedMaps()
        {
            Mapper.CreateMap<User, GlobalSearchItem>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.Format("{0} {1}", src.FirstName, src.LastName)))
                .ForMember(dest => dest.TypeDescription, opt => opt.MapFrom(src => ViewResources.SharedStrings.UsersTabName))
                .ForMember(dest => dest.ToolTipName, opt => opt.MapFrom(src =>
                    string.Format("{0} {1} {2}", ViewResources.SharedStrings.GlobalSearchGoToUser, src.FirstName, src.LastName)))
                .ForMember(dest => dest.Function, opt => opt.MapFrom(src =>
                    string.Format("javascript:GoToUser({0})", src.Id)));
        }
    }
}