using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.Common.Enums;
using FoxSec.Common.SendMail;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.ServiceResults;
using FoxSec.ServiceLayer.Services;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using ViewResources;
using System.Xml;
using System.Xml.Linq;
using FoxSec.Core.Infrastructure.UnitOfWork;
using System.Net;
using System.Reflection;
using System.Xml.Serialization;
using static FoxSec.Web.ViewModels.HRListViewModel;
using static FoxSec.Web.ViewModels.datatableListViewModel;
using FoxSec.Web.Controllers;
using System.Configuration;
using System.Data.SqlClient;
namespace FoxSec.Web.Controllers
{
    public class testvideoController : PaginatorControllerBase<UserItem>
    {
        private readonly IUserService _userService;
        private readonly IUserDepartmentService _userDepartmentService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITitleRepository _titleRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserDepartmentRepository _userDepartmentRepository;
        private readonly ICompanyBuildingObjectRepository _companyBuildingObjectRepository;
        private readonly IUserBuildingRepository _userBuildingRepository;
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly IUserPermissionGroupService _userPermissionGroupService;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUserTimeZonePropertyRepository _userTimeZonePropertyRepository;
        private readonly IUserPermissionGroupRepository _userPermissionGroupRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly IUsersAccessUnitService _userAccessUnitService;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IUserBuildingService _userBuildingService;
        private readonly IUsersAccessUnitService _cardService;
        private readonly IUsersAccessUnitRepository _usersAccessUnitRepository;
        private readonly IControllerUpdateService _controllerUpdateService;
        private readonly IFSINISettingRepository _FSINISettingsRepository;
        private ResourceManager _resourceManager;

        public testvideoController(IUserService userService,
                                IFSINISettingRepository FSINISettingsRepository,
                                IUsersAccessUnitService cardService,
                                IControllerUpdateService controllerUpdateService,
                                IUserDepartmentService userDepartmentService,
                                ICurrentUser currentUser,
                                IUserRepository userRepository,
                                IRoleRepository roleRepository,
                                ITitleRepository titleRepository,
                                ICompanyRepository companyRepository,
                                ICountryRepository countryRepositorty,
                                ILocationRepository locationRepository,
                                IBuildingRepository buildingRepository,
                                IUsersAccessUnitRepository usersAccessUnitRepository,
                                ICompanyBuildingObjectRepository companyBuildingObjectRepository,
                                IBuildingObjectRepository buildingObjectRepository,
                                IDepartmentRepository departmentRepository,
                                IUserDepartmentRepository userDepartmentRepository,
                                IUserPermissionGroupService userPermissionGroupService,
                                IUserBuildingRepository userBuildingRepository,
                                IUserTimeZoneRepository userTimeZoneRepository,
                                IUserTimeZonePropertyRepository userTimeZonePropertyRepository,
                                IUserPermissionGroupRepository userPermissionGroupRepository,
                                IUserPermissionGroupTimeZoneRepository userPermissionGroupTimeZoneRepository,
                                IUsersAccessUnitService usersAccessUnitService,
                                IUserBuildingService userBuildingService,
                                IClassificatorValueRepository classificatorValueRepository,
                                ILogger logger) : base(currentUser, logger)
        {
            _FSINISettingsRepository = FSINISettingsRepository;
            _controllerUpdateService = controllerUpdateService;
            _userService = userService;
            _cardService = cardService;
            _usersAccessUnitRepository = usersAccessUnitRepository;
            _userDepartmentService = userDepartmentService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _titleRepository = titleRepository;
            _companyRepository = companyRepository;
            _locationRepository = locationRepository;
            _countryRepository = countryRepositorty;
            _buildingRepository = buildingRepository;
            _companyBuildingObjectRepository = companyBuildingObjectRepository;
            _departmentRepository = departmentRepository;
            _userDepartmentRepository = userDepartmentRepository;
            _buildingObjectRepository = buildingObjectRepository;
            _userBuildingRepository = userBuildingRepository;
            _userPermissionGroupService = userPermissionGroupService;
            _userTimeZoneRepository = userTimeZoneRepository;
            _userTimeZonePropertyRepository = userTimeZonePropertyRepository;
            _userPermissionGroupRepository = userPermissionGroupRepository;
            _userPermissionGroupTimeZoneRepository = userPermissionGroupTimeZoneRepository;
            _userAccessUnitService = usersAccessUnitService;
            _classificatorValueRepository = classificatorValueRepository;
            _userBuildingService = userBuildingService;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
        }

