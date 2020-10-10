using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using AutoMapper;
using FoxSec.Common.Enums;
using FoxSec.Authentication;
using System.Globalization;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using FoxSec.Core.Infrastructure.UnitOfWork;
using System.Xml.Linq;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.Web.Controllers;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FoxSec.Web.Controllers
{
    public class PermissionController : BusinessCaseController
    {
        //select id,('Name: ' + FirstName + ' ' + LastName + ', Login Name: ' + LoginName) as fullname from users where active = 1 and isdeleted=0
        private readonly IUserPermissionGroupService _userPermissionGroupService;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUserTimeZoneService _userTimeZoneService;
        private readonly ILogService _logService;
        private readonly IUserTimeZonePropertyRepository _userTimeZonePropertyRepository;
        private readonly IUserPermissionGroupRepository _userPermissionGroupRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);

        private string flag = "";
        public PermissionController(ICurrentUser currentUser,
                                    ILogger logger,
                                    IUserPermissionGroupService userPermissionGroupService,
                                    IBuildingRepository buildingRepository,
                                    IBuildingObjectRepository buildingObjectRepository,
                                    IUserTimeZoneRepository userTimeZoneRepository,
                                    ILogService logService,
                                    IUserTimeZoneService userTimeZoneService,
                                    IUserTimeZonePropertyRepository userTimeZonePropertyRepository,
                                    IUserPermissionGroupRepository userPermissionGroupRepository,
                                    IUserPermissionGroupTimeZoneRepository userPermissionGroupTimeZoneRepository,
                                    IUserRepository userRepository,
                                    IRoleRepository roleRepository,
                                    IUserRoleRepository userRoleRepository) : base(currentUser, logger)
        {
            _userPermissionGroupService = userPermissionGroupService;
            _buildingRepository = buildingRepository;
            _buildingObjectRepository = buildingObjectRepository;
            _userTimeZoneRepository = userTimeZoneRepository;
            _userTimeZoneService = userTimeZoneService;
            _userTimeZonePropertyRepository = userTimeZonePropertyRepository;
            _userPermissionGroupRepository = userPermissionGroupRepository;
            _userPermissionGroupTimeZoneRepository = userPermissionGroupTimeZoneRepository;
            _userRepository = userRepository;
            _logService = logService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public ActionResult Index()
        {
            // return PartialView()
            //*Modified
            var hvm = CreateViewModel<HomeViewModel>();
            return View(hvm);
        }

        #region Search
        /*
        public ActionResult CheckObjectPermission(int id)
        {
            var usrs = CreateViewModel<UserListViewModel>();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && x.Company.Id == 123).ToList();
            
            //var perm = new List<UserPermissionGroupTimeZone>();
            //perm = _userPermissionGroupTimeZoneRepository.FindAll();


            return PartialView("ListUsr", usrs);
        }
        */

        public ActionResult Search(string name, string start)
        {
            var tzlvm = CreateViewModel<TimeZoneListViewModel>();
            List<UserTimeZone> zones = new List<UserTimeZone>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsBuildingAdmin)
            { zones = _userTimeZoneRepository.FindAll(x => !x.IsDeleted).ToList(); }
            else
            {
                List<UserTimeZone> UTZ = new List<UserTimeZone>();
                UTZ = _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.CompanyId == CurrentUser.Get().CompanyId).ToList();
                zones = _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.CompanyId == null).ToList();
                foreach (FoxSec.DomainModel.DomainObjects.UserTimeZone timezone in UTZ)
                {
                    int IndexToReplace = zones.FindIndex(x => x.TimeZoneId == timezone.TimeZoneId);
                    if (zones.Find(x => x.TimeZoneId == timezone.TimeZoneId) != null)
                    {
                        zones[IndexToReplace] = timezone;
                    }
                }
            }
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
            return PartialView("List", tzlvm);
        }

        public ActionResult GetActiveZone(int id, int objectId)
        {
            var tzlvm = CreateViewModel<TimeZoneListViewModel>();
            List<UserTimeZone> zones = new List<UserTimeZone>();
            zones.Add(_userTimeZoneRepository.FindById(_userPermissionGroupService.GetUserActiveTimeZoneIdByBuildingObjectId(id, objectId)));
            Mapper.Map(zones, tzlvm.TimeZones);
            return PartialView("List", tzlvm);
        }

        public ActionResult GetDefaultZone(int id)
        {
            var tzlvm = CreateViewModel<TimeZoneListViewModel>();
            var upg = _userPermissionGroupRepository.FindById(id);
            if (CurrentUser.Get().IsCompanyManager)
            {
                tzlvm.IsModelReadOnly = upg.PermissionIsActive;
            }
            List<UserTimeZone> zones = new List<UserTimeZone>();
            zones.Add(_userTimeZoneRepository.FindById(_userPermissionGroupService.GetUserDefaultTimeZoneId(id)));
            Mapper.Map(zones, tzlvm.TimeZones);
            return PartialView("List", tzlvm);
        }

        [ValidateInput(false)]
        [ChildActionOnly]
        public ActionResult Zone(int timeZoneId, int colNr)
        {
            var tzpvm = CreateViewModel<TimeZonePropertiesViewModel>();
            Mapper.Map(_userTimeZonePropertyRepository.FindAll(x => x.UserTimeZoneId == timeZoneId && x.OrderInGroup == colNr).FirstOrDefault(), tzpvm);
            return PartialView(tzpvm);
        }

        #endregion

        #region Tree

        public ActionResult GetTree(int? id)
        {
            var ctvm = CreateViewModel<PermissionTreeViewModel>();
            var role_buildingIds = GetUserBuildings(_buildingRepository, _userRepository, CurrentUser.Get().Id);
            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsCommonUser)
            {
                UserPermissionGroup upg;
                /*
            	var activeUpg =
            		_userPermissionGroupRepository.FindAll().Where(
            			x => x.PermissionIsActive && !x.IsDeleted && x.UserId == CurrentUser.Get().Id).FirstOrDefault();
                */
                var activeUpg =
                _userPermissionGroupRepository.FindByUserId(CurrentUser.Get().Id).Where(
                    x => x.PermissionIsActive && !x.IsDeleted).FirstOrDefault();
                var activeUpgBuildingIds = activeUpg != null ? (from cbo in activeUpg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.Active) select cbo.BuildingObjectId).ToList() : null;
                if (id.HasValue)
                {
                    upg = _userPermissionGroupRepository.FindById(id.Value);
                    //var cupg = _userPermissionGroupRepository.FindAll(x => x.ParentUserPermissionGroupId == id && x.UserId == CurrentUser.Get().Id);
                    var cupg = _userPermissionGroupRepository.FindByUserId(CurrentUser.Get().Id).Where(x => x.ParentUserPermissionGroupId == id);
                    if (CurrentUser.Get().IsCompanyManager && upg.ParentUserPermissionGroupId == null && cupg.Count() >= 1) { ctvm.IsCurrentUserAssignedGroup = true; }
                    else { ctvm.IsCurrentUserAssignedGroup = upg.PermissionIsActive; }
                }
                else
                {
                    if (_userPermissionGroupRepository.FindByUserId(CurrentUser.Get().Id).Where(x => !x.IsDeleted && x.IsOriginal == true).ToList().Count() == 0) return null;
                    upg = _userPermissionGroupRepository.FindByUserId(CurrentUser.Get().Id).Where(x => !x.IsDeleted && x.IsOriginal == true && x.PermissionIsActive == true).FirstOrDefault();
                    id = upg.Id;
                    ctvm.IsGroupExist = false;
                    ctvm.IsCurrentUserAssignedGroup = false;
                }

                ctvm.IsOriginal = upg.IsOriginal;
                List<int> companyBuildingObjectsIds = (from cbo in upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && (activeUpg == null || activeUpgBuildingIds.Contains(x.BuildingObjectId) || x.Active)) select cbo.BuildingObjectId).ToList();
                if (activeUpgBuildingIds != null)
                {
                    foreach (var actUpgBuildingId in
                        activeUpgBuildingIds.Where(actUpgBuildingId => !companyBuildingObjectsIds.Contains(actUpgBuildingId)))
                    {
                        companyBuildingObjectsIds.Add(actUpgBuildingId);
                    }
                }

                var companyBuildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && /*role_buildingIds.Contains(x.BuildingId)
                    && */companyBuildingObjectsIds.Contains(x.Id));

                // role_buildingIds.Contains(x.BuildingId) not in need
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
                        //1 illi 08.05.2018 UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();
                        int idd = id.GetValueOrDefault();
                        UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(idd).Where(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();

                        obn.IsArming = upgtz == null ? true : upgtz.IsArming;
                        obn.IsDefaultArming = upgtz == null ? true : upgtz.IsDefaultArming;
                        obn.IsDisarming = upgtz == null ? true : upgtz.IsDisarming;
                        obn.IsDefaultDisarming = upgtz == null ? true : upgtz.IsDefaultDisarming;
                    }
                }
                ctvm.Towns = ctvm.Towns.Where(town => ctvm.Buildings.Any(b => b.ParentId == town.MyId));
                ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));
            }
            else
            if (!(CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsCommonUser))
            {
                IEnumerable<Building> userBuildings;
                if (CurrentUser.Get().IsSuperAdmin)
                {
                    userBuildings = _buildingRepository.FindAll(x => !x.IsDeleted).ToList();
                }
                else
                if (CurrentUser.Get().IsBuildingAdmin)
                {
                    userBuildings = _buildingRepository.FindAll(x => role_buildingIds.Contains(x.Id));
                }
                else return null;
                ctvm.Buildings =
                    (from b in userBuildings
                     select
                         new Node
                         {
                             ParentId = b.LocationId,
                             MyId = b.Id,
                             Name = b.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Towns =
                    (from t in (from ub in userBuildings select ub.Location)
                     where !t.IsDeleted
                     select
                         new Node
                         {
                             ParentId = t.CountryId,
                             MyId = t.Id,
                             Name = t.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Countries =
                    (from c in (from cbo in userBuildings select cbo.Location.Country).OrderBy(cc => cc.Name)
                     where !c.IsDeleted
                     select
                         new Node
                         {
                             ParentId = 0,
                             MyId = c.Id,
                             Name = c.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                List<int> userBuildingIds = (from ub in userBuildings select ub.Id).ToList();
                var userBuildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && userBuildingIds.Contains(x.BuildingId)).ToList();

                ctvm.Floors =
                    (from f in userBuildingObjects
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
                    (from o in userBuildingObjects
                     where o.ParentObjectId.HasValue && (o.TypeId == (int)BuildingObjectTypes.Door || o.TypeId == (int)BuildingObjectTypes.Lift || o.TypeId == (int)BuildingObjectTypes.Room)
                     orderby o.TypeId descending
                     select
                         new Node
                         {
                             ParentId = o.ParentObjectId.Value,
                             MyId = o.Id,
                             Name = o.ObjectNr.HasValue ? "#" + o.ObjectNr + " " + o.Description : o.Description,
                             Comment = o.Comment,
                             IsDefaultTimeZone = id.HasValue ? _userPermissionGroupService.IsDefaultUserTimeZone(o.Id, id.Value) : true,
                             IsRoom = o.TypeId == (int)BuildingObjectTypes.Room ? 1 : 0,
                             StatusIcon = o.StatusIconId.HasValue ? String.Format("../../img/status/{0}.ico", o.StatusIconId) : String.Empty
                         }).Distinct(new NodeEqualityComparer()).ToList();

                if (id.HasValue)
                {

                    ctvm.ActiveObjectIds = _userPermissionGroupService.GetUserBuildingObjectIds(id.Value);
                    foreach (var obn in ctvm.Objects)
                    {
                        if (obn.IsRoom == 1)
                        {
                            int idd = id.GetValueOrDefault();
                            UserPermissionGroupTimeZone pgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(idd).Where(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();
                            obn.IsDisarming = pgtz == null ? true : pgtz.IsDisarming;
                            obn.IsDefaultDisarming = pgtz == null ? true : pgtz.IsDefaultDisarming;
                            obn.IsArming = pgtz == null ? true : pgtz.IsArming;
                            obn.IsDefaultArming = pgtz == null ? true : pgtz.IsDefaultArming;
                        }
                    }
                }
                else
                {
                    foreach (var obn in ctvm.Objects)
                    {
                        if (obn.IsRoom == 1)
                        {
                            obn.IsArming = obn.IsDisarming = obn.IsDefaultArming = obn.IsDefaultDisarming = true;
                        }
                    }
                }
                ctvm.Towns = ctvm.Towns.Where(town => ctvm.Buildings.Any(b => b.ParentId == town.MyId));
                ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));
            }
            return PartialView("Tree", ctvm);
        }

        #endregion

        #region Create/Edit

        [HttpGet]
        public ActionResult Create()
        {
            var pevm = CreateViewModel<PermissionEditViewModel>();
            if (CurrentUser.Get().IsSuperAdmin)
            {
                pevm.Groups = new SelectList(_userPermissionGroupRepository.FindAll(g => !g.IsDeleted && g.ParentUserPermissionGroupId == null).OrderBy(x=> x.Name), "Id", "Name");
            }
            else
            {
                pevm.Groups = new SelectList(_userPermissionGroupRepository.FindAll(g => !g.IsDeleted && g.UserId == CurrentUser.Get().Id).OrderBy(x => x.Name), "Id", "Name");
            }
            return PartialView(pevm);
        }
        [HttpGet]
        public ActionResult Message()
        {
            return PartialView();
        }

        public JsonResult CreateGroup(string name, string copyId, string defTzId, List<int> objectIds, List<int> selectedObjectIds)
        {
            int? copyIdValue = null;
            if (copyId != string.Empty) copyIdValue = int.Parse(copyId);
            int? defTzIdValue = null;
            if (defTzId != string.Empty) defTzIdValue = int.Parse(defTzId);
            return Json(_userPermissionGroupService.CreateUserPermissionGroup(name, copyIdValue, defTzIdValue, objectIds, selectedObjectIds), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveGroup(int id, List<int> objectIds, List<int> selectedObjectIds)
        {
            //illi 25.12.1012 v16 probleem siia tuleb Permission "Save" ja User Permissions "Save" ja vahet ei saa teha 1.Controller Update erinev 2.vaja Individual door märkida
            var pgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(id);
            var permgr = _userPermissionGroupRepository.FindById(id);
            List<int> ownObjets = new List<int>();
            var t = (from pg in pgtz where pg.Active == true && pg.IsDeleted != true select pg.BuildingObjectId);
            foreach (int tt in t) { ownObjets.Add(tt); }
            string message = "<LM> <LTS LSF='LogMessageUserPermissionGroupChanged'> <LSP> " + permgr.Name + " </LSP> Started </LTS></LM>";
            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                    message.ToString());
            _userPermissionGroupService.SaveUserPermissionGroup(id, objectIds, selectedObjectIds, true, true);
            _userPermissionGroupService.GroupSaveUserPermissionGroup(id, objectIds, selectedObjectIds, ownObjets);
            message = "<LM> <LTS LSF='LogMessageUserPermissionGroupChanged'> <LSP> " + permgr.Name + " </LSP> Ended </LTS></LM>";
            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                    message.ToString());
            string[] array = permgr.Name.Split('+');
            if (_userPermissionGroupRepository.FindAll().Where(x => !x.IsDeleted && x.Name.Trim().Contains("++" + permgr.Name.Trim() + "++") || x.Name.Trim().Split('+').Last() == permgr.Name.Trim()).Any())
            {
                _userPermissionGroupService.AddPermissionsToAdditionalGroups(id, selectedObjectIds, ownObjets);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserSaveGroup(int id, List<int> objectIds, List<int> selectedObjectIds)
        {
            //illi 25.12.1012 v16 probleem siia tuleb Permission "Save" ja User Permissions "Save" ja vahet ei saa teha 1.Controller Update erinev 2.vaja Individual door märkida
            _userPermissionGroupService.SaveUserPermissionGroup(id, objectIds, selectedObjectIds, true, true);
            //_userPermissionGroupService.GroupSaveUserPermissionGroup(id, objectIds, selectedObjectIds, selectedObjectIds);
            return null;
        }

        public ActionResult SetBuildingObjectTimeZone(int id, int objectId, int zoneId)
        {
            _userPermissionGroupService.ChangeUserPermissionGroupBuildingTimeZone(id, objectId, zoneId);
            _userPermissionGroupService.GroupChangeUserPermissionGroupBuildingTimeZone(id, objectId, zoneId);
            return GetPermissionGroups(id);
        }

        public ActionResult ResetBuildingObjectTimeZone(int id, int objectId)
        {
            _userPermissionGroupService.ChangeUserPermissionGroupBuildingTimeZoneToDefault(id, objectId);
            _userPermissionGroupService.GroupChangeUserPermissionGroupBuildingTimeZoneToDefault(id, objectId);
            return GetDefaultZone(id);
        }

        public ActionResult DeleteGroup(int id)
        {
            _userPermissionGroupService.DeleteUserPermissionGroup(id);
            return null;
        }

        public ActionResult ChangeDefaultTimeZone(int id, int zoneId)
        {
            _userPermissionGroupService.ChangeUserDefaultTimeZone(id, zoneId);
            _userPermissionGroupService.GroupChangeUserDefaultTimeZone(id, zoneId);
            return null;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var pevm = CreateViewModel<PermissionEditViewModel>();
            StringBuilder result = new StringBuilder();

            var check_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var rolelist = _roleRepository.FindAll().Where(x => !x.IsDeleted && x.Active == true && (x.RoleTypeId == 1 || x.RoleTypeId == 3)).ToList().Select(x => x.Id);
            var useridrolewise = _userRoleRepository.FindAll().Where(x => !x.IsDeleted && rolelist.Contains(x.RoleId) && x.ValidTo >= check_date && x.ValidFrom <= check_date).ToList().Select(x => x.UserId);

            List<User> UserList = _userRepository.FindAll(x => x.Active == true && x.IsDeleted == false && useridrolewise.Contains(x.Id)).OrderBy(x => x.FirstName).ToList();

            foreach (var obj in UserList)
            {
                var usrrole = "";
                if (obj.UserRoles.Count > 0)
                {
                    var rolel = obj.UserRoles.Where(x => !x.IsDeleted && x.UserId == obj.Id && DateTime.Now >= x.ValidFrom && DateTime.Now <= x.ValidTo).FirstOrDefault();
                    if (usrrole != null)
                    {
                        usrrole = rolel.Role.Name;
                    }
                }
                //var roleid = _userRoleRepository.FindAll().Where(x => !x.IsDeleted && x.UserId == obj.Id && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).Select(x => x.RoleId).FirstOrDefault();
                //var rolename = _roleRepository.FindAll().Where(x => !x.IsDeleted && x.Id == Convert.ToInt32(roleid)).Select(x => x.Name).FirstOrDefault();
                obj.FirstName = "Full Name: " + obj.FirstName.Trim() + " " + obj.LastName.Trim() + ", Role Name: " + usrrole.Trim();
            }

            pevm.UserList = new SelectList(UserList, "Id", "FirstName", pevm.User.Id);

            Mapper.Map(_userPermissionGroupRepository.FindById(id), pevm.Permission);
            return PartialView(pevm);
        }

        [HttpPost]
        public void EditAdditional(PermissionEditViewModel item)
        {

        }
        public void UpdateUserPermission(int id, string name, string oldnamegroupename) //illi  16.10.2017 bug
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
                var old_name = upg.Name;
                //illi  16.10.2017 bug found ver 145 crash groupe names ++               
                if (old_name.Contains("++") && old_name.Contains(oldnamegroupename)) //illi  16.10.2017 bug
                {
                    var newname = upg.Name.Replace(oldnamegroupename, name);
                    upg.Name = newname;
                }
                else
                {
                    upg.Name = name;
                }
                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePermissionGroupChanged", new List<string> { old_name, name }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      message.ToString());
                //illi 25.12.1012 v16 pg name change
                //ei ole vaja nime muutus _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, name);
            }
        }

        public void UpdateNewOwnerName(int id, int newownername)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                var roleid = _userRoleRepository.FindByUserId(newownername).FirstOrDefault().RoleId;
                var roletypeid = _roleRepository.FindAll().Where(x => x.Id == roleid).FirstOrDefault().RoleTypeId;
                UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
                if (roletypeid == 1)
                {
                    upg.IsOriginal = true;
                }
                else
                {
                    upg.IsOriginal = false;
                }

                upg.UserId = newownername;
                work.Commit();
            }
        }
        private int[] GetAffectedUserPermissionGroupIds(int upgId)
        {
            var permgr = _userPermissionGroupRepository.FindById(upgId).Name;
            int[] pgtz = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.User.Active && x.PermissionIsActive && (x.Name.Trim().Contains("++" + permgr.Trim()) || x.ParentUserPermissionGroupId == upgId || (x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null && x.Id == upgId))).Select(x => x.Id).ToArray();
            return pgtz;
        }

        public int[] GetAddAffectedUserPermissionGroupIds(string name)
        {
            int[] groups = (from upg in _userPermissionGroupRepository.FindByPupgHasValue().Where(x => x.Name.Trim().Contains("++" + name.Trim())) select upg.Id).ToArray();
            return groups;
        }

        public void UpdateAddUserPermission(int id, string name, string oldname)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
                var newname = upg.Name.Replace(oldname, name);
                upg.Name = newname;
                work.Commit();
            }
        }
        public void GroupUpdateUserPermission(int upgId, string name, string oldname)
        {
            //var affectedUpgs = _userPermissionGroupRepository.FindAll(x => GetAffectedUserPermissionGroupIds(upgId).Contains(x.Id)).ToList();
            var affIds = GetAffectedUserPermissionGroupIds(upgId);
            //List<UserPermissionGroupTimeZone> affectedUpgs = 
            //var affectedUpgs = _userPermissionGroupRepository.FindById(affIds)
            foreach (var upg in affIds)
            {
                UpdateUserPermission(upg, name, oldname); //illi  16.10.2017 bug
            }

            var addaffIds = GetAddAffectedUserPermissionGroupIds(oldname);
            foreach (var upg in addaffIds)
            {
                UpdateAddUserPermission(upg, name, oldname);
            }
        }
        [HttpPost]
        public ActionResult Edit(PermissionEditViewModel item)
        {
            var out_pg = CreateViewModel<PermissionEditViewModel>();
            out_pg.Permission = item.Permission;
            string err_msg = string.Empty;
            bool IsSomethingToChange = false;
            var oldname = _userPermissionGroupRepository.FindById(item.Permission.Id).Name;
            if (ModelState.IsValid)
            {
                try
                {
                    UpdateUserPermission(item.Permission.Id, item.Permission.Name, oldname);
                    GroupUpdateUserPermission(item.Permission.Id, item.Permission.Name, oldname);
                    UpdateNewOwnerName(item.Permission.Id, item.Permission.UserID);
                    //_userPermissionGroupService.UpdateUserPermission(item.Permission.Id, item.Permission.Name);
                    //_userPermissionGroupService.GroupUpdateUserPermission(item.Permission.Id, item.Permission.Name, oldname);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }

                if (_userPermissionGroupRepository.FindAll().Where(x => x.Name.Trim().Contains(item.Permission.Name.Trim())).Any())
                { IsSomethingToChange = true; }
            }

            return Json(new
            {
                item = item.Permission,
                Change = IsSomethingToChange,
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.TimeZonesEditMessage : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = ModelState.IsValid ? null : this.RenderPartialView("Edit", out_pg)
            });
        }

        /* public JsonResult EnableDelteteUsersFromPermission()
         {
             bool enable = false;
            // Int32 currentRoleID=Convert.ToInt32(Session["Role_ID"]);
             //CurrentUser.Get().IsCompanyManager
             var menues_list = (from menu in revm.Role.MenuItems.Where(x => x.IsItemAvailable && x.IsSelected) select menu.Value).ToList();
             if (CurrentUser.Get().IsCompanyManager)
             {
                 if (menues_list.Value[24].IsSelected) { enable = true; }
                 else
                 {
                     enable = false;
                 }
             }
             return Json(enable, JsonRequestBehavior.AllowGet);
         }*/


        #endregion

        #region Json Actions

        public JsonResult GetName(int id)
        {
            UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
            string name = upg.Name + " ( " + _userTimeZoneRepository.FindById(upg.DefaultUserTimeZoneId).Name + " ) ";
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsGroupEditable(int id)
        {
            UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
            return Json((CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsSuperAdmin) ? true : !upg.PermissionIsActive, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckName(string name, int? id)
        {
            string result = "ok";
            //int tc = _userPermissionGroupRepository.FindAll().Where(x => !x.IsDeleted && x.Name.Trim() == name.Trim() && !x.IsDeleted && x.ParentUserPermissionGroupId == null && x.Id != id).Count();
            int tc = _userPermissionGroupRepository.FindAll().Where(x => !x.IsDeleted && (x.Name.Trim().Contains(name.Trim()) || name.Trim().Contains(x.Name.Trim())) && !x.IsDeleted && x.ParentUserPermissionGroupId == null && x.Id != id).Count();
            if (tc != 0)
            { result = "error"; }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUsage(int id)
        {
            var permgr = _userPermissionGroupRepository.FindById(id).Name;
            return Json(_userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.User.Active && (x.Name.Trim().Contains("++" + permgr.Trim()) || x.ParentUserPermissionGroupId == id || (x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null && x.Id == id))).Count(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPermissionGroups(int? id)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.PermissionsSelectPermissionGroupOption + "</option>");
            IEnumerable<UserPermissionGroup> permGroups;
            if (!CurrentUser.Get().IsSuperAdmin && !CurrentUser.Get().IsBuildingAdmin)
            {
                string OwnPermissionGroup = (from c in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null) select c.Name).FirstOrDefault(); //First();
                var SamePermissionGroupUsers = (from c in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId != null && x.Name == OwnPermissionGroup) select c.UserId).ToArray();
                //permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null && SamePermissionGroupUsers.Contains(x.UserId));
                //illi 25.12.1012 v16 sort
                permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null && SamePermissionGroupUsers.Contains(x.UserId) || !x.IsDeleted && x.UserId == CurrentUser.Get().Id).OrderBy(x => x.Name);
                //permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null);
                //permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id).OrderBy(x => x.Name);
            }
            else
            {
                //illi 25.12.1012 v16 sort
                permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null).OrderBy(x => x.Name);
            }

            var curPermGroup = id.HasValue ? _userPermissionGroupRepository.FindAll().Where(upg => !upg.IsDeleted && upg.UserId == id && upg.PermissionIsActive).FirstOrDefault() : null;
            foreach (var pg in permGroups)
            {
                result.Append(string.Format("<option value=\"{0}\"{2}>{1}</option>", pg.Id, pg.Name, curPermGroup == null ? "" :
                    curPermGroup.Id == pg.Id ? "selected = \"selected\"" : ""));
            }
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EnableDeleteUserFromPermGroup()
        {
            // var flag = false;
            var revm = CreateViewModel<RoleEditViewModel>();
            var menues_list = (from menu in revm.Role.MenuItems.Where(x => x.IsItemAvailable && x.IsSelected) select menu.Value).ToList();
            if (menues_list.Contains(25))
            {
                return Json(menues_list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(revm, JsonRequestBehavior.AllowGet);
            }
        }
        //GetAll Users
        public JsonResult UsersInPermissionGroup(int Id)
        {
            if (Id != 0)
            {
                var permgr = _userPermissionGroupRepository.FindById(Id).Name;

                //var uvm = CreateViewModel<UserListViewModel>();
                List<int> usersID = new List<int>();
                var pgtz = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.User.Active && x.PermissionIsActive && (x.Name.Trim().Contains("++" + permgr.Trim()) || x.ParentUserPermissionGroupId == Id || (x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null && x.Id == Id)));
                /* if (pgtz.Count().Equals(0)) 
                 {
                 return "No users"; //need to handle this ?
                 }*/
                var name = permgr;

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
            }
            else
            {
                var message = "No Permision Id";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
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

        [HttpPost]
        public JsonResult CanRename(int id)
        {
            if (!CurrentUser.Get().IsCompanyManager) return Json("yes");
            UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
            if (!upg.IsOriginal) return Json("yes");
            else return Json("no");
        }
        #endregion

        #region dltPermission

        /*      public JsonResult CheckBOverlapping(int PermissionGroupID, string PermissionGroupName)
              {
               //PermissionGroupID=id of the group which is assigned to the user //47
               //PermissionGroupName=name of permission Group..Pärnu mnt. 102-2.Grupp 5xx++abcMe++demo 2++All areas24/7

                  bool isOverlappping = false;
                  var upgName = _userPermissionGroupRepository.FindAll(x => x.Id == PermissionGroupID).Select(x => x.Name).FirstOrDefault();

                  List<List<int>> buildingObjectIDs = new List<List<int>>();
                  List<int> AllBuildingObjIDs = new List<int>();

                  //If user have more than 1 permision Group. 
                  if (upgName.Contains("++"))
                  {
                      var allPermissionGroups = upgName.Split(new[] { "++" }, StringSplitOptions.None);
                      foreach (var grpName in allPermissionGroups)
                      { 
                      var grpID = _userPermissionGroupRepository.FindAll(x => x.Name == grpName && x.ParentUserPermissionGroupId == null && x.IsDeleted == false).FirstOrDefault().Id;

                          //select all BuildingObjectID for the GroupId from USERTIMEZONE table				
                          var grp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList();
                          foreach (var boid in grp_buildingObjIDs)
                          {
                              AllBuildingObjIDs.Add(boid);
                          }
                      }
                  }
                  else
                  {
                      //If User have only 1 Permission Group
                      var grpID = _userPermissionGroupRepository.FindAll(x => x.Name == upgName && x.ParentUserPermissionGroupId == null && x.IsDeleted == false).FirstOrDefault().Id;
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

                      var query = AllBuildingObjIDs.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList(); //?

                      //  var query2 = AllBuildingObjIDs.GroupBy(x => x)
                      //.Where(g => g.Count() > 1)
                      //.ToDictionary(x => x.Key, y => y.Count());

                      isOverlappping = query.Count > 0 ? true : false;
                  }
                  return Json(isOverlappping, JsonRequestBehavior.AllowGet);

              }
        */
        public JsonResult DelPermissionGroupFromUsers(int userId, int dltGroupId)
        {

            return Json(_userPermissionGroupService.DelPermissionGroupFromUsers(userId, dltGroupId), JsonRequestBehavior.AllowGet);

        }


        public void DelPermissionGroupFromUsersNew(string userId, int dltGroupId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                string[] arr = userId.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    int id = Convert.ToInt32(arr[i]);
                    var upg = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == id && x.PermissionIsActive == true);
                    if (upg.Count() > 0)
                    {
                        if (upg.FirstOrDefault().Id.ToString() != "0")
                        {
                            _userPermissionGroupService.DelPermissionGroupFromUsers(id, dltGroupId);
                        }
                    }
                }
            }
        }
        #endregion
    }
}