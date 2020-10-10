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
    public class RemoteRegistrationController : BusinessCaseController
    {
        private readonly IUserPermissionGroupService _userPermissionGroupService;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly ITitleRepository _titleRepository;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUserBuildingRepository _userBuildingRepository;
        private readonly IUserTimeZoneService _userTimeZoneService;
        private readonly ILogService _logService;
        private readonly IUserTimeZonePropertyRepository _userTimeZonePropertyRepository;
        private readonly IUserPermissionGroupRepository _userPermissionGroupRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserService _userService;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        

        public RemoteRegistrationController(IUserService userService, ICurrentUser currentUser,
                                   ILogger logger,
                                   IUserPermissionGroupService userPermissionGroupService,
                                   IBuildingRepository buildingRepository,
                                   ITitleRepository titleRepository,
                                   IUserBuildingRepository userBuildingRepository,
                                   IBuildingObjectRepository buildingObjectRepository,
                                   IUserTimeZoneRepository userTimeZoneRepository,
                                   ILogService logService,
                                   IUserTimeZoneService userTimeZoneService,
                                   IUserTimeZonePropertyRepository userTimeZonePropertyRepository,
                                   IUserPermissionGroupRepository userPermissionGroupRepository,
                                   IUserPermissionGroupTimeZoneRepository userPermissionGroupTimeZoneRepository,
                                   IUserRepository userRepository,
                                   IRoleRepository roleRepository,
                                   ICompanyRepository companyRepository,
                                   IUserRoleRepository userRoleRepository) : base(currentUser, logger)
        {
            _userPermissionGroupService = userPermissionGroupService;
            _buildingRepository = buildingRepository;
            _userService = userService;
            _buildingObjectRepository = buildingObjectRepository;
            _userBuildingRepository = userBuildingRepository;
            _userTimeZoneRepository = userTimeZoneRepository;
            _companyRepository = companyRepository;
            _titleRepository = titleRepository;
            _userTimeZoneService = userTimeZoneService;
            _userTimeZonePropertyRepository = userTimeZonePropertyRepository;
            _userPermissionGroupRepository = userPermissionGroupRepository;
            _userPermissionGroupTimeZoneRepository = userPermissionGroupTimeZoneRepository;
            _userRepository = userRepository;
            _logService = logService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        FoxSecDBContext db = new FoxSecDBContext();
        // GET: RemoteRegistration
        public ActionResult Index()
        {
            var hvm = CreateViewModel<HomeViewModel>();
            return View(hvm);
        }

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

                var companyBuildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted &&/*role_buildingIds.Contains(x.BuildingId)
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
                     where o.ParentObjectId.HasValue && (o.TypeId == (int)BuildingObjectTypes.Door)
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
                     where o.ParentObjectId.HasValue && (o.TypeId == (int)BuildingObjectTypes.Door)

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

        public String CheckObjectPermission(int id, int compid)
        {
            var uvm = CreateViewModel<UserListViewModel>();
            List<User> users = new List<User>();

            List<int> companyIds = new List<int>();
            companyIds = _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == compid || x.ParentId == compid)).Select(y => y.Id).ToList();
            users = _userRepository.FindAll(x => x.IsDeleted == false && x.Active == true && x.CompanyId != null && companyIds.Contains(Convert.ToInt32(x.CompanyId))).ToList();
            var userslist = users.OrderBy(x => x.LastName);

            String result = "<table cellpadding='1' cellspacing='0' style='margin: 0; width: 100%; padding: 1px; border-spacing: 0;'><th style='width: 5%; padding: 2px;'>  <input id='check_all' name='check_all' type='checkbox' class='tipsy_we' original-title='Select all!' onclick='javascript: CheckAll();' /></th><th style='width: 19%; padding: 2px;'>Last Name</th><th style='width: 19%; padding: 2px;'>First Name</th><th style='width: 19%; padding: 2px;'>Company</th><th style='width: 10%; padding: 2px;'>Title</th><th style='width: 10%; padding: 2px;'>Permission Group</th><th style='width: 10%; padding: 2px;'>Role</th>";
            var c = 0;

            foreach (var usr in userslist)
            {
                c++;
                var dep = usr.UserPermissionGroups.Count() != 0 ? usr.UserPermissionGroups.First().Name : "";// = usr.UserDepartments.Count() != 0 ? usr.UserDepartments.First().Department.Name : "";
                var role = "";
                if (usr.UserRoles.Count > 0)
                {
                    var rolel = usr.UserRoles.Where(x => !x.IsDeleted && x.UserId == usr.Id && DateTime.Now >= x.ValidFrom && DateTime.Now <= x.ValidTo).FirstOrDefault();
                    if(rolel != null)
                    {
                        role = rolel.Role.Name;
                    }
                }
                var fullNm = usr.FirstName + " " + usr.LastName;                                                                                                        //var roleN = _roleRepository.FindById(role);
                var title = "";
                if (usr.TitleId != null)
                {
                    title = _titleRepository.FindAll(x => x.Id == usr.TitleId).FirstOrDefault().Name;
                }
                var com = usr.Company != null ? usr.Company.Name : "";
                string add = "";
                if (c % 2 == 0)
                {
                    add = "<tr style='background-color:#CCC;'><td><input type='checkbox' id='" + usr.Id + "' name='sel_checkbox'/></td><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + title + "</td><td>" + dep + "</td><td>" + role + "</td></tr>";
                }
                else
                {
                    add = "<tr><td><input type='checkbox' id='" + usr.Id + "' name='sel_checkbox'/></td><td>" + usr.LastName + "</td><td>" + usr.FirstName + "</td><td>" + com + "</td><td>" + title + "</td><td>" + dep + "</td><td>" + role + "</td></tr>";
                }
                result = result + add;
            }
            result = result + "</table>";

            return result;
        }

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

        public JsonResult GetCompanies()
        {
            StringBuilder result = new StringBuilder();
            if (!CurrentUser.Get().IsCompanyManager)
            {
                result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");
            }

            var companies = _companyRepository.FindAll().Where(cc => !cc.IsDeleted && cc.Active).OrderBy(x => x.Name.ToLower()).ToList();
            if (CurrentUser.Get().IsCompanyManager)
            {
                //var uID = CurrentUser.Get().Id;
                //var comp = (from c in db.Companies
                //            join u in db.User on c.Id equals u.CompanyId
                //            where u.Id == uID
                //            select c.Id).FirstOrDefault();

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

                companies = companies.Where(x => companyIds.Contains(x.Id)).ToList();
            }

            if (CurrentUser.Get().IsBuildingAdmin)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Select(ub => ub.BuildingId).ToList();

                if (buildIds.Count > 0)
                {
                    companies =
                        companies.Where(
                            x =>
                            x.Active && !x.IsDeleted &&
                            x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildIds.Contains(y.BuildingObject.BuildingId))).ToList();

                    var parentComps = (from comp in companies where comp.ParentId != null select _companyRepository.FindById(comp.ParentId.Value)).ToList();

                    if (parentComps.Count > 0)
                    {
                        parentComps = parentComps.Where(
                            x =>
                            x.Active && !x.IsDeleted &&
                            x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildIds.Contains(y.BuildingObject.BuildingId))).ToList();
                    }
                    foreach (var parentComp in parentComps.Where(parentComp => !companies.Any(comp => comp.Id == parentComp.Id)))
                    {
                        companies.Add(parentComp);
                    }
                }
                else
                {
                    companies = new List<Company>();
                }
            }

            foreach (var cc in companies)
            {
                if (CurrentUser.Get().IsCompanyManager)
                {
                    int cmp_id = CurrentUser.Get().CompanyId.Value;
                    if (cmp_id == cc.Id)
                    {
                        result.Append("<option value=" + '"' + cc.Id + '"' + " selected>" + cc.Name + "</option>");
                    }
                    else
                    {
                        result.Append("<option value=" + '"' + cc.Id + '"' + ">" + cc.Name + "</option>");
                    }                 
                }
                else { result.Append("<option value=" + '"' + cc.Id + '"' + ">" + cc.Name + "</option>"); }
            }
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}