        #region Tree
        public ActionResult GetTree()
        {
            var ctvm = new CompanyTreeViewModel();
            var role_building_ids = GetUserBuildings(_buildingRepository, _userRepository, CurrentUser.Get().Id);

            ctvm.Countries =
                from country in _countryRepository.FindAll()
                select
                    new Node
                    {
                        ParentId = 0,
                        MyId = country.Id,
                        Name = country.Name
                    };

            ctvm.Towns =
                from town in _locationRepository.FindAll(x => !x.IsDeleted)
                select
                    new Node
                    {
                        ParentId = town.CountryId,
                        MyId = town.Id,
                        Name = town.Name
                    };

            ctvm.Offices =
                from office in _buildingRepository.FindAll(x => !x.IsDeleted && role_building_ids.Contains(x.Id))
                select
                    new Node
                    {
                        ParentId = office.LocationId,
                        MyId = office.Id,
                        Name = office.Name
                    };

            ctvm.Towns = ctvm.Towns.Where(town => ctvm.Offices.Any(off => off.ParentId == town.MyId));
            ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));

            List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && x.ParentId == null && x.Active) select c.Id).ToList();

            if (CurrentUser.Get().IsCompanyManager)
            {
                int? companyId = CurrentUser.Get().CompanyId;
                companyIds = (from c in _companyRepository.FindAll(c => !c.IsDeleted && c.Active && (c.Id == companyId || c.ParentId == companyId)) select c.Id).ToList();
            }

            ctvm.Companies =
                from company in
                    _companyBuildingObjectRepository.FindAll(x => !x.IsDeleted && companyIds.Contains(x.CompanyId))
                select
                    new Node
                    {
                        ParentId = company.BuildingObject.BuildingId,
                        MyId = company.CompanyId,
                        Name = company.Company.Name
                    };

            ctvm.Companies = ctvm.Companies.Distinct(new NodeEqualityComparer());

            ctvm.Partners =
                from partner in
                    _companyRepository.FindAll(x => !x.IsDeleted && x.ParentId != null && companyIds.Contains(x.ParentId.Value))
                select
                    new Node
                    {
                        ParentId = partner.ParentId.Value,
                        MyId = partner.Id,
                        Name = partner.Name
                    };

            ctvm.Floors =
                from floor in
                    _companyBuildingObjectRepository.FindAll(x => !x.IsDeleted && companyIds.Contains(x.CompanyId) && x.BuildingObject.TypeId == 1)
                select
                    new Node
                    {
                        ParentId = floor.CompanyId,
                        MyId = floor.Id,
                        Name = floor.BuildingObject.Description,
                        BuildingId = floor.BuildingObject.BuildingId
                    };
            return PartialView("UserTree", ctvm);
        }

        [HttpGet]
        public ActionResult ByCompany(int id, int buildingId)
        {
            var uvm = CreateViewModel<UserListViewModel>();
            List<User> users = UsersByCompany(id, buildingId);

            users = users.OrderBy(x => x.FirstName).ToList();

            IEnumerable<UserItem> list = new List<UserItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "User";

            uvm.Users = list;
            uvm.FilterCriteria = 1;
            return PartialView("List", uvm);
        }

        private List<User> UsersByCompany(int id, int buildingId)
        {
            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            users = users.Where(x => !x.IsDeleted && x.Active && x.CompanyId.HasValue && x.CompanyId == id &&
                    x.UserBuildings != null && x.UserBuildings.Any(ubo => !ubo.IsDeleted && ubo.BuildingId == buildingId)).
                    ToList();
            users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);
            return users;
        }

        [HttpGet]
        public ActionResult ByFloor(int floorId, int companyId)
        {
            var uvm = CreateViewModel<UserListViewModel>();

            List<User> users = UsersByFloor(floorId, companyId);

            if (!CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin)
            {
                users = users.Where(us => us.CompanyId != null).ToList();
            }
            users = users.Where(us => us.CompanyId != null && us.CompanyId.Value == companyId).ToList();

            users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);

            users = users.OrderBy(x => x.FirstName).ToList();

            IEnumerable<UserItem> list = new List<UserItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "User";
            uvm.Users = list;
            uvm.FilterCriteria = 1;
            return PartialView("List", uvm);
        }

        private List<User> UsersByFloor(int floorId, int companyId)
        {
            var bo = _companyBuildingObjectRepository.FindById(floorId);

            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            users = users.Where(x => !x.IsDeleted && x.Active && x.UserBuildings != null && x.UserBuildings.Any(ubo => !ubo.IsDeleted && ubo.BuildingObjectId == bo.BuildingObjectId)).
                    ToList();

            return users;
        }

        [HttpGet]
        public ActionResult ByPartner(int id, int buildingId)
        {
            var uvm = CreateViewModel<UserListViewModel>();
            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            users = users.Where(x => !x.IsDeleted && x.Active && x.CompanyId.HasValue && x.CompanyId == id &&
                   x.UserBuildings != null && x.UserBuildings.Any(ubo => !ubo.IsDeleted && ubo.BuildingId == buildingId)).
                   ToList();

            users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);
            users = users.OrderBy(x => x.FirstName).ToList();

            IEnumerable<UserItem> list = new List<UserItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "User";
            uvm.Users = list;
            uvm.FilterCriteria = 1;
            return PartialView("List", uvm);
        }

        [HttpGet]
        public ActionResult ByBuilding(int id)
        {
            var uvm = CreateViewModel<UserListViewModel>();

            List<User> users = UsersByBuilding(id);

            if (!CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin)
            {
                users = users.Where(us => us.CompanyId != null).ToList();
            }
            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                users = users.Where(x => x.CompanyId.HasValue && x.CompanyId.Value == CurrentUser.Get().CompanyId.Value).ToList();
            }

            users = users.OrderBy(x => x.FirstName).ToList();

            IEnumerable<UserItem> list = new List<UserItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "User";
            uvm.Users = list;
            uvm.FilterCriteria = 1;
            return PartialView("List", uvm);
        }

        private List<User> UsersByBuilding(int id)
        {
            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            users = users.Where(x => !x.IsDeleted && x.Active
                && x.UserBuildings != null && x.UserBuildings.Any(ubo => !ubo.IsDeleted && ubo.BuildingId == id)).ToList();
            return users;
        }

        [HttpGet]
        public ActionResult ByLocation(int id)
        {
            var uvm = CreateViewModel<UserListViewModel>();
            List<User> users = UsersByLocation(id);

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                users = users.Where(x => x.CompanyId.HasValue && x.CompanyId.Value == CurrentUser.Get().CompanyId.Value).ToList();
            }

            users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);
            users = users.OrderBy(x => x.FirstName).ToList();
            IEnumerable<UserItem> list = new List<UserItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "User";
            uvm.Users = list;
            uvm.FilterCriteria = 1;
            return PartialView("List", uvm);
        }

        private List<User> UsersByLocation(int id)
        {
            List<int> activeCompanyIds =
               (from c in _companyRepository.FindAll(x => !x.IsDeleted && x.Active) select c.Id).ToList();
            List<int> buildingIds =
                (from b in _buildingRepository.FindAll(x => !x.IsDeleted && x.LocationId == id) select b.Id).ToList();
            List<int> companyIds =
                (from b in
                     _companyBuildingObjectRepository.FindAll(
                         x => !x.IsDeleted && buildingIds.Contains(x.BuildingObject.BuildingId))
                 select b.CompanyId).ToList();
            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            users = users.Where(
                    x =>
                    !x.IsDeleted && x.Active && x.CompanyId.HasValue && companyIds.Contains((int)x.CompanyId) &&
                    activeCompanyIds.Contains((int)x.CompanyId)).ToList();
            return users;
        }

        [HttpGet]
        public ActionResult ByCountry(int id)
        {
            var uvm = CreateViewModel<UserListViewModel>();
            List<User> users = UsersByCountry(id);

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                users = users.Where(x => x.CompanyId.HasValue && x.CompanyId.Value == CurrentUser.Get().CompanyId.Value).ToList();
            }

            users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);

            users = users.OrderBy(x => x.FirstName).ToList();

            IEnumerable<UserItem> list = new List<UserItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "User";
            uvm.Users = list;
            uvm.FilterCriteria = 1;
            return PartialView("List", uvm);
        }

        private List<User> UsersByCountry(int id)
        {
            List<int> activeCompanyIds =
                (from c in _companyRepository.FindAll(x => !x.IsDeleted && x.Active) select c.Id).ToList();
            List<int> locationIds =
                (from l in _locationRepository.FindAll(x => !x.IsDeleted && x.CountryId == id) select l.Id).ToList();
            List<int> buildingIds =
                (from b in _buildingRepository.FindAll(x => !x.IsDeleted && locationIds.Contains(x.LocationId))
                 select b.Id).ToList();
            List<int> companyIds =
                (from b in
                     _companyBuildingObjectRepository.FindAll(x => !x.IsDeleted && buildingIds.Contains(x.BuildingObject.BuildingId))
                 select b.CompanyId).ToList();
            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            users = users.Where(
                    x =>
                    !x.IsDeleted && x.Active && x.CompanyId.HasValue && companyIds.Contains((int)x.CompanyId) &&
                    activeCompanyIds.Contains((int)x.CompanyId)).ToList();
            return users;
        }

        #endregion

        #region Building Object
        public List<SelectListItem> GetBuildings(int userId)
        {
            var result = new List<SelectListItem>();
            var user = new UserItem();
            Mapper.Map(_userRepository.FindById(userId), user);
            var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
            /*if( !user.IsSuperAdmin)
			{
				var user_role = user.UserRoles.Where(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).FirstOrDefault();
				if( user_role == null )
				{
					return result;
				}
				var user_role_building_ids = from b in user_role.Role.RoleBuildings.Where(rb => !rb.IsDeleted) select b.BuildingId;

				role_building_ids = role_building_ids.Where(x => user_role_building_ids.Contains(x)).ToList();
			}

            */
            if (!user.CompanyId.HasValue && !user.IsBuildingAdmin && !user.IsSuperAdmin)
            {
                return result;
            }
            if (user.CompanyId.HasValue)
            {
                var company = _companyRepository.FindById(user.CompanyId.Value);
                if (company.ParentId != null)
                {
                    company = _companyRepository.FindById(company.ParentId.Value);
                }

                List<int> companyBuildingObjectsIds =
                    (from cbo in company.CompanyBuildingObjects.Where(x => !x.IsDeleted) select cbo.BuildingObjectId).ToList();

                var companyBuildingObjects =
                    _buildingObjectRepository.FindAll(x => !x.IsDeleted && x.TypeId == 1 && companyBuildingObjectsIds.Contains(x.Id) && role_building_ids.Contains(x.BuildingId));

                foreach (var cbo in companyBuildingObjects.Where(cbo => !result.Any(rr => rr.Value == cbo.BuildingId.ToString())))
                {
                    result.Add(new SelectListItem() { Value = cbo.BuildingId.ToString(), Text = cbo.Building.Name });
                }
            }
            else
            {
                if (CurrentUser.Get().IsSuperAdmin && (user.IsBuildingAdmin || user.IsSuperAdmin))
                {
                    var buildingObjects =
                        _buildingObjectRepository.FindAll(x => !x.IsDeleted && x.TypeId == 1 && role_building_ids.Contains(x.BuildingId));

                    foreach (var cbo in buildingObjects.Where(cbo => !result.Any(rr => rr.Value == cbo.BuildingId.ToString())))
                    {
                        result.Add(new SelectListItem() { Value = cbo.BuildingId.ToString(), Text = cbo.Building.Name });
                    }
                }
                else
                {
                    if (CurrentUser.Get().IsBuildingAdmin && user.IsBuildingAdmin)
                    {
                        var userBuildingObjectIds = (from ubo in _userBuildingRepository.FindByUserId(CurrentUser.Get().Id) select ubo.BuildingId).ToList();

                        var buildingObjects =
                        _buildingObjectRepository.FindAll(x => !x.IsDeleted && x.TypeId == 1 && role_building_ids.Contains(x.BuildingId) && userBuildingObjectIds.Contains(x.BuildingId));

                        foreach (var cbo in buildingObjects.Where(cbo => !result.Any(rr => rr.Value == cbo.BuildingId.ToString())))
                        {
                            result.Add(new SelectListItem() { Value = cbo.BuildingId.ToString(), Text = cbo.Building.Name });
                        }
                    }
                }
            }
            return result;
        }

        public List<SelectListItem> GetFloors(int userId, int? buildingId)
        {
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Value = string.Empty, Text = ViewResources.SharedStrings.DefaultDropDownValueShort });
            if (!buildingId.HasValue)
            {
                return result;
            }

            var user = new UserItem();
            Mapper.Map(_userRepository.FindById(userId), user);
            if (!user.CompanyId.HasValue && !user.IsBuildingAdmin)
            {
                return result;
            }

            var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);

            if (user.CompanyId.HasValue)
            {
                var company = _companyRepository.FindById(user.CompanyId.Value);

                if (company.ParentId != null)
                {
                    company = _companyRepository.FindById(company.ParentId.Value);
                }
                List<int> companyBuildingObjectsIds =
                    (from cbo in company.CompanyBuildingObjects.Where(x => !x.IsDeleted) select cbo.BuildingObjectId).ToList();

                var companyBuildingObjects =
                    _buildingObjectRepository.FindAll(
                        x => !x.IsDeleted && x.BuildingId == buildingId && x.TypeId == 1
                            && companyBuildingObjectsIds.Contains(x.Id) && role_building_ids.Contains(x.BuildingId));

                foreach (var cbo in companyBuildingObjects.Where(cbo => !result.Any(rr => rr.Value == cbo.Id.ToString())))
                {
                    result.Add(new SelectListItem() { Value = cbo.Id.ToString(), Text = cbo.Description });
                }
            }
            else
            {
                var buildingObjects = _buildingObjectRepository.FindAll().Where(x => !x.IsDeleted && x.TypeId == 1 && role_building_ids.Contains(x.BuildingId) && x.BuildingId == buildingId);
                foreach (var buildingObject in buildingObjects.Where(cbo => !result.Any(rr => rr.Value == cbo.Id.ToString())))
                {
                    result.Add(new SelectListItem() { Value = buildingObject.Id.ToString(), Text = buildingObject.Description });
                }
            }

            return result;
        }

        public JsonResult GetFloorsByBuilding(int userId, int? buildingId)
        {
            StringBuilder result = new StringBuilder();
            var floors = GetFloors(userId, buildingId);

            foreach (var floor in floors)
            {
                result.Append("<option value=" + '"' + floor.Value + '"' + ">" + floor.Text + "</option>");
            }
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserData(int userId)
        {
            var userItem = new UserItem();
            Mapper.Map(_userRepository.FindById(userId), userItem);
            return Json(new
            {
                Id = userItem.Id,
                Name = string.Format("{0} {1}",
                userItem.FirstName, userItem.LastName),
                userItem.CardNumber,
                userItem.CompanyName,
                userItem.DepartmentName,
                userItem.TitleName,
                userItem.ValidToStr,
                userItem.Roles,
                CurrentUser.Get().IsDepartmentManager,
                CurrentUser.Get().IsCompanyManager,
                CurrentUser.Get().IsBuildingAdmin,
                CurrentUser.Get().IsSuperAdmin
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region User Permission Tree

        public ActionResult GetUserPermissionTree(int userId, int? id)
        {
            var ctvm = CreateViewModel<PermissionTreeViewModel>();
            var role_buildingIds = GetUserBuildings(_buildingRepository, _userRepository, CurrentUser.Get().Id);

            if (id.HasValue)
            {
                var upg = _userPermissionGroupRepository.FindById(id.Value);
                var cur_upg = _userPermissionGroupRepository.FindById(id.Value);
                /*
                if (CurrentUser.Get().IsCompanyManager)
                {
                    if (upg.UserId == userId)
                    {
                        upg = _userPermissionGroupRepository.FindById(upg.ParentUserPermissionGroupId.Value);
                        if( upg.UserId == CurrentUser.Get().Id)
                        {
                            ctvm.IsOriginal = false;
                            ctvm.IsCurrentUserAssignedGroup = upg.PermissionIsActive;
                        }
                    }
                    else
                    {*/
                ctvm.IsOriginal = false;
                ctvm.IsCurrentUserAssignedGroup = upg.PermissionIsActive;
                ctvm.IsCurrentUserAssignedGroup = false;
                /*
                    }
                }
                */
                List<int> buildingObjectsIds = new List<int>();
                buildingObjectsIds = _userRepository.IsCompanyManager(userId) ? (from cbo in upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted /*&& x.Active*/) select cbo.BuildingObjectId).ToList() : (from cbo in upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted) select cbo.BuildingObjectId).ToList();
                var buildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && /*role_buildingIds.Contains(x.BuildingId) &&*/ buildingObjectsIds.Contains(x.Id));
                if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager)
                {
                    var activePg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => x.PermissionIsActive && !x.IsDeleted).FirstOrDefault();
                    var userActiveBuildings = (from cbo in activePg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.Active) select cbo.BuildingObjectId).ToList();
                    buildingObjects =
                        _buildingObjectRepository.FindAll(x => !x.IsDeleted && role_buildingIds.Contains(x.BuildingId) || userActiveBuildings.Contains(x.Id));
                }

                if (CurrentUser.Get().IsCompanyManager)
                {
                    var curr_upg_buildingIds =
                        (from cbo in cur_upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.Active)
                         select cbo.BuildingObjectId).ToList();

                    var activeUpg =
                        _userPermissionGroupRepository.FindByUserId(CurrentUser.Get().Id).Where(
                            x => x.PermissionIsActive && !x.IsDeleted).
                            FirstOrDefault();

                    var activeUpgBuildingIds = activeUpg != null
                                                   ? (from cbo in
                                                          activeUpg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.Active)
                                                      select cbo.BuildingObjectId).ToList()
                                                   : null;

                    if (activeUpgBuildingIds != null)
                    {
                        foreach (var activeUpgBuildingId in activeUpgBuildingIds)
                        {
                            if (!curr_upg_buildingIds.Contains(activeUpgBuildingId))
                            {
                                curr_upg_buildingIds.Add(activeUpgBuildingId);
                            }
                        }
                    }
                    var activePg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => x.PermissionIsActive && !x.IsDeleted).FirstOrDefault();
                    var userActiveBuildings = (from cbo in activePg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.Active) select cbo.BuildingObjectId).ToList();
                    buildingObjects = _buildingObjectRepository.FindAll(x => curr_upg_buildingIds.Contains(x.Id) && role_buildingIds.Contains(x.BuildingId) || userActiveBuildings.Contains(x.Id));
                }

                List<int> floorsId = (from cbo in buildingObjects.Where(x => x.ParentObjectId.HasValue) select cbo.ParentObjectId.Value).ToList();
                var companyFloorObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && floorsId.Contains(x.Id));

                // start to make ctvm and html
                ctvm.Buildings =
                    (from b in
                         (from cbo in buildingObjects select cbo.Building)
                     where !b.IsDeleted
                     select
                         new Node
                         {
                             ParentId = b.LocationId,
                             MyId = b.Id,
                             Name = b.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Towns =
                    (from t in
                         (from cbo in buildingObjects select cbo.Building.Location)
                     where !t.IsDeleted
                     select
                         new Node
                         {
                             ParentId = t.CountryId,
                             MyId = t.Id,
                             Name = t.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Countries =
                    (from c in
                         (from cbo in buildingObjects select cbo.Building.Location.Country).OrderBy(cc => cc.Name)
                     where !c.IsDeleted
                     select
                         new Node
                         {
                             ParentId = 0,
                             MyId = c.Id,
                             Name = c.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Floors =
                    (from f in companyFloorObjects
                     where f.TypeId == (int)BuildingObjectTypes.Floor
                     select
                         new Node
                         {
                             ParentId = f.BuildingId,
                             MyId = f.Id,
                             Name = f.Description,
                             Comment = f.Comment
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Objects =
                    (from o in buildingObjects
                     where o.ParentObjectId.HasValue && (o.TypeId == (int)BuildingObjectTypes.Door || o.TypeId == (int)BuildingObjectTypes.Lift || o.TypeId == (int)BuildingObjectTypes.Room)
                     orderby o.TypeId descending
                     select
                         new Node
                         {
                             ParentId = o.ParentObjectId.Value,
                             MyId = o.Id,
                             Name = o.ObjectNr.HasValue ? "#" + o.ObjectNr + " " + o.Description : o.Description,
                             Comment = o.Comment,
                             IsDefaultTimeZone = _userPermissionGroupService.IsDefaultUserTimeZone(o.Id, id.Value),
                             IsRoom = o.TypeId == (int)BuildingObjectTypes.Room ? 1 : 0,
                             StatusIcon = o.StatusIconId.HasValue ? String.Format("../../img/status/{0}.ico", o.StatusIconId) : String.Empty
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.ActiveObjectIds = _userPermissionGroupService.GetUserBuildingObjectIds(id.Value);
                foreach (var obn in ctvm.Objects)
                {
                    if (obn.IsRoom == 1)
                    {
                        //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();
                        UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(id.Value).Where(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();
                        obn.IsArming = upgtz == null ? true : upgtz.IsArming;
                        obn.IsDefaultArming = upgtz == null ? true : upgtz.IsDefaultArming;
                        obn.IsDisarming = upgtz == null ? true : upgtz.IsDisarming;
                        obn.IsDefaultDisarming = upgtz == null ? true : upgtz.IsDefaultDisarming;
                    }
                }

                ctvm.Towns = ctvm.Towns.Where(town => ctvm.Buildings.Any(b => b.ParentId == town.MyId));
                ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));
            }
            // return view
            return PartialView("PermTree", ctvm);
            //return PartialView("Tree", ctvm);
        }

        #endregion

        #region User Permission

        public ActionResult SearchPerm(string name, string start)
        {
            var tzlvm = CreateViewModel<TimeZoneListViewModel>();
            List<UserTimeZone> zones = _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id).ToList();

            if (name != string.Empty)
            {
                zones = zones.Where(x => x.Name.ToLower().Contains(name.Trim().ToLower())).ToList();
            }

            if (start != string.Empty)
            {
                try
                {
                    DateTime fromTime = DateTime.ParseExact(start.Trim(), "HH:mm", CultureInfo.InvariantCulture);
                    zones = zones.Where(x => x.UserTimeZoneProperties.Any(y => y.ValidFrom.HasValue && y.ValidFrom.Value.TimeOfDay == fromTime.TimeOfDay)).ToList();
                    tzlvm.SearchStartTime = start;
                }
                catch { }
            }

            Mapper.Map(zones, tzlvm.TimeZones);
            return PartialView("PermList", tzlvm);
        }

        public JsonResult GetPermissionGroups(int? id)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<option value=" + '"' + '"' + ">" + SharedStrings.PermissionsSelectPermissionGroupOption + "</option>");

            IEnumerable<UserPermissionGroup> permGroups;
            if (!CurrentUser.Get().IsSuperAdmin && !CurrentUser.Get().IsBuildingAdmin)
            {//Tut

                string OwnPermissionGroup = (from c in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null) select c.Name).First();
                var SamePermissionGroupUsers = (from c in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId != null && x.Name == OwnPermissionGroup) select c.UserId).ToArray();
                //permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null && SamePermissionGroupUsers.Contains(x.UserId));
                //illi 25.12.1012 v16 sort
                permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null && SamePermissionGroupUsers.Contains(x.UserId) || !x.IsDeleted && x.UserId == CurrentUser.Get().Id).OrderBy(x => x.Name);

                //permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id).OrderBy(x => x.Name);
                //original
            }
            else
            {
                //illi 25.12.1012 v16 sort
                permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null).OrderBy(x => x.Name);
            }

            var curPermGroup = id.HasValue ? _userPermissionGroupRepository.FindAll().Where(upg => !upg.IsDeleted && upg.UserId == id && upg.PermissionIsActive).FirstOrDefault() : null;

            foreach (var pg in permGroups)
            {
                result.Append(string.Format("<option value=\"{0}\">{1}</option>", pg.ParentUserPermissionGroupId.HasValue ? pg.ParentUserPermissionGroupId : pg.Id, pg.Name));
            }

            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetUserDefaultZone(int id)
        {
            var tzlvm = CreateViewModel<TimeZoneListViewModel>();
            if (CurrentUser.Get().IsCompanyManager)
            {
                var upg = _userPermissionGroupRepository.FindById(id);
                //added
                tzlvm.IsModelReadOnly = upg.PermissionIsActive;
                //Commented
                /*if (upg.UserId == CurrentUser.Get().Id)
                {
                    tzlvm.IsModelReadOnly = upg.PermissionIsActive;
                }
                else
                {
                    upg = _userPermissionGroupRepository.FindById(upg.ParentUserPermissionGroupId.Value);
                    if( upg.UserId == CurrentUser.Get().Id)
                    {
                        tzlvm.IsModelReadOnly = upg.PermissionIsActive; 
                    }
                }*/
            }

            var zones = new List<UserTimeZone>
                            {_userTimeZoneRepository.FindById(_userPermissionGroupService.GetUserDefaultTimeZoneId(id))};

            Mapper.Map(zones, tzlvm.TimeZones);
            return PartialView("PermList", tzlvm);
        }

        public ActionResult GetUserActiveZone(int id, int objectId)
        {
            var tzlvm = CreateViewModel<TimeZoneListViewModel>();
            var zones = new List<UserTimeZone>
                            {
                                _userTimeZoneRepository.FindById(
                                    _userPermissionGroupService.GetUserActiveTimeZoneIdByBuildingObjectId(id, objectId))
                            };

            Mapper.Map(zones, tzlvm.TimeZones);
            return PartialView("PermList", tzlvm);
        }

        public JsonResult CreateUserPermissionGroup(int userId, int pgId)
        {
            return Json(_userPermissionGroupService.AssignUserPermissionGroup(userId, pgId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeUserPermissionGroup(int userId, int newUpgId)
        {
            return Json(_userPermissionGroupService.ChangeUserPermissionGroup(userId, newUpgId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddUserPermissionGroup(int userId, int newUpgId)
        {
            return Json(_userPermissionGroupService.AddUserPermissionGroup(userId, newUpgId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelUserPermissionGroup(int userId, int newUpgId)
        {
            var pg = _userPermissionGroupRepository.FindByUserId(userId);
            if (pg.First().ParentUserPermissionGroupId == newUpgId)
            {
                return null;
            }
            return Json(_userPermissionGroupService.DelUserPermissionGroup(userId, newUpgId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUserPermissionGroup(int upgId, List<int> boIds, List<int> sboIds)
        {
            _userPermissionGroupService.SaveUserPermissionGroup(upgId, boIds, sboIds);
            _userPermissionGroupService.GroupSaveUserPermissionGroup(upgId, boIds, sboIds, sboIds);
            return null;
        }

        public ActionResult SetBuildingObjectTimeZone(int id, int objectId, int zoneId)
        {
            _userPermissionGroupService.ChangeUserPermissionGroupBuildingTimeZone(id, objectId, zoneId);
            _userPermissionGroupService.GroupChangeUserPermissionGroupBuildingTimeZone(id, objectId, zoneId);
            return null;
        }

        public ActionResult ResetBuildingObjectTimeZone(int id, int objectId)
        {
            _userPermissionGroupService.ChangeUserPermissionGroupBuildingTimeZoneToDefault(id, objectId);
            _userPermissionGroupService.GroupChangeUserPermissionGroupBuildingTimeZoneToDefault(id, objectId);
            return null;
        }

        [ValidateInput(false)]
        [ChildActionOnly]
        public ActionResult PermZone(int timeZoneId, int colNr)
        {
            var tzpvm = CreateViewModel<TimeZonePropertiesViewModel>();
            Mapper.Map(_userTimeZonePropertyRepository.FindAll(x => x.UserTimeZoneId == timeZoneId && x.OrderInGroup == colNr).FirstOrDefault(), tzpvm);
            return PartialView(tzpvm);
        }

        public JsonResult GetPermName(int id)
        {
            var result = new List<string>();
            var upg = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == id && x.PermissionIsActive == true);

            if (upg.Count() == 0)
            {
                result.Add("0");
                result.Add("- none -");
                result.Add("0");
            }
            else
            {
                result.Add(upg.FirstOrDefault().Id.ToString());
                string name = upg.FirstOrDefault().Name + " ( " + _userTimeZoneRepository.FindById(upg.FirstOrDefault().DefaultUserTimeZoneId).Name + " ) ";
                result.Add(name);
                result.Add(upg.FirstOrDefault().ParentUserPermissionGroupId.ToString());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ToggleArming(int boId, int pgId)
        {
            _userPermissionGroupService.ToggleUserArming(boId, pgId);
            _userPermissionGroupService.GroupToggleUserArming(boId, pgId);
            return null;
        }

        [HttpPost]
        public ActionResult ToggleDefaultArming(int boId, int pgId)
        {
            _userPermissionGroupService.ToggleUserDefaultArming(boId, pgId);
            _userPermissionGroupService.GroupToggleUserDefaultArming(boId, pgId);
            return null;
        }

        [HttpPost]
        public ActionResult ToggleDisarming(int boId, int pgId)
        {
            _userPermissionGroupService.ToggleUserDisarming(boId, pgId);
            _userPermissionGroupService.GroupToggleUserDisarming(boId, pgId);
            return null;
        }

        [HttpPost]
        public ActionResult ToggleDefaultDisarming(int boId, int pgId)
        {
            _userPermissionGroupService.ToggleUserDefaultDisarming(boId, pgId);
            _userPermissionGroupService.GroupToggleUserDefaultDisarming(boId, pgId);
            return null;
        }
        #endregion

    }
}