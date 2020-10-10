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
    public class UserController : PaginatorControllerBase<UserItem>
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
        private readonly IUserRoleRepository _userrolerepository;

        public UserController(IUserService userService,
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
                                IUserRoleRepository userrolerepository,
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
            _userrolerepository = userrolerepository;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        private static string GeneratePasswordString()
        {
            string password = "";
            const int passwordLength = 10;
            const int quantity = 1;
            var arrCharPool = new ArrayList();
            var rndNum = new Random();
            arrCharPool.Clear();

            for (int i = 97; i < 123; i++)
            {
                arrCharPool.Add(Convert.ToChar(i).ToString());
            }

            for (int i = 48; i < 58; i++)
            {
                arrCharPool.Add(Convert.ToChar(i).ToString());
            }

            for (int i = 65; i < 91; i++)
            {
                arrCharPool.Add(Convert.ToChar(i).ToString());
            }

            for (int x = 0; x < quantity; x++)
            {
                for (int i = 0; i < passwordLength; i++)
                {
                    password += arrCharPool[rndNum.Next(arrCharPool.Count)].ToString();
                }
            }
            return password;
        }

        private static string GeneratePinString()
        {
            string pin = "";
            const int pinLength = 8;
            const int quantity = 1;
            var arrCharPool = new ArrayList();
            var rndNum = new Random();
            arrCharPool.Clear();
            for (int i = 48; i < 58; i++)
            {
                arrCharPool.Add(Convert.ToChar(i).ToString());
            }
            for (int x = 0; x < quantity; x++)
            {
                for (int i = 0; i < pinLength; i++)
                {
                    pin += arrCharPool[rndNum.Next(arrCharPool.Count)].ToString();
                }
            }
            return pin;
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

        [HttpGet]
        [OutputCache(Duration = 0, VaryByParam = "None")]
        public JsonResult GeneratePassword()
        {
            var regexp = new Regex("^.*(?=.{6,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).*$");
            var password = string.Empty;
            while (!regexp.IsMatch(password))
            {
                password = GeneratePasswordString();
            }
            return Json(new { passw = password }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 0, VaryByParam = "None")]
        public JsonResult GeneratePin()
        {
            var pin = GeneratePinString();
            return Json(new { pin1 = pin.Substring(0, 4), pin2 = pin.Substring(0, 4) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckLoginName(string login, int? userId)
        {
            List<User> users = _userRepository.FindAll(x => x.LoginName == login.Trim()).ToList();
            if (userId.HasValue)
            {
                users = users.Where(us => us.Id != userId.Value).ToList();
            }
            return Json(new { isInUse = users.Count > 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TabContent()
        {
            // GetFiles();
            var hmv = CreateViewModel<HomeViewModel>();
            hmv.HRService = _FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).Any();
            int logOnUserId= CurrentUser.Get().Id;
            hmv.DisableAddUser = db.User.SingleOrDefault(x => x.Id == logOnUserId).DisableAddUsers.GetValueOrDefault();
            return PartialView(hmv);
        }

        public ActionResult UserRightsTabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        public ActionResult UserCardsTabContent()
        {
            int loggedInUserId = CurrentUser.Get().Id;
            bool CannotAddUsersAndCard = db.User.SingleOrDefault(x => x.Id == loggedInUserId).DisableAddUsers.GetValueOrDefault();
            var hmv = CreateViewModel<CardsTabContentViewModel>();
            hmv.CanAddCard = true;
            hmv.CannotAddUsersAndCard = CannotAddUsersAndCard;
            return PartialView(hmv);
        }

        #region Search User
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

        public ActionResult Search(string name, string comment, string cardSer, string cardDk, string cardCode, string company, string title, int filter, int departmentId, int? nav_page, int? rows, int? sort_field, int? sort_direction,
            int countryId, int locationId, int buildingId, int companyId, int floorId)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            IEnumerable<UserItem> filt_users = new List<UserItem>();
            var uvm = CreateViewModel<UserListViewModel>();
            //var uvm = CreateViewModel<LiveVideoListViewModel>();
            List<User> users = new List<User>();

            if (company != null && company != String.Empty)
            {
                //var comp = _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower() == company.ToLower()).FirstOrDefault();
                //if (comp == null)
                //{
                var comp = _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower().Contains(company.ToLower())).FirstOrDefault();
                //}
                if (comp == null)
                {
                    uvm.Paginator = SetupPaginator(ref filt_users, nav_page, rows);
                    uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
                    uvm.Paginator.Prefix = "User";
                    uvm.Users = filt_users;
                    uvm.FilterCriteria = filter;
                    return PartialView("List", uvm);
                }
            }

            if (!String.IsNullOrEmpty(comment)) //(comment != "")
            {
                users = SearchByComment(comment);
                /*
                Mapper.Map(users, filt_users);
                uvm.Paginator = SetupPaginator(ref filt_users, nav_page, rows);
                uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
                uvm.Paginator.Prefix = "User";

                uvm.Users = filt_users;
                uvm.FilterCriteria = filter;*/
                uvm.Comment = true;
                //return PartialView("List", uvm);
            }
            else
            {
                var user_priority = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
                users = _userRepository.FindAll(x => !x.IsDeleted && user_priority <= x.RolePriority()).ToList();
            }
            if (countryId != 0)
            {
                users = UsersByCountry(countryId);
            }

            if (locationId != 0)
            {
                users = UsersByLocation(locationId);
            }

            if (buildingId != 0)
            {
                users = UsersByBuilding(buildingId);
            }

            if (companyId != 0)
            {
                if (floorId != 0)
                {
                    users = UsersByFloor(floorId, companyId);
                }
                else
                {
                    users = UsersByCompany(companyId, buildingId);
                }
            }

            if (!CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin)
            {
                //	users = users.Where(us => us.CompanyId != null).ToList();
            }

            //users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);
            if (name != String.Empty)
            {
                string[] split = name.ToLower().Trim().Split(' ');

                switch (split.Count())
                {
                    case 1:
                        users = users.Where(x => (x.FirstName.ToLower().Contains(split[0]) || x.LastName.ToLower().Contains(split[0]) || (x.PersonalId != null && x.PersonalId.ToLower().Contains(split[0])))).ToList();
                        break;
                    case 2:
                        users = users.Where(x => x.FirstName.ToLower().Contains(split[0]) && x.LastName.ToLower().Contains(split[1])).ToList();
                        break;
                    default:
                        users = users.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(name.ToLower())).ToList();
                        break;
                }
            }

            if (company != null && company != String.Empty)
            {
                //var comp = _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower() == company.ToLower()).FirstOrDefault();

                //if (comp != null)
                //{
                //    int cmp_id = comp.Id;
                //    List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmp_id || x.ParentId == cmp_id)) select c.Id).ToList();
                //    users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
                //}
                List<int> companyIds = new List<int>();
                // List<int> comp = _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower() == company.ToLower()).Select(y => y.Id).ToList();
                //if (comp.Count() == 0 || comp == null)
                //{
                List<int> comp = _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower().Contains(company.ToLower())).Select(y => y.Id).ToList();
                // }
                if (comp != null)
                {
                    for (int b = 0; b < comp.Count(); b++)
                    {
                        int cmp_id = comp[b];
                        List<int> companyIds1 = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmp_id || x.ParentId == cmp_id)) select c.Id).ToList();
                        companyIds.AddRange(companyIds1);
                    }
                }
                users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
            }
            else
            {
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    int cmp_id = CurrentUser.Get().CompanyId.Value;
                    List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmp_id || x.ParentId == cmp_id)) select c.Id).ToList();
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("select ParentCompanieId from CompanieSubCompanies where IsDeleted=0 and CompanyId='" + cmp_id + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    List<int> subcompanyIds = new List<int>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        subcompanyIds.Add(Convert.ToInt32(dr["ParentCompanieId"]));
                    }
                    companyIds = companyIds.Concat(subcompanyIds).ToList();
                    users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
                }
                if (CurrentUser.Get().IsDepartmentManager)
                {
                    var user_department = _userDepartmentRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.IsDepartmentManager == true).FirstOrDefault();
                    users = user_department != null ? users.Where(x => x.UserDepartments.Any(y => y.DepartmentId == user_department.DepartmentId)).ToList() : new List<User>();
                }
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
            Mapper.Map(users, filt_users);

            if (sort_field.HasValue && sort_direction.HasValue)
            {
                switch (sort_field)
                {
                    case 1:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.FirstName.ToUpper()).ThenBy(x => x.LastName.ToUpper()).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.FirstName.ToUpper()).ThenByDescending(x => x.LastName.ToUpper()).ToList();
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
                                filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.TitleName).ToList() : filt_users.OrderByDescending(x => x.TitleName).ToList();
                            }
                            else
                            {
                                if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.ValidTo).ToList();
                                else filt_users = filt_users.OrderByDescending(x => x.ValidTo).ToList();
                            }
                        }
                        break;
                    case 5:
                        if (CurrentUser.Get().IsCompanyManager)
                        {
                            filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.ValidTo).ToList() : filt_users.OrderByDescending(x => x.ValidTo).ToList();
                        }
                        else
                        {
                            filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.Roles).ToList() : filt_users.OrderByDescending(x => x.Roles).ToList();
                        }
                        break;
                    case 6:
                        filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.UserStatus) : filt_users.OrderByDescending(x => x.UserStatus);
                        break;
                    case 7:
                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.Comment).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.Comment).ToList();
                        break;
                    default:
                        filt_users = filt_users.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
                        break;
                }
            }
            else
            {
                filt_users = filt_users.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
            }

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

            uvm.Paginator = SetupPaginator(ref filt_users, nav_page, rows);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "User";
            uvm.Users = filt_users;
            uvm.FilterCriteria = filter;
            return PartialView("List", uvm);
        }
        private List<FSCameras> UsersByCamera()
        {
            string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
            SqlConnection myConnection1 = new SqlConnection(connectionString1);
            myConnection1.Open();

            SqlCommand myCommand1 = new SqlCommand("select Id,Name from FSCameras", myConnection1);
            SqlDataAdapter adap1 = new SqlDataAdapter(myCommand1);
            DataTable dt1 = new DataTable();
            adap1.Fill(dt1);

            List<FSCameras> list1 = new List<FSCameras>();
            foreach (DataRow dr in dt1.Rows)
            {
                FSCameras usr = new FSCameras();
                usr.Id = Convert.ToInt32(dr[0]);
                usr.Name = dr["Name"].ToString();
                list1.Add(usr);
            }
            List<FSCameras> FSCameras1 = list1.ToList();
            return FSCameras1;
        }
        [ValidateInput(false)]
        public JsonResult ImportUsers_xml(HRItem[] a, xmlHR[] xHR)
        {
            string result = "";
            int saved = 0, err = 0, duplicate = 0, updated = 0, exist = 0;
            if (xHR.Length > 100)
            {
                result = "Error! More than 100 records can't be selected.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            IFoxSecIdentity identity = CurrentUser.Get();
            int UId = 0;
            int Uid = 0;
            int compId = 0;
            int companyId = 0;
            string CompanyName = "";
            DateTime from = new DateTime();
            DateTime fromto = new DateTime();
            Companies objCompanies = new Companies();
            List<string> TargetTables = new List<string>();
            FSHR f = new FSHR();
            f.fsHrList = new List<FSHR>();
            f.fsHrList = db.Database.SqlQuery<FSHR>("Select * from FSHR where IndexFilename LIKE '%.xml' or IndexFilename LIKE '%.XML' order by Id").ToList();
            TargetTables = f.fsHrList.Select(x => x.FoxsecTableName).Distinct().ToList();

            foreach (var x1 in xHR)
            {
                string PersonalCode = "";
                string FirstName = "";
                string LastName = "";
                string LoginName = "";
                //string Email = "";
                string validFrom = "";
                string validTo = "";
                string serial = "";
                string dk = "";
                bool flag = false;
                string strQuery = "Select * from Users where ";
                int k = 0;
                foreach (var cellval in x1.xmlRowVal)
                {
                    var ans = f.fsHrList.Any(x => x.FoxSecFieldName == cellval.columnName && x.FoxsecTableName == "Users" && x.IsIndex);
                    if (ans)
                    {
                        if (String.IsNullOrEmpty(cellval.ColVal))
                        {
                            err++;
                            flag = true;
                            break;
                        }
                        if (k == 0)
                        {
                            strQuery = strQuery + cellval.columnName + "='" + cellval.ColVal + "'";
                        }
                        else
                        {
                            strQuery = strQuery + " and " + cellval.columnName + "='" + cellval.ColVal + "'";
                        }
                        k++;
                    }
                }
                if (flag) continue;
                var Reslt = db.Database.SqlQuery<Users>(strQuery).FirstOrDefault();
                PersonalCode = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("personalcode")).Select(x => x.ColVal).First();
                FirstName = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("firstname")).Select(x => x.ColVal).First();
                LastName = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("lastname")).Select(x => x.ColVal).First();
                LoginName = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("loginname")).Select(x => x.ColVal).First();
                validFrom = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("validfrom")).Select(x => x.ColVal).First();
                validTo = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("validto")).Select(x => x.ColVal).First();
                serial = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("serial")).Select(x => x.ColVal).First();
                dk = x1.xmlRowVal.Where(x => x.columnName.ToLower().Contains("dk")).Select(x => x.ColVal).First();

                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(LoginName))
                {
                    err++;
                    result = result + " Details missing for Personal code(" + PersonalCode + ").";
                    continue;
                }

                var today = DateTime.Now.Date;

                Session["reg_start"] = (validFrom == null || validFrom == "") ? today.AddYears(-1).ToString("dd/MM/yyyy") : validFrom.Replace(".", "/");
                Session["reg_end"] = (validTo == null || validTo == "") ? today.AddYears(50).ToString("dd/MM/yyyy") : validTo.Replace(".", "/");

                if (Reslt == null)//null condition
                {
                    try
                    {
                        foreach (var tbl in TargetTables)
                        {
                            var records = db.Database.SqlQuery<HRtableFields>("SELECT c.name 'ColumnName',c.is_nullable,t.Name 'Datatype' FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id WHERE c.object_id = OBJECT_ID('" + tbl + "')").ToList();
                            if (tbl.ToLower() == "users")
                            {
                                //var notNullRecords = records.Where(x => x.is_nullable == false && !x.ColumnName.ToLower().Contains("timestamp")).Select(x => x.ColumnName).ToList();
                                UId = Save_Edit_Users(x1.xmlRowVal, f.fsHrList, records, identity.LoginName, 0);
                                var usr = db.User.Where(x => x.Id == UId).SingleOrDefault();
                                from = usr.RegistredStartDate;
                                fromto = usr.RegistredEndDate;
                                saved++;
                            }
                            else if (tbl.ToLower() == "companies")
                            {
                                var reslt = (from x0 in x1.xmlRowVal
                                             join f0 in f.fsHrList on x0.columnName equals f0.FoxSecFieldName
                                             where f0.FoxsecTableName.ToLower() == tbl.ToLower() && x0.columnName.ToLower() == "name"
                                             select x0).FirstOrDefault();
                                if (!String.IsNullOrEmpty(reslt.ColVal))
                                {
                                    var ResultCompany = db.Companies.Where(x => x.Name.ToLower() == reslt.ColVal.ToLower()).Any();
                                    if (!ResultCompany)
                                    {
                                        using (IUnitOfWork work = UnitOfWork.Begin())
                                        {
                                            objCompanies.Name = reslt.ColVal;
                                            objCompanies.ModifiedBy = identity.LoginName;
                                            objCompanies.ModifiedLast = DateTime.Now;
                                            objCompanies.Active = true;
                                            objCompanies.IsDeleted = false;
                                            objCompanies.IsCanUseOwnCards = true;
                                            db.Companies.Add(objCompanies);
                                            db.SaveChanges();
                                            work.Commit();
                                            compId = objCompanies.Id;
                                            var objUser1 = db.User.Find(UId);
                                            if (objUser1 != null)
                                            {
                                                objUser1.Id = UId;
                                                objUser1.CompanyId = compId;
                                                db.SaveChanges();
                                                work.Commit();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var ResultCompanyExist = db.Companies.Where(x => x.Name.ToLower() == reslt.ColVal.ToLower()).SingleOrDefault();
                                            using (IUnitOfWork work = UnitOfWork.Begin())
                                            {
                                                compId = ResultCompanyExist.Id;
                                                var objUser1 = db.User.Find(UId);
                                                if (objUser1 != null)
                                                {
                                                    objUser1.Id = UId;
                                                    objUser1.CompanyId = compId;
                                                    db.SaveChanges();
                                                    work.Commit();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            string msg = ex.Message;
                                        }
                                    }
                                }
                            }
                            else if (tbl.ToLower() == "usersaccessunit")
                            {
                                if (compId == 0)
                                {
                                    string query = "INSERT INTO UsersAccessUnit (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId,Serial,Dk) VALUES (@ValidFrom,@ValidTo,@Active,@IsDeleted,@CreatedBy,@BuildingId,@Free,@UserId,@Serial,@Dk)";
                                    UsersAccessUnit(0, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, UId, compId, serial, dk);
                                }
                                else
                                {
                                    string query = "INSERT INTO UsersAccessUnit (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId,CompanyId,Serial,Dk) VALUES (@ValidFrom,@ValidTo,@Active,@IsDeleted,@CreatedBy,@BuildingId,@Free,@UserId,@CompanyId,@Serial,@Dk)";
                                    UsersAccessUnit(1, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, UId, compId, serial, dk);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        err++;
                        string msg = e.Message;
                    }
                }
                else
                {
                    Uid = Reslt.Id;
                    List<HRtableFields> records = new List<HRtableFields>();
                    int updt = Save_Edit_Users(x1.xmlRowVal, f.fsHrList, records, identity.LoginName, 1);
                    if (updt > 0)
                    { /*updated++;*/ }

                    CompanyName = x1.xmlRowVal.Where(x => x.columnName.ToLower() == "name").Select(x => x.ColVal).First();
                    if (!String.IsNullOrEmpty(CompanyName))
                    {
                        var ResultCompany = db.Companies.Where(x => x.Name.ToLower() == CompanyName.ToLower()).FirstOrDefault();

                        if (ResultCompany != null)
                        {
                            compId = ResultCompany.Id;

                            companyId = Convert.ToInt32(Reslt.CompanyId);

                            if (companyId == 0)
                            {
                                using (IUnitOfWork work = UnitOfWork.Begin())
                                {
                                    if (CompanyName == null || CompanyName == "")
                                    { }
                                    else
                                    {
                                        objCompanies.Name = CompanyName;
                                        objCompanies.ModifiedBy = identity.LoginName;
                                        objCompanies.ModifiedLast = DateTime.Now;
                                        objCompanies.Active = true;
                                        objCompanies.IsDeleted = false;
                                        objCompanies.IsCanUseOwnCards = true;
                                        db.Companies.Add(objCompanies);
                                        db.SaveChanges();
                                        work.Commit();
                                        compId = objCompanies.Id;
                                        var objUser1 = db.User.Find(Uid);
                                        if (objUser1 != null)
                                        {
                                            objUser1.Id = Uid;
                                            objUser1.CompanyId = compId;
                                            db.SaveChanges();
                                            work.Commit();
                                        }
                                    }
                                }
                            }
                            var GetCompanyName = db.Companies.Where(x => x.Id == compId).SingleOrDefault(); //companyId).SingleOrDefault();
                            CompanyName = GetCompanyName.Name;
                            string _ValidFrom = "";
                            string _ValidTo = "";
                            string updatecid = "";

                            //SqlConnection con = new SqlConnection(Connection.connstring);
                            //con.Open();
                            //SqlCommand cmd = new SqlCommand();
                            //string query = "select  * from UsersAccessUnit where UserId=@UserId";
                            //cmd.Connection = con;
                            //cmd.CommandText = query;
                            //cmd.Parameters.AddWithValue("@UserId", Uid);
                            //SqlDataReader rdr = cmd.ExecuteReader();
                            //while (rdr.Read())
                            //{
                            //    _ValidFrom = rdr["ValidFrom"].ToString();
                            //    _ValidTo = rdr["ValidTo"].ToString();
                            //    updatecid = rdr["CompanyId"].ToString();
                            //}
                            //con.Close();

                            string getAccessDetQuery = @" select  * from UsersAccessUnit where UserId={0}";
                            var getAccessDet = db.Database.SqlQuery<UserAccessUnit_temp>(getAccessDetQuery, Uid).FirstOrDefault();
                            if (getAccessDet != null)
                            {
                                _ValidFrom = getAccessDet.ValidFrom.ToString();
                                _ValidTo = getAccessDet.ValidTo.ToString();
                                updatecid = getAccessDet.CompanyId.ToString();
                            }

                            if (CompanyName.ToLower() == CompanyName.ToLower() && (_ValidFrom == validFrom || _ValidTo == validTo))
                            {
                                duplicate++;
                            }
                            else
                            {
                                using (IUnitOfWork work = UnitOfWork.Begin())
                                {
                                    var objUser1 = db.User.Find(Uid);
                                    if (objUser1 != null)
                                    {
                                        objUser1.Id = Uid;
                                        objUser1.CompanyId = compId;
                                        db.SaveChanges();
                                        work.Commit();
                                    }

                                }
                                if (updatecid == null || updatecid == "")
                                {
                                    string query1 = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
                                    UsersAccessUnit(0, query1, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, 0, serial, dk);
                                }
                                else
                                {
                                    string query1 = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,CompanyId=@CompanyId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
                                    UsersAccessUnit(1, query1, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, Convert.ToInt32(updatecid), serial, dk);
                                }
                                updated++;
                            }
                        }
                        else
                        {
                            using (IUnitOfWork work = UnitOfWork.Begin())
                            {
                                if (CompanyName == null || CompanyName == "")
                                { }
                                else
                                {
                                    objCompanies.Name = CompanyName;
                                    objCompanies.ModifiedBy = identity.LoginName;
                                    objCompanies.ModifiedLast = DateTime.Now;
                                    objCompanies.Active = true;
                                    objCompanies.IsDeleted = false;
                                    objCompanies.IsCanUseOwnCards = true;
                                    db.Companies.Add(objCompanies);
                                    db.SaveChanges();
                                    work.Commit();
                                    compId = objCompanies.Id;
                                    var objUser1 = db.User.Find(Uid);
                                    if (objUser1 != null)
                                    {
                                        objUser1.Id = Uid;
                                        objUser1.CompanyId = compId;
                                        db.SaveChanges();
                                        work.Commit();
                                    }
                                }
                            }
                            if (compId == 0)
                            {
                                string query = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
                                UsersAccessUnit(0, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, compId, serial, dk);
                            }
                            else
                            {
                                string query = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,CompanyId=@CompanyId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
                                UsersAccessUnit(1, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, compId, serial, dk);
                            }
                            updated++;
                        }
                    }
                    else
                    {
                        exist++;
                    }
                }
            }

            #region commented code
            //foreach (var us in a)
            //{
            //    var Result = db.User.Where(x => x.PersonalCode == us.PersonalCode
            // && x.FirstName == us.FirstName && x.LastName == us.LastName && x.LoginName == us.LoginName && x.Email == us.Email).SingleOrDefault();

            //    Session["reg_start"] = us.ValidFrom == null ? "" : us.ValidFrom.Replace(".", "/");
            //    Session["reg_end"] = us.ValidTo == null ? "" : us.ValidTo.Replace(".", "/");
            //    if (Result == null)
            //    {
            //        try
            //        {
            //            using (var ctx = new FoxSecDBContext())
            //            {
            //                var ObjUser = new Users();
            //                ObjUser.PersonalCode = us.PersonalCode;
            //                ObjUser.FirstName = us.FirstName;
            //                ObjUser.LastName = us.LastName;
            //                ObjUser.LoginName = us.LoginName;
            //                ObjUser.Email = us.Email;
            //                ObjUser.Active = true;
            //                ObjUser.CreatedBy = us.CreatedBy;
            //                ObjUser.RegistredStartDate = DateTime.Now;
            //                ObjUser.RegistredEndDate = DateTime.Now.AddYears(50);
            //                ObjUser.ModifiedBy = identity.LoginName;
            //                ObjUser.ModifiedLast = DateTime.Now;
            //                ObjUser.IsDeleted = false;
            //                ctx.User.Add(ObjUser);
            //                ctx.SaveChanges();
            //                UId = ObjUser.Id;
            //                from = ObjUser.RegistredStartDate;
            //                fromto = ObjUser.RegistredEndDate;
            //                //Session["reg_start"] = from;
            //                //Session["reg_end"] = fromto;

            //                saved++;
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            err++;
            //            string msg = ex.Message;
            //        }

            //        if (!String.IsNullOrEmpty(us.Name))
            //        {
            //            var ResultCompany = db.Companies.Where(x => x.Name.ToLower() == us.Name.ToLower()).Any();
            //            if (!ResultCompany)
            //            {
            //                using (IUnitOfWork work = UnitOfWork.Begin())
            //                {
            //                    objCompanies.Name = us.Name;
            //                    objCompanies.ModifiedBy = identity.LoginName;
            //                    objCompanies.ModifiedLast = DateTime.Now;
            //                    objCompanies.Active = true;
            //                    objCompanies.IsDeleted = false;
            //                    objCompanies.IsCanUseOwnCards = true;
            //                    db.Companies.Add(objCompanies);
            //                    db.SaveChanges();
            //                    work.Commit();
            //                    compId = objCompanies.Id;
            //                    var objUser1 = db.User.Find(UId);
            //                    if (objUser1 != null)
            //                    {
            //                        objUser1.Id = UId;
            //                        objUser1.CompanyId = compId;
            //                        db.SaveChanges();
            //                        work.Commit();
            //                    }

            //                }
            //            }
            //            else
            //            {
            //                try
            //                {
            //                    var ResultCompanyExist = db.Companies.Where(x => x.Name.ToLower() == us.Name.ToLower()).SingleOrDefault();
            //                    using (IUnitOfWork work = UnitOfWork.Begin())
            //                    {
            //                        compId = ResultCompanyExist.Id;
            //                        var objUser1 = db.User.Find(UId);
            //                        if (objUser1 != null)
            //                        {
            //                            objUser1.Id = UId;
            //                            objUser1.CompanyId = compId;
            //                            db.SaveChanges();
            //                            work.Commit();
            //                        }
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    string msg = ex.Message;
            //                }
            //            }
            //        }
            //        if (compId == 0)
            //        {
            //            string query = "INSERT INTO UsersAccessUnit (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId,Serial,Dk) VALUES (@ValidFrom,@ValidTo,@Active,@IsDeleted,@CreatedBy,@BuildingId,@Free,@UserId,@Serial,@Dk)";
            //            UsersAccessUnit(0, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, true, UId, compId, us.Serial, us.Dk);
            //        }
            //        else
            //        {
            //            string query = "INSERT INTO UsersAccessUnit (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId,CompanyId,Serial,Dk) VALUES (@ValidFrom,@ValidTo,@Active,@IsDeleted,@CreatedBy,@BuildingId,@Free,@UserId,@CompanyId,@Serial,@Dk)";
            //            UsersAccessUnit(1, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, true, UId, compId, us.Serial, us.Dk);
            //        }
            //    }
            //    else
            //    {
            //        if (!String.IsNullOrEmpty(us.Name))
            //        {
            //            Uid = Result.Id;

            //            var ResultCompany = db.Companies.Where(x => x.Name.ToLower() == us.Name.ToLower()).FirstOrDefault();

            //            if (ResultCompany != null)
            //            {
            //                compId = ResultCompany.Id;

            //                companyId = Convert.ToInt32(Result.CompanyId);

            //                if (companyId == 0)
            //                {
            //                    using (IUnitOfWork work = UnitOfWork.Begin())
            //                    {
            //                        if (us.Name == null || us.Name == "")
            //                        {

            //                        }
            //                        else
            //                        {
            //                            objCompanies.Name = us.Name;
            //                            objCompanies.ModifiedBy = identity.LoginName;
            //                            objCompanies.ModifiedLast = DateTime.Now;
            //                            objCompanies.Active = true;
            //                            objCompanies.IsDeleted = false;
            //                            objCompanies.IsCanUseOwnCards = true;
            //                            db.Companies.Add(objCompanies);
            //                            db.SaveChanges();
            //                            work.Commit();
            //                            compId = objCompanies.Id;
            //                            var objUser1 = db.User.Find(Uid);
            //                            if (objUser1 != null)
            //                            {
            //                                objUser1.Id = Uid;
            //                                objUser1.CompanyId = compId;
            //                                db.SaveChanges();
            //                                work.Commit();
            //                            }
            //                        }
            //                    }
            //                }
            //                var GetCompanyName = db.Companies.Where(x => x.Id == compId).SingleOrDefault(); //companyId).SingleOrDefault();
            //                CompanyName = GetCompanyName.Name;
            //                string ValidFrom = "";
            //                string ValidTo = "";
            //                string updatecid = "";

            //                SqlConnection con = new SqlConnection(Connection.connstring);
            //                con.Open();
            //                SqlCommand cmd = new SqlCommand();
            //                string query = "select  * from UsersAccessUnit where UserId=@UserId";
            //                cmd.Connection = con;
            //                cmd.CommandText = query;
            //                cmd.Parameters.AddWithValue("@UserId", Uid);
            //                SqlDataReader rdr = cmd.ExecuteReader();
            //                while (rdr.Read())
            //                {
            //                    ValidFrom = rdr["ValidFrom"].ToString();
            //                    ValidTo = rdr["ValidTo"].ToString();
            //                    updatecid = rdr["CompanyId"].ToString();
            //                }
            //                con.Close();

            //                if (CompanyName.ToLower() == us.Name.ToLower() && (ValidFrom == us.ValidTo || ValidTo == us.ValidFrom))
            //                {
            //                    duplicate++;
            //                }
            //                else
            //                {
            //                    using (IUnitOfWork work = UnitOfWork.Begin())
            //                    {
            //                        var objUser1 = db.User.Find(Uid);
            //                        if (objUser1 != null)
            //                        {
            //                            objUser1.Id = Uid;
            //                            objUser1.CompanyId = compId;
            //                            db.SaveChanges();
            //                            work.Commit();
            //                        }

            //                    }
            //                    if (updatecid == null || updatecid == "")
            //                    {
            //                        string query1 = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
            //                        UsersAccessUnit(0, query1, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, true, Uid, 0, us.Serial, us.Dk);

            //                    }
            //                    else
            //                    {
            //                        string query1 = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,CompanyId=@CompanyId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
            //                        UsersAccessUnit(1, query1, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, true, Uid, Convert.ToInt32(updatecid), us.Serial, us.Dk);
            //                    }
            //                    updated++;
            //                }
            //            }
            //            else
            //            {
            //                using (IUnitOfWork work = UnitOfWork.Begin())
            //                {
            //                    if (us.Name == null || us.Name == "")
            //                    { }
            //                    else
            //                    {
            //                        objCompanies.Name = us.Name;
            //                        objCompanies.ModifiedBy = identity.LoginName;
            //                        objCompanies.ModifiedLast = DateTime.Now;
            //                        objCompanies.Active = true;
            //                        objCompanies.IsDeleted = false;
            //                        objCompanies.IsCanUseOwnCards = true;
            //                        db.Companies.Add(objCompanies);
            //                        db.SaveChanges();
            //                        work.Commit();
            //                        compId = objCompanies.Id;
            //                        var objUser1 = db.User.Find(Uid);
            //                        if (objUser1 != null)
            //                        {
            //                            objUser1.Id = Uid;
            //                            objUser1.CompanyId = compId;
            //                            db.SaveChanges();
            //                            work.Commit();
            //                        }
            //                    }

            //                }

            //                if (compId == 0)
            //                {
            //                    string query = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
            //                    UsersAccessUnit(0, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, true, Uid, compId, us.Serial, us.Dk);
            //                }
            //                else
            //                {
            //                    string query = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,CompanyId=@CompanyId,Serial=@Serial,Dk=@Dk where UserId=@UserId";
            //                    UsersAccessUnit(1, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, true, Uid, compId, us.Serial, us.Dk);
            //                }
            //                updated++;
            //            }
            //        }
            //        else
            //        {
            //            exist++;
            //        }
            //    }
            //}
            #endregion

            if (saved > 0 || updated > 0)
            {
                result = result.Trim() + "" + (saved + updated) + "" + " Records saved/updated.";
            }
            if (err > 0)
            {
                result = result.Trim() + " some errors.";
            }
            if (duplicate > 0)
            {
                result = result.Trim() + " duplicate user data.";
            }
            if (exist > 0)
            {
                result = result.Trim() + exist + " users already exists.";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public JsonResult ImportUsers(List<int> a)
        {
            string result = "Success!";
            var users = CreateViewModel<HRListViewModel>();
            var crtl = new HR();
            crtl.ControllerContext = ControllerContext;
            users = crtl.GetUsers(_FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).First().Value);
            var currentUsers = users.HRItems.Where(x => a.Contains(x.Id));
            int? company = null;
            foreach (var us in currentUsers)
            {
                if (_userRepository.FindAll(x => x.TableNumber == us.Id).Any())
                {
                    var U = _userRepository.FindAll(x => x.TableNumber == us.Id).First();
                    if (_companyRepository.FindAll(x => x.Name.ToLower() == us.CompanyName.ToLower()).Any())
                    {
                        company = _companyRepository.FindAll(x => x.Name.ToLower() == us.CompanyName.ToLower()).First().Id;
                    }
                    _userService.Update(U.Id, U.FirstName, U.LastName, U.FirstName + "." + U.LastName, company);

                    if (_departmentRepository.FindAll(x => x.Name.ToLower() == us.Department.ToLower()).Any())
                    {
                        var dep = _departmentRepository.FindAll(x => x.Name.ToLower() == us.Department.ToLower()).First();
                        _userDepartmentService.AddUserDepartment(false, dep.Id, false, false, U.Id, DateTime.Now, DateTime.Now.AddYears(50));
                    }
                }
                else
                {
                    if (_companyRepository.FindAll(x => x.Name.ToLower() == us.CompanyName.ToLower()).Any())
                    {
                        company = _companyRepository.FindAll(x => x.Name.ToLower() == us.CompanyName.ToLower()).First().Id;
                    }
                    var usr = _userService.ImportUser(us.Id, us.Name, us.LastName, company);
                    if (_departmentRepository.FindAll(x => x.Name.ToLower() == us.Department.ToLower()).Any())
                    {
                        var dep = _departmentRepository.FindAll(x => x.Name.ToLower() == us.Department.ToLower()).First();
                        _userDepartmentService.AddUserDepartment(false, dep.Id, false, false, usr.Id, DateTime.Now, DateTime.Now.AddYears(50));
                    }
                }
            }
            UserItem user = new UserItem();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        /// 
        private void InsertDataIntoUserAccess()
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
            }
        }
        public JsonResult ImportCsvUsers(datatable[] a)
        {
            IFoxSecIdentity identity = CurrentUser.Get();
            FoxSecDBContext db = new FoxSecDBContext();
            Users objUser = new Users();
            Companies objCompanies = new Companies();
            string result = " ";
            string CompanyName = "";
            int UId = 0;
            int Uid = 0;
            int compId = 0;
            int companyId = 0;
            DateTime from;
            DateTime fromto;
            int saved = 0, err = 0, duplicate = 0, updated = 0, exist = 0;
            var usrs = CreateViewModel<datatableListViewModel>();
            var crt = new CsvData();
            crt.ControllerContext1 = ControllerContext;
            //var currentUsers = db.HrClones.Where(x => x.ois_id_isik=="").ToList();

            bool Result1;
            foreach (var us in a)
            {
                CompanyName = "";
                UId = 0;
                Uid = 0;
                compId = 0;
                companyId = 0;
                //DateTime regSt_Dt, regEd_Dt;
                string str = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                using (SqlConnection con4 = new SqlConnection(str))
                {

                    con4.Open();
                    using (SqlCommand cmd4 = new SqlCommand("select RegistredStartDate,RegistredEndDate from Users where ExternalPersonalCode=@ExternalPersonalCode", con4))
                    {
                        cmd4.Parameters.AddWithValue("@ExternalPersonalCode", us.ois_id_isik);
                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = cmd4;
                            DataSet ds = new DataSet();
                            da.Fill(ds, "getid");
                            if (ds.Tables["getid"].Rows.Count > 0)
                            {
                                string fromDt = ds.Tables["getid"].Rows[0]["RegistredStartDate"].ToString();
                                string toDt = ds.Tables["getid"].Rows[0]["RegistredEndDate"].ToString();
                                int index = fromDt.IndexOf(" ");
                                int index1 = toDt.IndexOf(" ");
                                Session["reg_start"] = fromDt == "" ? "" : fromDt.Replace(".", "/").Remove(index);
                                Session["reg_end"] = toDt == "" ? "" : toDt.Replace(".", "/").Remove(index1);
                            }
                        } // reader closed and disposed up here
                    }
                } // command disposed here

                Session["reg_start"] = us.toosuhte_algus == null ? "" : us.toosuhte_algus.Replace(".", "/");
                Session["reg_end"] = us.toosuhte_lopp == null ? "" : us.toosuhte_lopp.Replace(".", "/");
                using (var ctx = new FoxSecDBContext())
                {
                    Result1 = ctx.User.Any(x => x.ExternalPersonalCode == us.ois_id_isik
             && x.FirstName == us.e_nimi && x.LastName == us.p_nimi && x.LoginName == us.kasutajatunnus && x.Email == us.ylikooli_e_post);
                }

                var Result = db.User.Where(x => x.ExternalPersonalCode == us.ois_id_isik).Any();
                if (!Result1)
                {
                    try
                    {
                        using (var ctx = new FoxSecDBContext())
                        {
                            objUser.PersonalId = us.isikukood;
                            if (us.e_nimi.Length > 20)
                            {
                                string fname = us.e_nimi.Substring(0, 15);
                                objUser.FirstName = fname;
                            }
                            else
                            {
                                objUser.FirstName = us.e_nimi;
                            }

                            if (us.p_nimi.Length > 20)
                            {
                                string lname = us.p_nimi.Substring(0, 15);
                                objUser.LastName = lname;
                            }
                            else
                            {
                                objUser.LastName = us.p_nimi;
                            }
                            objUser.LoginName = us.kasutajatunnus;
                            objUser.Email = us.ylikooli_e_post;
                            objUser.Active = true;
                            objUser.LoginName = us.kasutajatunnus;
                            objUser.ModifiedBy = identity.LoginName;
                            objUser.ModifiedLast = DateTime.Now;
                            objUser.ExternalPersonalCode = us.ois_id_isik;
                            objUser.RegistredStartDate = DateTime.Now;
                            objUser.RegistredEndDate = DateTime.Now.AddYears(50);
                            objUser.IsDeleted = false;
                            objUser.CreatedBy = identity.LastName;
                            ctx.User.Add(objUser);
                            ctx.SaveChanges();
                            UId = objUser.Id;
                            from = objUser.RegistredStartDate;
                            fromto = objUser.RegistredEndDate;
                            //Session["reg_start"] = from;
                            //Session["reg_end"] = fromto;
                            //result = "Success!";
                            saved++;
                        }
                    }
                    catch (Exception ex)
                    {
                        //result = "Some Error!!";
                        err++;
                        continue;
                        throw ex;
                    }

                    if (!String.IsNullOrEmpty(us.tookoha_aadress))
                    {
                        var ResultCompany = db.Companies.Where(x => x.Name.ToLower() == us.tookoha_aadress.ToLower()).Any();
                        if (!ResultCompany)
                        {
                            using (IUnitOfWork work = UnitOfWork.Begin())
                            {
                                objCompanies.Name = us.tookoha_aadress;
                                objCompanies.ModifiedBy = identity.LoginName;
                                objCompanies.ModifiedLast = DateTime.Now;
                                objCompanies.Active = true;
                                objCompanies.IsDeleted = false;
                                objCompanies.IsCanUseOwnCards = true;
                                db.Companies.Add(objCompanies);
                                db.SaveChanges();
                                work.Commit();
                                compId = objCompanies.Id;
                                var objUser1 = db.User.Find(UId);
                                if (objUser1 != null)
                                {
                                    objUser1.Id = UId;
                                    objUser1.CompanyId = compId;
                                    db.SaveChanges();
                                    work.Commit();
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                var ResultCompanyExist = db.Companies.Where(x => x.Name.ToLower() == us.tookoha_aadress.ToLower()).SingleOrDefault();
                                using (IUnitOfWork work = UnitOfWork.Begin())
                                {
                                    compId = ResultCompanyExist.Id;
                                    var objUser1 = db.User.Find(UId);
                                    if (objUser1 != null)
                                    {
                                        objUser1.Id = UId;
                                        objUser1.CompanyId = compId;
                                        db.SaveChanges();
                                        work.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string msg = ex.Message;
                            }
                        }
                    }
                    if (compId == 0)
                    {
                        string query = "INSERT INTO UsersAccessUnit (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId) VALUES (@ValidFrom,@ValidTo,@Active,@IsDeleted,@CreatedBy,@BuildingId,@Free,@UserId)";
                        UsersAccessUnit(0, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, UId, compId);
                    }
                    else
                    {
                        string query = "INSERT INTO UsersAccessUnit (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId,CompanyId) VALUES (@ValidFrom,@ValidTo,@Active,@IsDeleted,@CreatedBy,@BuildingId,@Free,@UserId,@CompanyId)";
                        UsersAccessUnit(1, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, UId, compId);
                    }
                }
                else
                {
                    if (us.tookoha_aadress != string.Empty && Result1 == true)
                    {
                        SqlConnection con5 = new SqlConnection(Connection.connstring);
                        con5.Open();
                        SqlCommand cmd5 = new SqlCommand("select Id from users where ExternalPersonalCode=@ExternalPersonalCode ", con5);
                        cmd5.Parameters.AddWithValue("@ExternalPersonalCode", us.ois_id_isik);
                        SqlDataAdapter da5 = new SqlDataAdapter();
                        da5.SelectCommand = cmd5;
                        DataSet ds2 = new DataSet();
                        da5.Fill(ds2, "GetId");
                        if (ds2.Tables["GetId"].Rows.Count > 0)
                        {
                            Uid = Convert.ToInt32(ds2.Tables["GetId"].Rows[0]["Id"]);
                        }

                        var ResultCompany = db.Companies.Where(x => x.Name.ToLower() == us.tookoha_aadress.ToLower()).FirstOrDefault();
                        if (ResultCompany != null)
                        {
                            compId = ResultCompany.Id;
                            var rslt = db.User.Where(x => x.ExternalPersonalCode == us.ois_id_isik).ToList();
                            foreach (var rs_usr in rslt)
                            {
                                companyId = Convert.ToInt32(rs_usr.CompanyId);
                            }
                            if (companyId == 0)
                            {
                                using (IUnitOfWork work = UnitOfWork.Begin())
                                {
                                    if (us.tookoha_aadress == null || us.tookoha_aadress == "")
                                    {
                                    }
                                    else
                                    {
                                        objCompanies.Name = us.tookoha_aadress;
                                        objCompanies.ModifiedBy = identity.LoginName;
                                        objCompanies.ModifiedLast = DateTime.Now;
                                        objCompanies.Active = true;
                                        objCompanies.IsDeleted = false;
                                        objCompanies.IsCanUseOwnCards = true;
                                        db.Companies.Add(objCompanies);
                                        db.SaveChanges();
                                        work.Commit();
                                        compId = objCompanies.Id;
                                        var objUser1 = db.User.Find(Uid);
                                        if (objUser1 != null)
                                        {
                                            objUser1.Id = Uid;
                                            objUser1.CompanyId = compId;
                                            db.SaveChanges();
                                            work.Commit();
                                        }
                                    }
                                }
                            }
                            var GetCompanyName = db.Companies.Where(x => x.Id == compId).SingleOrDefault(); //companyId).SingleOrDefault();
                            CompanyName = GetCompanyName.Name;
                            string ValidFrom = "";
                            string ValidTo = "";
                            string updatecid = "";

                            SqlConnection con = new SqlConnection(Connection.connstring);
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            string query = "select  * from UsersAccessUnit where UserId=@UserId";
                            cmd.Connection = con;
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@UserId", Uid);
                            SqlDataReader rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                ValidFrom = rdr["ValidFrom"].ToString();
                                ValidTo = rdr["ValidTo"].ToString();
                                updatecid = rdr["CompanyId"].ToString();
                            }
                            con.Close();
                            if (CompanyName.ToLower() == us.tookoha_aadress.ToLower() && (ValidFrom == us.toosuhte_lopp || ValidTo == us.toosuhte_algus))
                            {
                                //result = "User Already Exists!";
                                duplicate++;
                                //return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                using (IUnitOfWork work = UnitOfWork.Begin())
                                {
                                    var objUser1 = db.User.Find(Uid);
                                    if (objUser1 != null)
                                    {
                                        objUser1.Id = Uid;
                                        objUser1.CompanyId = compId;
                                        db.SaveChanges();
                                        work.Commit();
                                    }
                                }
                                if (updatecid == null || updatecid == "")
                                {
                                    string query1 = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId where UserId=@UserId";
                                    UsersAccessUnit(0, query1, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, 0);
                                }
                                else
                                {
                                    string query1 = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,CompanyId=@CompanyId where UserId=@UserId";
                                    UsersAccessUnit(1, query1, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, Convert.ToInt32(updatecid));
                                }
                                updated++;
                                //return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            using (IUnitOfWork work = UnitOfWork.Begin())
                            {
                                if (us.tookoha_aadress == null || us.tookoha_aadress == "")
                                {
                                }
                                else
                                {
                                    objCompanies.Name = us.tookoha_aadress;
                                    objCompanies.ModifiedBy = identity.LoginName;
                                    objCompanies.ModifiedLast = DateTime.Now;
                                    objCompanies.Active = true;
                                    objCompanies.IsDeleted = false;
                                    objCompanies.IsCanUseOwnCards = true;
                                    db.Companies.Add(objCompanies);
                                    db.SaveChanges();
                                    work.Commit();
                                    compId = objCompanies.Id;
                                    var objUser1 = db.User.Find(Uid);
                                    if (objUser1 != null)
                                    {
                                        objUser1.Id = Uid;
                                        objUser1.CompanyId = compId;
                                        db.SaveChanges();
                                        work.Commit();
                                    }
                                }
                            }

                            if (compId == 0)
                            {
                                string query = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId where UserId=@UserId";
                                UsersAccessUnit(0, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, compId);
                            }
                            else
                            {
                                string query = "UPDATE UsersAccessUnit SET ValidFrom = @ValidFrom,ValidTo = @ValidTo,Active = @Active, IsDeleted= @IsDeleted,CreatedBy=@CreatedBy,BuildingId=@BuildingId,Free=@Free,UserId=@UserId,CompanyId=@CompanyId where UserId=@UserId";
                                UsersAccessUnit(1, query, Session["reg_start"].ToString(), Session["reg_end"].ToString(), true, false, identity.Id, 1, false, Uid, compId);
                            }
                            updated++;
                        }
                    }
                    else
                    {
                        exist++;
                    }
                }
            }
            if (saved > 0 || updated > 0)
            {
                result = result.Trim() + "" + (saved + updated) + "" + " Records saved/updated.";
            }
            if (err > 0)
            {
                result = result.Trim() + " some errors.";
            }
            if (duplicate > 0)
            {
                result = result.Trim() + " duplicate user data.";
            }
            if (exist > 0)
            {
                result = result.Trim() + exist + " users already exists.";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int Save_Edit_Users(List<xmlfieldVal> xlist, List<FSHR> flist, List<HRtableFields> UserFields, string identityID, int type)
        {
            int modified = 0;
            var reslt = (from x1 in xlist
                         join f in flist on x1.columnName equals f.FoxSecFieldName
                         where f.FoxsecTableName.ToLower() == "users"
                         select x1).ToList();
            SqlConnection con = new SqlConnection(Connection.connstring);

            if (type == 0)
            {
                string columns = string.Join(","
        , reslt.Select(c => c.columnName));
                string values = string.Join(","
                    , reslt.Select(c => string.Format("@{0}", c.columnName)));
                columns = columns + ",Active,RegistredStartDate,RegistredEndDate,ModifiedBy,ModifiedLast,IsDeleted";
                values = values + ",@Active,@RegistredStartDate,@RegistredEndDate,@ModifiedBy,@ModifiedLast,@IsDeleted";
                String sqlCommandInsert = string.Format("INSERT INTO Users({0}) output INSERTED.Id  VALUES ({1})", columns, values);

                using (SqlCommand cmd = new SqlCommand(sqlCommandInsert, con))
                {
                    foreach (var r in reslt)
                    {
                        string colval = String.IsNullOrEmpty(r.ColVal) ? "" : r.ColVal;
                        cmd.Parameters.AddWithValue("@" + r.columnName, colval);
                    }
                    cmd.Parameters.AddWithValue("@Active", true);
                    cmd.Parameters.AddWithValue("@RegistredStartDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@RegistredEndDate", DateTime.Now.AddYears(50));
                    cmd.Parameters.AddWithValue("@ModifiedBy", identityID);
                    cmd.Parameters.AddWithValue("@ModifiedLast", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IsDeleted", false);
                    con.Open();
                    modified = (int)cmd.ExecuteScalar();
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                }
            }
            else if (type == 1)
            {
                var values = new Dictionary<string, object>();
                string personalCode = "";

                foreach (var r in reslt)
                {
                    values.Add(r.columnName, String.IsNullOrEmpty(r.ColVal) ? "" : r.ColVal);
                    if (r.columnName == "PersonalCode")
                    {
                        personalCode = r.ColVal.ToString().Trim();
                    }
                }
                values.Add("Active", true);
                values.Add("ModifiedBy", identityID);
                values.Add("ModifiedLast", DateTime.Now);
                values.Add("IsDeleted", false);

                var equals = new List<string>();
                var parameters = new List<SqlParameter>();
                var i = 0;
                foreach (var item in values)
                {
                    var pn = "@sp" + i.ToString();
                    equals.Add(string.Format("{0}={1}", item.Key, pn));
                    parameters.Add(new SqlParameter(pn, item.Value));
                    i++;
                }

                string command = string.Format("update Users set {0} where PersonalCode='" + personalCode + "'", string.Join(", ", equals.ToArray()));
                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    modified = (int)cmd.ExecuteNonQuery();
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                }
            }
            return modified;
        }

        public void UsersAccessUnit(int type, string Query, string from, string to, bool isActive, bool isDeleted, int ID, int buildingID, bool isFree, int uID, int compID, string serial = "***", string dk = "***")
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                if (String.IsNullOrEmpty(from))
                {
                    if (Query.ToLower().Contains("update"))
                    {
                        Query = Query.Replace("ValidFrom = @ValidFrom,", "");
                    }
                    else
                    {
                        Query = Query.Replace("@ValidFrom,", "");
                        Query = Query.Replace("ValidFrom,", "");
                    }
                }
                if (String.IsNullOrEmpty(to))
                {
                    if (Query.ToLower().Contains("update"))
                    {
                        Query = Query.Replace("ValidTo = @ValidTo,", "");
                    }
                    else
                    {
                        Query = Query.Replace("@ValidTo,", "");
                        Query = Query.Replace("ValidTo,", "");
                    }
                }
                SqlConnection con = new SqlConnection(Connection.connstring);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = Query;
                if (!String.IsNullOrEmpty(from))
                {
                    DateTime date = DateTime.ParseExact(from, "dd/MM/yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidFrom", date);
                }
                if (!String.IsNullOrEmpty(to))
                {
                    DateTime date1 = DateTime.ParseExact(to, "dd/MM/yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidTo", date1);
                }
                cmd.Parameters.AddWithValue("@Active", isActive);
                cmd.Parameters.AddWithValue("@IsDeleted", isDeleted);
                cmd.Parameters.AddWithValue("@CreatedBy", ID);
                cmd.Parameters.AddWithValue("@BuildingId", buildingID);
                cmd.Parameters.AddWithValue("@Free", isFree);
                cmd.Parameters.AddWithValue("@UserId", uID);
                if (type == 1)
                {
                    cmd.Parameters.AddWithValue("@CompanyId", compID);
                }
                if (serial != "***")
                {
                    cmd.Parameters.AddWithValue("@Serial", serial);
                }
                if (dk != "***")
                {
                    cmd.Parameters.AddWithValue("@Dk", dk);
                }
                cmd.ExecuteNonQuery();
                con.Close();
                work.Commit();
            }
        }

        public ActionResult Tab()
        {
            FoxSecDBContext db = new FoxSecDBContext();
            FSINISettings objFSINISettings = new FSINISettings();
            var ResultFSINISettings = db.FSINISettings.Where(x => x.SoftType == 6 && !x.IsDeleted).First();
            var url = "";
            if (ResultFSINISettings != null)
            {
                url = ResultFSINISettings.Value;
            }
            if (url.Contains(".csv") || url.Contains(".CSV") || url.Contains("EMY"))
            {
                var users = CreateViewModel<datatableListViewModel>();
                var crt2 = new CsvData();
                crt2.ControllerContext1 = ControllerContext;
                users = crt2.GetUsrs(_FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).First().Value);
                TempData["List_User"] = users.datatables.ToList();
                TempData.Keep();
                return PartialView("CsvHRTab", users);
            }

            else if (url.Contains(".xml"))
            {
                HRmodel users = new HRmodel();
                var crtl = new HR();
                //crtl.ControllerContext3 = ControllerContext;
                users.xmlTable = crtl.GetUsers_xml(_FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).First().Value);
                //users.fshrlist = crtl.GetFSHRlist();
                //ViewBag.fshrList = users.fshrlist;
                TempData["List_HR"] = users.xmlTable;
                TempData.Keep();
                return PartialView("xmlHRtab", users);
            }

            else
            {
                var users = CreateViewModel<HRListViewModel>();
                var crtl = new HR();
                crtl.ControllerContext = ControllerContext;
                users = crtl.GetUsers(_FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).First().Value);
                return PartialView("HRTab", users);
            }
        }

        public ActionResult xmlHRList()
        {
            var users = new HRmodel();
            //var crtl = new HR();
            //crtl.ControllerContext = ControllerContext;
            //users = crtl.GetUsers(_FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).First().Value);
            //users.HRItems = (List<HRItem>)TempData["List_HR"];
            users.xmlTable = (DataTable)TempData["List_HR"];
            TempData["List_User"] = users.xmlTable;
            TempData.Keep();
            return PartialView("xmlHRlist", users);
        }

        public JsonResult GetFSHRlist()
        {
            List<FSHR> fshrlist = new List<ViewModels.FSHR>();
            try
            {
                //var result = db.FSHR.Where(x => x.FoxSecTableId != 0 && x.IndexFilename.ToLower().Contains(".xml")).ToList();
                var result = db.Database.SqlQuery<FSHR>("Select * from FSHR where (IndexFilename LIKE '%.xml' or IndexFilename LIKE '%.XML') and FoxSecTableId <> 0 order by Id").ToList();
                fshrlist = result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return Json(fshrlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HRList()
        {
            var users = CreateViewModel<HRListViewModel>();
            var crtl = new HR();
            crtl.ControllerContext = ControllerContext;
            users = crtl.GetUsers(_FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).First().Value);
            return PartialView("HRList", users.HRItems);
        }

        public ActionResult CsvHRList()
        {
            var users = CreateViewModel<datatableListViewModel>();
            //var crt2 = new CsvData();
            //crt2.ControllerContext1 = ControllerContext;
            //users = crt2.GetUsrs(_FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).First().Value);
            List<datatable> usr = new List<datatable>();
            //var result = db.HrClones.ToList();
            //foreach (var items in result)
            //{
            //    usr.Add(new datatable() { Id = items.Id, ois_id_isik = items.ois_id_isik, isikukood = items.Personal_code, e_nimi = items.f_name, p_nimi = items.l_name, kasutajatunnus = items.username, toosuhte_algus = items.dateform, toosuhte_lopp = items.dateto, ylikooli_e_post = items.email, tookoha_aadress = items.Address });
            //}
            users.datatables = (List<datatable>)TempData["List_User"];
            TempData["List_User"] = users.datatables;
            TempData.Keep();
            return PartialView("CsvHRList", users.datatables);
        }

        public String CheckObjectPermission(int id)
        {
            var uvm = CreateViewModel<UserListViewModel>();
            List<User> users = new List<User>();
            List<int> u = new List<int>();
            var pgtz = _userPermissionGroupTimeZoneRepository.FindGpTZbyBuilding(id);

            var obj = _buildingObjectRepository.FindById(id);
            var name = "#" + obj.ObjectNr.ToString() + " " + obj.Description;

            foreach (var p in pgtz)
            {
                var pg = _userPermissionGroupRepository.FindById(p.UserPermissionGroupId);
                //if (pg != null && pg.ParentUserPermissionGroupId != null && /*!u.Contains(pg.UserId) &&*/ pg.ParentUserPermissionGroupId != null && pg.User.Active)
                if (pg != null && pg.ParentUserPermissionGroupId != null && /*!u.Contains(pg.UserId) &&*/ pg.ParentUserPermissionGroupId != null && pg.PermissionIsActive == true && pg.User.Active)
                { u.Add(pg.UserId); }
            }
            u.Sort();
            foreach (int usrId in u)
            {
                User usr = _userRepository.FindById(usrId);
                users.Add(usr);
            }
            var userslist = users.OrderBy(x => x.LastName);

            String result = "<H1>" + name + " (" + userslist.Count() + ")</H1><table cellpadding='1' cellspacing='0' style='margin: 0; width: 100%; padding: 1px; border-spacing: 0;'><th style='width: 19%; padding: 2px;'>Last Name</th><th style='width: 19%; padding: 2px;'>First Name</th><th style='width: 19%; padding: 2px;'>Company</th><th style='width: 19%; padding: 2px;'>Permission Group</th><th style='width: 19%; padding: 2px;'>Role</th><th style='width: 5%; padding: 2px;'></th>";
            var c = 0;

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsCommonUser)
            {
                foreach (var usr in userslist)
                {
                    int cmp_id = CurrentUser.Get().CompanyId.Value;
                    c++;
                    var dep = usr.UserPermissionGroups.Count() != 0 ? usr.UserPermissionGroups.First().Name : "";// = usr.UserDepartments.Count() != 0 ? usr.UserDepartments.First().Department.Name : "";
                    //var role = usr.UserRoles.Count() != 0 ? _roleRepository.FindById(usr.UserRoles.First().RoleId).Name : ""; //usr.Roles.Count() != 0 ? usr.Roles.First().Name : "";
                    var role = "";
                    if (usr.UserRoles.Count > 0)
                    {
                        var rolel = usr.UserRoles.Where(x => !x.IsDeleted && x.UserId == usr.Id && DateTime.Now >= x.ValidFrom && DateTime.Now <= x.ValidTo).FirstOrDefault();
                        if (rolel != null)
                        {
                            role = rolel.Role.Name;
                        }
                    }
                    var fullNm = usr.FirstName + " " + usr.LastName;                                                                                                   //var roleN = _roleRepository.FindById(role);
                    var com = usr.Company != null ? usr.Company.Name : "";
                    string add = "";

                    string linkToUserPermission = "";
                    if (usr.CompanyId == cmp_id)
                    {
                        linkToUserPermission = "<span id='button' class='use-address icon icon_green_go tipsy_we'  original-title='User Permission for " + usr.FirstName + "' onclick='EditUser(\"submit_edit_user\", " + usr.Id + ", \"" + fullNm + "\")' />";
                    }
                    else
                    {
                        linkToUserPermission = "<span id='button' class='use-address icon icon_green_go tipsy_we'  original-title='User Permission for " + usr.FirstName + "' onclick='NotCompanyUser()' />";
                    }

                    if (c % 2 == 0)
                    {
                        add = "<tr style='background-color:#CCC;'><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td><td>" + linkToUserPermission + "</td></tr>";
                    }
                    else
                    {
                        add = "<tr><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td><td>" + linkToUserPermission + "</td></tr>";
                    }
                    result = result + add;
                }
                result = result + "</table>";
            }
            else
            {
                foreach (var usr in userslist)
                {
                    c++;
                    var dep = usr.UserPermissionGroups.Count() != 0 ? usr.UserPermissionGroups.First().Name : "";// = usr.UserDepartments.Count() != 0 ? usr.UserDepartments.First().Department.Name : "";
                    var role = "";
                    if (usr.UserRoles.Count > 0)
                    {
                        var role1 = usr.UserRoles.Where(x => !x.IsDeleted && x.UserId == usr.Id && DateTime.Now >= x.ValidFrom && DateTime.Now <= x.ValidTo).FirstOrDefault();
                        if(role1 != null)
                        {
                            role = role1.Role.Name;
                        }
                    }
                    var fullNm = usr.FirstName + " " + usr.LastName;                                                                                                        //var roleN = _roleRepository.FindById(role);
                    string linkToUserPermission = "<span id='button' class='use-address icon icon_green_go tipsy_we'  original-title='User Permission for " + usr.FirstName + "' onclick='EditUser(\"submit_edit_user\", " + usr.Id + ", \"" + fullNm + "\")' />";
                    var com = usr.Company != null ? usr.Company.Name : "";
                    string add = "";
                    if (c % 2 == 0)
                    {
                        add = "<tr style='background-color:#CCC;'><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td><td>" + linkToUserPermission + "</td></tr>";
                    }
                    else
                    {
                        add = "<tr><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td><td>" + linkToUserPermission + "</td></tr>";
                    }
                    result = result + add;
                }
                result = result + "</table>";
            }
            return result;
        }

        public String CheckPermissionUsers(int id)
        {
            var permgr = _userPermissionGroupRepository.FindById(id).Name;
            var uvm = CreateViewModel<UserListViewModel>();
            List<User> users = new List<User>();

            var pgtz = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.User.Active && x.PermissionIsActive && x.ParentUserPermissionGroupId != null && (x.Name.Trim().Contains("++" + permgr.Trim()) || x.ParentUserPermissionGroupId == id || (x.UserId == CurrentUser.Get().Id && x.Id == id)));

            if (pgtz.Count().Equals(0)) { return "No users"; }
            var name = permgr;

            foreach (var p in pgtz)
            {
                if (!p.IsDeleted && p.PermissionIsActive)
                {
                    var usrId = p.UserId;

                    User usr = _userRepository.FindById(usrId);
                    if (usr.Active)
                    {
                        users.Add(usr);
                    }
                }
            }

            //String result = "<H1>" + name + "</H1><table cellpadding='1' cellspacing='0' style='margin: 0; width: 100%; padding: 1px; border-spacing: 0;'><th style='width: 20%; padding: 2px;'>Last Name</th><th style='width: 20%; padding: 2px;'>First Name</th><th style='width: 20%; padding: 2px;'>Company</th><th style='width: 20%; padding: 2px;'>Department</th><th style='width: 20%; padding: 2px;'>Role</th>";
            String result = "<H1>" + name + "</H1><table cellpadding='1' cellspacing='0' style='margin: 0; width: 100%; padding: 1px; border-spacing: 0;'><th style='padding: 2px;'>Last Name</th><th style='padding: 2px;'>First Name</th><th style='padding: 2px;'>Company</th><th style='padding: 2px;'>Role</th><th style='padding: 2px;'>User Permision</th><th style='width:5%' align='right'></th>";

            var c = 0;
            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsCommonUser)
            {
                int cmp_id = CurrentUser.Get().CompanyId.Value;
                foreach (var usr in users)
                {
                    List<UserPermissionGroup> userper = usr.UserPermissionGroups.ToList();
                    c++;
                    //var dep = usr.UserDepartments.Count() != 0 ? usr.UserDepartments.First().Department.Name : "";
                    //var role = usr.UserRoles.Count() != 0 ? _roleRepository.FindById(usr.UserRoles.First().RoleId).Name : ""; //usr.Roles.Count() != 0 ? usr.Roles.First().Name : "";
                    //var roleN = _roleRepository.FindById(role);

                    var role = getrolename(usr.Id);
                    var com = usr.Company != null ? usr.Company.Name : "";
                    var fullNm = usr.FirstName + " " + usr.LastName;
                    string linkToUserPermission = "";
                    if (usr.CompanyId == cmp_id)
                    {
                        linkToUserPermission = "<span id='button' class='use-address icon icon_green_go tipsy_we'  original-title='User Permission for " + usr.FirstName + "' onclick='EditUser(\"submit_edit_user\", " + usr.Id + ", \"" + fullNm + "\")' />";
                    }
                    else
                    {
                        linkToUserPermission = "<span id='button' class='use-address icon icon_green_go tipsy_we'  original-title='User Permission for " + usr.FirstName + "' onclick='NotCompanyUser()' />";
                    }
                    string add = "";
                    //if (c % 2 == 0)
                    //{
                    //    add = "<tr style='background-color:#CCC;'><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td></tr>";
                    //}
                    //else
                    //{
                    //    add = "<tr><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td></tr>";
                    //}<span id="button" class='use-address icon icon_green_go tipsy_we'  original-title='OPEN CAMERA' onclick="fnselect()" />
                    if (c % 2 == 0)
                    {
                        add = "<tr style='background-color:#CCC;'><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + role + "</td><td>" + userper[0].Name + "</td>" +
                            "<td>" + linkToUserPermission + "</td></tr>";
                    }
                    else
                    {
                        add = "<tr><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + role + "</td><td>" + userper[0].Name + "</td>" +
                            "<td>" + linkToUserPermission + "</td></tr>";
                    }
                    result = result + add;
                }
                result = result + "</table>";
            }
            else
            {
                foreach (var usr in users)
                {
                    List<UserPermissionGroup> userper = usr.UserPermissionGroups.ToList();
                    c++;
                    //var dep = usr.UserDepartments.Count() != 0 ? usr.UserDepartments.First().Department.Name : "";
                    //var role = usr.UserRoles.Count() != 0 ? _roleRepository.FindById(usr.UserRoles.First().RoleId).Name : ""; //usr.Roles.Count() != 0 ? usr.Roles.First().Name : "";
                    //var roleN = _roleRepository.FindById(role);
                    //var role = usr.UserRoles.Where(x => x.IsDeleted == false && x.UserId == usr.Id).Count() != 0 ? _roleRepository.FindAll().Where(y=>y.Id==(usr.UserRoles.Where(x => x.IsDeleted == false && x.UserId == usr.Id).First().RoleId)).FirstOrDefault().Name : "";
                    var role = getrolename(usr.Id);
                    var com = usr.Company != null ? usr.Company.Name : "";
                    var fullNm = usr.FirstName + " " + usr.LastName;
                    string linkToUserPermission = "<span id='button' class='use-address icon icon_green_go tipsy_we'  original-title='User Permission for " + usr.FirstName + "' onclick='EditUser(\"submit_edit_user\", " + usr.Id + ", \"" + fullNm + "\")' />";
                    string add = "";
                    //if (c % 2 == 0)
                    //{
                    //    add = "<tr style='background-color:#CCC;'><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td></tr>";
                    //}
                    //else
                    //{
                    //    add = "<tr><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + dep + "</td><td>" + role + "</td></tr>";
                    //}<span id="button" class='use-address icon icon_green_go tipsy_we'  original-title='OPEN CAMERA' onclick="fnselect()" />
                    if (c % 2 == 0)
                    {
                        add = "<tr style='background-color:#CCC;'><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + role + "</td><td>" + userper[0].Name + "</td>" +
                            "<td>" + linkToUserPermission + "</td></tr>";
                    }
                    else
                    {
                        add = "<tr><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + role + "</td><td>" + userper[0].Name + "</td>" +
                            "<td>" + linkToUserPermission + "</td></tr>";
                    }
                    result = result + add;
                }
                result = result + "</table>";
            }
            return result;
        }

        //GET all the user's Id for permisison Group
        public string getrolename(int id)
        {
            string rolename = "";
            var tc = _userrolerepository.FindAll().Where(x => x.IsDeleted == false && x.UserId == id).ToList();
            if (tc != null && tc.Count > 0)
            {
                int? roleid = tc.FirstOrDefault().RoleId;
                var rname = _roleRepository.FindAll().Where(x => x.IsDeleted == false && x.Id == roleid).ToList();
                if (rname != null && rname.Count > 0)
                {
                    rolename = rname.FirstOrDefault().Name;
                }
            }
            return rolename;
        }

        public List<User> SearchByComment(string term)
        {
            var uvm = CreateViewModel<UserListViewModel>();
            term = term.ToLower();
            var user_priority = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_priority <= x.RolePriority() && x.Comment != null && x.Comment.ToLower().Contains(term)).ToList();
            //users = _userRepository.FindAll(x => x.Comment != null && x.Comment.ToLower().Contains(term)).ToList();
            if (!(!CurrentUser.Get().IsBuildingAdmin || !CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().CompanyId != null))
            {
                users = users.Where(us => us.CompanyId == CurrentUser.Get().CompanyId).ToList();
            }
            //users = users.Where(x => x.Comment.ToLower().Contains(term)).ToList();
            return users;
        }

        [HttpGet]
        public JsonResult SearchByNameAutoComplete(string term, int filter)
        {
            term = term.ToLower();
            var user_priority = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            IEnumerable<User> users = _userRepository.FindAll(x => !x.IsDeleted && x.FirstName.ToLower().Contains(term) || x.LastName.ToLower().Contains(term) && !x.IsDeleted && user_priority <= x.RolePriority());
            users = ApplyUserStatusFilter(users, filter);
            users = GetUsersByBuildingInRole(users.ToList(), _buildingRepository, _userRepository);
            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                int cmp_id = CurrentUser.Get().CompanyId.Value;
                List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmp_id || x.ParentId == cmp_id)) select c.Id).ToList();
                users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
            }
            if (CurrentUser.Get().IsDepartmentManager)
            {
                var user_department = _userDepartmentRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.IsDepartmentManager == true).FirstOrDefault();
                users = user_department != null ? users.Where(x => x.UserDepartments.Any(y => y.DepartmentId == user_department.DepartmentId)).ToList() : new List<User>();
            }
            List<string> result = users.Select(user => user.FirstName + " " + user.LastName).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByTitleAutoComplete(string term)
        {
            term = term.ToLower();
            IEnumerable<Title> titles = _titleRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower().Contains(term));
            List<string> result = titles.Select(x => x.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByCompanyAutoComplete(string term)
        {
            term = term.ToLower();
            IEnumerable<Company> companies =
                _companyRepository.FindAll(x => !x.IsDeleted && x.Name.ToLower().Contains(term));
            List<string> result = companies.Select(x => x.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /*  public JsonResult UsersInPermissionGroup(int grpId)
          {

              var permgr = _userPermissionGroupRepository.FindById(grpId).Name;

              //var uvm = CreateViewModel<UserListViewModel>();
              List<int> usersID = new List<int>();
              var pgtz = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.User.Active && x.PermissionIsActive && (x.Name.Trim().Contains("++" + permgr.Trim()) || x.ParentUserPermissionGroupId == grpId || (x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null && x.Id == grpId)));
              /* if (pgtz.Count().Equals(0)) 
               {
               return "No users"; //need to handle this ?
               }*/
        /*   var name = permgr;

           foreach (var p in pgtz)
           {

               if (!p.IsDeleted && p.PermissionIsActive)
               {
                   var usrId = p.UserId;
                   User usr = _userRepository.FindById(usrId);
                   if (usr.Active)
                   {

                       usersID.Add(p.UserId);

                   }
               }

           }

           return Json(usersID, JsonRequestBehavior.AllowGet);
       }*/

        #endregion
        #region Create User
        [HttpGet]
        public ActionResult Create()
        {
            var uvm = CreateViewModel<UserEditViewModel>();

            uvm.FoxSecUser.RoleItems =
                from role in _roleRepository.FindAll().Where(x => x.IsDeleted == false && x.Active)
                select
                    new SelectListItem
                    {
                        Selected = false,
                        Text = role.Name + "#" + role.Description,
                        Value = role.Id.ToString()
                    };

            uvm.FoxSecUser.IsInCreateMode = true;
            uvm.FoxSecUser.BirthDayStr = "";
            uvm.LanguageItems = GetLanguages();

            var companyId = CurrentUser.Get().CompanyId.HasValue ? CurrentUser.Get().CompanyId.Value : -1;
            IEnumerable<Company> companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active).OrderBy(x => x.Name.ToLower());
            if (CurrentUser.Get().IsBuildingAdmin)
            {
                var buildIds = GetUserBuildings(_buildingRepository, _userRepository);

                companies = companies.Where(x => x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildIds.Contains(y.BuildingObject.BuildingId)));
            }

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                List<int> subcompanyIds = new List<int>();
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("select ParentCompanieId from CompanieSubCompanies where IsDeleted=0 and CompanyId='" + CurrentUser.Get().CompanyId + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        subcompanyIds.Add(Convert.ToInt32(dr["ParentCompanieId"]));
                    }
                }

                companies =
                    companies.Where(cc => cc.Id == companyId || (cc.ParentId != null && cc.ParentId.Value == companyId) || subcompanyIds.Contains(cc.Id));
            }
            uvm.Companies = new SelectList(companies, "Id", "Name", uvm.FoxSecUser.CompanyId);
            uvm.Titles = new SelectList(_titleRepository.FindAll(t => !t.IsDeleted).OrderBy(t => t.Name.ToLower()), "Id", "Name");
            return PartialView("Create", uvm);
        }

        [HttpPost]
        public ActionResult CreatePersonalData(UserItem user)
        {
            var resId = -1;
            int? tc = 0;
            int id = 0;
            int tc1 = 0;
            DateTime? validto = null;
            bool IsSucceed = false;
            IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue("Users");
            foreach (var obj in cv)
            {
                tc = obj.Legal;
                id = obj.Id;
                validto = obj.ValidTo;
            }
            if (validto == null && tc == null)
            {
                resId = 0;
            }
            else
            {
                tc1 = _userRepository.FindAll(x => !x.IsDeleted && x.Active == true).ToList().Count();
                int remaining = Convert.ToInt32(tc) - tc1;
                remaining = remaining < 0 ? 0 : remaining;
                if (remaining > 0 && validto > DateTime.Now)
                {
                    if (ModelState.IsValid)
                    {
                        byte[] image = null;

                        if (Session["NewUserPhoto"] != null)
                        {
                            string fileName = Session["NewUserPhoto"].ToString();

                            Image i = Image.FromFile(fileName);
                            var ms = new MemoryStream();
                            i.Save(ms, ImageFormat.Png);
                            image = ms.ToArray();

                            Session["NewUserPhoto"] = null;
                        }

                        if (user.BirthDayStr != null) user.Birthday = DateTime.ParseExact(user.BirthDayStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                        UserCreateResult result = _userService.CreateUser(user.FirstName, user.LastName, user.LoginName,
                                                                          user.Password, user.PersonalId, user.Email, user.PersonalCode,
                                                                          user.ExternalPersonalCode, user.Birthday,
                                                                          user.CompanyId, user.PIN1, user.PIN2, image, HostName, user.LanguageId, id);
                        Session["NewUserId"] = result.Id;
                        resId = result.Id;
                        IsSucceed = true;

                        //illi 25.12.1012    Logger4SendingEMail.LogSender.Info("\n REGISTRATION INFO \n Your username is: '" + user.LoginName +
                        //illi 25.12.1012                                       "'\n Your password is: '" + user.Password + "'" +
                        //illi 25.12.1012                                       "\n You can log in to page: http://foxsec.hgrupp.ee/");
                        //illi 25.12.1012    Logger4SendingEMail.InitSender(user.Email);
                    }
                }
                else
                {
                    resId = Convert.ToInt32(tc);
                }
            }
            return Json(new
            {
                IsSucceed,
                Id = resId
            });
        }

        [HttpGet]
        public ActionResult UploadImage(int userId)
        {
            return PartialView(userId);
        }

        [HttpPost]
        public ActionResult TransferImage(int userId)
        {
            string fileName = String.Empty;
            string newName = string.Empty;

            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase image = Request.Files[inputTagName];

                if (image.ContentLength > 0)
                {

                    try
                    {
                        string fullPath = Request.MapPath("../Uploads");
                        if (System.IO.File.Exists(fullPath))
                        {
                            //System.IO.File.Delete(fullPath);
                        }
                        else
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    fileName = Path.Combine(HttpContext.Server.MapPath("../Uploads"), Path.GetFileName(image.FileName));
                    image.SaveAs(fileName);

                    try
                    {
                        Image i = Image.FromFile(fileName);
                        if (!ImageFormat.Jpeg.Equals(i.RawFormat) && !ImageFormat.Gif.Equals(i.RawFormat) &&
                            !ImageFormat.Png.Equals(i.RawFormat))
                        {
                            return null;
                        }
                        int intNewWidth = 0;
                        int intNewHeight = 0;
                        int intOldWidth = i.Width;
                        int intOldHeight = i.Height;
                        int intMaxSide = intOldWidth >= intOldHeight ? intOldWidth : intOldHeight;
                        if (intMaxSide > 140)
                        {
                            double dblCoef = 140 / (double)intMaxSide;
                            intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
                            intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
                        }
                        else
                        {
                            intNewWidth = intOldWidth;
                            intNewHeight = intOldHeight;
                        }

                        var bmp = new Bitmap(intNewWidth, intNewHeight, PixelFormat.Format32bppRgb);
                        //var bmp = new Bitmap(150, 200, PixelFormat.Format32bppRgb);
                        Graphics gr = Graphics.FromImage(bmp);
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.CompositingQuality = CompositingQuality.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var rectDestination = new Rectangle(0, 0, intNewWidth, intNewHeight);
                        gr.DrawImage(i, rectDestination, 0, 0, i.Width, i.Height, GraphicsUnit.Pixel);
                        newName = Path.GetFileNameWithoutExtension(image.FileName) + "_" +
                                  DateTime.Now.ToString("MMddyyyyHHmmss") + ".png";
                        MemoryStream ms = new MemoryStream();
                        bmp.Save(Path.Combine(HttpContext.Server.MapPath("../Uploads"), newName), ImageFormat.Png);
                        bmp.Save(ms, ImageFormat.Png);
                        //ms = Path.Combine(HttpContext.Server.MapPath("../Uploads").+ newName;
                        using (IUnitOfWork work = UnitOfWork.Begin())
                        {
                            var usr = _userRepository.FindById(userId);
                            byte[] im = ms.ToArray();
                            usr.Image = im;
                            work.Commit();
                        }

                        i.Dispose();
                        bmp.Dispose();
                        gr.Dispose();
                        if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
                        Session["NewUserPhoto"] = Path.Combine(HttpContext.Server.MapPath("../Uploads"), newName);
                        //my path
                        _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, userId, UpdateParameter.PictureUpdate, ControllerStatus.Created, newName);
                        //end
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return PartialView("UploadImageReady", "../Uploads/" + newName);
        }

        public FileResult PhotoContent(int id)
        {
            User user = _userRepository.FindById(id);
            return new FileContentResult(user.Image, "image/png");
        }

        [HttpPost]
        public ActionResult RemovePhoto(int id)
        {
            _userService.RemoveUserPhoto(id);
            //  _controllerUpdateService.DeleteControllerUpdate(CurrentUser.Get().Id, /*userId,*/ UpdateParameter.PictureUpdate, ControllerStatus.Created/*, newName*/);
            return null;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if ((string)(filterContext.RouteData.Values["action"]) == "PhotoContent")
            {
                filterContext.Result = PhotoContent(int.Parse(Request.Params["id"]));
            }
        }

        #endregion

        #region Edit User

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User user = _userRepository.FindById(id);
            var uvm = CreateViewModel<UserEditViewModel>();
            Mapper.Map(user, uvm.FoxSecUser);
            uvm.LanguageItems = GetLanguages();

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

            uvm.FoxSecUser.UserRoleItems.UserId = id;
            uvm.FoxSecUser.UserRoleItems.EServiceAllowed = user.EServiceAllowed;
            uvm.FoxSecUser.UserRoleItems.IsVisitor = user.IsVisitor;
            uvm.FoxSecUser.UserRoleItems.WorkTime = user.WorkTime;
            uvm.FoxSecUser.UserRoleItems.CardAlarm = user.CardAlarm;
            uvm.FoxSecUser.UserRoleItems.IsShortTermVisitor = user.IsShortTermVisitor;
            uvm.FoxSecUser.UserRoleItems.ApproveTerminals = user.ApproveTerminals;
            uvm.FoxSecUser.UserRoleItems.ApproveVisitor = user.ApproveVisitor;
            uvm.FoxSecUser.UserRoleItems.IsCurrentUser = CurrentUser.Get().Id == id;
            IEnumerable<Role> available_roles =
                _roleRepository.FindAll(
                    x =>
                    !x.IsDeleted && x.Active && x.RoleTypeId >= CurrentUser.Get().RoleTypeId &&
                    x.RoleBuildings.Any(rb => !rb.IsDeleted && user_building_ids.Contains(rb.BuildingId))).OrderBy(r => r.Name.ToLower());

           
            if (CurrentUser.Get().IsCompanyManager)
            {
                FoxSecDBContext dBContext = new FoxSecDBContext();
                var currentUserCompanyId = CurrentUser.Get().CompanyId.Value;
                var companieRoleList = dBContext.CompanieRoles.Where(x => !x.IsDeleted && x.CompanyId == currentUserCompanyId).Select(y => y.RoleId).ToList();
                available_roles = available_roles.Where(x => companieRoleList.Contains(x.Id));
            }
            /*
			if( !CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin )
			{
				available_roles =
					available_roles.Where(
						x => user.UserRoles.Any(ur=>!ur.IsDeleted && ur.RoleId == x.Id) || (x.User != null && CurrentUser.Get().CompanyId == x.User.CompanyId));
			}
            */
            foreach (var role in available_roles)
            {
                var user_role_item = new UserRoleItem()
                {
                    IsSelected = user.UserRoles.Any(userRole => userRole.RoleId == role.Id && !userRole.IsDeleted),
                    RoleName = role.Name,
                    RoleDescription = role.Description,
                    ValidFrom = user.UserRoles.Any(userRole => userRole.RoleId == role.Id && !userRole.IsDeleted)
                                                    ? user.UserRoles.Where
                                                    (userRole => userRole.RoleId == role.Id && !userRole.IsDeleted).FirstOrDefault().ValidFrom.ToString("dd.MM.yyyy")
                                                    : string.Empty,
                    ValidTo = user.UserRoles.Any(userRole => userRole.RoleId == role.Id && !userRole.IsDeleted)
                                                    ? user.UserRoles.Where
                                                    (userRole => userRole.RoleId == role.Id && !userRole.IsDeleted).FirstOrDefault().ValidTo.ToString("dd.MM.yyyy")
                                                    : string.Empty,
                    RoleId = role.Id
                };
                uvm.FoxSecUser.UserRoleItems.Roles.Add(user_role_item);
            }

            uvm.FoxSecUser.BirthDayStr = uvm.FoxSecUser.Birthday.HasValue ? uvm.FoxSecUser.Birthday.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.ContractStartDateStr = uvm.FoxSecUser.ContractStartDate.HasValue ? uvm.FoxSecUser.ContractStartDate.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.ContractEndDateStr = uvm.FoxSecUser.ContractEndDate.HasValue ? uvm.FoxSecUser.ContractEndDate.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.PermitOfWorkStr = uvm.FoxSecUser.PermitOfWork.HasValue ? uvm.FoxSecUser.PermitOfWork.Value.ToString("dd.MM.yyyy") : "";
            uvm.FoxSecUser.RegistredDateStr = uvm.FoxSecUser.RegistredStartDate.ToString("dd.MM.yyyy");

            IEnumerable<UsersAccessUnit> userCompanyid = _usersAccessUnitRepository.FindAll().Where(ud => !ud.IsDeleted && ud.UserId == id && ud.CompanyId != uvm.FoxSecUser.CompanyId).ToList();
            IEnumerable<UserDepartment> userDepartments = _userDepartmentRepository.FindAll(ud => !ud.IsDeleted && ud.UserId == id);
            Mapper.Map(userDepartments, uvm.FoxSecUser.UserDepartmentsList);
            Mapper.Map(userCompanyid, uvm.FoxSecUser.UsersAccessUnit);
            int depId = 0;

            UserDepartment userDepartment = userDepartments.FirstOrDefault(ud => ud.CurrentDep);
            if (userDepartment != null) depId = userDepartment.DepartmentId;

            var departmentsList = new SelectList(_departmentRepository.FindAll(d => !d.IsDeleted).ToList(), "Id", "Name", depId);
            ViewData["DepartmentsList"] = departmentsList;

            uvm.FoxSecUser.IsInCreateMode = false;
            List<UserRole> activeUserRoles = user.UserRoles.Where(x => !x.IsDeleted).ToList();

            if (activeUserRoles.Count != 0)
            {
                uvm.FoxSecUser.RoleId = activeUserRoles.First().RoleId;
            }

            var companyId = CurrentUser.Get().CompanyId.HasValue ? CurrentUser.Get().CompanyId.Value : -1;
            IEnumerable<Company> companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active).OrderBy(x => x.Name.ToLower());
            if (CurrentUser.Get().IsBuildingAdmin || uvm.FoxSecUser.IsBuildingAdmin)
            {
                var buildIds = GetUserBuildings(_buildingRepository, _userRepository);
                companies = companies.Where(x => x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildIds.Contains(y.BuildingObject.BuildingId)));
            }

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                List<int> subcompanyIds = new List<int>();
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("select ParentCompanieId from CompanieSubCompanies where IsDeleted=0 and CompanyId='" + CurrentUser.Get().CompanyId + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        subcompanyIds.Add(Convert.ToInt32(dr["ParentCompanieId"]));
                    }
                }

                companies =
                    companies.Where(cc => cc.Id == companyId || (cc.ParentId != null && cc.ParentId.Value == companyId) || subcompanyIds.Contains(cc.Id));
            }

            uvm.Companies = new SelectList(companies, "Id", "Name", uvm.FoxSecUser.CompanyId);

            IEnumerable<Title> titles = _titleRepository.FindAll(c => !c.IsDeleted).OrderBy(x => x.Name.ToLower());
            if (!CurrentUser.Get().IsSuperAdmin)
            {
                titles = titles.Where(x => companies.Any(c => c.Id == x.CompanyId));
            }
            uvm.Titles = new SelectList(titles, "Id", "Name", uvm.FoxSecUser.TitleId);

            var userBuildings = new List<UserBuildingItem>();
            Mapper.Map(_userBuildingRepository.FindByUserId(uvm.FoxSecUser.Id.Value), userBuildings);
            /*  var userBuildings1 = _buildingObjectRepository.FindAll(x => x.TypeId == 1 && x.BuildingId == 1);
              foreach(var ub in userBuildings1)
              {

              }*//*
              if(uvm.FoxSecUser.CompanyId !=null )
              {
                  var item = new UserBuildingItem();

                  item.BuildingId = _userBuildingRepository.FindAll(x=> x.IsDeleted != true && x.BuildingObjectId == null).Select(x=>x.BuildingId).FirstOrDefault();
                  item.BuildingName = _buildingRepository.FindById(item.BuildingId.Value).Name;
                  item.IsBuildingAvailable = true;
                  item.IsDeleted = _buildingRepository.FindById(item.BuildingId.Value).IsDeleted;
                  item.UserId = uvm.FoxSecUser.Id;
                  userBuildings.Add(item);

              }
              */
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
            //By manoranjan
            int loggedInUserId = id;
            bool disableAddUsers = db.User.SingleOrDefault(x => x.Id == loggedInUserId).DisableAddUsers.GetValueOrDefault();
            uvm.FoxSecUser.UserRoleItems.CannotAddUsersAndCard = disableAddUsers;
            //
            return PartialView("Edit", uvm);
        }

        [HttpPost]
        public ActionResult EditPersonalData(UserItem user, int? Cardchange)
        {
            if (ModelState.IsValid)
            {
                byte[] image = null;

                if (Session["NewUserPhoto"] != null)
                {
                    string fileName = Session["NewUserPhoto"].ToString();

                    Image i = Image.FromFile(fileName);
                    var ms = new MemoryStream();
                    i.Save(ms, ImageFormat.Png);
                    image = ms.ToArray();

                    Session["NewUserPhoto"] = null;
                }

                User oldUserData = _userRepository.FindById((int)user.Id);

                if (oldUserData.Password != user.Password)
                {
                    //illi 25.12.1012   Logger4SendingEMail.LogSender.Info("\n PASSWORD CHANGE INFO \n Your username is: '" + user.LoginName +
                    //illi 25.12.1012                                      "'\n Your new password is: '" + user.Password + "'");
                    //illi 25.12.1012   Logger4SendingEMail.InitSender(user.Email);
                }

                /* is it needed?

                if (oldUserData.UserRoles.FirstOrDefault().CompanyId != user.CompanyId)
                {
                    _userService.UpdateUserRoles((int)user.Id, user.CompanyId);
                }
                */

                if (user.BirthDayStr != null) user.Birthday = DateTime.ParseExact(user.BirthDayStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                if (user.RegistredDateStr != null) user.RegistredStartDate = DateTime.ParseExact(user.RegistredDateStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                _userService.EditUserPersonalData((int)user.Id, user.FirstName, user.LastName, user.LoginName,
                                                  user.Password, user.PersonalId, user.Email, user.PersonalCode,
                                                  user.ExternalPersonalCode, user.Birthday, user.RegistredStartDate,
                                                  user.CompanyId, user.PIN1, user.PIN2, image, HostName, user.LanguageId);
                //var cardIds = user.UserCards.Cards.ToList();
                var cardIds = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == user.Id);
                int reasonId = 22;
                if (Cardchange.GetValueOrDefault() == 2)
                {
                    foreach (var cardId in cardIds)
                    {
                        _cardService.SetFreeState(cardId.Id, reasonId);
                        _cardService.SetValidTo(cardId.Id, DateTime.Now);
                    }
                }
                if (Cardchange.GetValueOrDefault() == 1)
                {

                    foreach (var cardId in cardIds)
                    {
                        var card = _usersAccessUnitRepository.FindById(cardId.Id);
                        _cardService.EditCard(card.Id, card.UserId, card.TypeId, user.CompanyId, card.BuildingId, card.Serial, card.Dk, card.Code, card.Free, card.ValidFrom, card.ValidTo, card.Comment, card.IsMainUnit);
                    }  //building not changed
                }
            }
            return null;
        }
        [HttpPost]
        public ActionResult EditOtherData(UserItem user)
        {
            if (user.Id != null)
            {
                _userService.EditOtherData(user.Id.GetValueOrDefault(), user.Residence);
            }
            return null;
        }

        [HttpPost]
        public ActionResult EditRoles(UserRoleModel userRoles)
        {
            int logOnUserId =  userRoles.UserId;
            var userLoggedIn = db.User.SingleOrDefault(x => x.Id == logOnUserId);
            if (userRoles.CannotAddUsersAndCard.GetValueOrDefault() == true)
            {

                userLoggedIn.DisableAddUsers = true;
                db.SaveChanges();
            }
            else
            {
                userLoggedIn.DisableAddUsers = false;
                db.SaveChanges();
            }
            string err_msg = string.Empty;
            UserRoleDto new_active_role = null;
            int? new_building = null;
            bool is_active_roleChanged = true;
            if (ModelState.IsValid)
            {
                ValidateUserRoles(userRoles.Roles);
                if (ModelState.IsValid)
                {
                    var usr = _userRepository.FindById(userRoles.UserId);
                    usr.EServiceAllowed = userRoles.EServiceAllowed;
                    usr.IsVisitor = userRoles.IsVisitor;
                    // usr.WorkTime = userRoles.WorkTime;
                    usr.CardAlarm = userRoles.CardAlarm;
                    usr.IsShortTermVisitor = userRoles.IsShortTermVisitor;
                    usr.ApproveTerminals = userRoles.ApproveTerminals;
                    usr.ApproveVisitor = userRoles.ApproveVisitor;
                    var user_role_dtos = new List<UserRoleDto>();
                    Mapper.Map(userRoles.Roles.Where(ur => ur.IsSelected), user_role_dtos);
                    new_active_role =
                        user_role_dtos.Where(x => x.IsSelected && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo.AddDays(1)).
                            FirstOrDefault();

                    if (new_active_role != null)
                    {
                        if (_userRepository.FindById(userRoles.UserId).UserRoles.Any(x => !x.IsDeleted && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now && x.RoleId == new_active_role.RoleId))
                        {
                            is_active_roleChanged = false;
                        }

                        var user = _userRepository.FindById(userRoles.UserId);


                        var companyId = user.CompanyId == null
                                            ? user.CompanyId
                                            : user.Company.ParentId ?? user.CompanyId;
                        var buildings = GetBuildings(new_active_role.RoleId, companyId);
                        if (buildings != null)
                        {
                            new_building = buildings.FirstOrDefault();
                        }
                    }

                    try
                    {
                        _userService.EditUserRoles(userRoles.UserId, user_role_dtos, HostName, new_building, is_active_roleChanged);
                    }
                    catch (Exception ex)
                    {
                        err_msg = ex.Message;
                        ModelState.AddModelError("", err_msg);
                    }
                }
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.CommonDataSavedMessage : err_msg,
                viewData = ModelState.IsValid ? null : this.RenderPartialView("UserRoles", userRoles)
            });
        }

        private List<int> GetBuildings(int roleId, int? companyId)
        {
            var role_building_ids = from rb in _roleRepository.FindById(roleId).RoleBuildings.Where(x => !x.IsDeleted) select rb.BuildingId;

            var role = _roleRepository.FindById(roleId);

            if (role_building_ids.Count() == 0)
            {
                return null;
            }

            if (role.RoleTypeId == (int)RoleTypeEnum.BA || role.RoleTypeId == (int)RoleTypeEnum.SA)
            {
                return role_building_ids.ToList();
            }
            if (!companyId.HasValue)
            {
                return null;
            }
            List<int> companyBuildingObjectsIds =
                    (from cbo in _companyRepository.FindById(companyId.Value).CompanyBuildingObjects.Where(x => !x.IsDeleted) select cbo.BuildingObjectId).ToList();

            var companyBuildingObjects =
                _buildingObjectRepository.FindAll(x => !x.IsDeleted && x.TypeId == 1 && companyBuildingObjectsIds.Contains(x.Id) && role_building_ids.Contains(x.BuildingId));

            if (companyBuildingObjects.Count() == 0)
            {
                return null;
            }
            return (from cb in companyBuildingObjects select cb.BuildingId).ToList();
        }

        private void ValidateUserRoles(IEnumerable<UserRoleItem> roles)
        {
            var i = 0;
            foreach (var userRole in roles)
            {
                var isFromDateEntered = !string.IsNullOrWhiteSpace(userRole.ValidFrom);
                var isToDateEntered = !string.IsNullOrWhiteSpace(userRole.ValidFrom);
                if (userRole.IsSelected)
                {
                    if (isFromDateEntered && isToDateEntered)
                    {
                        try
                        {
                            DateTime.ParseExact(userRole.ValidFrom.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidFrom", i),
                                                     ViewResources.SharedStrings.CommonDateFormat);
                        }

                        try
                        {
                            DateTime.ParseExact(userRole.ValidTo.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidTo", i),
                                                     ViewResources.SharedStrings.CommonDateFormat);
                        }
                    }
                    else
                    {
                        if (!isFromDateEntered)
                        {
                            ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidFrom", i),
                                                     ViewResources.SharedStrings.UsersNotAllUserRoleFieldsEntered);
                        }
                        if (!isToDateEntered)
                        {
                            ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidTo", i), isFromDateEntered ? ViewResources.SharedStrings.UsersNotAllUserRoleFieldsEntered : " ");
                        }
                    }
                }

                if (!userRole.IsSelected && (isToDateEntered || isFromDateEntered))
                {
                    ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidFrom", i),
                                                     ViewResources.SharedStrings.UsersNotAllUserRoleFieldsEntered);
                    ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidTo", i),
                                                     " ");
                    ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "IsSelected", i),
                                                     " ");
                }

                i++;
            }
            i = 0;

            if (ModelState.IsValid)
            {
                foreach (var userRole in roles)
                {
                    if (userRole.IsSelected)
                    {
                        var validFrom = DateTime.ParseExact(userRole.ValidFrom.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        var validTo = DateTime.ParseExact(userRole.ValidTo.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        if (validTo < validFrom)
                        {
                            ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidFrom", i), ViewResources.SharedStrings.UsersRoleValidationPeriodError);
                        }
                        if (roles.Where(ur => ur.RoleId != userRole.RoleId && ur.IsSelected && !IsValidDateInterval(ur, validFrom, validTo)).Count() > 0)
                        {
                            ModelState.AddModelError(ViewHelper.GetPrefixedName("Roles", "ValidFrom", i), ViewResources.SharedStrings.UserRolesDateIntersectionError);
                        }
                    }
                    i++;
                }
            }
        }

        private bool IsValidDateInterval(UserRoleItem role, DateTime startDate, DateTime endDate)
        {
            var role_start_date = DateTime.ParseExact(role.ValidFrom.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var role_end_date = DateTime.ParseExact(role.ValidTo.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            return (startDate < role_start_date && endDate < role_start_date) ||
                   (startDate > role_end_date && endDate > role_end_date);
        }

        [HttpPost]
        public ActionResult EditContactData(UserItem user)
        {
            if (user.Id == 0)
            {
                user.Id = int.Parse(Session["NewUserId"].ToString());
            }

            if (ModelState.IsValid)
            {
                _userService.EditUserContactData((int)user.Id, user.Residence, user.PhoneNumber, HostName);
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditWorkData(UserItem user)
        {
            var err_msg = string.Empty;

            if (user.Id == 0)
            {
                user.Id = int.Parse(Session["NewUserId"].ToString());
            }

            if (user.ContractStartDateStr != null)
                user.ContractStartDate = DateTime.ParseExact(user.ContractStartDateStr.Trim(), "dd.MM.yyyy",
                                                             CultureInfo.InvariantCulture);
            if (user.ContractEndDateStr != null)
                user.ContractEndDate = DateTime.ParseExact(user.ContractEndDateStr.Trim(), "dd.MM.yyyy",
                                                           CultureInfo.InvariantCulture);
            if (user.PermitOfWorkStr != null)
                user.PermitOfWork = DateTime.ParseExact(user.PermitOfWorkStr.Trim(), "dd.MM.yyyy",
                                                        CultureInfo.InvariantCulture);
            try
            {
                if (user.UserDepartmentItemId != null)
                {
                    _userDepartmentService.AddUserDepartment(true,
                                                             (int)user.UserDepartmentItemId,
                                                             false,
                                                             false,
                                                             (int)user.Id,
                                                             DateTime.ParseExact(user.DepartmentStartDateStr.Trim(),
                                                                                 "dd.MM.yyyy",
                                                                                 CultureInfo.InvariantCulture),
                                                             DateTime.ParseExact(user.DepartmentEndDateStr.Trim(),
                                                                                 "dd.MM.yyyy",
                                                                                 CultureInfo.InvariantCulture)
                        );
                }

                UserDepartment userDepartmentOld =
                    _userDepartmentRepository.FindByUserId((int)user.Id).FirstOrDefault(
                        d => !d.IsDeleted && d.CurrentDep);
                if (userDepartmentOld != null)
                    _userDepartmentService.SetCurrentDepartament((int)user.Id, false);
            }
            catch (Exception e)
            {
                err_msg = ViewResources.SharedStrings.UsersErrorEditingUserDepartments + user.Id;
                ModelState.AddModelError("", err_msg);
                Logger.Write("Error editing  User Id=" + user.Id, e);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userService.EditUserWorkData((int)user.Id, user.TitleId, user.ContractNum, user.ContractStartDate,
                                                  user.ContractEndDate, user.PermitOfWork, user.WorkTime, user.TableNumber,
                                                  user.EServiceAllowed, HostName);
                }
                catch (Exception)
                {
                    err_msg = ViewResources.SharedStrings.UsersErrorEditingUserWorkData + user.Id;
                    ModelState.AddModelError("", err_msg);
                }
            }
            foreach (var ubo in user.UserBuildingObjects)
            {
                if (user.UserBuildingObjects.Where(x => x.BuildingId == ubo.BuildingId && x.BuildingObjectId == ubo.BuildingObjectId).Count() > 1)
                {
                    err_msg = err_msg = ViewResources.SharedStrings.UsersErrorEditingUserBuildingsNotUnique;
                    ModelState.AddModelError("", err_msg);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userBuildingService.DeleteUserBuildings(user.Id.Value, HostName);
                    foreach (var ubo in user.UserBuildingObjects)
                    {
                        _userBuildingService.CreateUserBuilding(user.Id.Value, ubo.BuildingId.Value, ubo.BuildingObjectId, HostName);
                    }
                }
                catch (Exception)
                {
                    err_msg = ViewResources.SharedStrings.UsersErrorEditingUserWorkData + user.Id;
                    ModelState.AddModelError("", err_msg);
                }
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err_msg
            });
        }

        [HttpGet]
        public ActionResult DeactivateUserCards()
        {
            var dcvm = CreateViewModel<DeactivateCardViewModel>();
            dcvm.Reasons = new SelectList(_classificatorValueRepository.FindAll(cv => cv.ClassificatorId == 5).OrderBy(cv => cv.Value.ToLower()), "Id", "Value");
            dcvm.IsDeactivateDialog = true;
            return PartialView("ActivateDeactivate", dcvm);
        }

        [HttpGet]
        public ActionResult Activate()
        {
            var dcvm = CreateViewModel<DeactivateCardViewModel>();
            dcvm.Reasons = new SelectList(_classificatorValueRepository.FindAll(cv => cv.ClassificatorId == 4).OrderBy(cv => cv.Value.ToLower()), "Id", "Value");
            return PartialView("ActivateDeactivate", dcvm);
        }

        [HttpGet]
        public ActionResult Deactivate()
        {
            var dcvm = CreateViewModel<DeactivateCardViewModel>();
            dcvm.Reasons = new SelectList(_classificatorValueRepository.FindAll(cv => cv.ClassificatorId == 1).OrderBy(cv => cv.Value.ToLower()), "Id", "Value");
            dcvm.IsDeactivateDialog = true;
            return PartialView("ActivateDeactivate", dcvm);
        }

        [HttpPost]
        public ActionResult Activate(int[] usersIds, int reasonId, string cardactivate)
        {
            foreach (int id in usersIds)
            {
                _userService.Activate(id, reasonId, HostName, cardactivate);
            }
            return null;
        }

    //    [HttpPost]
    //    public ActionResult Deactivate(int[] usersIds, int reasonId)
    //    {
    //        foreach (int id in usersIds)
    //        {
    //            _userService.Deactivate(id, reasonId, HostName);
    //            var user = _userRepository.FindById(id);/*
				//foreach (var uau in user.UsersAccessUnits)
				//{
				//	if (!uau.IsDeleted && uau.Active)
				//	{
				//		//_userAccessUnitService.Deactivate(uau.Id, reasonId);
				//	}
				//}*/
    //        }
    //        return null;
    //    }

        [HttpPost]
        public ActionResult Deactivate(int[] usersIds, int reasonId, bool isMoveToFree)
        {
           
            foreach (int id in usersIds)
            {
                _userService.Deactivate(id, reasonId, HostName,isMoveToFree);
                var user = _userRepository.FindById(id);/*
				foreach (var uau in user.UsersAccessUnits)
				{
					if (!uau.IsDeleted && uau.Active)
					{
						//_userAccessUnitService.Deactivate(uau.Id, reasonId);
					}
				}*/
            }
            return null;
        }
        #endregion

        #region Delete User

        [HttpPost]
        public ActionResult Delete(int[] usersIds, bool? cards)
        {
            foreach (int id in usersIds)
            {
                try
                {
                    _userService.DeleteUserRoles(id);
                    _userService.DeleteUser(id, HostName);

                    var user = _userRepository.FindById(id);
                    if (cards != null)
                    {
                        if (!cards.GetValueOrDefault(false))
                        {
                            foreach (var uau in user.UsersAccessUnits)
                            {
                                if (!uau.IsDeleted)
                                {

                                    _userAccessUnitService.EditCard(uau.Id, null, uau.TypeId, uau.CompanyId, null, uau.Serial, uau.Dk, uau.Code, true, uau.ValidFrom, uau.ValidTo, uau.Comment, uau.IsMainUnit);
                                }
                            }
                        }
                        else
                        {
                            foreach (var uau in user.UsersAccessUnits)
                            {
                                if (!uau.IsDeleted)
                                {
                                    _userAccessUnitService.Delete(uau.Id);//EditCard(uau.Id, null, uau.TypeId, uau.CompanyId, null, uau.Serial, uau.Dk, uau.Code, true, uau.ValidFrom, uau.ValidTo);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Write("Error deleting User Id=" + id, e);
                }
            }

            return null;
        }

        #endregion

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
                    if (activePg != null)
                    {
                        var userActiveBuildings = (from cbo in activePg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.Active) select cbo.BuildingObjectId).ToList();
                        buildingObjects =
                            _buildingObjectRepository.FindAll(x => !x.IsDeleted && role_buildingIds.Contains(x.BuildingId) || userActiveBuildings.Contains(x.Id));
                    }
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
                //string name = upg.FirstOrDefault().Name + " ( " + _userTimeZoneRepository.FindById(upg.FirstOrDefault().DefaultUserTimeZoneId).Name + " ) ";
                string name = upg.FirstOrDefault().Name;
                result.Add(name);
                result.Add(upg.FirstOrDefault().ParentUserPermissionGroupId.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetParentUserPermissionGroup(int id)
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
        public JsonResult CheckBOverlapping(int PermissionGroupID, string Permission_Name)
        {
            bool isOverlappping = false;
            var upg = _userPermissionGroupRepository.FindAll(x => x.Id == PermissionGroupID).Select(x => x.Name).FirstOrDefault();
            List<List<int>> buildingObjectIDs = new List<List<int>>();
            List<int> AllBuildingObjIDs = new List<int>();
            if (upg.Contains("++"))
            {
                var grps = upg.Split(new[] { "++" }, StringSplitOptions.None);
                foreach (var id in grps)
                {
                    var grpID = _userPermissionGroupRepository.FindAll(x => x.Name == id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false).FirstOrDefault().Id;
                    var grp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList();
                    foreach (var boid in grp_buildingObjIDs)
                    {
                        AllBuildingObjIDs.Add(boid);
                    }
                    //buildingObjectIDs.Add(grp_buildingObjIDs);
                }
            }
            //Modified
            else
            {
                //If User have only 1 Permission Group
                var grpID = _userPermissionGroupRepository.FindAll(x => x.Name == upg && x.ParentUserPermissionGroupId == null && x.IsDeleted == false).FirstOrDefault().Id;
                var grp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList();

                foreach (var boid in grp_buildingObjIDs)
                {
                    AllBuildingObjIDs.Add(boid);
                }
            }
            if (AllBuildingObjIDs != null)
            {
                //  var query = AllBuildingObjIDs.GroupBy(x => x)
                //.Where(g => g.Count() > 1)
                //.Select(y => new { Element = y.Key, Counter = y.Count() })
                //.ToList();

                var query = AllBuildingObjIDs.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
                //  var query2 = AllBuildingObjIDs.GroupBy(x => x)
                //.Where(g => g.Count() > 1)
                //.ToDictionary(x => x.Key, y => y.Count());
                isOverlappping = query.Count > 0 ? true : false;
            }
            return Json(isOverlappping, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public string GetFsProjectsUsersMemoryList(int id)
        {
            string str = @"<table cellpadding='6' cellspacing='6' style='margin: 0; width: 100 %; padding: 6px; border - spacing: 8px;' class='TFtable'><tr>   
                         <th style = 'width: 16%;padding: 6px;'> Project </th>     
                         <th style = 'width: 16%;padding: 6px;'> DoorMemory </th>      
                         <th style = 'width: 16%;padding: 6px;'> ArmingMemory </th>       
                         <th style = 'width: 16%;padding: 6px;'> IsDeleted </th>           
                         <th style = 'width: 16%;padding: 6px;'> DoorDeleted </th>           
                         <th style = 'width: 16%;padding: 6px;'> ArmingDeleted </th>           
                         </tr>";
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select (select Name from FSProjects where Id=FP.ProjectId) Project,DoorFloorMemoryPos,RoomMemoryPos,IsDeleted,DateDelRoomPermission,DateDelDoorLiftPermission from FsProjectsUsersMemoryPos FP where UserId='" + id + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string project = "", DoorFloorMemoryPos = "", RoomMemoryPos = "", IsDeleted = "", DateDelRoomPermission = "", DateDelDoorLiftPermission = "";
                project = Convert.ToString(dr["Project"]);
                DoorFloorMemoryPos = Convert.ToString(dr["DoorFloorMemoryPos"]);
                RoomMemoryPos = Convert.ToString(dr["RoomMemoryPos"]);
                IsDeleted = Convert.ToString(dr["IsDeleted"]);
                if (IsDeleted == "1" || IsDeleted == "True" || IsDeleted == "true")
                {
                    IsDeleted = "Yes";
                }
                else
                {
                    IsDeleted = "No";
                }
                DateDelRoomPermission = Convert.ToString(dr["DateDelRoomPermission"]);
                if (!string.IsNullOrEmpty(DateDelRoomPermission))
                {
                    DateDelRoomPermission = Convert.ToDateTime(DateDelRoomPermission).ToString("dd.MM.yyyy");
                }
                DateDelDoorLiftPermission = Convert.ToString(dr["DateDelDoorLiftPermission"]);
                if (!string.IsNullOrEmpty(DateDelDoorLiftPermission))
                {
                    DateDelDoorLiftPermission = Convert.ToDateTime(DateDelDoorLiftPermission).ToString("dd.MM.yyyy");
                }
                str = str + "<tr> <td style = 'width: 16%;padding: 6px;'> " + project + "</td><td style = 'width: 16%;padding: 6px;'> " + DoorFloorMemoryPos + " </td> <td style = 'width: 16%;padding: 6px;'> " + RoomMemoryPos + "</td><td style = 'width: 16%;padding: 6px;'> " + IsDeleted + " </td><td style = 'width: 16%;padding: 6px;'> " + DateDelRoomPermission + " </td><td style = 'width: 16%;padding: 6px;'>  " + DateDelDoorLiftPermission + " </td></tr>";
            }
            str = str + "</table>";
            con.Close();
            return str;
        }

        [HttpGet]
        public ActionResult AtWork(int userId)
        {
            var uvm = CreateViewModel<UserEditViewModel>();
            List<UserBuildingObjects> lst = new List<UserBuildingObjects>();

            User user = _userRepository.FindById(userId);

            Mapper.Map(user, uvm.FoxSecUser);
            var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
            var userBuildings = new List<UserBuildingItem>();
            Mapper.Map(_userBuildingRepository.FindByUserId(uvm.FoxSecUser.Id.Value), userBuildings);

            foreach (var userBuildingItem in userBuildings)
            {
                userBuildingItem.IsBuildingAvailable = role_building_ids.Any(x => userBuildingItem.BuildingId == x);
            }

            uvm.FoxSecUser.BuildingItems = GetBuildings(uvm.FoxSecUser.Id.Value);

            uvm.FoxSecUser.UserBuildingObjects = userBuildings;

            if (uvm.FoxSecUser.UserBuildingObjects.Count > 0)
            {
                var id = uvm.FoxSecUser.UserBuildingObjects[0].BuildingId;
                var objitems = _buildingObjectRepository.FindAll().Where(x => x.BuildingId == id && x.IsDeleted == false && x.GlobalBuilding != null && x.TypeId == 8).ToList();
                foreach (var ob in objitems)
                {
                    lst.Add(new UserBuildingObjects
                    {
                        Id = ob.Id,
                        BuildingObjectName = ob.Description,
                        BuildingId = ob.BuildingId
                    });
                }
            }
            else
            {
                var objitems = _buildingObjectRepository.FindAll().Where(x => x.IsDeleted == false && x.GlobalBuilding != null && x.TypeId == 8).ToList();
                foreach (var ob in objitems)
                {
                    lst.Add(new UserBuildingObjects
                    {
                        Id = ob.Id,
                        BuildingObjectName = ob.Description,
                        BuildingId = ob.BuildingId
                    });
                }

                foreach (var bo in lst)
                {
                    con.Open();
                    SqlCommand cmdc = new SqlCommand("select name from Buildings where id='" + bo.BuildingId + "'", con);
                    string bildname = Convert.ToString(cmdc.ExecuteScalar());
                    if (!string.IsNullOrEmpty(bildname))
                    {
                        bo.BuildingObjectName = "Building: " + bildname + ", Building Object: " + bo.BuildingObjectName;
                    }
                    con.Close();
                }
            }

            var DistinctItems = lst.GroupBy(x => new { x.Id }).Select(y => y.First());
            uvm.FoxSecUser.UserBuildingObjectsItems = DistinctItems.OrderBy(x => x.BuildingId).ToList();
            return PartialView("AtWork", uvm);
        }

        [HttpGet]
        public ActionResult Leaving(int userId)
        {
            var uvm = CreateViewModel<UserEditViewModel>();
            List<UserBuildingObjects> lst = new List<UserBuildingObjects>();
            if (userId == 0)
            {
                var objitems = _buildingObjectRepository.FindAll().Where(x => x.IsDeleted == false).ToList();
                foreach (var ob in objitems)
                {
                    lst.Add(new UserBuildingObjects
                    {
                        Id = ob.Id,
                        BuildingObjectName = ob.Description
                    });
                }
            }
            else
            {
                User user = _userRepository.FindById(userId);

                Mapper.Map(user, uvm.FoxSecUser);
                var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
                var userBuildings = new List<UserBuildingItem>();
                Mapper.Map(_userBuildingRepository.FindByUserId(uvm.FoxSecUser.Id.Value), userBuildings);

                foreach (var userBuildingItem in userBuildings)
                {
                    userBuildingItem.IsBuildingAvailable = role_building_ids.Any(x => userBuildingItem.BuildingId == x);
                }

                uvm.FoxSecUser.BuildingItems = GetBuildings(uvm.FoxSecUser.Id.Value);

                uvm.FoxSecUser.UserBuildingObjects = userBuildings;


                if (uvm.FoxSecUser.UserBuildingObjects.Count > 0)
                {
                    var id = uvm.FoxSecUser.UserBuildingObjects[0].BuildingId;
                    var objitems = _buildingObjectRepository.FindAll().Where(x => x.BuildingId == id && x.IsDeleted == false && x.GlobalBuilding != null).ToList();
                    foreach (var ob in objitems)
                    {
                        lst.Add(new UserBuildingObjects
                        {
                            Id = ob.Id,
                            BuildingObjectName = ob.Description
                        });
                    }

                }
                else
                {
                    var objitems = _buildingObjectRepository.FindAll().Where(x => x.IsDeleted == false).ToList();
                    foreach (var ob in objitems)
                    {
                        lst.Add(new UserBuildingObjects
                        {
                            Id = ob.Id,
                            BuildingObjectName = ob.Description
                        });
                    }
                }
            }
            uvm.FoxSecUser.UserBuildingObjectsItems = lst.OrderBy(x => x.BuildingObjectName).ToList();
            return PartialView("Leaving", uvm);
        }

        [HttpGet]
        public void SaveWorkLeaving(int boid, int type, int id)
        {
            string buildingname = "";
            string node = "";
            con.Open();
            SqlCommand cmdg1 = new SqlCommand("select LogTypeId from log where id=(select max(id)  from log where UserId='" + id + "' and ((LogTypeId=30 and  EventKey like 'TOL00001%') or (LogTypeId=31 and EventKey like 'TOL00002%')))", con);
            string lgid = Convert.ToString(cmdg1.ExecuteScalar());
            if (type == 2)//leaving
            {
                if (lgid == "31")
                {
                }
                else
                {
                    SqlCommand cmdg = new SqlCommand("select BuildingObjectId from log where id=(select max(id)  from log where UserId='" + id + "' and (LogTypeId=30 and  EventKey like 'TOL00001%'))", con);
                    string bid = Convert.ToString(cmdg.ExecuteScalar());

                    if (!string.IsNullOrEmpty(bid))
                    {
                        boid = Convert.ToInt32(bid);
                        SqlCommand cmd = new SqlCommand("select Name from Buildings where id=(select BuildingId from BuildingObjects where id='" + boid + "')", con);
                        buildingname = Convert.ToString(cmd.ExecuteScalar());

                        SqlCommand cmd1 = new SqlCommand("select Description from BuildingObjects where id='" + boid + "'", con);
                        node = Convert.ToString(cmd1.ExecuteScalar());
                        con.Close();
                        _userService.SaveAtWorkLeaving(boid, id, type, buildingname, node, lgid);
                    }
                }
            }
            if (type == 1)//at work
            {
                SqlCommand cmd = new SqlCommand("select Name from Buildings where id=(select BuildingId from BuildingObjects where id='" + boid + "')", con);
                buildingname = Convert.ToString(cmd.ExecuteScalar());

                SqlCommand cmd1 = new SqlCommand("select Description from BuildingObjects where id='" + boid + "'", con);
                node = Convert.ToString(cmd1.ExecuteScalar());
                con.Close();
                _userService.SaveAtWorkLeaving(boid, id, type, buildingname, node, lgid);
            }
        }

        [HttpPost]
        public ActionResult SaveWorkLeavingUsers(int[] usersIds, int boid, int type)
        {
            foreach (int id in usersIds)
            {
                string buildingname = "";
                string node = "";
                con.Open();
                SqlCommand cmdg1 = new SqlCommand("select LogTypeId from log where id=(select max(id)  from log where UserId='" + id + "' and ((LogTypeId=30 and  EventKey like 'TOL00001%') or (LogTypeId=31 and EventKey like 'TOL00002%')))", con);
                string lgid = Convert.ToString(cmdg1.ExecuteScalar());

                if (type == 2)
                {
                    if (lgid == "31")
                    {
                    }
                    else
                    {
                        SqlCommand cmdg = new SqlCommand("select BuildingObjectId from log where id=(select max(id)  from log where UserId='" + id + "' and (LogTypeId=30 and  EventKey like 'TOL00001%'))", con);
                        string bid = Convert.ToString(cmdg.ExecuteScalar());
                        if (!string.IsNullOrEmpty(bid))
                        {
                            boid = Convert.ToInt32(bid);
                            SqlCommand cmd = new SqlCommand("select Name from Buildings where id=(select BuildingId from BuildingObjects where id='" + boid + "')", con);
                            buildingname = Convert.ToString(cmd.ExecuteScalar());

                            SqlCommand cmd1 = new SqlCommand("select Description from BuildingObjects where id='" + boid + "'", con);
                            node = Convert.ToString(cmd1.ExecuteScalar());

                            _userService.SaveAtWorkLeaving(boid, id, type, buildingname, node, lgid);
                        }
                    }
                }
                if (type == 1)
                {
                    SqlCommand cmd = new SqlCommand("select Name from Buildings where id=(select BuildingId from BuildingObjects where id='" + boid + "')", con);
                    buildingname = Convert.ToString(cmd.ExecuteScalar());

                    SqlCommand cmd1 = new SqlCommand("select Description from BuildingObjects where id='" + boid + "'", con);
                    node = Convert.ToString(cmd1.ExecuteScalar());
                    _userService.SaveAtWorkLeaving(boid, id, type, buildingname, node, lgid);
                }
                con.Close();
            }
            return null;
        }


        [HttpGet]
        public ActionResult AtWorkMutli(int[] usersIds)
        {
            var uvm = CreateViewModel<UserEditViewModel>();
            List<UserBuildingObjects> lst = new List<UserBuildingObjects>();

            foreach (var uid in usersIds)
            {
                User user = _userRepository.FindById(uid);

                Mapper.Map(user, uvm.FoxSecUser);
                var role_building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
                var userBuildings = new List<UserBuildingItem>();
                Mapper.Map(_userBuildingRepository.FindByUserId(uvm.FoxSecUser.Id.Value), userBuildings);

                foreach (var userBuildingItem in userBuildings)
                {
                    userBuildingItem.IsBuildingAvailable = role_building_ids.Any(x => userBuildingItem.BuildingId == x);
                }

                uvm.FoxSecUser.BuildingItems = GetBuildings(uvm.FoxSecUser.Id.Value);

                uvm.FoxSecUser.UserBuildingObjects = userBuildings;

                if (uvm.FoxSecUser.UserBuildingObjects.Count > 0)
                {
                    var id = uvm.FoxSecUser.UserBuildingObjects[0].BuildingId;
                    var objitems = _buildingObjectRepository.FindAll().Where(x => x.BuildingId == id && x.IsDeleted == false && x.GlobalBuilding != null 
                    && x.TypeId == 8).ToList();
                    foreach (var ob in objitems)
                    {
                        lst.Add(new UserBuildingObjects
                        {
                            Id = ob.Id,
                            BuildingObjectName = ob.Description,
                            BuildingId = ob.BuildingId
                        });
                    }
                }
                else
                {
                    var objitems = _buildingObjectRepository.FindAll().Where(x => x.IsDeleted == false && x.GlobalBuilding != null && x.TypeId == 8).ToList();
                    foreach (var ob in objitems)
                    {
                        lst.Add(new UserBuildingObjects
                        {
                            Id = ob.Id,
                            BuildingObjectName = ob.Description,
                            BuildingId = ob.BuildingId
                        });
                    }
                }
            }

            var DistinctItems = lst.GroupBy(x => new { x.Id }).Select(y => y.First());

            foreach (var bo in DistinctItems)
            {
                con.Open();
                SqlCommand cmdc = new SqlCommand("select name from Buildings where id='" + bo.BuildingId + "'", con);
                string bildname = Convert.ToString(cmdc.ExecuteScalar());
                if (!string.IsNullOrEmpty(bildname))
                {
                    bo.BuildingObjectName = "Building: " + bildname + ", Building Object: " + bo.BuildingObjectName;
                }
                con.Close();
            }

            uvm.FoxSecUser.UserBuildingObjectsItems = DistinctItems.OrderBy(x => x.BuildingId).ToList();
            return PartialView("AtWork", uvm);
        }
    }
}