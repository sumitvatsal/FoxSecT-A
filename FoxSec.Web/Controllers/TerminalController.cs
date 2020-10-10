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
//using LinqKit;
using System.Data.SqlTypes;

namespace FoxSec.Web.Controllers
{
    public class TerminalController : PaginatorControllerBase<TerminalModel>
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
        private readonly ILogService _logService;
        private readonly IBuildingObjectRepository _BuildingObjectRepository;
        private ResourceManager _resourceManager;

        public TerminalController(IUserService userService,
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
                                IClassificatorValueRepository classificatorValueRepository, ILogService logService,
                                IBuildingObjectRepository buildingObjectepository,
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
            _logService = logService;
            _BuildingObjectRepository = buildingObjectepository;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
        }
        // GET: Terminal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TabContent()
        {
            // GetFiles();
            var hmv = CreateViewModel<HomeViewModel>();
            //hmv.HRService = _FSINISettingsRepository.FindAll(x => x.SoftType == 6 && !x.IsDeleted).Any();
            return PartialView(hmv);
        }

        public ActionResult Search(string Tname, string TerminalID, int? companyId, string UserName, string _lastLogin, int? nav_page, int? rows, int? sort_field, int? sort_direction, int filter)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            var uvm = CreateViewModel<TerminallViewModel>();
            string Query = "Select * from Terminal WHERE isdeleted=0 ";

            List<string> conditions = new List<string>();
            List<TerminalModel> tm_list = new List<TerminalModel>();

