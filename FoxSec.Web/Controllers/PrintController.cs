using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Resources;
using System.Xml.Linq;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.Common.Enums;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using Winnovative.WnvHtmlConvert;
using ViewResources;
using System.Configuration;
using FoxSec.Infrastructure.EF.Repositories.Interfaces;
using System.Data.SqlClient;

namespace FoxSec.Web.Controllers
{
    public class PrintController : BusinessCaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IVisitorRepository _visitorRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogRepository _logRepository;
        private readonly IUserBuildingRepository _userBuildingRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ITitleRepository _titleRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserDepartmentRepository _userDepartmentRepository;
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUsersAccessUnitRepository _usersAccessUnitRepository;
        private readonly IUserPermissionGroupRepository _userPermissionGroupRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly IUserPermissionGroupService _userPermissionGroupService;
        private ResourceManager _resourceManager;
        private readonly ICompanyService _companyService;
        private readonly IUserDepartmentService _userDepartmentService;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);

        public PrintController(IUserRepository userRepository,
                                IVisitorRepository visitorRepository,
                                IRoleRepository roleRepository,
                                ILogRepository logRepository,
                                ITitleRepository titleRepository,
                                IDepartmentRepository departmentRepository,
                                IBuildingRepository buildingRepository,
                                IBuildingObjectRepository buildingObjectRepository,
                                IUserDepartmentRepository userDepartmentRepository,
                                IUsersAccessUnitRepository usersAccessUnitRepository,
                                IUserBuildingRepository userBuildingRepository,
                                ICompanyRepository companyRepository,
                                IUserTimeZoneRepository userTimeZoneRepository,
                                IUserPermissionGroupRepository userPermissionGroupRepository,
                                IUserPermissionGroupTimeZoneRepository userPermissionGroupTimeZoneRepository,
                                IUserPermissionGroupService userPermissionGroupService,
                                ICurrentUser currentUser,
                                ICompanyService companyService,
                                IUserDepartmentService userDepartmentService,
                                ILogger logger) : base(currentUser, logger)
        {
            _visitorRepository = visitorRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _logRepository = logRepository;
            _titleRepository = titleRepository;
            _buildingRepository = buildingRepository;
            _buildingObjectRepository = buildingObjectRepository;
            _departmentRepository = departmentRepository;
            _userDepartmentRepository = userDepartmentRepository;
            _usersAccessUnitRepository = usersAccessUnitRepository;
            _userBuildingRepository = userBuildingRepository;
            _userPermissionGroupRepository = userPermissionGroupRepository;
            _userPermissionGroupTimeZoneRepository = userPermissionGroupTimeZoneRepository;
            _userPermissionGroupService = userPermissionGroupService;
            _userTimeZoneRepository = userTimeZoneRepository;
            _companyRepository = companyRepository;
            _companyService = companyService;
            _userDepartmentService = userDepartmentService;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
            RegisterLogRelatedMap();
        }

        #region User Permission Group Export
        public PermissionTreeViewModel GetUserPermissionTree(int userId)
        {
            var ctvm = CreateViewModel<PermissionTreeViewModel>();
            var role_buildingIds = GetUserBuildings(_buildingRepository, _userRepository);
            var upg = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == userId).FirstOrDefault();

            if (upg != null)
            {
                ctvm.IsOriginal = false;

                List<int> companyBuildingObjectsIds = (from cbo in upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted) select cbo.BuildingObjectId).ToList();
                var companyBuildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && role_buildingIds.Contains(x.BuildingId)
                    && companyBuildingObjectsIds.Contains(x.Id));

                List<int> floorsId = (from cbo in companyBuildingObjects.Where(x => x.ParentObjectId.HasValue) select cbo.ParentObjectId.Value).ToList();
                var companyFloorObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && floorsId.Contains(x.Id));

                ctvm.Buildings =
                    (from b in
                         (from cbo in companyBuildingObjects select cbo.Building)
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
                         (from cbo in companyBuildingObjects select cbo.Building.Location)
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
                         (from cbo in companyBuildingObjects select cbo.Building.Location.Country).OrderBy(cc => cc.Name)
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
                    (from o in companyBuildingObjects
                     where o.ParentObjectId.HasValue && (o.TypeId == (int)BuildingObjectTypes.Door || o.TypeId == (int)BuildingObjectTypes.Lift || o.TypeId == (int)BuildingObjectTypes.Room)
                     select
                         new Node
                         {
                             ParentId = o.ParentObjectId.Value,
                             MyId = o.Id,
                             Name = o.Description,
                             Comment = o.Comment,
                             IsDefaultTimeZone = _userPermissionGroupService.IsDefaultUserTimeZone(o.Id, upg.Id),
                             IsRoom = o.TypeId == (int)BuildingObjectTypes.Room ? 1 : 0,
                             TimeZoneName = _userTimeZoneRepository.FindById(_userPermissionGroupService.GetUserActiveTimeZoneIdByBuildingObjectId(upg.Id, o.Id)).Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.ActiveObjectIds = _userPermissionGroupService.GetUserBuildingObjectIds(upg.Id);
                foreach (var obn in ctvm.Objects)
                {
                    if (obn.IsRoom == 1)
                    {
                        // illi 08.05.2018                  UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == upg.Id).FirstOrDefault();
                        //   UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();

                        int idd = upg.Id;//.GetValueOrDefault();
                        UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(idd).Where(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == upg.Id).FirstOrDefault();



                        obn.IsArming = upgtz.IsArming;
                        obn.IsDefaultArming = upgtz.IsDefaultArming;
                        obn.IsDisarming = upgtz.IsDisarming;
                        obn.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                    }
                }

                ctvm.Towns = ctvm.Towns.Where(town => ctvm.Buildings.Any(b => b.ParentId == town.MyId));
                ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));
            }

            return ctvm;
        }

        #endregion

        #region User Personal Data Export
        public PdfResult UserDataPdf(int id)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.UserDataExport(id));
            return new PdfResult(html, "UserData", true, PDFPageOrientation.Portrait);
        }

        public ExcelResult UserDataExcel(int id)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.UserDataExport(id));
            return new ExcelResult(html, "UserData", true);
        }

        public PdfResult UserDataPdfVisiotr(int id)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.UserDataExportVisitor(id));
            return new PdfResult(html, "UserData", true, PDFPageOrientation.Portrait);
        }

        public ExcelResult UserDataExcelVisiotr(int id)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.UserDataExportVisitor(id));
            return new ExcelResult(html, "VisitorData", true);
        }

        public ActionResult UserDataExport(int id)
        {
            var vm = FillUserDataModel(id);
            return View("UserDataExport", vm);
        }

        public ActionResult UserDataExportVisitor(int id)
        {
            var vm = FillUserDataModelVisitor(id);
            return View("UserDataExportVisitor", vm);
        }

        public List<SelectListItem> GetFloors(int userId, int? buildingId)
        {
            var result = new List<SelectListItem>();
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

        public List<SelectListItem> GetBuildings(int userId)
        {
            var result = new List<SelectListItem>();

            var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);

            var user = new UserItem();
            Mapper.Map(_userRepository.FindById(userId), user);
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
                        var userBuildingIds = (from ubo in _userBuildingRepository.FindByUserId(CurrentUser.Get().Id) select ubo.BuildingId).ToList();

                        var buildingObjects =
                        _buildingObjectRepository.FindAll(x => !x.IsDeleted && x.TypeId == 1 && role_building_ids.Contains(x.BuildingId) && userBuildingIds.Contains(x.BuildingId));

                        foreach (var cbo in buildingObjects.Where(cbo => !result.Any(rr => rr.Value == cbo.BuildingId.ToString())))
                        {
                            result.Add(new SelectListItem() { Value = cbo.BuildingId.ToString(), Text = cbo.Building.Name });
                        }

                    }
                }
            }

            return result;
        }

        private IEnumerable<SelectListItem> GetLanguages()
        {
            var result = new List<SelectListItem>();

            foreach (var ul in Enum.GetValues(typeof(UserLanguageEnum)))
            {
                var item = new SelectListItem()
                {
                    Value = ((int)ul).ToString(),
                    Text = (string)_resourceManager.GetObject(Enum.GetName(typeof(UserLanguageEnum), ul), Thread.CurrentThread.CurrentCulture),
                };

                result.Add(item);
            }

            return result;
        }

        public UserEditViewModel FillUserDataModel(int id)
        {
            User user = _userRepository.FindById(id);
            var uvm = CreateViewModel<UserEditViewModel>();
            Mapper.Map(user, uvm.FoxSecUser);
            uvm.LanguageItems = GetLanguages();
            uvm.FoxSecUser.UserPermissionTree = GetUserPermissionTree(id);

            var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
            var user_building_ids = GetUserBuildings(_buildingRepository, _userRepository);

            uvm.FoxSecUser.RoleItems =
                from role in _roleRepository.FindAll(x => !x.IsDeleted && x.Active && x.RoleTypeId >= CurrentUser.Get().RoleTypeId && x.RoleBuildings.Any(rb => !rb.IsDeleted && user_building_ids.Contains(rb.BuildingId)))
                select
                    new SelectListItem
                    {
                        Selected = user.UserRoles.Any(userRole => userRole.RoleId == role.Id),
                        Text = role.Name + "#" + role.Description,
                        Value = role.Id.ToString()
                    };

            uvm.FoxSecUser.BirthDayStr = uvm.FoxSecUser.Birthday.HasValue ? uvm.FoxSecUser.Birthday.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.ContractStartDateStr = uvm.FoxSecUser.ContractStartDate.HasValue ? uvm.FoxSecUser.ContractStartDate.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.ContractEndDateStr = uvm.FoxSecUser.ContractEndDate.HasValue ? uvm.FoxSecUser.ContractEndDate.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.PermitOfWorkStr = uvm.FoxSecUser.PermitOfWork.HasValue ? uvm.FoxSecUser.PermitOfWork.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.RegistredDateStr = uvm.FoxSecUser.RegistredStartDate.ToString("dd.MM.yyyy");

            IEnumerable<UserDepartment> userDepartments = _userDepartmentRepository.FindAll(ud => !ud.IsDeleted && ud.UserId == id);
            Mapper.Map(userDepartments, uvm.FoxSecUser.UserDepartments);

            foreach (var ud in uvm.FoxSecUser.UserDepartments)
            {
                ud.Manager = GetDepartmentManagerName(ud.DepartmentId);
                ud.ValidFrom = DateTime.Parse(ud.ValidFrom).ToString("dd.MM.yyyy");
                ud.ValidTo = DateTime.Parse(ud.ValidTo).ToString("dd.MM.yyyy");
            }

            Mapper.Map(userDepartments, uvm.FoxSecUser.UserDepartmentsList);

            int depId = 0;

            UserDepartment userDepartment = userDepartments.FirstOrDefault(ud => ud.CurrentDep);
            if (userDepartment != null) depId = userDepartment.DepartmentId;

            var departmentsList = new SelectList(_departmentRepository.FindAll(d => !d.IsDeleted).ToList(), "Id", "Name", depId);
            ViewData["DepartmentsList"] = departmentsList;

            try
            {
                uvm.FoxSecUser.DepartmentStartDateStr =
                    uvm.FoxSecUser.UserDepartments.FirstOrDefault(d => d.CurrentDep).ValidFrom;
            }
            catch
            {
                uvm.FoxSecUser.DepartmentStartDateStr = "";
            }

            try
            {
                uvm.FoxSecUser.DepartmentEndDateStr =
                    uvm.FoxSecUser.UserDepartments.FirstOrDefault(d => d.CurrentDep).ValidTo;
            }
            catch
            {
                uvm.FoxSecUser.DepartmentEndDateStr = "";
            }

            uvm.FoxSecUser.IsInCreateMode = false;
            List<UserRole> activeUserRoles = user.UserRoles.Where(x => !x.IsDeleted).ToList();

            if (activeUserRoles.Count != 0)
            {
                uvm.FoxSecUser.RoleId = activeUserRoles.First().RoleId;
            }

            var companyId = CurrentUser.Get().CompanyId.HasValue ? CurrentUser.Get().CompanyId.Value : -1;
            var companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active);
            if (CurrentUser.Get().IsBuildingAdmin || uvm.FoxSecUser.IsBuildingAdmin)
            {
                var buildIds = GetUserBuildings(_buildingRepository, _userRepository);

                companies = companies.Where(x => x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildIds.Contains(y.BuildingObject.BuildingId)));
            }

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                companies =
                    companies.Where(cc => cc.Id == companyId || (cc.ParentId != null && cc.ParentId.Value == companyId));
            }

            uvm.Companies = new SelectList(companies, "Id", "Name", uvm.FoxSecUser.CompanyId);

            var titles = _titleRepository.FindAll(c => !c.IsDeleted);
            if (!CurrentUser.Get().IsSuperAdmin)
            {
                titles = titles.Where(x => companies.Any(c => c.Id == x.CompanyId));
            }
            uvm.Titles = new SelectList(titles, "Id", "Name", uvm.FoxSecUser.TitleId);

            var userBuildings = new List<UserBuildingItem>();
            Mapper.Map(_userBuildingRepository.FindByUserId(uvm.FoxSecUser.Id.Value), userBuildings);
            foreach (var userBuildingItem in userBuildings)
            {
                userBuildingItem.IsBuildingAvailable = role_building_ids.Any(x => userBuildingItem.BuildingId == x);
            }

            uvm.FoxSecUser.BuildingItems = GetBuildings(uvm.FoxSecUser.Id.Value);
            if (userBuildings.Count() > 0)
            {
                foreach (var ubo in userBuildings)
                {
                    ubo.FloorItems = GetFloors(ubo.UserId.Value, ubo.BuildingId);
                }
            }
            else
            {
                userBuildings.Add(new UserBuildingItem());
            }
            uvm.FoxSecUser.UserBuildingObjects = userBuildings;

            return uvm;
        }

        public VisitorEditViewModel FillUserDataModelVisitor(int id)
        {
            Visitor user = _visitorRepository.FindById(id);
            var uvm = CreateViewModel<VisitorEditViewModel>();
            Mapper.Map(user, uvm.FoxSecUser);
            uvm.LanguageItems = GetLanguages();
            return uvm;
        }

        #endregion

        #region Card List Export
        public PdfResult CardListPDF(string cardSer,
                                        string cardDk,
                                        string cardNo,
                                        string cardName,
                                        string company,
                                        string building,
                                        string validation,
                                        int filter,
                                        int? sort_field,
                                        int? sort_direction,
                                        bool display1, bool display2, bool display3, bool display4, bool display5, bool display6, bool display7, bool display8, bool display9)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.CardListExport(cardSer, cardDk, cardNo, cardName, company, building, validation, filter, sort_field, sort_direction, display1, display2, display3, display4, display5, display6, display7, display8, display9));
            return new PdfResult(html, "CardList", true, PDFPageOrientation.Portrait);
        }

        public ExcelResult CardListExcel(string cardSer,
                                            string cardDk,
                                            string cardNo,
                                            string cardName,
                                            string company,
                                            string building,
                                            string validation,
                                            int filter,
                                            int? sort_field,
                                            int? sort_direction,
                                            bool display1, bool display2, bool display3, bool display4, bool display5, bool display6, bool display7, bool display8, bool display9)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.CardListExport(cardSer, cardDk, cardNo, cardName, company, building, validation, filter, sort_field, sort_direction, display1, display2, display3, display4, display5, display6, display7, display8, display9));
            return new ExcelResult(html, "CardList", true);
        }

        public ActionResult CardListExport(string cardSer,
                                            string cardDk,
                                            string cardNo,
                                            string cardName,
                                            string company,
                                            string building,
                                            string validation,
                                            int filter,
                                            int? sort_field,
                                            int? sort_direction,
                                            bool display1, bool display2, bool display3, bool display4, bool display5, bool display6, bool display7, bool display8, bool display9)
        {
            var vm = FillCardListModel(cardSer, cardDk, cardNo, cardName, company, building, validation, filter, sort_field, sort_direction, display1, display2, display3, display4, display5, display6, display7, display8, display9);
            return View("CardListExport", vm);
        }

        private IEnumerable<UsersAccessUnit> ApplyCardStatusFilter(IEnumerable<UsersAccessUnit> list, int filter)
        {
            if (filter == 1)
            {
                list = list.Where(x => x.Active && !x.Free).ToList();
            }
            else if (filter == 0)
            {
                list = list.Where(x => !x.Active && !x.Free).ToList();
            }
            else if (filter == 2)
            {
                list = list.Where(x => x.Free).ToList();
            }
            return list;
        }

        //public UserAccessUnitListViewModel FillCardListModel(string cardSer,
        //                                                        string cardDk,
        //                                                        string cardNo,
        //                                                        string cardName,
        //                                                        string company,
        //                                                        string building,
        //                                                        string validation,
        //                                                        int filter,
        //                                                        int? sort_field,
        //                                                        int? sort_direction,
        //                                                        bool display1, bool display2, bool display3, bool display4, bool display5, bool display6, bool display7, bool display8, bool display9)
        //{
        //    var uaulvm = CreateViewModel<UserAccessUnitListViewModel>();
        //    var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted);

        //    var building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
        //    var user_building_ids = GetUserBuildings(_buildingRepository, _userRepository);
        //    cards = cards.Where(x => building_ids.Contains(x.BuildingId) && user_building_ids.Contains(x.BuildingId));

        //    if (cardSer != string.Empty)
        //    {
        //        cards = cards.Where(x => x.Serial != null && x.Serial.ToLower().Contains(cardSer.Trim().ToLower())).ToList();
        //    }
        //    if (cardDk != string.Empty)
        //    {
        //        cards = cards.Where(x => x.Dk != null && x.Dk.ToLower().Contains(cardDk.Trim().ToLower())).ToList();
        //    }
        //    if (cardNo != string.Empty)
        //    {
        //        cards = cards.Where(x => x.Code != null && x.Code.ToLower().Contains(cardNo.Trim().ToLower())).ToList();
        //    }
        //    if (cardName != string.Empty)
        //    {
        //        List<int> userIds = (from u in _userRepository.FindAll(x => !x.IsDeleted && (x.FirstName + " " + x.LastName).ToLower().Contains(cardName.Trim().ToLower())) select u.Id).ToList();
        //        cards = cards.Where(x => x.UserId.HasValue && userIds.Contains(x.UserId.Value));
        //    }
        //    if (company != null && company != string.Empty)
        //    {
        //        cards = cards.Where(x => x.CompanyId.HasValue && x.Company.Name.ToLower().Contains(company.Trim().ToLower())).ToList();
        //    }
        //    else
        //    {
        //        if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
        //        {
        //            cards = cards.Where(x => x.CompanyId.HasValue && x.CompanyId == CurrentUser.Get().CompanyId).ToList();
        //        }
        //        else
        //            if (CurrentUser.Get().IsDepartmentManager)
        //        {
        //            var department = _userDepartmentRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.IsDepartmentManager == true).FirstOrDefault().Department;

        //            if (department != null)
        //            {
        //                List<int> userIds = (from u in _userRepository.FindAll(x => !x.IsDeleted && x.UserDepartments.Any(y => y.DepartmentId == department.Id)) select u.Id).ToList();
        //                cards = cards.Where(x => !x.IsDeleted && x.UserId.HasValue && userIds.Contains(x.UserId.Value)).ToList();
        //            }
        //        }
        //    }
        //    if (building != string.Empty)
        //    {
        //        cards = cards.Where(x => x.Building.Name.Trim().ToLower().Contains(building.Trim().ToLower())).ToList();
        //    }
        //    if (validation != String.Empty)
        //    {
        //        cards = cards.Where(x => x.ValidTo.HasValue && DateTime.Compare(x.ValidTo.Value, DateTime.ParseExact(validation.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture)) < 0).ToList();
        //    }

        //    cards = ApplyCardStatusFilter(cards, filter);
        //    if (validation != String.Empty)
        //    {
        //        //cards = cards.Where(x => x.ValidTo.HasValue && DateTime.Compare(x.ValidTo.Value, DateTime.ParseExact(validation.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture)) < 0).ToList();
        //        cards = cards.Where(x => x.Classificator_dt.HasValue && x.Classificator_dt.Value.Date.Equals(validation)).ToList();
        //    }
        //    IEnumerable<UserAccessUnitListItem> cards_list = new List<UserAccessUnitListItem>();

        //    Mapper.Map(cards, cards_list);

        //    if (sort_field.HasValue && sort_direction.HasValue)
        //    {
        //        switch (sort_field)
        //        {
        //            case 1:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.CardStatus) : cards_list.OrderByDescending(x => x.CardStatus);
        //                break;
        //            case 2:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.FullCardCode) : cards_list.OrderByDescending(x => x.FullCardCode);
        //                break;
        //            case 3:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.Name) : cards_list.OrderByDescending(x => x.Name);
        //                break;
        //            case 4:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.Building) : cards_list.OrderByDescending(x => x.Building);
        //                break;
        //            case 5:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.CompanyName) : cards_list.OrderByDescending(x => x.CompanyName);
        //                break;
        //            case 6:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.ValidTo) : cards_list.OrderByDescending(x => x.ValidTo);
        //                break;
        //            case 7:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.TypeName) : cards_list.OrderByDescending(x => x.TypeName);
        //                break;
        //            case 8:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.DeactivationReason) : cards_list.OrderByDescending(x => x.DeactivationReason);
        //                break;
        //            case 9:
        //                cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.DeactivationDateTime) : cards_list.OrderByDescending(x => x.DeactivationDateTime);
        //                break;
        //            default:
        //                cards_list = cards_list.OrderBy(x => x.FullCardCode);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        cards_list = cards_list.OrderBy(x => x.FullCardCode);
        //    }

        //    uaulvm.Cards = cards_list;

        //    uaulvm.CardExists = cards.Count() > 0;
        //    uaulvm.FilterCriteria = filter;

        //    foreach (var c in uaulvm.Cards)
        //    {
        //        if (c.ValidToStr == String.Empty)
        //        {
        //            c.ValidToStr = "-";
        //        }
        //    }

        //    uaulvm.IsDispalyColumn1 = display1;
        //    uaulvm.IsDispalyColumn2 = display2;
        //    uaulvm.IsDispalyColumn3 = display3;
        //    uaulvm.IsDispalyColumn4 = display4;
        //    uaulvm.IsDispalyColumn5 = display5;
        //    uaulvm.IsDispalyColumn6 = display6;
        //    uaulvm.IsDispalyColumn7 = display7;
        //    uaulvm.IsDispalyColumn8 = display8;
        //    uaulvm.IsDispalyColumn9 = display9;

        //    return uaulvm;
        //}

        public UserAccessUnitListViewModel FillCardListModel(string cardSer,
                                                        string cardDk,
                                                        string cardNo,
                                                        string cardName,
                                                        string company,
                                                        string building,
                                                        string validation,
                                                        int filter,
                                                        int? sort_field,
                                                        int? sort_direction,
                                                        bool display1, bool display2, bool display3, bool display4, bool display5, bool display6, bool display7, bool display8, bool display9)
        {
            int companyid = 0;

            if (CurrentUser.Get().CompanyId != null && !CurrentUser.Get().IsSuperAdmin)
            {
                company = _companyRepository.FindAll().Where(cc => cc.Id == (CurrentUser.Get().CompanyId)).First().Name;
                //companyid = _companyRepository.FindAll().Where(cc => cc.Id == (CurrentUser.Get().CompanyId)).First().Id;
            }
            var uaulvm = CreateViewModel<UserAccessUnitListViewModel>();
            var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted);


            var building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
            var user_building_ids = GetUserBuildings(_buildingRepository, _userRepository);
            cards = cards.Where(x => building_ids.Contains(x.BuildingId) && user_building_ids.Contains(x.BuildingId));

            //New code

            if (cardSer != string.Empty)
            {
                cards = cards.Where(x => x.Serial != null && x.Serial.ToLower().Contains(cardSer.Trim().ToLower())).ToList();
            }
            if (cardDk != string.Empty)
            {
                // ye walla code
                cards = cards.Where(x => x.Dk != null && x.Dk.ToLower().Contains(cardDk.Trim().ToLower())).ToList();
            }
            if (cardNo != string.Empty)
            {
                cards = cards.Where(x => x.Code != null && x.Code.ToLower().Contains(cardNo.Trim().ToLower())).ToList();
            }
            if (cardName != string.Empty)
            {
                List<int> userIds = (from u in _userRepository.FindAll(x => !x.IsDeleted && (x.FirstName + " " + x.LastName).ToLower().Contains(cardName.Trim().ToLower())) select u.Id).ToList();
                cards = cards.Where(x => x.UserId.HasValue && userIds.Contains(x.UserId.Value));
            }
            if (company != null && company != string.Empty)
            {
                var cards1 = cards.Where(x => x.CompanyId.HasValue && x.CompanyId == companyid).ToList();
                cards = cards.Where(x => x.CompanyId.HasValue && x.Company.Name.ToLower().Contains(company.Trim().ToLower())).ToList();
            }
            else
            {
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    cards = cards.Where(x => x.CompanyId.HasValue && (x.CompanyId == CurrentUser.Get().CompanyId || x.Company.ParentId == CurrentUser.Get().CompanyId)).ToList();
                }
                else
                if (CurrentUser.Get().IsDepartmentManager)
                {
                    var department = _userDepartmentRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.IsDepartmentManager == true).FirstOrDefault().Department;

                    if (department != null)
                    {
                        List<int> userIds = (from u in _userRepository.FindAll(x => !x.IsDeleted && x.UserDepartments.Any(y => y.DepartmentId == department.Id)) select u.Id).ToList();
                        cards = cards.Where(x => !x.IsDeleted && x.UserId.HasValue && userIds.Contains(x.UserId.Value)).ToList();
                    }
                }
            }
            if (building != string.Empty)
            {
                cards = cards.Where(x => x.Building.Name.Trim().ToLower().Contains(building.Trim().ToLower())).ToList();
            }
            if (validation != String.Empty)
            {
                DateTime dt = DateTime.ParseExact(validation.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                //  string formattedDate = dt.ToString("YYYY.MM.DD");

                // IFormatProvider culture = new CultureInfo("en-US", true);
                // DateTime dateVal = DateTime.ParseExact(validation, "yyyy-MM-dd", culture);

                //DateTime dt = DateTime.ParseExact(validation, "dd.MM.yyyy",
                //                                   CultureInfo.InvariantCulture);
                //String datetime = dt.ToString("yyyy-MM-dd" + " 00:00:00.000");

                cards = cards.Where(x => x.Classificator_dt.HasValue && x.Classificator_dt.Value.Date.Equals(dt.Date) && !x.Active && !x.Free).ToList();
            }
          
            cards = ApplyCardStatusFilter(cards, filter);

            IEnumerable<UserAccessUnitListItem> cards_list = new List<UserAccessUnitListItem>();
            Mapper.Map(cards, cards_list);

            if (sort_field.HasValue && sort_direction.HasValue)
            {
                switch (sort_field)
                {
                    case 1:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.CardStatus) : cards_list.OrderByDescending(x => x.CardStatus);
                        break;
                    case 2:
                        //  cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.DeactivationDateTime) : cards_list.OrderByDescending(x => x.DeactivationDateTime);
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.FullCardCode) : cards_list.OrderByDescending(x => x.FullCardCode);
                        break;
                    case 3:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.Name) : cards_list.OrderByDescending(x => x.Name);
                        break;
                    case 4:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.Building) : cards_list.OrderByDescending(x => x.Building);
                        break;
                    case 5:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.CompanyName) : cards_list.OrderByDescending(x => x.CompanyName);
                        break;
                    case 6:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.ValidTo) : cards_list.OrderByDescending(x => x.ValidTo);
                        break;
                    case 7:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.TypeName) : cards_list.OrderByDescending(x => x.TypeName);
                        break;
                    case 8:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.DeactivationReason) : cards_list.OrderByDescending(x => x.DeactivationReason);
                        break;
                    case 9:
                        cards_list = sort_direction.Value == 0 ? cards_list.OrderBy(x => x.DeactivationDateTime) : cards_list.OrderByDescending(x => x.DeactivationDateTime);
                        break;
                    default:
                        cards_list = cards_list.OrderBy(x => x.FullCardCode);
                        break;
                }
            }
            else
            {
                cards_list = cards_list.OrderBy(x => x.FullCardCode);
            }

            uaulvm.Cards = cards_list;

            uaulvm.CardExists = cards.Count() > 0;

            if (cards.Count() > 0)
            {
                uaulvm.IsInList = 0;
            }
            else
            {
                if (CurrentUser.Get().CompanyId != null && !CurrentUser.Get().IsSuperAdmin)
                {
                    var cards1 = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted);
                    if (cardSer != string.Empty)
                    {
                        cards1 = cards1.Where(x => x.Serial != null && x.Serial.ToLower().Contains(cardSer.Trim().ToLower())).ToList();
                    }
                    if (cardDk != string.Empty)
                    {
                        // ye walla code
                        cards1 = cards1.Where(x => x.Dk != null && x.Dk.ToLower().Contains(cardDk.Trim().ToLower())).ToList();
                    }
                    if (cardNo != string.Empty)
                    {
                        cards1 = cards1.Where(x => x.Code != null && x.Code.ToLower().Contains(cardNo.Trim().ToLower())).ToList();
                    }
                    if (cardName != string.Empty)
                    {
                        List<int> userIds = (from u in _userRepository.FindAll(x => !x.IsDeleted && (x.FirstName + " " + x.LastName).ToLower().Contains(cardName.Trim().ToLower())) select u.Id).ToList();
                        cards1 = cards1.Where(x => x.UserId.HasValue && userIds.Contains(x.UserId.Value));
                    }
                    if (cards1.Count() > 0)
                    {
                        uaulvm.IsInList = 1;
                    }
                    else
                    {
                        uaulvm.IsInList = 0;
                    }
                }
                else
                {
                    uaulvm.IsInList = 0;
                }
            }

            uaulvm.FilterCriteria = filter;

            foreach (var c in uaulvm.Cards)
            {
                if (c.ValidToStr == String.Empty)
                {
                    c.ValidToStr = "-";
                }
            }

            uaulvm.IsDispalyColumn1 = display1;
            uaulvm.IsDispalyColumn2 = display2;
            uaulvm.IsDispalyColumn3 = display3;
            uaulvm.IsDispalyColumn4 = display4;
            uaulvm.IsDispalyColumn5 = display5;
            uaulvm.IsDispalyColumn6 = display6;
            uaulvm.IsDispalyColumn7 = display7;
            uaulvm.IsDispalyColumn8 = display8;
            uaulvm.IsDispalyColumn9 = display9;

            return uaulvm;
        }


        #endregion

        #region User List Export
        public PdfResult UserListPDF(string name,
                                        string cardSer,
                                        string cardDk,
                                        string cardCode,
                                        string company,
                                        string title,
                                        int filter,
                                        int departmentId,
                                        int? sort_field,
                                        int? sort_direction,
                                        bool display1, bool display2, bool display3, bool display4, bool display5, bool display6)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.UserListExport(name, cardSer, cardDk, cardCode, company, title, filter, departmentId, sort_field, sort_direction, display1, display2, display3, display4, display5, display6));
            return new PdfResult(html, "UserList", true, PDFPageOrientation.Portrait);
        }

        public PdfResult VisitorListPDF(string name,
                                      string company,
                                      int filter,
                                      int? sort_field,
                                      int? sort_direction,
                                      bool display1, bool display2, bool display3, bool display5, bool display4, bool display6, bool display7)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.VisitorListExport(name, company, filter, sort_field, sort_direction, display1, display2, display3, display5, display4, display6, display7));
            return new PdfResult(html, "VisitorList", true, PDFPageOrientation.Portrait);
        }

        public ExcelResult UserListExcel(string name,
                                            string cardSer,
                                            string cardDk,
                                            string cardCode,
                                            string company,
                                            string title,
                                            int filter,
                                            int departmentId,
                                            int? sort_field,
                                            int? sort_direction,
                                            bool display1, bool display2, bool display3, bool display4, bool display5, bool display6)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.UserListExport(name, cardSer, cardDk, cardCode, company, title, filter, departmentId, sort_field, sort_direction, display1, display2, display3, display4, display5, display6));
            return new ExcelResult(html, "UserList", true);
        }

        public ExcelResult VisitorListExcel(string name,
                                           string company,
                                           int filter,
                                           int? sort_field,
                                           int? sort_direction,
                                           bool display1, bool display2, bool display3, bool display5, bool display4, bool display6, bool display7)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.VisitorListExport(name, company, filter, sort_field, sort_direction, display1, display2, display3, display5, display4, display6, display7));
            return new ExcelResult(html, "VisitorList", true);
        }

        public ActionResult UserListExport(string name,
                                            string cardSer,
                                            string cardDk,
                                            string cardCode,
                                            string company,
                                            string title,
                                            int filter,
                                            int departmentId,
                                            int? sort_field,
                                            int? sort_direction,
                                            bool display1, bool display2, bool display3, bool display4, bool display5, bool display6)
        {
            var vm = FillUserListModel(name, cardSer, cardDk, cardCode, company, title, filter, departmentId, sort_field, sort_direction, display1, display2, display3, display4, display5, display6);
            return View("UserListExport", vm);
        }


        public ActionResult VisitorListExport(string name,
                                            string company,
                                            int filter,
                                            int? sort_field,
                                            int? sort_direction,
                                            bool display1, bool display2, bool display3, bool display5, bool display4, bool display6, bool display7)
        {
            var vm = FillVisitorListModel(name, company, filter, sort_field, sort_direction, display1, display2, display3, display5, display4, display6, display7);
            return View("VisitorListExport", vm);
        }
        private IEnumerable<User> ApplyUserStatusFilter(IEnumerable<User> users, int filter)
        {
            if (filter == 1)
            {
                users = users.Where(x => x.Active).ToList();
            }
            else if (filter == 0)
            {
                users = users.Where(x => !x.Active).ToList();
            }
            return users;
        }

        private IEnumerable<Visitor> ApplyVisitorStatusFilter(IEnumerable<Visitor> users, int filter)
        {
            if (filter == 1)
            {
                users = users.Where(x => x.StopDateTime >= DateTime.Now).ToList();
            }
            else if (filter == 0)
            {
                users = users.Where(x => x.StopDateTime < DateTime.Now).ToList();
            }
            else
            {
                users = users.Where(x => !x.IsDeleted).ToList();
            }
            return users;
        }

        public UserListViewModel FillUserListModel(string name,
                                                    string cardSer,
                                                    string cardDk,
                                                    string cardCode,
                                                    string company,
                                                    string title,
                                                    int filter,
                                                    int departmentId,
                                                    int? sort_field,
                                                    int? sort_direction,
                                                    bool display1, bool display2, bool display3, bool display4, bool display5, bool display6)
        {
            var uvm = CreateViewModel<UserListViewModel>();

            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            if (!CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin)
            {
                users = users.Where(us => us.CompanyId != null).ToList();
            }

            users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);

            if (name != String.Empty)
            {
                string[] split = name.ToLower().Trim().Split(' ');

                if (split.Count() == 1)
                {
                    users = users.Where(x => (x.FirstName.ToLower().Contains(split[0]) || (x.PersonalId != null && x.PersonalId.ToLower().Contains(split[0])))).ToList();
                }
                else if (split.Count() == 2)
                {
                    users = users.Where(x => x.FirstName.ToLower().Contains(split[0]) && x.LastName.ToLower().Contains(split[1])).ToList();
                }
            }

            if (company != null && company != String.Empty)
            {
                var comp = _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower() == company.ToLower()).FirstOrDefault();

                if (comp != null)
                {
                    int companyId = comp.Id;
                    List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == companyId || x.ParentId == companyId)) select c.Id).ToList();
                    users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
                }
            }
            else
            {
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    int companyId = CurrentUser.Get().CompanyId.Value;
                    List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == companyId || x.ParentId == companyId)) select c.Id).ToList();
                    users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
                }
                if (CurrentUser.Get().IsDepartmentManager)
                {
                    var user_department = _userDepartmentRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.IsDepartmentManager == true).FirstOrDefault();
                    users = user_department != null ? users.Where(x => x.UserDepartments.Any(y => y.DepartmentId == user_department.DepartmentId)).ToList() : new List<User>();
                }
                //if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsCompanyManager)
                //{
                //    var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                //    var buildIds = user_buildings.Select(ub => ub.BuildingId).ToList();
                //    if (user_buildings != null)
                //    {
                //        users = users.Where(us => us.UserBuildings != null && us.UserBuildings.Any(ubo => buildIds.Contains(ubo.BuildingId))).ToList();
                //    }
                //}
            }

            if (title != String.Empty)
            {
                var t = _titleRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower() == title.ToLower()).FirstOrDefault();
                users = users.Where(x => x.TitleId.HasValue && x.TitleId.Value == t.Id).ToList();
            }

            if (departmentId != 0)
            {
                users = users.Where(x => x.UserDepartments.Where(ud => !ud.IsDeleted && ud.DepartmentId == departmentId).FirstOrDefault() != null).ToList();
            }

            users = ApplyUserStatusFilter(users, filter).ToList();

            var filt_users = new List<UserItem>();
            Mapper.Map(users, filt_users);

            if (!string.IsNullOrWhiteSpace(cardCode))
            {
                filt_users =
                    filt_users.Where(
                        us =>
                        !string.IsNullOrWhiteSpace(us.CardNumber) && !us.CardNumber.Contains('+') && us.CardNumber.Contains(cardCode)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(cardSer))
            {
                filt_users =
                    filt_users.Where(
                        us =>
                        !string.IsNullOrWhiteSpace(us.CardNumber) && us.CardNumber.Contains('+') && us.CardNumber.Split('+')[0].Contains(cardSer)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(cardDk))
            {
                filt_users =
                    filt_users.Where(
                        us =>
                        !string.IsNullOrWhiteSpace(us.CardNumber) && us.CardNumber.Contains('+') && us.CardNumber.Split('+')[1].Contains(cardDk)).ToList();
            }

            if (sort_field.HasValue && sort_direction.HasValue)
            {
                switch (sort_field)
                {
                    case 1:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName).ToList();
                        break;
                    case 2:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.CardNumber).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.CardNumber).ToList();
                        break;
                    case 3:
                        if (!CurrentUser.Get().IsDepartmentManager && !CurrentUser.Get().IsCompanyManager)
                        {
                            if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.CompanyName).ToList();
                            else filt_users = filt_users.OrderByDescending(x => x.CompanyName).ToList();
                        }
                        else
                        {
                            if (CurrentUser.Get().IsCompanyManager)
                            {
                                if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.DepartmentName).ToList();
                                else filt_users = filt_users.OrderByDescending(x => x.DepartmentName).ToList();
                            }
                            else
                            {
                                if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.TitleName).ToList();
                                else filt_users = filt_users.OrderByDescending(x => x.TitleName).ToList();
                            }
                        }
                        break;
                    case 4:
                        if (!CurrentUser.Get().IsDepartmentManager && !CurrentUser.Get().IsCompanyManager)
                        {
                            if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.DepartmentName).ToList();
                            else filt_users = filt_users.OrderByDescending(x => x.DepartmentName).ToList();
                        }
                        else
                        {
                            if (CurrentUser.Get().IsCompanyManager)
                            {
                                if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.TitleName).ToList();
                                else filt_users = filt_users.OrderByDescending(x => x.TitleName).ToList();
                            }
                            else
                            {
                                filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.ValidTo).ToList() : filt_users.OrderByDescending(x => x.ValidTo).ToList();
                            }
                        }
                        break;
                    case 5:
                        if (CurrentUser.Get().IsCompanyManager)
                        {
                            if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.ValidTo).ToList();
                            else filt_users = filt_users.OrderByDescending(x => x.ValidTo).ToList();
                        }
                        else
                        {
                            if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.Roles).ToList();
                            else filt_users = filt_users.OrderByDescending(x => x.Roles).ToList();
                        }
                        break;
                    case 6:
                        filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.UserStatus).ToList() : filt_users.OrderByDescending(x => x.UserStatus).ToList();
                        break;
                    default:
                        filt_users = filt_users.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).ToList();
                        break;
                }
            }
            else
            {
                filt_users = filt_users.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).ToList();
            }

            foreach (var obj in filt_users)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select top 1 Name from UserPermissionGroups where IsDeleted=0 and PermissionIsActive=1 and userid='" + obj.Id + "'", con);
                obj.UserPermissionGroupName = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }
            uvm.Users = filt_users;
            uvm.IsDispalyColumn1 = display1;
            uvm.IsDispalyColumn2 = display2;
            uvm.IsDispalyColumn3 = display3;
            uvm.IsDispalyColumn4 = display4;
            uvm.IsDispalyColumn5 = display5;
            uvm.IsDispalyColumn6 = display6;
            return uvm;
        }

        public VisitorListViewModel FillVisitorListModel(string name,
                                                   string company,
                                                   int filter,
                                                   int? sort_field,
                                                   int? sort_direction,
                                                   bool display1, bool display2, bool display3, bool display5, bool display4, bool display6, bool display7)
        {
            var uvm = CreateViewModel<VisitorListViewModel>();


            List<Visitor> users = _visitorRepository.FindAll(x => !x.IsDeleted).ToList();

            if (name != String.Empty)
            {
                string[] split = name.ToLower().Trim().Split(' ');

                if (split.Count() == 1)
                {
                    users = users.Where(x => (x.FirstName.ToLower().Contains(split[0]))).ToList();
                }
                else if (split.Count() == 2)
                {
                    users = users.Where(x => x.FirstName.ToLower().Contains(split[0]) && x.LastName.ToLower().Contains(split[1])).ToList();
                }
            }

            if (company != null && company != String.Empty)
            {
                var comp = _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower() == company.ToLower()).FirstOrDefault();

                if (comp != null)
                {
                    int companyId = comp.Id;
                    List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == companyId || x.ParentId == companyId)) select c.Id).ToList();
                    users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
                }
            }
            else
            {
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    int companyId = CurrentUser.Get().CompanyId.Value;
                    List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == companyId || x.ParentId == companyId)) select c.Id).ToList();
                    users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
                }
            }

            users = ApplyVisitorStatusFilter(users, filter).ToList();
            foreach (var obj in users)
            {
                string joinname = "";

                if (obj.UserId != null)
                {
                    string firstname = _userRepository.FindById(Convert.ToInt32(obj.UserId)).FirstName;
                    string lastname = _userRepository.FindById(Convert.ToInt32(obj.UserId)).LastName;
                    if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname))
                    {
                        joinname = "(" + firstname + " " + lastname + ")";
                    }
                    obj.LastName = obj.LastName + " " + joinname;
                }

                obj.ToDate = obj.StopDateTime.ToString();
                if (!String.IsNullOrEmpty(obj.ToDate))
                {
                    obj.ToDate = Convert.ToDateTime(obj.StopDateTime).ToString("dd.MM.yyyy HH:mm");
                }

                obj.FromDate = obj.StartDateTime.ToString();
                if (!String.IsNullOrEmpty(obj.FromDate))
                {
                    obj.FromDate = Convert.ToDateTime(obj.StartDateTime).ToString("dd.MM.yyyy HH:mm");
                }

                if (!String.IsNullOrEmpty(obj.StopDateTime.ToString()))
                {
                    DateTime validto = Convert.ToDateTime(obj.StopDateTime);
                    if (obj.ReturnDate == null)
                    {
                        obj.DateReturn = null;
                    }
                    if (obj.ReturnDate == (validto).AddDays(1))
                    {
                        obj.DateReturn = Convert.ToDateTime(obj.ReturnDate.ToString()).ToString("dd.MM.yyyy HH:mm");
                    }
                    else
                    {
                        obj.DateReturn = null;
                    }
                }
                else
                {
                    obj.DateReturn = null;
                }

                obj.ChangeLast = obj.LastChange.ToString();
                if (!String.IsNullOrEmpty(obj.ChangeLast))
                {
                    obj.ChangeLast = Convert.ToDateTime(obj.LastChange).ToString("dd.MM.yyyy HH:mm");
                }

                if (obj.CompanyId == null)
                {
                    obj.Company = "";
                }
                else
                {
                    List<Company> companies = _companyRepository.FindAll(x => x.Id == (int)obj.CompanyId).ToList();
                    if (companies.Count == 0)
                    {
                        obj.Company = "";
                    }
                    else
                    {
                        foreach (var cc in companies)
                        {
                            obj.Company = cc.Name;
                        }
                    }
                }
            }
            var filt_users = new List<VisitorItem>();
            Mapper.Map(users, filt_users);


            if (sort_field.HasValue && sort_direction.HasValue)
            {
                switch (sort_field)
                {
                    case 1:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.UserStatus.ToUpper()).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.UserStatus.ToUpper()).ToList();
                        break;
                    case 2:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.FirstName.ToUpper()).ThenBy(x => x.LastName.ToUpper()).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.FirstName.ToUpper()).ThenByDescending(x => x.LastName.ToUpper()).ToList();
                        break;
                    case 3:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.CompanyName.ToUpper()).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.CompanyName.ToUpper()).ToList();
                        break;
                    case 5:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.StartDateTime).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.StartDateTime).ToList();
                        break;
                    case 6:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.DateReturn).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.DateReturn).ToList();
                        break;
                    case 7:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.LastChange).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.LastChange).ToList();
                        break;

                    case 4:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.StopDateTime).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.StopDateTime).ToList();
                        break;
                    default:
                        filt_users = filt_users.OrderByDescending(x => x.StopDateTime).ToList();
                        break;
                }
            }
            else
            {
                filt_users = filt_users.OrderByDescending(x => x.StopDateTime).ToList();
            }


            uvm.Visitors = filt_users;
            uvm.IsDispalyColumn1 = display1;
            uvm.IsDispalyColumn2 = display2;
            uvm.IsDispalyColumn3 = display3;
            uvm.IsDispalyColumn5 = display5;
            uvm.IsDispalyColumn4 = display4;
            uvm.IsDispalyColumn6 = display6;
            uvm.IsDispalyColumn7 = display7;
            return uvm;
        }

        #endregion
        #region
        public PdfResult LocListPDF(LogFilterItem filter, int? sort_field, int? sort_direction, int? reportId, string report, string company,int reptype)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.LocListExport(filter, sort_field, sort_direction, reportId, report, company, reptype));
            return new PdfResult(html, "LogList", true, PDFPageOrientation.Portrait);
        }

        public ExcelResult LocListExcel(LogFilterItem filter, int? sort_field, int? sort_direction, int? reportId, string report, string company, int reptype)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.LocListExport(filter, sort_field, sort_direction, reportId, report, company, reptype));
            return new ExcelResult(html, "LogList", true);
        }

        public ActionResult LocListExport(LogFilterItem filter, int? sort_field, int? sort_direction, int? reportId, string report, string company, int reptype)
        {
            var vm = FillLocListModel(filter, sort_field, sort_direction, reportId, report, company, reptype);
            return View("LogListExport", vm);
        }

        public LogListViewModel FillLocListModel(LogFilterItem filter, int? sort_field, int? sort_direction, int? reportId, string report, string companyname, int type)
        {
            var FocSecModelcontexLoction = ConfigurationManager.ConnectionStrings["FoxSecDBEntities"].ConnectionString;

            System.Data.Entity.Core.Objects.ObjectContext Sql = new System.Data.Entity.Core.Objects.ObjectContext(FocSecModelcontexLoction);
            var lvm = CreateViewModel<LogListViewModel>();
            var logs = new List<FoxSec.DomainModel.DomainObjects.Log>();

            //List<Log> filteredLogs = new List<Log>();
            //bool isCommonsearch = false;
            //bool isFilterApplied = false;
            if (!string.IsNullOrWhiteSpace(filter.CommonSearch))
            {
                filter.Activity = filter.CommonSearch;
                filter.Building = filter.CommonSearch;
                var company = _companyRepository.FindAll().Where(cc => cc.Name.ToLower().Contains(filter.CommonSearch.ToLower())).FirstOrDefault();
                if (company != null)
                {
                    filter.CompanyId = company.Id;
                }
                else
                {
                    filter.CompanyId = null;
                }
                filter.Node = filter.CommonSearch;
                filter.UserName = filter.CommonSearch;
            }

            var log_filter = new LogFilter();

            log_filter.Activity = filter.Activity;
            log_filter.Building = filter.Building;
            log_filter.Node = string.IsNullOrWhiteSpace(filter.Node) ? string.Empty : filter.Node.Trim();
            log_filter.Building = filter.Building;
            log_filter.UserName = filter.UserName;
            log_filter.CompanyId = filter.CompanyId;

            log_filter.IsShowDefaultLog = filter.IsShowDefaultLog;
            if (string.IsNullOrWhiteSpace(filter.FromDate))
            {
                log_filter.FromDate = null;
            }
            else
            {
                log_filter.FromDate = DateTime.ParseExact(filter.FromDate, "dd.MM.yyyy HH:mm",
                                                          CultureInfo.InvariantCulture);
            }
            if (string.IsNullOrWhiteSpace(filter.ToDate))
            {
                log_filter.ToDate = null;
            }
            else
            {
                log_filter.ToDate = DateTime.ParseExact(filter.ToDate, "dd.MM.yyyy HH:mm",
                                                          CultureInfo.InvariantCulture);
            }

            var restr_user_ids = GetRestrictedUserIds();

            var allowed_user_ids = GetAllowedUserIds(filter.UserName);

            allowed_user_ids = allowed_user_ids.Where(x => !restr_user_ids.Contains(x)).ToList();

            var allowed_company_ids = GetAllowedCompanies(filter.CompanyId);

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                allowed_company_ids = GetAllowedCompanies(CurrentUser.Get().CompanyId);
            }

            int searched_rows_count = 0;

            // creating sql query
            string Query = "SELECT DISTINCT TOP (100) PERCENT Log_1.Id AS LogId FROM            (SELECT        UserId, MAX(EventTime) AS MaxEventTime                          FROM            dbo.[Log]                          WHERE        (LogTypeId = 30 OR                                                    LogTypeId = 31) AND (EventTime BETWEEN '01-09-2013 15:58:10' AND '01-10-2013 15:58:10')                          GROUP BY UserId) AS MaxLog INNER JOIN                         dbo.[Log] AS Log_1 ON MaxLog.UserId = Log_1.UserId AND MaxLog.MaxEventTime = Log_1.EventTime INNER JOIN                         dbo.FSSublocationObjects ON Log_1.BuildingObjectId = dbo.FSSublocationObjects.BuildingObjectId AND                          Log_1.LogTypeId = dbo.FSSublocationObjects.LogTypeId INNER JOIN                         dbo.FSSublocations ON dbo.FSSublocationObjects.SublocationId = dbo.FSSublocations.Id LEFT OUTER JOIN                         dbo.BuildingObjects ON dbo.FSSublocationObjects.BuildingObjectId = dbo.BuildingObjects.Id WHERE        (dbo.FSSublocations.Id = ReportsId) AND (dbo.FSSublocationObjects.IsDeleted = 0)";

            //date filter
            string todate = String.Format("{0:MM-dd-yyyy HH:mm:ss}", log_filter.ToDate);
            string fromdate = String.Format("{0:MM-dd-yyyy HH:mm:ss}", log_filter.FromDate);
            Query = Query.Replace("01-09-2013 15:58:10", fromdate);
            Query = Query.Replace("01-10-2013 15:58:10", todate);

            Query = Query.Replace("ReportsId", reportId.Value.ToString());

            /*
            if (filter.CompanyId != null)
            {
                Query = Query.Replace("(dbo.FSSublocationObjects.IsDeleted = 0)", "(dbo.FSSublocationObjects.IsDeleted = 0) AND (dbo.Users.CompanyId = {0})");
                Query = Query.Replace("{0}", filter.CompanyId.ToString());
            }
            if (CurrentUser.Get().IsCompanyManager)
            {
                Query = Query.Replace("(dbo.FSSublocationObjects.IsDeleted = 0)", "(dbo.FSSublocationObjects.IsDeleted = 0) AND (dbo.Users.CompanyId = {0})");
                Query = Query.Replace("{0}", CurrentUser.Get().CompanyId.ToString());
            }


            */

            var logtest = Sql.ExecuteStoreQuery<SqlToLocation>(Query);
            if (logtest != null)
            {

                if (filter.CompanyId.HasValue || CurrentUser.Get().IsCompanyManager)
                {
                    int CompId = filter.CompanyId.HasValue ? filter.CompanyId.GetValueOrDefault(0) : CurrentUser.Get().CompanyId.GetValueOrDefault(0);
                    foreach (var logt in logtest)
                    {
                        searched_rows_count = searched_rows_count + 1;
                        var qwe = _logRepository.FindById(logt.LogId);
                        if (qwe.CompanyId == CompId)
                        {
                            logs.Add(qwe);
                        }
                    }
                }
                else
                {
                    foreach (var logt in logtest)
                    {
                        searched_rows_count = searched_rows_count + 1;
                        var qwe = _logRepository.FindById(logt.LogId);
                        logs.Add(qwe);
                    }
                }
            }

            logs.OrderBy(s => s.User.LastName).ToList();

            IEnumerable<LogItem> list = new List<LogItem>();

            Mapper.Map(logs, list);
            var usrid = list.Select(x => x.UserId).Distinct().Count();
            lvm.Items = list;
            lvm.Company = companyname;
            lvm.Report = report;
            lvm.DateFrom =Convert.ToDateTime(fromdate).ToString("dd.MM.yyyy HH:mm");
            lvm.DateTo = Convert.ToDateTime(todate).ToString("dd.MM.yyyy HH:mm");
            lvm.ReportType = type;
            lvm.TotalUsers = usrid;
            return lvm;
        }

        #endregion

        #region Log List Export
        public PdfResult LogListPDF(LogFilterItem filter, int? sort_field, int? sort_direction)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.LogListExport(filter, sort_field, sort_direction));
            return new PdfResult(html, "LogList", true, PDFPageOrientation.Portrait);
        }

        public ExcelResult LogListExcel(LogFilterItem filter, int? sort_field, int? sort_direction)
        {
            string html = this.CaptureActionHtml(this, c => (ViewResult)c.LogListExport(filter, sort_field, sort_direction));
            return new ExcelResult(html, "LogList", true);
        }

        public ActionResult LogListExport(LogFilterItem filter, int? sort_field, int? sort_direction)
        {
            var vm = FillLogListModel(filter, sort_field, sort_direction);
            return View("LogListExport", vm);
        }

        public LogListViewModel FillLogListModel(LogFilterItem filter, int? sort_field, int? sort_direction)
        {
            var lvm = CreateViewModel<LogListViewModel>();
            var logs = new List<FoxSec.DomainModel.DomainObjects.Log>();
            //List<Log> filteredLogs = new List<Log>();
            //bool isCommonsearch = false;
            //bool isFilterApplied = false;
            filter.CompId = filter.CompanyId;
            if (!string.IsNullOrWhiteSpace(filter.CommonSearch))
            {
                //isCommonsearch = true;
                filter.Activity = filter.CommonSearch;
                filter.Building = filter.CommonSearch;
                var company = _companyRepository.FindAll().Where(cc => cc.Name.ToLower().Contains(filter.CommonSearch.ToLower())).FirstOrDefault();
                if (company != null)
                {
                    filter.CompanyId = company.Id;
                }
                else
                {
                    filter.CompanyId = null;
                }
                filter.Node = filter.CommonSearch;
                filter.UserName = filter.CommonSearch;
            }

            var log_filter = new LogFilter
            {
                Activity = filter.Activity,
                Building = filter.Building,
                Node = string.IsNullOrWhiteSpace(filter.Node) ? string.Empty : filter.Node.Trim()
            };
            log_filter.Building = filter.Building;
            log_filter.UserName = filter.UserName;
            log_filter.CompanyId = filter.CompanyId;
            log_filter.IsShowDefaultLog = filter.IsShowDefaultLog;
            if (string.IsNullOrWhiteSpace(filter.FromDate))
            {
                log_filter.FromDate = null;
            }
            else
            {
                log_filter.FromDate = DateTime.ParseExact(filter.FromDate, "dd.MM.yyyy HH:mm",
                                                          CultureInfo.InvariantCulture);
            }
            if (string.IsNullOrWhiteSpace(filter.ToDate))
            {
                log_filter.ToDate = null;
            }
            else
            {
                log_filter.ToDate = DateTime.ParseExact(filter.ToDate, "dd.MM.yyyy HH:mm",
                                                          CultureInfo.InvariantCulture);
            }

            var restr_user_ids = GetRestrictedUserIds();

            var allowed_user_ids = GetAllowedUserIds(filter.UserName);

            allowed_user_ids = allowed_user_ids.Where(x => !restr_user_ids.Contains(x)).ToList();

            var allowed_company_ids = GetAllowedCompanies(filter.CompanyId);

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                allowed_company_ids = GetAllowedCompanies(CurrentUser.Get().CompanyId);
            }

            int searched_rows_count = 0;

            if (string.IsNullOrWhiteSpace(filter.CommonSearch))
            {
                logs = _logRepository.GetSerachedRecords(log_filter, allowed_user_ids, allowed_company_ids, null,
                                                         null, sort_direction, sort_field, out searched_rows_count).
                    ToList();
            }
            else
            {
                logs = _logRepository.GetSearchedRecordsCommonSearch(log_filter, allowed_user_ids, restr_user_ids, allowed_company_ids,
                                                                     null, null, sort_direction, sort_field,
                                                                     out searched_rows_count,
                                                                     CurrentUser.Get().IsCompanyManager, CurrentUser.Get().IsSuperAdmin, filter.CompId).ToList();
            }

            IEnumerable<LogItem> list = new List<LogItem>();
            Mapper.Map(logs, list);

            lvm.Items = list;

            return lvm;
        }

        private void RegisterLogRelatedMap()
        {
            Mapper.CreateMap<FoxSec.DomainModel.DomainObjects.Log, LogItem>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? string.Format("{0} {1}", src.User.FirstName, src.User.LastName) : ""))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : ""))
                .ForMember(dest => dest.EventTimeStr, opt => opt.MapFrom(src => src.EventTime.ToString("dd.MM.yyyy - HH:mm:ss")))
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => GetLogAction(src.Action)));
        }

        private string GetLogAction(string action)
        {
            try
            {
                XElement xx = XElement.Parse(action);
                var sb = new StringBuilder();
                foreach (var xElement in xx.Elements(XMLLogLiterals.LOG_TRANSLATABLE_SENTENSE))
                {
                    IEnumerable<string> sentense_params = from param in xElement.Elements(XMLLogLiterals.LOG_SENTENSE_PARAM)
                                                          select param.Value;
                    var resource_name = xElement.Attribute(XMLLogLiterals.LOG_SENTENCE_FORMAT).Value;
                    string message_template = (string)_resourceManager.GetObject(resource_name, Thread.CurrentThread.CurrentCulture);
                    var message = sentense_params == null ? message_template : string.Format(message_template, sentense_params.ToArray());

                    sb.Append(string.Format("{0} ", message));
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return action;
            }
        }

        #endregion

        private Dictionary<string, bool> GetRequestData()
        {
            var cbxKeys = Request.QueryString.AllKeys.Where(x => x.StartsWith("cbx")).ToList();
            Dictionary<string, bool> showColumns = new Dictionary<string, bool>();
            foreach (var k in cbxKeys)
            {
                showColumns.Add(k, Request.QueryString[k] == "true" ? true : false);
            }
            return showColumns;
        }

        private bool GetDictVal(Dictionary<string, bool> dict, string key)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return false;
        }

        public string GetDepartmentManagerName(int departmentId)
        {
            return string.Join(", ", (from m in GetDepartmentManagers(departmentId) select m.User.FirstName + " " + m.User.LastName).ToArray());
        }

        public IEnumerable<UserDepartment> GetDepartmentManagers(int departmentId)
        {
            int companyId = _departmentRepository.FindById(departmentId).CompanyId;

            var companyManagers = _companyService.GetCompanyManagers(companyId);
            IEnumerable<UserDepartment> managersList = _userDepartmentService.GetDepartmentManagers(departmentId).Where(ud => ud.Department.CompanyId == companyId);

            managersList = managersList.Where(ud => companyManagers.ContainsKey(ud.UserId));
            return managersList;
        }

        private List<int> GetAllowedCompanies(int? companyId)
        {
            var result = new List<int>();
            if (companyId != null)
            {
                result.AddRange(from cc in _companyRepository.FindAll(x => x.ParentId == companyId) select cc.Id);
                result.Add(companyId.Value);
            }
            else
            {
                result.AddRange(from cc in _companyRepository.FindAll(x => x.ParentId == companyId) select cc.Id);
            }
            return result;
        }

        private List<int> GetAllowedUserIds(string filtUserName)
        {
            if (string.IsNullOrEmpty(filtUserName))
            {
                return (from us in _userRepository.FindAll() select us.Id).ToList();
            }

            string[] split = filtUserName.ToLower().Trim().Split(' ');
            switch (split.Count())
            {
                case 1:
                    return (from us in _userRepository.FindAll(
                                x => x.FirstName.ToLower().Contains(split[0]) ||
                                    x.LastName.ToLower().Contains(split[0]))
                            select us.Id).ToList();

                case 2:
                    return (from us in _userRepository.FindAll(
                                x => x.FirstName.ToLower().Contains(split[0]) && x.LastName.ToLower().Contains(split[1]))
                            select us.Id).ToList();

                default:

                    return (from us in _userRepository.FindAll(
                                x => x.FirstName.ToLower().Contains(filtUserName.ToLower()) || x.FirstName.ToLower().Contains(filtUserName.ToLower())
                        || string.Format("{0} {1}", x.FirstName.ToLower(), x.LastName.ToLower()).Contains(filtUserName))
                            select us.Id).ToList();
            }
        }

        private List<int> GetRestrictedUserIds()
        {
            var restr_user_ids = new List<int>();
            if (CurrentUser.Get().IsBuildingAdmin)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Where(x => !x.IsDeleted).Select(ub => ub.BuildingId).ToList();
                restr_user_ids = (from us in
                                      _userRepository.FindAll().Where(
                                          x =>
                                          x.UserRoles.Any(
                                              ur =>
                                              !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now &&
                                              ur.Role.RoleTypeId < (int)RoleTypeEnum.BA)
                                              || !x.UserBuildings.Any(ub => !ub.IsDeleted && buildIds.Contains(ub.BuildingId)))
                                  select us.Id).ToList();
            }

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Where(x => !x.IsDeleted).Select(ub => ub.BuildingId).ToList();
                restr_user_ids = (from us in
                                      _userRepository.FindAll().Where(
                                          x =>
                                          x.UserRoles.Any(
                                              ur =>
                                              !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now &&
                                              ur.Role.RoleTypeId < (int)RoleTypeEnum.CM) || !x.UserBuildings.Any(ub => !ub.IsDeleted && buildIds.Contains(ub.BuildingId))
                                              || x.CompanyId != CurrentUser.Get().CompanyId || (x.Company != null && x.Company.ParentId != CurrentUser.Get().CompanyId))
                                  select us.Id).ToList();
            }
            return restr_user_ids;
        }
    }
}