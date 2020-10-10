using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.Services;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FoxSec.Web.Controllers
{
    public class CompanyController : PaginatorControllerBase<Company>
    {
        private readonly ICompanyManagerService _companyManagerService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyManagerRepository _companyManagerRepository;
        private readonly ICompanyService _companyService;
        private readonly ICountryRepository _countryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICompanyBuildingObjectRepository _companyBuildingObjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IUsersAccessUnitService _userAccessUnitService;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly IUserBuildingRepository _userBuildingRepository;

        public CompanyController(ICompanyService companyService,
                                 ICompanyManagerService companyManagerService,
                                 ICompanyRepository companyRepository,
                                 ICompanyManagerRepository companyManagerRepository,
                                 ICurrentUser currentUser,
                                 IRoleRepository roleRepository,
                                 ICountryRepository countryRepositorty,
                                 ILocationRepository locationRepository,
                                 IBuildingRepository buildingRepository,
                                 ICompanyBuildingObjectRepository companyBuildingObjectRepository,
                                 IUserRepository userRepository,
                                 IUserService userService,
                                 IUsersAccessUnitService usersAccessUnitService,
                                 IClassificatorValueRepository classificatorValueRepository,
                                 IBuildingObjectRepository buildingObjectRepository,
                                 IUserBuildingRepository userBuildingRepository,
                                 ILogger logger) : base(currentUser, logger)
        {
            _companyRepository = companyRepository;
            _companyManagerRepository = companyManagerRepository;
            _companyService = companyService;
            _companyManagerService = companyManagerService;
            _locationRepository = locationRepository;
            _countryRepository = countryRepositorty;
            _buildingRepository = buildingRepository;
            _companyBuildingObjectRepository = companyBuildingObjectRepository;
            _userRepository = userRepository;
            _userService = userService;
            _userAccessUnitService = usersAccessUnitService;
            _classificatorValueRepository = classificatorValueRepository;
            _roleRepository = roleRepository;
            _buildingObjectRepository = buildingObjectRepository;
            _userBuildingRepository = userBuildingRepository;
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        #region Search

        private IEnumerable<Company> ApplyCompanyStatusFilter(IEnumerable<Company> companies, int filter)
        {
            if (filter == 1)
            {
                companies = companies.Where(x => x.Active).ToList();
            }
            else if (filter == 0)
            {
                companies = companies.Where(x => !x.Active).ToList();
            }
            return companies;
        }

        public ActionResult List(string name, string building, string floor, string info, int filter, int? nav_page, int? rows, int? sort_field, int? sort_direction, int countryId, int locationId, int buildingId)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            var clvm = CreateViewModel<CompanyListViewModel>();
            List<Company> companies = new List<Company>();
            //List<Company> companies = _companyRepository.FindAll(x => !x.IsDeleted && x.ParentId == null).ToList();
            if (CurrentUser.Get().IsCompanyManager)
            {

                int? cmpid = CurrentUser.Get().CompanyId;
                if (cmpid != null)
                {
                    companies = _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmpid || x.ParentId == cmpid)).ToList();
                }
            }
            else
            {
                //companies = _companyRepository.FindAll(x => !x.IsDeleted && x.ParentId == null).ToList();
                companies = _companyRepository.FindAll(x => !x.IsDeleted).ToList();
            }
            if (countryId != 0)
            {
                var country_companies = CompaniesByCountry(countryId);
                companies = companies.Where(x => country_companies.Any(cc => cc.Id == x.Id)).ToList();
            }

            if (locationId != 0)
            {
                var location_companies = CompaniesByLocation(locationId);
                companies = companies.Where(x => location_companies.Any(cc => cc.Id == x.Id)).ToList();
            }

            if (buildingId != 0)
            {
                IEnumerable<Company> building_companies =
                _companyRepository.FindAll(
                    x =>
                    x.Active && !x.IsDeleted && x.ParentId == null &&
                    x.CompanyBuildingObjects.Any(y => !y.IsDeleted && y.BuildingObject.BuildingId == buildingId)).ToList();

                companies = companies.Where(x => building_companies.Any(cc => cc.Id == x.Id)).ToList();
            }

            var role_building_ids = GetUserBuildings(_buildingRepository, _userRepository);

            companies =
                companies.Where(
                    x =>
                    x.CompanyBuildingObjects.Where(cbo => !cbo.IsDeleted).Count() == 0 ||
                    x.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && role_building_ids.Contains(cbo.BuildingObject.BuildingId))).ToList();

            companies = ApplyCompanyStatusFilter(companies, filter).ToList();

            if (name != String.Empty)
                companies = companies.Where(x => x.Name.ToLower().Contains(name.Trim().ToLower())).ToList();

            if (building != String.Empty)
                companies =
                    companies.Where(
                        x =>
                        x.CompanyBuildingObjects.Any(
                            y =>
                            !y.IsDeleted && y.BuildingObject.Building.Name.ToLower().Contains(building.Trim().ToLower())))
                        .ToList();

            if (floor != String.Empty)
                companies =
                    companies.Where(
                        x =>
                        x.CompanyBuildingObjects.Any(
                            y => !y.IsDeleted && y.BuildingObject.Description.ToLower().Contains(floor.Trim().ToLower())))
                        .ToList();

            if (info != String.Empty)
                companies = companies.Where(x => (x.Comment ?? string.Empty).Trim().ToLower().Contains((info ?? string.Empty).Trim().ToLower())).ToList();
            //fi => (fi.DESCRIPTION ?? string.Empty).ToLower().Contains((description ?? string.Empty).ToLower())

            companies.ForEach(delegate (Company c) { List<String> e = GetExtraInfo(c); c.BuidingNames = e[0]; c.Floors = e[1]; });
            IEnumerable<Company> cmp = null;
            if (sort_field.HasValue && sort_direction.HasValue)
            {
                if (sort_field.Value == 0) cmp = sort_direction == 0 ? companies.OrderBy(x => x.Name) : companies.OrderByDescending(x => x.Name);
                else if (sort_field.Value == 1) cmp = sort_direction == 0 ? companies.OrderBy(x => x.BuidingNames) : companies.OrderByDescending(x => x.BuidingNames);
                else if (sort_field.Value == 2) cmp = sort_direction == 0 ? companies.OrderBy(x => x.Floors) : companies.OrderByDescending(x => x.Floors);
                else cmp = sort_direction == 0 ? companies.OrderBy(x => x.Comment) : companies.OrderByDescending(x => x.Comment);
            }

            clvm.Paginator = SetupPaginator(ref cmp, nav_page, rows);
            clvm.Paginator.DivToRefresh = "CompaniesList";
            clvm.Paginator.Prefix = "Companies";
            Mapper.Map(cmp, clvm.Companies);
            clvm.FilterCriteria = filter;

            foreach (CompanyItem c in clvm.Companies)
            {
                string bNames = string.Join(", ", (from bn in c.CompanyBuildingObjects
                                                   where bn.IsDeleted == false
                                                   select bn.BuildingObject.Building.Name).GroupBy(y => y).Select(x => x.Key).ToArray());

                c.BuidingNames = bNames.Length == 0 ? "-" : bNames;
                var cbos =
                    c.CompanyBuildingObjects.Where(x => !x.IsDeleted && x.BuildingObject.TypeId == 1).GroupBy(
                        cbo => cbo.BuildingObject.BuildingId);
                var resFloors = new List<string>();
                foreach (var cbo in cbos)
                {
                    string floors1 = string.Join(", ", (from fn in cbo
                                                        where fn.IsDeleted == false && fn.BuildingObject.TypeId == 1
                                                        select fn.BuildingObject.Description).GroupBy(y => y).Select(x => x.Key).ToArray());
                    resFloors.Add(floors1);
                }
                string floors = string.Join(";", resFloors);
                c.Floors = floors.Length == 0 ? "-" : floors;
            }
            return PartialView(clvm);
        }

        private List<String> GetExtraInfo(Company c)
        {
            List<String> result = new List<String>();
            string bNames = string.Join(", ", (from bn in c.CompanyBuildingObjects
                                               where bn.IsDeleted == false
                                               select bn.BuildingObject.Building.Name).GroupBy(y => y).Select(x => x.Key).ToArray());

            result.Add(bNames.Length == 0 ? "-" : bNames);
            var cbos =
                c.CompanyBuildingObjects.Where(x => !x.IsDeleted && x.BuildingObject.TypeId == 1).GroupBy(
                    cbo => cbo.BuildingObject.BuildingId);

            var resFloors = new List<string>();
            foreach (var cbo in cbos)
            {
                string floors1 = string.Join(", ", (from fn in cbo
                                                    where fn.IsDeleted == false && fn.BuildingObject.TypeId == 1
                                                    select fn.BuildingObject.Description).GroupBy(y => y).Select(x => x.Key).ToArray());
                resFloors.Add(floors1);
            }
            string floors = string.Join(";", resFloors);
            result.Add(floors.Length == 0 ? "-" : floors);
            return result;
        }

        #endregion

        #region Create

        [HttpGet]
        public ActionResult Create()
        {
            var rvm = CreateViewModel<CompanyEditViewModel>();
            var building_ids = GetUserBuildings(_buildingRepository, _userRepository);
            var buildings = _buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id));
            Mapper.Map(buildings, rvm.BuildingItems);
            rvm.Company.CompanyBuildingItems.Add(new CompanyBuildingItem() { BuildingItems = rvm.BuildingItems, IsAvailable = true });
            var complist = _companyRepository.FindAll().Where(x => x.IsDeleted == false && x.Active == true && x.ParentId == null).OrderBy(y => y.Name).ToList();
            rvm.CompanyItems = complist;
            return PartialView("Create", rvm);
        }

        public CompanyBuildingItem GetFloorItems(int? buildingId, int? companyId)
        {
            var building = _buildingRepository.FindById(buildingId.Value);
            var building_ids = GetUserBuildings(_buildingRepository, _userRepository);
            var buildingObjects =
                _buildingObjectRepository.FindAll().Where(x => !x.IsDeleted && x.BuildingId == buildingId && x.TypeId == 1);

            var result = new CompanyBuildingItem()
            {
                BuildingId = buildingId,
                IsAvailable = building_ids.Contains(buildingId.Value),
                BuildingName = building.Name
            };

            var floors = new List<CompanyFloorItem>();
            var companyBuildingObjects = companyId == null
                                             ? null
                                             : _companyBuildingObjectRepository.FindAll().Where(
                                                 x => x.CompanyId == companyId.Value && x.BuildingObject.BuildingId == buildingId.Value);

            foreach (var buildingObject in buildingObjects)
            {
                var item = new CompanyFloorItem();
                item.CompanyId = companyId;
                item.BuildingObjectId = buildingObject.Id;
                item.IsAvailable = building_ids.Contains(buildingObject.BuildingId);
                var cbo = companyBuildingObjects == null ? null : companyBuildingObjects.Where(x => x.BuildingObjectId == buildingObject.Id).FirstOrDefault();
                if (cbo == null)
                {
                    item.IsSelected = false;
                    item.Id = null;
                }
                else
                {
                    item.IsSelected = !cbo.IsDeleted;
                    item.Id = cbo.Id;
                }
                floors.Add(item);
            }
            result.CompanyFloors = floors;
            return result;
        }

        public ActionResult GetBuildingFloors(int? buildingId, int? companyId)
        {
            var model = GetFloorItems(buildingId, companyId);
            return PartialView("Floors", model);
        }

        public ActionResult FakeBuilding()
        {
            var building_ids = GetUserBuildings(_buildingRepository, _userRepository);
            var buildings = _buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id));
            var buildingItem = new CompanyBuildingItem();
            buildingItem.IsAvailable = true;
            Mapper.Map(buildings, buildingItem.BuildingItems);
            return PartialView("Building", buildingItem);
        }

        public ActionResult Buildings()
        {
            var bvm = CreateViewModel<CompanyBuildingViewModel>();
            bvm.Id = 1;
            if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsCompanyManager)
            {
                var buildIds = GetRoleBuildings(_buildingRepository, _roleRepository);
                bvm.Buildings = new SelectList(_buildingRepository.FindAll(x => !x.IsDeleted && buildIds.Contains(x.Id)), "Id", "Name");
            }
            else
            {
                bvm.Buildings = new SelectList(_buildingRepository.FindAll(x => !x.IsDeleted), "Id", "Name");
            }
            return PartialView("Building", bvm);
        }

        public ActionResult NextBuilding(int Id)
        {
            var bvm = CreateViewModel<CompanyBuildingViewModel>();
            bvm.Id = Id;
            if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsCompanyManager)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Select(ub => ub.BuildingId).ToList();
                bvm.Buildings = new SelectList(_buildingRepository.FindAll(x => !x.IsDeleted && buildIds.Contains(x.Id)), "Id", "Name");
            }
            else
            {
                bvm.Buildings = new SelectList(_buildingRepository.FindAll(x => !x.IsDeleted), "Id", "Name");
            }
            return PartialView("Building", bvm);
        }

        public ActionResult Floors(int Id)
        {
            var fvm = CreateViewModel<CompanyFloorViewModel>();
            fvm.FloorsCount = _buildingRepository.FindAll(x => x.Id == Id && !x.IsDeleted).FirstOrDefault().Floors;
            fvm.BuildingId = Id;
            return PartialView("Floors", fvm);
        }

        [HttpPost]
        public ActionResult Add(CompanyEditViewModel cevm)
        {
            int? tc = 0;
            int id1 = 0;
            int tc1 = 0;
            DateTime? validto = null;

            IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue("Companies");
            foreach (var obj in cv)
            {
                tc = obj.Legal;
                id1 = obj.Id;
                validto = obj.ValidTo;
            }
            if (validto == null && tc == null)
            {
                return Json(new
                {
                    IsSucceed = false,
                    Msg = "licence error",
                    Count = 0
                });
            }
            else
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(distinct CompanyId) from users where IsDeleted=0 and CompanyId is not null and id in (select UserId from uSERROLES where IsDeleted=0 and RoleId in (select id from Roles where IsDeleted=0 and RoleTypeId=3))", con);
                tc1 = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                int remaining = Convert.ToInt32(tc) - tc1;
                remaining = remaining < 0 ? 0 : remaining;
                if (remaining > 0 && validto > DateTime.Now)
                {
                    int id = 0;
                    string err = string.Empty;
                    if (cevm.Company.CompanyBuildingItems.Any(x => x.BuildingId == null))
                    {
                        ModelState.AddModelError("Company.CompanyBuildingItems", ViewResources.SharedStrings.CompaniesEmptyBuildingSelected);
                    }
                    if (cevm.Company.CompanyBuildingItems.Any(x => x.BuildingId != null && !x.CompanyFloors.Any(cf => cf.IsSelected)))
                    {
                        ModelState.AddModelError("Company.CompanyBuildingItems", ViewResources.SharedStrings.CompaniesNoBuildingSelectedMessage);
                    }

                    var build_ids = GetUserBuildings(_buildingRepository, _userRepository);
                    if (build_ids.Any(x => cevm.Company.CompanyBuildingItems.Where(cb => cb.BuildingId == x).Count() > 1))
                    {
                        ModelState.AddModelError("Company.CompanyBuildingItems", ViewResources.SharedStrings.CompaniesEqualBildingsErrorMessage);
                    }

                    var building_items = new List<SelectListItem>();

                    Mapper.Map(_buildingRepository.FindAll().Where(x => build_ids.Contains(x.Id)), building_items);

                    foreach (var cbi in cevm.Company.CompanyBuildingItems)
                    {
                        cbi.BuildingItems = building_items;
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            IEnumerable<CompanyBuildingDto> company_buildings = GetSelectedCompanyBuildings(cevm.Company);
                            id = _companyService.CreateCompany(cevm.Company.Name, cevm.Company.Comment, cevm.Company.IsCanUseOwnCards, HostName, company_buildings);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message);
                            err = ex.Message;
                        }
                    }
                    if (id < 0)
                    {
                        return Json(new { IsSucceed = false });
                    }
                    else
                    {
                        var complist = _companyRepository.FindAll().Where(x => x.IsDeleted == false && x.Active == true && x.ParentId == null).OrderBy(y => y.Name).ToList();
                        cevm.CompanyItems = complist;
                        return Json(new
                        {
                            IsSucceed = ModelState.IsValid,
                            Id = id,
                            Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err,
                            viewData = ModelState.IsValid ? string.Empty : this.RenderPartialView("Create", cevm)
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        IsSucceed = false,
                        Msg = "licence error",
                        Count = tc
                    });
                }
            }
        }

        private IEnumerable<CompanyBuildingDto> GetSelectedCompanyBuildings(CompanyItem company)
        {
            var result = new List<CompanyBuildingDto>();
            foreach (var companyBuildingItem in company.CompanyBuildingItems)
            {
                var cbos = new List<CompanyBuildingDto>();
                Mapper.Map(companyBuildingItem.CompanyFloors.Where(x => x.BuildingObjectId != 0), cbos);
                result.AddRange(cbos);
            }
            return result;
        }

        #endregion

        #region Edit

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cevm = CreateViewModel<CompanyEditViewModel>();
            var company = _companyRepository.FindById(id);
            Mapper.Map(company, cevm.Company);
            var building_ids = GetUserBuildings(_buildingRepository, _userRepository);
            var buildings = _buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id));
            Mapper.Map(buildings, cevm.BuildingItems);

            //var cbos =
            //      company.CompanyBuildingObjects.OrderBy(x => x.BuildingObject.BuildingId).Where(x => !x.IsDeleted && (x.BuildingObject.TypeId == 1)).GroupBy(
            //          cbo => cbo.BuildingObject.BuildingId);

            var cbos =
                    company.CompanyBuildingObjects.OrderBy(x => x.BuildingObject.BuildingId).Where(x => !x.IsDeleted && (x.BuildingObject.TypeId == 1 || x.BuildingObject.TypeId == 2 || x.BuildingObject.TypeId == 3 || x.BuildingObject.TypeId == 8 || x.BuildingObject.TypeId == 9 || x.BuildingObject.TypeId == 10 || x.BuildingObject.TypeId == 11)).GroupBy(
                        cbo => cbo.BuildingObject.BuildingId);

            foreach (var cbo in cbos)
            {
                var company_building_item = GetFloorItems(cbo.FirstOrDefault().BuildingObject.BuildingId, id);
                company_building_item.BuildingItems = cevm.BuildingItems;
                cevm.Company.CompanyBuildingItems.Add(company_building_item);
            }
            var complist = _companyRepository.FindAll().Where(x => x.IsDeleted == false && x.Active == true && x.ParentId == null && x.Id != id).OrderBy(y => y.Name).ToList();
            cevm.CompanyItems = complist;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select ParentCompanieId from CompanieSubCompanies where IsDeleted=0 and CompanyId='" + id + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            List<int> arr = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                arr.Add(Convert.ToInt32(dr["ParentCompanieId"]));
            }
            cevm.SelCompanyItems = arr.ToList();
            var roleList = _roleRepository.FindAll(x => !x.IsDeleted && x.Active && x.RoleTypeId >= CurrentUser.Get().RoleTypeId).ToList();
            
            List<SelectListItem> roleListItems = new List<SelectListItem>();
            List<UserRoleItem> userRole = new List<UserRoleItem>();

            FoxSecDBContext dBContext = new FoxSecDBContext();
            var companyRoleListItems = dBContext.CompanieRoles.Where(x => !x.IsDeleted && x.CompanyId == id).Select(y => y.RoleId).ToList();

            roleList.ForEach(x => { var addRoleItems = new SelectListItem { Selected = companyRoleListItems.Contains(x.Id) ? true:false, Text = x.Name, Value = x.Id.ToString() }; roleListItems.Add(addRoleItems); });
            cevm.FoxSecUser.RoleItems = roleListItems;
            User user = _userRepository.FindById(CurrentUser.Get().Id);
            //roleList.ForEach(x => { var role = new UserRoleItem { IsSelected = user.UserRoles.Any(y => y.RoleId == y.Id && !y.IsDeleted), RoleName = x.Name, RoleDescription = "", RoleId = x.Id, ValidFrom = user.UserRoles.Any(userRoles => userRoles.RoleId == x.Id && !userRoles.IsDeleted) ? user.UserRoles.Where(userRoles => userRoles.RoleId == x.Id && !userRoles.IsDeleted).FirstOrDefault().ValidFrom.ToString("dd.MM.yyyy") : string.Empty, ValidTo = user.UserRoles.Any(userRoles => userRoles.RoleId == x.Id && !userRoles.IsDeleted) ? user.UserRoles.Where(userRoles => userRoles.RoleId == x.Id && !userRoles.IsDeleted).FirstOrDefault().ValidTo.ToString("dd.MM.yyyy") : string.Empty }; userRole.Add(role); });
            //cevm.FoxSecUser.UserRoleItems.Roles = userRole;
            

            return PartialView(cevm);
        }

        public ActionResult BuildingList(int id)
        {
            var cevm = CreateViewModel<CompanyEditViewModel>();
            Mapper.Map(_companyRepository.FindById(id), cevm.Company);
            cevm.Buildings = _buildingRepository.FindAll(x => !x.IsDeleted).ToList();
            cevm.Company.CompanyBuildings =
                (from bn in _companyBuildingObjectRepository.FindAll(x => x.CompanyId == id && !x.IsDeleted)
                 select bn.BuildingObject.Building).Distinct().ToList();
            return PartialView(cevm);
        }

        [HttpPost]
        public ActionResult Update(CompanyEditViewModel cevm)
        {
            var tcevm = CreateViewModel<CompanyEditViewModel>();
            string err = string.Empty;

            if (cevm.Company.CompanyBuildingItems.Any(x => x.BuildingId == null))
            {
                ModelState.AddModelError("Company.CompanyBuildingItems", ViewResources.SharedStrings.CompaniesEmptyBuildingSelected);
            }
            if (cevm.Company.CompanyBuildingItems.Any(x => x.BuildingId != null && !x.CompanyFloors.Any(cf => cf.IsSelected)))
            {
                ModelState.AddModelError("Company.CompanyBuildingItems", ViewResources.SharedStrings.CompaniesNoBuildingSelectedMessage);
            }

            var build_ids = GetUserBuildings(_buildingRepository, _userRepository);

            if (build_ids.Any(x => cevm.Company.CompanyBuildingItems.Where(cb => cb.BuildingId == x).Count() > 1))
            {
                ModelState.AddModelError("Company.CompanyBuildingItems", ViewResources.SharedStrings.CompaniesEqualBildingsErrorMessage);
            }

            var building_items = new List<SelectListItem>();
            Mapper.Map(_buildingRepository.FindAll().Where(x => build_ids.Contains(x.Id)), building_items);

            foreach (var cbi in cevm.Company.CompanyBuildingItems)
            {
                cbi.BuildingItems = building_items;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<CompanyBuildingDto> company_buildings = GetSelectedCompanyBuildings(cevm.Company);
                    _companyService.UpdateCompany(cevm.Company.Id.Value, cevm.Company.Name, cevm.Company.Comment, cevm.Company.IsCanUseOwnCards, HostName, company_buildings);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    err = ex.Message;
                }
            }
            else
            {
                tcevm.Company = cevm.Company;
            }
            if (ModelState.IsValid == false)
            {
                var complist = _companyRepository.FindAll().Where(x => x.IsDeleted == false && x.Active == true && x.ParentId == null && x.Id != cevm.Company.Id).OrderBy(y => y.Name).ToList();
                tcevm.CompanyItems = complist;
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Id = cevm.Company.Id,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err,
                viewData = ModelState.IsValid ? string.Empty : this.RenderPartialView("Edit", tcevm)
            });
        }

        [HttpGet]
        public ActionResult Activate()
        {
            var dcvm = CreateViewModel<DeactivateCardViewModel>();
            dcvm.Reasons = new SelectList(_classificatorValueRepository.FindAll(cv => cv.ClassificatorId == 3).OrderBy(cv => cv.Value.ToLower()), "Id", "Value");
            return PartialView("ActivateDeactivate", dcvm);
        }

        [HttpGet]
        public ActionResult Deactivate()
        {
            var dcvm = CreateViewModel<DeactivateCardViewModel>();
            dcvm.Reasons = new SelectList(_classificatorValueRepository.FindAll(cv => cv.ClassificatorId == 2).OrderBy(cv => cv.Value.ToLower()), "Id", "Value");
            dcvm.IsDeactivateDialog = true;
            return PartialView("ActivateDeactivate", dcvm);
        }

        [HttpPost]
        public ActionResult Activate(int[] companiesIds, int reasonId)
        {
            foreach (int id in companiesIds)
            {
                _companyService.Activate(id, reasonId, HostName);
            }
            return null;
        }

        [HttpPost]
        public ActionResult Deactivate(int[] companiesIds, int reasonId)
        {
            foreach (int id in companiesIds)
            {
                _companyService.Deactivate(id, reasonId, HostName);

                foreach (var user in _userRepository.FindAll().Where(us => !us.IsDeleted && us.Active && us.CompanyId == id))
                {
                    _userService.Deactivate(user.Id, reasonId, CurrentUser.Get().HostName);

                    if (user.UsersAccessUnits != null && user.UsersAccessUnits.Count() > 0)
                    {
                        foreach (var uau in user.UsersAccessUnits)
                        {
                            if (!uau.IsDeleted && uau.Active)
                            {
                                _userAccessUnitService.Deactivate(uau.Id, reasonId);
                            }
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int[] companiesIds)
        {
            var err = string.Empty;
            List<string> non_deleted_companies_list = (from companyId in companiesIds
                                                       select _companyRepository.FindById(companyId)
                                                       into company
                                                       where company.Users.Any(x => !x.IsDeleted)
                                                       select company.Name).ToList();

            if (non_deleted_companies_list.Count > 0)
            {
                var non_deleted_companies = string.Join(",", non_deleted_companies_list);
                err = string.Format(ViewResources.SharedStrings.CompaniesCompaniesContainsUsersError, non_deleted_companies);
                ModelState.AddModelError("", err);
            }
            if (ModelState.IsValid)
            {
                foreach (int id in companiesIds)
                {
                    try
                    {
                        _companyService.DeleteCompany(id, HostName);
                    }
                    catch (Exception e)
                    {
                        Logger.Write("Error deleting Company Id=" + id, e);
                        err = "Error deleting Company Id=" + id;
                        ModelState.AddModelError("", err);
                    }
                }
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err
            });
        }

        #endregion

        #region Tree

        public ActionResult GetTree()
        {
            var ctvm = new CompanyTreeViewModel();
            var role_building_ids = GetUserBuildings(_buildingRepository, _userRepository);

            ctvm.Countries =
                from country in _countryRepository.FindAll(x => !x.IsDeleted)
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

            if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsCompanyManager)
            {
                ctvm.Offices = role_building_ids.Count > 0 ? ctvm.Offices.Where(build => role_building_ids.Contains(build.MyId)).ToList() : new List<Node>();
                ctvm.Towns = ctvm.Towns.Where(town => ctvm.Offices.Any(off => off.ParentId == town.MyId));
                ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));
            }

            List<int> companyIds =
                (from c in _companyRepository.FindAll(x => !x.IsDeleted && x.Active && x.ParentId == null) select c.Id).ToList();

            ctvm.Companies =
                from company in
                    _companyBuildingObjectRepository.FindAll(x => !x.IsDeleted && companyIds.Contains(x.CompanyId))
                select
                    new Node
                    {
                        ParentId = company.BuildingObject.BuildingId,
                        MyId = company.Company.Id,
                        Name = company.Company.Name
                    };

            ctvm.Companies = ctvm.Companies.Distinct(new NodeEqualityComparer());

            ctvm.Partners =
                from company in
                    _companyRepository.FindAll(x => !x.IsDeleted && x.ParentId != null)
                select
                    new Node
                    {
                        ParentId = company.ParentId.Value,
                        MyId = company.Id,
                        Name = company.Name
                    };

            ctvm.Partners = ctvm.Partners.Distinct(new NodeEqualityComparer());
            return PartialView("CompanyTree", ctvm);
        }

        [HttpGet]
        public ActionResult ByBuilding(int id)
        {
            var clvm = CreateViewModel<CompanyListViewModel>();
            IEnumerable<Company> result =
                _companyRepository.FindAll(
                    x =>
                    x.Active && !x.IsDeleted && x.ParentId == null &&
                    x.CompanyBuildingObjects.Any(y => !y.IsDeleted && y.BuildingObject.BuildingId == id)).ToList();

            clvm.Paginator = SetupPaginator(ref result, 0, 10);
            clvm.Paginator.DivToRefresh = "CompaniesList";
            clvm.Paginator.Prefix = "Companies";
            Mapper.Map(result, clvm.Companies);

            clvm.FilterCriteria = 1;

            foreach (CompanyItem c in clvm.Companies)
            {
                string bNames = string.Join(", ",
                                            (from bn in c.CompanyBuildingObjects
                                             where bn.IsDeleted == false
                                             select bn.BuildingObject.Building.Name).GroupBy(y => y).Select(x => x.Key).
                                                ToArray());
                c.BuidingNames = bNames.Length == 0 ? "-" : bNames;

                var cbos =
                    c.CompanyBuildingObjects.Where(x => !x.IsDeleted && x.BuildingObject.TypeId == 1).GroupBy(
                        cbo => cbo.BuildingObject.BuildingId);

                var resFloors = new List<string>();
                foreach (var cbo in cbos)
                {
                    string floors1 = string.Join(", ", (from fn in cbo
                                                        where fn.IsDeleted == false && fn.BuildingObject.TypeId == 1
                                                        select fn.BuildingObject.Description).GroupBy(y => y).Select(x => x.Key).ToArray());
                    resFloors.Add(floors1);
                }
                string floors = string.Join(";", resFloors);
                c.Floors = floors.Length == 0 ? "-" : floors;
            }
            return PartialView("List", clvm);
        }

        [HttpGet]
        public ActionResult ByLocation(int id)
        {
            var clvm = CreateViewModel<CompanyListViewModel>();
            IEnumerable<Company> result = CompaniesByLocation(id);

            clvm.Paginator = SetupPaginator(ref result, 0, 10);
            clvm.Paginator.DivToRefresh = "CompaniesList";
            clvm.Paginator.Prefix = "Companies";
            Mapper.Map(result, clvm.Companies);

            clvm.FilterCriteria = 1;

            foreach (CompanyItem c in clvm.Companies)
            {
                string bNames = string.Join(", ",
                                            (from bn in c.CompanyBuildingObjects
                                             where bn.IsDeleted == false
                                             select bn.BuildingObject.Building.Name).GroupBy(y => y).Select(x => x.Key).
                                                ToArray());
                c.BuidingNames = bNames.Length == 0 ? "-" : bNames;

                var cbos =
                     c.CompanyBuildingObjects.Where(x => !x.IsDeleted && x.BuildingObject.TypeId == 1).GroupBy(
                         cbo => cbo.BuildingObject.BuildingId);

                var resFloors = new List<string>();
                foreach (var cbo in cbos)
                {
                    string floors1 = string.Join(", ", (from fn in cbo
                                                        where fn.IsDeleted == false && fn.BuildingObject.TypeId == 1
                                                        select fn.BuildingObject.Description).GroupBy(y => y).Select(x => x.Key).ToArray());
                    resFloors.Add(floors1);
                }
                string floors = string.Join(";", resFloors);
                c.Floors = floors.Length == 0 ? "-" : floors;
            }
            return PartialView("List", clvm);
        }

        private IEnumerable<Company> CompaniesByLocation(int id)
        {
            List<int> buildingIds =
                (from b in _buildingRepository.FindAll(x => !x.IsDeleted && x.LocationId == id) select b.Id).ToList();
            IEnumerable<Company> result =
                _companyRepository.FindAll(
                    x =>
                    x.Active && !x.IsDeleted && x.ParentId == null &&
                    x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildingIds.Contains(y.BuildingObject.BuildingId)))
                    .ToList();
            return result;
        }

        public ActionResult ByCountry(int id)
        {
            var clvm = CreateViewModel<CompanyListViewModel>();

            IEnumerable<Company> result = CompaniesByCountry(id);

            clvm.Paginator = SetupPaginator(ref result, 0, 10);
            clvm.Paginator.DivToRefresh = "CompaniesList";
            clvm.Paginator.Prefix = "Companies";
            Mapper.Map(result, clvm.Companies);

            clvm.FilterCriteria = 1;

            foreach (CompanyItem c in clvm.Companies)
            {
                string bNames = string.Join(", ",
                                            (from bn in c.CompanyBuildingObjects
                                             where bn.IsDeleted == false
                                             select bn.BuildingObject.Building.Name).GroupBy(y => y).Select(x => x.Key).
                                                ToArray());
                c.BuidingNames = bNames.Length == 0 ? "-" : bNames;

                var cbos =
                    c.CompanyBuildingObjects.Where(x => !x.IsDeleted && x.BuildingObject.TypeId == 1).GroupBy(
                        cbo => cbo.BuildingObject.BuildingId);

                var resFloors = new List<string>();
                foreach (var cbo in cbos)
                {
                    string floors1 = string.Join(", ", (from fn in cbo
                                                        where fn.IsDeleted == false && fn.BuildingObject.TypeId == 1
                                                        select fn.BuildingObject.Description).GroupBy(y => y).Select(x => x.Key).ToArray());

                    resFloors.Add(floors1);
                }
                string floors = string.Join(";", resFloors);
                c.Floors = floors.Length == 0 ? "-" : floors;
            }
            return PartialView("List", clvm);
        }

        private IEnumerable<Company> CompaniesByCountry(int id)
        {
            List<int> locationIds =
                (from l in _locationRepository.FindAll(x => !x.IsDeleted && x.CountryId == id) select l.Id).ToList();
            List<int> buildingIds =
                (from b in _buildingRepository.FindAll(x => !x.IsDeleted && locationIds.Contains(x.LocationId))
                 select b.Id).ToList();
            IEnumerable<Company> result =
                _companyRepository.FindAll(
                    x =>
                    x.Active && !x.IsDeleted && x.ParentId == null &&
                    x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildingIds.Contains(y.BuildingObject.BuildingId)))
                    .ToList();

            return result;
        }

        #endregion

        #region My Company

        [HttpGet]
        public ActionResult MyCompany()
        {
            var mcvm = CreateViewModel<MyCompanyViewModel>();
            int? companyId = CurrentUser.Get().CompanyId;
            if (companyId.HasValue)
            {
                Mapper.Map(_companyRepository.FindById(companyId.Value), mcvm.Company);
            }
            return PartialView("MyCompany", mcvm);
        }

        [HttpPost]
        public ActionResult SaveCompanyInfo(CompanyItem company)
        {
            _companyService.UpdateCompany(company.Id.Value, company.Name, company.Comment, HostName);
            return null;
        }

        [HttpGet]
        public ActionResult CreatePartner(int companyId)
        {
            var pevm = CreateViewModel<PartnerEditViewModel>();

            var companyIds = (from cc in _companyRepository.FindAll(x => !x.IsDeleted && x.Active && x.ParentId == companyId) select cc.Id).ToList();

            var users =
                _userRepository.FindAll(x => !x.IsDeleted && x.CompanyId.HasValue && (x.CompanyId == companyId || companyIds.Contains(x.CompanyId.Value))
                    && x.UserRoles.Any(ur => !ur.IsDeleted && ur.Role.RoleTypeId == (int)RoleTypeEnum.CM && ur.ValidFrom < DateTime.Now && ur.ValidTo.AddDays(1) > DateTime.Now));

            Mapper.Map(users, pevm.Managers);
            pevm.Partner.CompanyId = companyId;
            return View(pevm);
        }

        [HttpGet]
        public ActionResult PartnerList(int companyId)
        {
            var plvm = CreateViewModel<PartnerListViewModel>();
            Mapper.Map(_companyRepository.FindByParentId(companyId), plvm.Partners);
            foreach (PartnerItem partnerItem in plvm.Partners)
            {
                partnerItem.ManagerName = _companyManagerRepository.GetCompanyManagerName(partnerItem.Id.Value);
            }
            return View(plvm);
        }

        public ActionResult CreatePartner(PartnerItem partner)
        {
            if (partner.CompanyId != 0)
            {
                _companyService.CreateCompany(partner.CompanyId, partner.Name, partner.Comment, false, HostName);
                if (partner.ManagerId != 0)
                {
                    _companyManagerService.SaveCompanyManager(partner.CompanyId, partner.ManagerId, HostName);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult DeletePartner(int id)
        {
            _companyService.DeleteCompany(id, HostName);
            return null;
        }

        [HttpGet]
        public ActionResult EditPartner(int id)
        {
            var pevm = CreateViewModel<PartnerEditViewModel>();

            var companyId = _companyRepository.FindById(id).ParentId;

            Mapper.Map(_companyRepository.FindById(id), pevm.Partner);

            pevm.Partner.ManagerId = _companyManagerRepository.GetCompanyManagerId(pevm.Partner.Id.Value);
            var companyIds = (from cc in _companyRepository.FindAll(x => !x.IsDeleted && x.Active && x.ParentId == companyId) select cc.Id).ToList();

            var users =
                _userRepository.FindAll(x => !x.IsDeleted && x.CompanyId.HasValue && (x.CompanyId == companyId || companyIds.Contains(x.CompanyId.Value))
                    && x.UserRoles.Any(ur => !ur.IsDeleted && ur.Role.RoleTypeId == (int)RoleTypeEnum.CM && ur.ValidFrom < DateTime.Now && ur.ValidTo.AddDays(1) > DateTime.Now));

            Mapper.Map(users, pevm.Managers);
            return View(pevm);
        }

        [HttpPost]
        public ActionResult EditPartner(PartnerItem partner)
        {
            _companyService.UpdateCompany(partner.Id.Value, partner.Name, partner.Comment, HostName);
            _companyService.SetState(partner.Id.Value, null, partner.Active, HostName);

            if (partner.ManagerId != 0)
            {
                _companyManagerService.SaveCompanyManager(partner.Id.Value, partner.ManagerId, HostName);
            }
            return null;
        }

        #endregion

        [HttpPost]
        public void SaveSubComapnyDetails(int compid, List<int> complist)
        {
            try
            {
                _companyService.SaveSubComapnyDetails(compid, complist, HostName);
            }
            catch
            {

            }
        }

        [HttpPost]
        public void CompanieUserRoleSave(List<int> roleItems,int companyId)
        {
            FoxSecDBContext dBContext = new FoxSecDBContext();
            int currentUserId =0;
            if (CurrentUser.Get().IsSuperAdmin)
            {
              currentUserId  = companyId;
            }
            else if(CurrentUser.Get().IsCompanyManager)
            {
                currentUserId = CurrentUser.Get().CompanyId.Value;
            }
            
            var companieRolesRecords = dBContext.CompanieRoles.Where(y => y.CompanyId == currentUserId).Any() ? dBContext.CompanieRoles.Where(x => !x.IsDeleted && x.CompanyId == currentUserId).ToList() : null;
            if (companieRolesRecords != null)
            {
                foreach (var roles in companieRolesRecords)
                {
                    roles.IsDeleted = true;
                }
                dBContext.SaveChanges();
            }
            foreach (int roleId in roleItems)
            {
              
                var companieRoleModel = new CompanyRoleModel { CompanyId = currentUserId, RoleId = roleId, IsDeleted = false };
                dBContext.CompanieRoles.Add(companieRoleModel);
                dBContext.SaveChanges();
            }
        }
    }
}