            if (!string.IsNullOrEmpty(Tname))
            {
                conditions.Add(" LOWER(Name) like '%" + Tname.ToLower() + "%'");
            }
            if (!string.IsNullOrEmpty(TerminalID))
            {
                conditions.Add("TerminalId like '%" + TerminalID + "%'");
            }
            if (companyId > 0)
            {
                conditions.Add("CompanyId=" + companyId + "");
            }
            if (!string.IsNullOrEmpty(_lastLogin))
            {
                DateTime lastLogin = DateTime.ParseExact(_lastLogin, "dd.MM.yyyy",
                                                      System.Globalization.CultureInfo.InvariantCulture);
                conditions.Add("LastLogin=" + lastLogin + "");
            }
            if (filter != 2)
            {
                string approvedevice = filter == 1 ? "ApprovedDevice=" + true : "ApprovedDevice=" + false;
                conditions.Add("ApprovedDevice=" + filter);
            }
            if (UserName != String.Empty)
            {
                string[] split = UserName.ToLower().Trim().Split(' ');
                var users = _userRepository.FindAll(x => !x.IsDeleted && x.Active);

                switch (split.Count())
                {
                    case 1:
                        users = users.Where(x => (x.FirstName.ToLower().Contains(split[0]) || x.LastName.ToLower().Contains(split[0]) || (x.PersonalId != null && x.PersonalId.ToLower().Contains(split[0])))).ToList();
                        break;
                    case 2:
                        users = users.Where(x => x.FirstName.ToLower().Contains(split[0]) && x.LastName.ToLower().Contains(split[1])).ToList();
                        break;
                    default:
                        users = users.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(UserName.ToLower())).ToList();
                        break;
                }
                int id = users.FirstOrDefault().Id;
                if (id != 0 && id < 1)
                {
                    conditions.Add("MaxUserId=" + id + "");
                }
            }

            if (conditions.Any())
                Query += " AND " + string.Join(" AND ", conditions.ToArray());
            var terminal_list = db.Database.SqlQuery<Terminal>(Query).ToList();
            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                int cmp_id = CurrentUser.Get().CompanyId.Value;
                List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmp_id || x.ParentId == cmp_id)) select c.Id).ToList();
                terminal_list = terminal_list.Where(x => !String.IsNullOrEmpty(Convert.ToString(x.CompanyId)) && companyIds.Contains(Convert.ToInt32(x.CompanyId))).ToList();
            }
            foreach (var t in terminal_list)
            {
                TerminalModel tm = new TerminalModel();
                tm.term = t;
                int comp = Convert.ToInt32(t.CompanyId);
                tm.CompanyName = t.CompanyId == null ? "" : db.Companies.FirstOrDefault(x => x.Id == comp).Name;
                var usr = _userRepository.FindById(t.MaxUserId);
                if (usr != null)
                {
                    tm.MaxUserFName = usr.FirstName;
                    tm.MaxUserLName = usr.LastName;
                }
                else
                {
                    tm.MaxUserFName = "";
                    tm.MaxUserLName = "";
                }
                tm.status = t.ApprovedDevice == true ? "A" : "U";
                tm.lastLoginDt = t.LastLogin == null ? "" : Convert.ToDateTime(t.LastLogin).ToString("dd.MM.yyyy hh:mm");
                tm_list.Add(tm);
            }

            if (sort_field.HasValue && sort_direction.HasValue)
            {
                switch (sort_field)
                {
                    case 1:
                        if (sort_direction.Value == 0) tm_list = tm_list.OrderBy(x => x.term.Name.ToUpper()).ToList();
                        else tm_list = tm_list.OrderByDescending(x => x.term.Name.ToUpper()).ToList();
                        break;
                    case 2:
                        if (!CurrentUser.Get().IsDepartmentManager && !CurrentUser.Get().IsCompanyManager)
                        {
                            if (sort_direction.Value == 0) tm_list = tm_list.OrderBy(x => x.CompanyName).ToList();
                            else tm_list = tm_list.OrderByDescending(x => x.CompanyName).ToList();
                        }
                        break;
                    case 3:
                        if (sort_direction.Value == 0) tm_list = tm_list.OrderBy(x => x.MaxUserFName.ToUpper()).ThenBy(x => x.MaxUserLName.ToUpper()).ToList();
                        else tm_list = tm_list.OrderByDescending(x => x.MaxUserFName.ToUpper()).ThenByDescending(x => x.MaxUserLName.ToUpper()).ToList();
                        break;
                    case 4:
                        tm_list = sort_direction.Value == 0 ? tm_list.OrderBy(x => x.term.LastLogin).ToList() : tm_list.OrderByDescending(x => x.term.LastLogin).ToList();
                        break;
                    case 6:
                        tm_list = sort_direction.Value == 0 ? tm_list.OrderBy(x => x.status).ToList() : tm_list.OrderByDescending(x => x.status).ToList();
                        break;
                    default:
                        tm_list = tm_list.OrderBy(x => x.term.LastLogin).ToList();
                        break;
                }
            }
            else
            {
                tm_list = tm_list.OrderBy(x => x.term.LastLogin).ToList();
            }
            IEnumerable<TerminalModel> i_tList = tm_list;

            uvm.Paginator = SetupPaginator(ref i_tList, nav_page, rows);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "Terminal";
            uvm.terminals = i_tList.ToList();
            uvm.FilterCriteria = filter;
            return PartialView("List", uvm);
        }

        public string Approve_Unapprove(List<int> terminals, int status)
        {
            var usr = CurrentUser.Get();
            string msg = "";
            if (status != 2)
            {
                foreach (var t in terminals)
                {
                    var data = db.Database.SqlQuery<Terminal>("Select * from Terminal where Id=" + t).FirstOrDefault();
                    var update = db.Database.ExecuteSqlCommand("Update Terminal set ApprovedDevice=" + status + " where Id=" + t);
                    if (update > 0)
                    {
                        _logService.CreateLog(usr.Id, "Terminal", "", "", usr.CompanyId, "TerminalId " + data.TerminalId + (status == 0 ? "unapproved" : "approved") + " ");
                    }
                }
                db.SaveChanges();
                msg = status == 0 ? "selected terminals unapproved successfully" : "selected terminals approved successfully";
            }
            else
            {
                msg = "Please check status!!";
            }
            return msg;
        }

        public ActionResult Edit(int id)
        {
            TerminalModel tm = new TerminalModel();

            var terminal_list = db.Database.SqlQuery<Terminal>("Select * from Terminal where Id=" + id).SingleOrDefault();
            tm.term = terminal_list;

            SelectListItem item = new SelectListItem();
            item.Value = "";
            item.Text = "--Select--";

            SelectList complist = new SelectList(_companyRepository.FindAll().Where(x => !x.IsDeleted && x.Active).OrderBy(s => s.Name), "Id", "Name", tm.term.CompanyId);
            tm.companies = CardController.AddFirstItem(complist, item);


            SelectListItem item1 = new SelectListItem();
            item1.Value = "";
            item1.Text = "--Select--";

            SelectList tabuildinglist = new SelectList(_BuildingObjectRepository.FindAll(x => (x.TypeId == 8 || x.TypeId == 1) && !x.IsDeleted), "Id", "Description", tm.term.TARegisterBoId);
            tm.TABuildingList = CardController.AddFirstItem(tabuildinglist, item1);

            return PartialView("Edit", tm);
        }

        [HttpPost]
        public JsonResult EditTerminal(int id, bool ShowScreensaver, string ScreenSaverShowAfter, int? CompanyId, bool ApprovedDevice, string Name, bool InfoKioskMode, string SoundAlarms, string ShowMaxAlarmsFistPage, string MaxUser, string TerminalId,int? TARegisterBoId)
        {
            if (MaxUser == "")
            {
                MaxUser = "0";
            }

            string companyIdd = string.Empty;
            if (CompanyId == null)
            {
                companyIdd = "";
            }
            else
            {
                companyIdd = CompanyId.ToString();
            }

            string taobjectid = string.Empty;
            if (TARegisterBoId == null)
            {
                taobjectid = "";
            }
            else
            {
                taobjectid = TARegisterBoId.ToString();
            }

            string Query = "select * from terminal where id=" + id;
            var terminal_list = db.Database.SqlQuery<Terminal>(Query).SingleOrDefault();
            bool done = true;// _cardService.EditCard(card.Id, card.UserId, card.TypeId, card.CompanyId, card.BuildingId, card.Serial, card.Dk, card.Code, card.Free, validFrom, validTo, card.Comment);
            var usr = CurrentUser.Get();
            var values = new Dictionary<string, object>();
            try
            {
                if (!string.IsNullOrEmpty(ScreenSaverShowAfter))
                {
                    TimeSpan ts = TimeSpan.Parse(ScreenSaverShowAfter);
                    values.Add("ScreenSaverShowAfter", ts);
                }
                else
                {
                    TimeSpan ts = TimeSpan.Parse("00:00:00");
                    values.Add("ScreenSaverShowAfter", ts);
                }
                values.Add("ShowScreensaver", ShowScreensaver);
                values.Add("MaxUserId", Convert.ToInt32(MaxUser));
                values.Add("CompanyId", companyIdd);
                values.Add("ApprovedDevice", ApprovedDevice);
                values.Add("Name", Name);
                values.Add("InfoKioskMode", InfoKioskMode);
                values.Add("SoundAlarms", SoundAlarms);
                values.Add("ShowMaxAlarmsFistPage", ShowMaxAlarmsFistPage);
                values.Add("TARegisterBoId", taobjectid);
                var query = SqlUpdate("Terminal", values, "Id=" + id);

                var update = db.Database.ExecuteSqlCommand(query);
                if (update > 0)
                {
                    if (CompanyId == null)
                    {
                        db.Database.ExecuteSqlCommand("update Terminal set CompanyId=null where Id=" + id);
                    }
                    done = true;
                    _logService.CreateLog(usr.Id, "Terminal", "", "", usr.CompanyId, "Records updated for Terminal-> (Terminal name change from " + terminal_list.Name + " to " + Name + ".Show Screensaver change from " + terminal_list.ShowScreensaver + " to " + ShowScreensaver +
                                       ".Screensaver show After change from " + terminal_list.ScreenSaverShowAfter + " to " + ScreenSaverShowAfter + ". Company Id change from " + terminal_list.CompanyId + " to " + companyIdd + ". Max user ID change from " + terminal_list.MaxUserId + " to " + MaxUser +
                                       ". Approved Device change from " + terminal_list.ApprovedDevice + " to " + ApprovedDevice + ". InfoKiosk Mode change from " + terminal_list.InfoKioskMode + " to " + InfoKioskMode
                                       + ".Sound Alarms change from " + terminal_list.SoundAlarms + " to " + SoundAlarms + ". Maximum Alarm On First Page change from " + terminal_list.ShowMaxAlarmsFistPage + " to " + ShowMaxAlarmsFistPage + ".   )");
                }
                else
                {
                    done = false;
                }
                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                string mg = ex.Message;
                done = false;
            }
            return Json(done, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteTerminal(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                var usr = CurrentUser.Get();
                string Query = "select * from terminal where id=" + id;
                var terminal_list = db.Database.SqlQuery<Terminal>(Query).SingleOrDefault();
                var values = new Dictionary<string, object>();
                try
                {
                    values.Add("IsDeleted", 1);
                    var query = SqlUpdate("Terminal", values, "Id=" + id);

                    var update = db.Database.ExecuteSqlCommand(query);
                    if (update > 0)
                    {
                        _logService.CreateLog(id, "Terminal", "", "", usr.CompanyId, "Records deleted for Terminal " + terminal_list.Name);
                    }
                }
                catch
                {
                }
            }
        }

        public string SqlUpdate(string table, Dictionary<string, object> values, string where)
        {
            var equals = new List<string>();
            var parameters = new List<SqlParameter>();
            var i = 0;
            foreach (var item in values)
            {
                var pn = "@sp" + i.ToString();
                var type = item.Value.GetType().ToString();
                equals.Add(string.Format("{0}=\'{1}\'", item.Key, item.Value));
                parameters.Add(new SqlParameter(pn, item.Value));
                i++;
            }

            string command = string.Format("update {0} set {1} where {2}", table, string.Join(", ", equals.ToArray()), where);
            return command;
        }

        public JsonResult fetchUserNameByCompanyId(int Id)
        {
            User tm = new User();
            var userss = db.User.Where(s => s.CompanyId == Id && s.Active == true).OrderBy(c => c.FirstName).ToList();
            //            var terminal_list = db.Database.SqlQuery<Terminal>("Select * from Terminal where Id=" + id).SingleOrDefault();
            //          tm.term = terminal_list;
            return Json(userss, JsonRequestBehavior.AllowGet);
        }


        public JsonResult fetchUserNameBySuperAdmin()
        {
            User tm = new User();

            var userss = db.User.Where(s => s.Active == true).OrderBy(c => c.FirstName).ToList();
            //            var terminal_list = db.Database.SqlQuery<Terminal>("Select * from Terminal where Id=" + id).SingleOrDefault();
            //          tm.term = terminal_list;
            //return Json(userss, JsonRequestBehavior.AllowGet); // this is old code

            return new JsonResult() // this is new code
            {
                Data = userss,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

    }
}