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
using System.Data;
using FoxSec.Web.ListModel;
using FoxSec.Accounts;


namespace FoxSec.Web.Controllers
{
    public class NewUserController : PaginatorControllerBase<UserItem>
    {
        FoxSecDBContext db = new FoxSecDBContext();
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

        public NewUserController(IUserService userService,
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
        // GET: NewUser
        public ActionResult TabContentNew()
        {
            // GetFiles();
            var hmv = CreateViewModel<HomeViewModel>();
            hmv.HRService = _FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).Any();
            return PartialView(hmv);
        }

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
            return PartialView("NewUserTree", ctvm);
        }
    }
}