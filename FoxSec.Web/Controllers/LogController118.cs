using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Xml.Linq;
using AutoMapper;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.ViewModels;
using ViewResources;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using FoxSec.Web.ListModel;



//using System.Web.UI.WebControls;

namespace FoxSec.Web.Controllers
{

    public class LogController : PaginatorControllerBase<LogItem>
    {
        FoxSecDBContext db = new FoxSecDBContext();

        //private readonly FoxSec.Infrastructure.EF.Repositories.Interfaces.IFSSubLocationRepository _FSSubLocationRepository;
        private readonly ILogRepository _logRepository;
        private readonly IUserBuildingRepository _userBuildingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogFilterRepository _logFilterRepository;
        private readonly ILogFilterService _logFilterService;
        private readonly IDepartmentRepository _DepartmentRepository;
        private ResourceManager _resourceManager;

        public LogController(   //FoxSec.Infrastructure.EF.Repositories.Interfaces.IFSSubLocationRepository FSSubLocation,
                                ILogger logger,
                                ICurrentUser currentUser,
                                ILogRepository logRepository,
                                IUserBuildingRepository userBuildingRepository,
                                IUserRepository userRepository,
                                ICompanyRepository companyRepository,
                                IDepartmentRepository DepartmentRepository,
                                ILogFilterRepository logFilterRepository,
                                ILogFilterService logFilterService)
            : base(currentUser, logger)
        {
            // _FSSubLocationRepository = FSSubLocation;

            _logRepository = logRepository;
            _userBuildingRepository = userBuildingRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _logFilterRepository = logFilterRepository;
            _DepartmentRepository = DepartmentRepository;
            _logFilterService = logFilterService;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
            RegisterLogRelatedMap();
        }

        public ActionResult TabContent()
        {


            var hmv = CreateViewModel<HomeViewModel>();


            return PartialView(hmv);
        }
        public ActionResult ManageDeteteFilterLog(string Building, string IsShowDefaultLog, string searchbox, string Node, string CompanyId, string Name, string selectedFilter, string UserName, string Activity, string btnclick)
        {
            //DateTime From = DateTime.ParseExact(FromDate, "dd.MM.yyyy HH:mm",
            //                            System.Globalization.CultureInfo.InvariantCulture);
            //DateTime To = DateTime.ParseExact(ToDate, "dd.MM.yyyy HH:mm",
            //                           System.Globalization.CultureInfo.InvariantCulture);

            try
            {

                int totalRecordView = 0;
                int k;
                int role = Convert.ToInt32(Session["Role_ID"]);
                var RoleDetails = db.UserRoles.Where(x => x.Id == role).FirstOrDefault();
                string UserRole = RoleDetails.Name;

                int UserId = Convert.ToInt32(Session["User_Id"]);
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, UserId).FirstOrDefault();
                string FullNmae = "";
                if (userLogDeatil != null)
                {

                    FullNmae = userLogDeatil.FirstName + " " + userLogDeatil.LastName;
                }




                string Insertquery = @" insert into UserLog (UserId,UserFullName,UserRole,SearchBOx,Datefrom,dateto,buildingfilter,ShowDefaultOrfullLog,Node,CompanyFiter,Name,Filter_text,user_text,Activity,button_clicked,totalRecordView,VisitDate) 
values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})";
                int sm = 0;
                //var Executequery = db.Database.SqlQuery<UserLog>(Insertquery, UserId, FullNmae, UserRole, searchbox, FromDate, ToDate, Building, IsShowDefaultLog.Trim(), Node, "0", Name, "0", UserName, Activity, "NA", 20, System.DateTime.Now);
                sm = sm + db.Database.ExecuteSqlCommand(Insertquery, UserId, FullNmae, UserRole, searchbox, "", "", Building, IsShowDefaultLog.Trim(), Node, "0", Name, "0", UserName, Activity, btnclick, totalRecordView, System.DateTime.Now);

                string getquery = @" select top 1 id from UserLog order by id desc";
                var getid = db.Database.SqlQuery<Box111>(getquery).FirstOrDefault();

                k = getid.id;
                return Json(k, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult SaveFilterDetailsinUserLog(string FromDate, string ToDate, string Building, string IsShowDefaultLog, string searchbox, string Node, string CompanyId, string Name, string selectedFilter, string UserName, string Activity, string btnclick)
        {
            DateTime From = DateTime.ParseExact(FromDate, "dd.MM.yyyy HH:mm",
                                        System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDate, "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

            try
            {

                int totalRecordView = 0;
                int k;
                int role = Convert.ToInt32(Session["Role_ID"]);
                var RoleDetails = db.UserRoles.Where(x => x.Id == role).FirstOrDefault();
                string UserRole = RoleDetails.Name;

                int UserId = Convert.ToInt32(Session["User_Id"]);
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, UserId).FirstOrDefault();
                string FullNmae = "";
                if (userLogDeatil != null)
                {

                    FullNmae = userLogDeatil.FirstName + " " + userLogDeatil.LastName;
                }




                string Insertquery = @" insert into UserLog (UserId,UserFullName,UserRole,SearchBOx,Datefrom,dateto,buildingfilter,ShowDefaultOrfullLog,Node,CompanyFiter,Name,Filter_text,user_text,Activity,button_clicked,totalRecordView,VisitDate) 
values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})";
                int sm = 0;
                //var Executequery = db.Database.SqlQuery<UserLog>(Insertquery, UserId, FullNmae, UserRole, searchbox, FromDate, ToDate, Building, IsShowDefaultLog.Trim(), Node, "0", Name, "0", UserName, Activity, "NA", 20, System.DateTime.Now);
                sm = sm + db.Database.ExecuteSqlCommand(Insertquery, UserId, FullNmae, UserRole, searchbox, From, To, Building, IsShowDefaultLog.Trim(), Node, "0", Name, "0", UserName, Activity, btnclick, totalRecordView, System.DateTime.Now);

                string getquery = @" select top 1 id from UserLog order by id desc";
                var getid = db.Database.SqlQuery<Box111>(getquery).FirstOrDefault();

                k = getid.id;
                return Json(k, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult ManageUserLog(string FromDate, string ToDate, string Building, string IsShowDefaultLog, string searchbox, string Node, string CompanyId, string Name, string selectedFilter, string UserName, string Activity, string btnclick)
        {
            DateTime From = DateTime.ParseExact(FromDate, "dd.MM.yyyy HH:mm",
                                        System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDate, "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

            try
            {

                int totalRecordView = Convert.ToInt32(Session["totalrowcount"]);
                int k;
                int role = Convert.ToInt32(Session["Role_ID"]);
                var RoleDetails = db.UserRoles.Where(x => x.Id == role).FirstOrDefault();
                string UserRole = RoleDetails.Name;

                int UserId = Convert.ToInt32(Session["User_Id"]);
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, UserId).FirstOrDefault();
                string FullNmae = "";
                if (userLogDeatil != null)
                {

                    FullNmae = userLogDeatil.FirstName + " " + userLogDeatil.LastName;
                }




                string Insertquery = @" insert into UserLog (UserId,UserFullName,UserRole,SearchBOx,Datefrom,dateto,buildingfilter,ShowDefaultOrfullLog,Node,CompanyFiter,Name,Filter_text,user_text,Activity,button_clicked,totalRecordView,VisitDate) 
values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})";
                int sm = 0;
                //var Executequery = db.Database.SqlQuery<UserLog>(Insertquery, UserId, FullNmae, UserRole, searchbox, FromDate, ToDate, Building, IsShowDefaultLog.Trim(), Node, "0", Name, "0", UserName, Activity, "NA", 20, System.DateTime.Now);
                sm = sm + db.Database.ExecuteSqlCommand(Insertquery, UserId, FullNmae, UserRole, searchbox, From, To, Building, IsShowDefaultLog.Trim(), Node, "0", Name, "0", UserName, Activity, btnclick, totalRecordView, System.DateTime.Now);

                string getquery = @" select top 1 id from UserLog order by id desc";
                var getid = db.Database.SqlQuery<Box111>(getquery).FirstOrDefault();

                k = getid.id;
                return Json(k, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult UpdateUserLog(int rowcount, int id)
        {
            int sm = 0;
            string queryUpdateusrlog = "update UserLog set  totalRecordView={0} where id={1}";



            sm = sm + db.Database.ExecuteSqlCommand(queryUpdateusrlog, rowcount, id);

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Location()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }
        public class report
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? companyId { get; set; }

        }

        public int ToAllCompanies(LogFilterItem filter, int? reportId)
        {
            var FocSecModelcontexLoction = ConfigurationManager.ConnectionStrings["FoxSecDBEntities"].ConnectionString;
            ObjectContext Sql = new ObjectContext(FocSecModelcontexLoction);
            int id = reportId.GetValueOrDefault();
            var Query = "SELECT Id, Sublocation AS Name, CompanyId FROM dbo.FSSublocations WHERE (Id = " + id + ")";
            var reports = Sql.ExecuteStoreQuery<report>(Query);
            foreach (var cc in reports)
            {
                if (cc.companyId != null) { return 1; }
            }
            return 2;
        }
        public bool ReportToAllCompanies(LogFilterItem filter, int? reportId)
        {
            var FocSecModelcontexLoction = ConfigurationManager.ConnectionStrings["FoxSecDBEntities"].ConnectionString;
            ObjectContext Sql = new ObjectContext(FocSecModelcontexLoction);
            int id = reportId.GetValueOrDefault();
            var Query = "UPDATE [FSSublocations] SET CompanyId = NULL WHERE id=" + reportId;
            try
            {
                var logtest = Sql.ExecuteStoreCommand(Query);
            }
            catch (Exception)
            { return false; }

            return true;
        }
        public JsonResult GetReports()
        {
            var FocSecModelcontexLoction = ConfigurationManager.ConnectionStrings["FoxSecDBEntities"].ConnectionString;
            ObjectContext Sql = new ObjectContext(FocSecModelcontexLoction);
            var Query = "SELECT Id, Sublocation As Name FROM dbo.FSSublocations";
            if (CurrentUser.Get().CompanyId != null)
            {
                Query = Query + " WHERE (CompanyId =" + CurrentUser.Get().CompanyId + ") OR (CompanyId IS NULL)";
            }
            var reports = Sql.ExecuteStoreQuery<report>(Query);

            StringBuilder result = new StringBuilder();
            // result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");


            foreach (var cc in reports)
            {
                result.Append("<option value=" + '"' + cc.Id + '"' + ">" + cc.Name + "</option>");
            }
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public bool ReportCompany(LogFilterItem filter, int? reportId)
        {
            if (reportId != null && filter.CompanyId != null)
            {
                try
                {
                    var FocSecModelcontexLoction = ConfigurationManager.ConnectionStrings["FoxSecDBEntities"].ConnectionString;

                    ObjectContext Sql = new ObjectContext(FocSecModelcontexLoction);
                    var Query = "UPDATE [FSSublocations] SET CompanyId = " + filter.CompanyId + " WHERE id=" + reportId;

                    var logtest = Sql.ExecuteStoreCommand(Query);
                }
                catch (Exception)
                { return false; }
                return true;
            }
            else
            { return false; }
        }

        public JsonResult GetCompanies()
        {
            StringBuilder result = new StringBuilder();
            result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");

            var companies = _companyRepository.FindAll().Where(cc => !cc.IsDeleted && cc.Active).OrderBy(x => x.Name.ToLower()).ToList();
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
                result.Append("<option value=" + '"' + cc.Id + '"' + ">" + cc.Name + "</option>");
            }

            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentsByCompany(int? id)
        {
            StringBuilder result = new StringBuilder();
            FoxSecDBContext db = new FoxSecDBContext();
            int rol_id = Convert.ToInt32(Session["Role_ID"]);
            int user_id = Convert.ToInt32(Session["User_Id"]);

            var depar_ids = db.UserDeparts.Where(x => x.UserId == user_id).ToList();
            //var names1 = new string[] { "Alex", "Colin", "Danny", "Diego" };

            if (rol_id != 5)
            {
                foreach (var items in depar_ids)
                {
                    int did = Convert.ToInt32(items.DepartmentId);

                    var buildings = db.Departments.Where(x => x.Id == did && x.IsDeleted == false).ToList();
                    result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");
                    foreach (var cc in buildings)
                    {
                        result.Append("<option value=" + '"' + cc.Id + '"' + ">" + cc.Name + "</option>");
                    }
                }
                return Json(result.ToString(), JsonRequestBehavior.AllowGet);
            }

            //Session["Role_ID"] = role_id;
            //Session["User_Id"] = role.UserId;
            //var dep_id=db.de.
            else
            {
                //StringBuilder result = new StringBuilder();
                result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");

                var departments = id != null ? _DepartmentRepository.FindAll().Where(dp => !dp.IsDeleted && dp.CompanyId == id).OrderBy(x => x.Name.ToLower()).ToList() : _DepartmentRepository.FindAll().ToList();

                foreach (var dp in departments)
                {
                    result.Append("<option value=" + '"' + dp.Id + '"' + ">" + dp.Name + "</option>");
                }
                return Json(result.ToString(), JsonRequestBehavior.AllowGet);
            }
            //return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOld(LogFilterItem filter, int? nav_page, int? rows, int? sort_field, int? sort_direction)
        {
            var lvm = CreateViewModel<LogListViewModel>();
            List<Log> logs = _logRepository.FindAll().OrderByDescending(lg => lg.EventTime).ToList();
            List<Log> filteredLogs = new List<Log>();
            bool isCommonsearch = false;
            bool isFilterApplied = false;

            if (!string.IsNullOrEmpty(filter.Node))
            {
                filter.Node = filter.Node.Trim();
            }

            if (!string.IsNullOrWhiteSpace(filter.CommonSearch))
            {
                isCommonsearch = true;
                filter.Activity = filter.CommonSearch;
                filter.Building = filter.CommonSearch;
                var company = _companyRepository.FindAll().Where(cc => cc.Name.ToLower().Contains(filter.CommonSearch.ToLower())).FirstOrDefault();
                if (company != null)
                {
                    filter.CompanyId = company.Id;
                }
                filter.Node = filter.CommonSearch;
                filter.UserName = filter.CommonSearch;
                filter.FromDate = filter.CommonSearch;
                filter.ToDate = filter.CommonSearch;
            }

            var curr_user = CurrentUser.Get();
            if (curr_user.IsBuildingAdmin)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Where(x => !x.IsDeleted).Select(ub => ub.BuildingId).ToList();
                var restr_user_ids = from us in
                                         _userRepository.FindAll().Where(
                                             x =>
                                             x.UserRoles.Any(
                                                 ur =>
                                                 !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now &&
                                                 ur.Role.RoleTypeId < (int)RoleTypeEnum.BA))
                                     select us.Id;

                if (user_buildings != null)
                {
                    logs = logs.Where(log => !restr_user_ids.Contains(log.UserId.HasValue ? log.UserId.Value : 0) && log.User != null && log.User.UserBuildings != null &&
                        log.User.UserBuildings.Any(ubo => !ubo.IsDeleted && buildIds.Contains(ubo.BuildingId))).ToList();
                }
            }

            if (curr_user.IsCompanyManager && curr_user.CompanyId.HasValue)
            {
                logs = logs.Where(log => log.CompanyId != null && (log.CompanyId == curr_user.CompanyId
                                                                   ||
                                                                   (log.Company.ParentId != null &&
                                                                    log.Company.ParentId == curr_user.CompanyId))).ToList();
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Where(x => !x.IsDeleted).Select(ub => ub.BuildingId).ToList();

                var restr_user_ids = from us in
                                         _userRepository.FindAll().Where(
                                             x =>
                                             x.UserRoles.Any(
                                                 ur =>
                                                 !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now &&
                                                 ur.Role.RoleTypeId < (int)RoleTypeEnum.CM))
                                     select us.Id;

                if (user_buildings != null)
                {
                    logs = logs.Where(log => log.User != null && !restr_user_ids.Contains(log.UserId.Value) && log.User.UserBuildings != null &&
                        log.User.UserBuildings.Any(ubo => !ubo.IsDeleted && buildIds.Contains(ubo.BuildingId))).ToList();
                }
            }

            if (!String.IsNullOrWhiteSpace(filter.UserName))
            {
                string[] split = filter.UserName.ToLower().Trim().Split(' ');

                var filtLogs = new List<Log>();
                if (split.Count() == 1)
                {
                    filtLogs = logs.Where(x => (x.UserId != null && x.User.FirstName.ToLower().Contains(split[0]) || x.User.LoginName.ToLower().Contains(split[0]) || x.User.LastName.ToLower().Contains(split[0]) || (x.User.PersonalId != null && x.User.PersonalId.ToLower().Contains(split[0])))).ToList();
                }
                else if (split.Count() == 2)
                {
                    filtLogs = logs.Where(x => x.UserId != null && x.User.FirstName.ToLower().Contains(split[0]) && x.User.LastName.ToLower().Contains(split[1])).ToList();
                }

                filteredLogs.AddRange(filtLogs);

                isFilterApplied = true;
            }

            if (filter.CompanyId.HasValue && !curr_user.IsCompanyManager)
            {
                var search_col = isCommonsearch ? logs : isFilterApplied ? filteredLogs : logs;

                var filt_logs =
                    search_col.Where(
                        log =>
                        log.CompanyId != null &&
                        (log.CompanyId == filter.CompanyId || (log.Company.ParentId != null && log.Company.ParentId == filter.CompanyId))).ToList();

                if (isCommonsearch)
                {
                    foreach (var filtLog in filt_logs)
                    {
                        if (!filteredLogs.Any(fl => fl.Id == filtLog.Id))
                        {
                            filteredLogs.Add(filtLog);
                        }
                    }
                }
                else
                {
                    filteredLogs = filt_logs;
                }

                isFilterApplied = true;
            }

            if (!string.IsNullOrWhiteSpace(filter.Building))
            {
                var search_col = isCommonsearch ? logs : isFilterApplied ? filteredLogs : logs;

                var filt_logs =
                    search_col.Where(
                        log => !string.IsNullOrWhiteSpace(log.Building) && log.Building.ToLower() == filter.Building.ToLower()).ToList();

                if (isCommonsearch)
                {
                    foreach (var filtLog in filt_logs)
                    {
                        if (!filteredLogs.Any(fl => fl.Id == filtLog.Id))
                        {
                            filteredLogs.Add(filtLog);
                        }
                    }
                }
                else
                {
                    filteredLogs = filt_logs;
                }

                isFilterApplied = true;
            }

            if (!string.IsNullOrWhiteSpace(filter.Node))
            {
                var search_col = isCommonsearch ? logs : isFilterApplied ? filteredLogs : logs;

                var filt_logs =
                    search_col.Where(
                        log => !string.IsNullOrWhiteSpace(log.Node) && log.Node.ToLower().Contains(filter.Node.ToLower())).ToList();

                if (isCommonsearch)
                {
                    foreach (var filtLog in filt_logs)
                    {
                        if (!filteredLogs.Any(fl => fl.Id == filtLog.Id))
                        {
                            filteredLogs.Add(filtLog);
                        }
                    }
                }
                else
                {
                    filteredLogs = filt_logs;
                }

                isFilterApplied = true;
            }

            if (!string.IsNullOrWhiteSpace(filter.Activity))
            {
                var search_col = isCommonsearch ? logs : isFilterApplied ? filteredLogs : logs;
                var filt_logs =
                    search_col.Where(
                        log => !string.IsNullOrWhiteSpace(log.Action) && GetLogAction(log.Action).ToLower().Contains(filter.Activity.ToLower())).ToList();

                if (isCommonsearch)
                {
                    foreach (var filtLog in filt_logs)
                    {
                        if (!filteredLogs.Any(fl => fl.Id == filtLog.Id))
                        {
                            filteredLogs.Add(filtLog);
                        }
                    }
                }
                else
                {
                    filteredLogs = filt_logs;
                }

                isFilterApplied = true;
            }

            if (!string.IsNullOrWhiteSpace(filter.FromDate))
            {
                try
                {
                    var fromDate = DateTime.ParseExact(filter.FromDate, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

                    var search_col = isCommonsearch ? logs : isFilterApplied ? filteredLogs : logs;
                    var filt_logs = search_col.Where(log => log.EventTime.Date >= fromDate.Date).ToList();

                    if (isCommonsearch)
                    {
                        foreach (var filtLog in filt_logs)
                        {
                            if (!filteredLogs.Any(fl => fl.Id == filtLog.Id))
                            {
                                filteredLogs.Add(filtLog);
                            }
                        }
                    }
                    else
                    {
                        filteredLogs = filt_logs;
                    }

                    isFilterApplied = true;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Date format should be 'dd.MM.yyyy'");
                }
            }

            if (filter.IsShowDefaultLog)
            {
                filteredLogs = filteredLogs.Where(x => x.LogType.IsDefault).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.ToDate))
            {
                try
                {
                    var search_col = isCommonsearch ? logs : isFilterApplied ? filteredLogs : logs;

                    var toDate = DateTime.ParseExact(filter.ToDate, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

                    var filt_logs = search_col.Where(log => log.EventTime.Date <= toDate.Date).ToList();

                    if (isCommonsearch)
                    {
                        foreach (var filtLog in filt_logs)
                        {
                            if (!filteredLogs.Any(fl => fl.Id == filtLog.Id))
                            {
                                filteredLogs.Add(filtLog);
                            }
                        }
                    }
                    else
                    {
                        filteredLogs = filt_logs;
                    }

                    isFilterApplied = true;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Date format should be 'dd.MM.yyyy'");
                }
            }

            if (!isFilterApplied)
            {
                filteredLogs = logs;
            }

            IEnumerable<LogItem> list = new List<LogItem>();
            Mapper.Map(filteredLogs, list);

            if (sort_field.HasValue && sort_direction.HasValue)
            {
                switch (sort_field)
                {
                    case 1:
                        if (sort_direction.Value == 0) list = list.OrderBy(log => log.EventTime);
                        else list = list.OrderByDescending(log => log.EventTime);
                        break;
                    case 2:
                        if (sort_direction.Value == 0) list = list.OrderBy(log => log.Building);
                        else list = list.OrderByDescending(log => log.Building);
                        break;
                    case 3:
                        if (sort_direction.Value == 0) list = list.Where(x => x.Node.Trim() != String.Empty).OrderBy(log => log.Node);
                        else list = list.Where(x => x.Node.Trim() != String.Empty).OrderByDescending(log => log.Node);
                        break;
                    case 4:
                        if (sort_direction.Value == 0) list = list.OrderBy(log => log.CompanyName);
                        else list = list.OrderByDescending(log => log.CompanyName);
                        break;
                    case 5:
                        if (sort_direction.Value == 0) list = list.OrderBy(log => log.UserName);
                        else list = list.OrderByDescending(log => log.UserName);
                        break;
                    case 6:
                        if (sort_direction.Value == 0) list = list.OrderBy(log => log.Action);
                        else list = list.OrderByDescending(log => log.Action);
                        break;
                    default:
                        list = list.OrderByDescending(log => log.EventTime);
                        break;
                }
            }
            else
            {
                list = list.OrderByDescending(log => log.EventTime);
            }
            if (!rows.HasValue)
            {
                rows = 50;
            }

            lvm.Paginator = SetupPaginator(ref list, nav_page, rows);
            lvm.Paginator.RowsPerPageItems = new List<SelectListItem>
                            {
                                new SelectListItem()
                                    {Value = "50", Text = string.Format("{0} {1}", 50, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "100", Text = string.Format("{0} {1}", 100, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "200", Text = string.Format("{0} {1}", 200, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "1000", Text = string.Format("{0} {1}", 1000, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "5000", Text = string.Format("{0} {1}", 5000, ViewResources.SharedStrings.CommonPerPage)}
                            };
            lvm.Paginator.DivToRefresh = "AreaLogSearchResults";
            lvm.Paginator.Prefix = "Log";
            lvm.Items = list;

            return PartialView("List", lvm);
        }

        private void RegisterLogRelatedMap()
        {
            Mapper.CreateMap<Log, LogItem>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? string.Format("{0} {1}", src.User.FirstName, src.User.LastName) : ""))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : ""))
                .ForMember(dest => dest.EventTimeStr, opt => opt.MapFrom(src => src.EventTime.ToString("dd.MM.yyyy - HH:mm:ss")))
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => GetLogAction(src.Action)))
                .ForMember(dest => dest.IsUserDeleted, opt => opt.MapFrom(src => src.User == null ? false : src.User.IsDeleted))
                .ForMember(dest => dest.IsCompanyDeleted, opt => opt.MapFrom(src => src.Company == null ? false : src.Company.IsDeleted))
                .ForMember(dest => dest.LogRecordColor, opt => opt.MapFrom(src => string.Format("#{0}", src.LogType.Color)));
        }

        //private string GetLogAction(string action)
        //{
        //    try
        //    {
        //        XElement xx = XElement.Parse(action);
        //        var sb = new StringBuilder();
        //        foreach (var xElement in xx.Elements(XMLLogLiterals.LOG_TRANSLATABLE_SENTENSE))
        //        {
        //            IEnumerable<string> sentense_params = from param in xElement.Elements(XMLLogLiterals.LOG_SENTENSE_PARAM)
        //                                                  select param.Value;
        //            var resource_name = xElement.Attribute(XMLLogLiterals.LOG_SENTENCE_FORMAT).Value;
        //            string message_template = (string) _resourceManager.GetObject(resource_name, Thread.CurrentThread.CurrentCulture);
        //            var message = sentense_params == null ? message_template : string.Format(message_template, sentense_params.ToArray());

        //            sb.Append(string.Format("{0} ", message));
        //        }

        //        return sb.ToString();
        //    }
        //    catch(Exception)
        //    {
        //        return action;
        //    }
        //}
        private List<int> GetRestrictedUserIds()
        {
            var restr_user_ids = new List<int>();

            if (CurrentUser.Get().IsBuildingAdmin)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Where(x => !x.IsDeleted).Select(ub => ub.BuildingId).ToList();
                restr_user_ids = (from us in
                                      _userRepository.FindAll().Where(
                                          x =>/*
                                          x.UserRoles.Any(
                                              ur =>
                                              !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now &&
                                              ur.Role.RoleTypeId < (int)RoleTypeEnum.BA)
                                              || */!x.UserBuildings.Any(ub => !ub.IsDeleted && buildIds.Contains(ub.BuildingId)))
                                  select us.Id).ToList();
            }

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Where(x => !x.IsDeleted).Select(ub => ub.BuildingId).ToList();

                restr_user_ids = (from us in
                                      _userRepository.FindAll().Where(
                                          x =>/*
                                          x.UserRoles.Any(
                                              ur =>
                                              !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now &&
                                              ur.Role.RoleTypeId < (int)RoleTypeEnum.CM) || !x.UserBuildings.Any(ub => !ub.IsDeleted && buildIds.Contains(ub.BuildingId))
                                              || */x.CompanyId != CurrentUser.Get().CompanyId /*|| (x.Company != null && x.Company.ParentId != CurrentUser.Get().CompanyId)*/)
                                  select us.Id).ToList();
            }

            return restr_user_ids;
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
                result.AddRange(from cc in _companyRepository.FindAll() select cc.Id);

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

        [HttpGet]
        public ActionResult List(LogFilterItem filter, int? nav_page, int? rows, int? sort_field, int? sort_direction)
        {
            Session["totalrow"] = rows;
            if (!CurrentUser.Get().IsSuperAdmin) { filter.CompanyId = CurrentUser.Get().CompanyId; }
            var lvm = CreateViewModel<LogListViewModel>();
            var logs = new List<Log>();
            //List<Log> filteredLogs = new List<Log>();
            //bool isCommonsearch = false;
            //bool isFilterApplied = false;
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

            var log_filter = new LogFilter();
            log_filter.Activity = filter.Activity;
            log_filter.Building = filter.Building;
            log_filter.Node = string.IsNullOrWhiteSpace(filter.Node) ? string.Empty : filter.Node.Trim();
            log_filter.Building = filter.Building;
            log_filter.UserName = filter.UserName;
            log_filter.CompanyId = filter.CompanyId;
            log_filter.IsShowDefaultLog = !filter.IsShowDefaultLog;
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

            if (!rows.HasValue)
            {
                rows = 50;
            }
            if (string.IsNullOrWhiteSpace(filter.CommonSearch))
            {
                logs = _logRepository.GetSerachedRecords(log_filter, allowed_user_ids, allowed_company_ids, nav_page,
                                                         rows, sort_direction, sort_field, out searched_rows_count).
                    ToList();
            }
            else
            {
                logs = _logRepository.GetSearchedRecordsCommonSearch(log_filter, allowed_user_ids, restr_user_ids, allowed_company_ids,
                                                                     nav_page, rows, sort_direction, sort_field,
                                                                     out searched_rows_count,
                                                                     CurrentUser.Get().IsCompanyManager, CurrentUser.Get().IsSuperAdmin).ToList();
            }

            if (filter.ischeck)
            {

            }
            else
            { }
            if (searched_rows_count <= rows)
            {
                nav_page = 0;
            }
            Session["totalrowcount"] = searched_rows_count;
            IEnumerable<LogItem> list = new List<LogItem>();
            Mapper.Map(logs, list);

            lvm.Paginator = SetupPaginator(nav_page, rows, searched_rows_count, logs.Count);
            lvm.Paginator.RowsPerPageItems = new List<SelectListItem>
                            {
                                new SelectListItem()
                                    {Value = "50", Text = string.Format("{0} {1}", 50, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "100", Text = string.Format("{0} {1}", 100, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "200", Text = string.Format("{0} {1}", 200, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "1000", Text = string.Format("{0} {1}", 1000, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "5000", Text = string.Format("{0} {1}", 5000, ViewResources.SharedStrings.CommonPerPage)}
                            };
            lvm.Paginator.DivToRefresh = "AreaLogSearchResults";
            lvm.Paginator.Prefix = "Log";
            lvm.Items = list;
            if (rows == 1000 || rows > 1000)
            {
                System.Threading.Thread.Sleep(3000);

                Session["Loadingstatus"] = "y";

            }
            //

            return PartialView("List", lvm);
        }

        [HttpGet]
        public ActionResult LocationList(LogFilterItem filter, int? nav_page, int? rows, int? sort_field, int? sort_direction, int? reportId)
        {
            if (!CurrentUser.Get().IsSuperAdmin) { filter.CompanyId = CurrentUser.Get().CompanyId; }

            var FocSecModelcontexLoction = ConfigurationManager.ConnectionStrings["FoxSecDBEntities"].ConnectionString;

            ObjectContext Sql = new ObjectContext(FocSecModelcontexLoction);
            var lvm = CreateViewModel<LogListViewModel>();
            var logs = new List<Log>();
            //var loga = new List<Log>();
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
            string Query = "";
            if (filter.ischeck)
            {
                Query = "SELECT DISTINCT TOP (100) PERCENT Log_1.Id AS LogId FROM            (SELECT        UserId, MAX(EventTime) AS MaxEventTime                          FROM            dbo.[Log]                          WHERE        (LogTypeId = 30 OR                                                    LogTypeId = 31) AND (EventTime BETWEEN '01-09-2013 15:58:10' AND '01-10-2013 15:58:10')                          GROUP BY UserId) AS MaxLog INNER JOIN                         dbo.[Log] AS Log_1 ON MaxLog.UserId = Log_1.UserId AND MaxLog.MaxEventTime = Log_1.EventTime INNER JOIN                         dbo.FSSublocationObjects ON Log_1.BuildingObjectId = dbo.FSSublocationObjects.BuildingObjectId AND                          Log_1.LogTypeId = dbo.FSSublocationObjects.LogTypeId INNER JOIN                         dbo.FSSublocations ON dbo.FSSublocationObjects.SublocationId = dbo.FSSublocations.Id LEFT OUTER JOIN                         dbo.BuildingObjects ON dbo.FSSublocationObjects.BuildingObjectId = dbo.BuildingObjects.Id WHERE        (dbo.FSSublocations.Id = ReportsId) AND (dbo.FSSublocationObjects.IsDeleted = 0)";
            }
            else
            {
                Query = "SELECT DISTINCT TOP(100) PERCENT Log_1.Id AS LogId FROM(SELECT        UserId, MAX(EventTime) AS MaxEventTime   FROM            dbo.[Log]   WHERE(LogTypeId = 30 OR LogTypeId = 31) AND(EventTime BETWEEN '01-09-2013 15:58:10' AND '01-10-2013 15:58:10')  GROUP BY UserId) AS MaxLog INNER JOIN   dbo.[Log] AS Log_1 ON MaxLog.UserId = Log_1.UserId AND MaxLog.MaxEventTime = Log_1.EventTime INNER JOIN dbo.FSSublocationObjects ON Log_1.BuildingObjectId = dbo.FSSublocationObjects.BuildingObjectId AND Log_1.LogTypeId = dbo.FSSublocationObjects.LogTypeId INNER JOIN  dbo.FSSublocations ON dbo.FSSublocationObjects.SublocationId = dbo.FSSublocations.Id LEFT OUTER JOIN    dbo.BuildingObjects ON dbo.FSSublocationObjects.BuildingObjectId = dbo.BuildingObjects.Id   WHERE(dbo.FSSublocations.Id = ReportsId) AND(dbo.FSSublocationObjects.IsDeleted = 0)";
            }

            // creating sql query
            //string Query = "SELECT DISTINCT Log_1.Id AS LogId FROM (SELECT UserId, MAX(EventTime) AS MaxEventTime FROM dbo.[Log] WHERE (LogTypeId = 30 OR LogTypeId = 31) AND (EventTime BETWEEN '01-09-2013 15:58:10' AND '01-10-2013 15:58:10') GROUP BY UserId) AS MaxLog INNER JOIN dbo.[Log] AS Log_1 ON MaxLog.UserId = Log_1.UserId AND MaxLog.MaxEventTime = Log_1.EventTime INNER JOIN dbo.Users ON Log_1.UserId = dbo.Users.Id INNER JOIN dbo.FSSublocationObjects ON Log_1.BuildingObjectId = dbo.FSSublocationObjects.BuildingObjectId AND Log_1.LogTypeId = dbo.FSSublocationObjects.LogTypeId INNER JOIN dbo.FSSublocations ON dbo.FSSublocationObjects.SublocationId = dbo.FSSublocations.Id INNER JOIN dbo.LogTypes ON dbo.FSSublocationObjects.LogTypeId = dbo.LogTypes.Id LEFT OUTER JOIN dbo.BuildingObjects ON dbo.FSSublocationObjects.BuildingObjectId = dbo.BuildingObjects.Id WHERE (dbo.FSSublocations.Id = ReportsId AND dbo.FSSublocationObjects.IsDeleted = 0)";
            //string Query = "SELECT DISTINCT TOP (100) PERCENT Log_1.Id AS LogId FROM            (SELECT        UserId, MAX(EventTime) AS MaxEventTime                          FROM            dbo.[Log]                          WHERE        (LogTypeId = 30 OR                                                    LogTypeId = 31) AND (EventTime BETWEEN '01-09-2013 15:58:10' AND '01-10-2013 15:58:10')                          GROUP BY UserId) AS MaxLog INNER JOIN                         dbo.[Log] AS Log_1 ON MaxLog.UserId = Log_1.UserId AND MaxLog.MaxEventTime = Log_1.EventTime INNER JOIN                         dbo.FSSublocationObjects ON Log_1.BuildingObjectId = dbo.FSSublocationObjects.BuildingObjectId AND                          Log_1.LogTypeId = dbo.FSSublocationObjects.LogTypeId INNER JOIN                         dbo.FSSublocations ON dbo.FSSublocationObjects.SublocationId = dbo.FSSublocations.Id LEFT OUTER JOIN                         dbo.BuildingObjects ON dbo.FSSublocationObjects.BuildingObjectId = dbo.BuildingObjects.Id WHERE        (dbo.FSSublocations.Id = ReportsId) AND (dbo.FSSublocationObjects.IsDeleted = 0)";
            //date filter

            string todate = String.Format("{0:MM-dd-yyyy HH:mm:ss}", log_filter.ToDate);
            string fromdate = String.Format("{0:MM-dd-yyyy HH:mm:ss}", log_filter.FromDate);
            Query = Query.Replace("01-09-2013 15:58:10", fromdate);
            Query = Query.Replace("01-10-2013 15:58:10", todate);

            Query = Query.Replace("ReportsId", reportId.Value.ToString());

            /*
             if(filter.CompanyId != null)
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
                        if (qwe.CompanyId == CompId && qwe.User.Active && (!qwe.User.UsersAccessUnits.Any() || !qwe.User.UsersAccessUnits.Where(x => !x.IsDeleted && x.Active).Any() || qwe.User.UsersAccessUnits.Where(x => x.ValidTo > DateTime.Now && x.Active && !x.IsDeleted).Any()) && (!qwe.User.UserRoles.Any() || !qwe.User.UserRoles.Where(x => !x.IsDeleted).Any() || qwe.User.UserRoles.Where(x => x.ValidTo > DateTime.Now && !x.IsDeleted).Any()))
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
                        if (qwe.User.Active && (!qwe.User.UsersAccessUnits.Any() || !qwe.User.UsersAccessUnits.Where(x => !x.IsDeleted && x.Active).Any() || qwe.User.UsersAccessUnits.Where(x => x.ValidTo > DateTime.Now && x.Active && !x.IsDeleted).Any()) && (!qwe.User.UserRoles.Any() || !qwe.User.UserRoles.Where(x => !x.IsDeleted).Any() || qwe.User.UserRoles.Where(x => x.ValidTo > DateTime.Now && !x.IsDeleted).Any()))
                        {

                            logs.Add(qwe);
                        }
                    }
                }
            }
            rows = 5000;

            nav_page = 0;
            if (sort_field != null)
            {
                switch (sort_field)
                {

                    case 1:
                        logs = logs.OrderBy(s => s.EventTime).ToList();

                        break;

                    case 3:
                        logs = logs.OrderBy(s => s.Node).ToList();
                        break;
                    case 4:
                        logs = logs.OrderBy(s => s.User.CompanyId).ToList();
                        break;
                    case 5:
                        logs = logs.OrderBy(s => s.User.LastName).ToList();
                        break;

                }
                if (sort_direction == 1) { logs.Reverse(); }
            }
            IEnumerable<LogItem> list = new List<LogItem>();
            Mapper.Map(logs, list);

            lvm.Paginator = SetupPaginator(nav_page, rows, searched_rows_count, logs.Count);
            lvm.Paginator.RowsPerPageItems = new List<SelectListItem>
                            {
                                new SelectListItem()
                                    {Value = "50", Text = string.Format("{0} {1}", 50, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "100", Text = string.Format("{0} {1}", 100, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "200", Text = string.Format("{0} {1}", 200, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "1000", Text = string.Format("{0} {1}", 1000, ViewResources.SharedStrings.CommonPerPage)},
                                new SelectListItem()
                                    {Value = "5000", Text = string.Format("{0} {1}", 5000, ViewResources.SharedStrings.CommonPerPage)}
                            };
            lvm.Paginator.DivToRefresh = "AreaLogSearchResults";
            lvm.Paginator.Prefix = "Log";
            lvm.Items = list;






            return PartialView("List", lvm);
        }


        //private void RegisterLogRelatedMap()
        //{
        //    Mapper.CreateMap<Log, LogItem>()
        //        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? string.Format("{0} {1}", src.User.FirstName, src.User.LastName) : ""))
        //        .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : ""))
        //        .ForMember(dest => dest.EventTimeStr, opt => opt.MapFrom(src => src.EventTime.ToString("dd.MM.yyyy - HH:mm:ss")))
        //        .ForMember(dest => dest.Action, opt => opt.MapFrom(src => GetLogAction(src.Action)))
        //        .ForMember(dest => dest.IsUserDeleted, opt => opt.MapFrom(src => src.User == null ? false : src.User.IsDeleted))
        //        .ForMember(dest => dest.IsCompanyDeleted, opt => opt.MapFrom(src => src.Company == null ? false : src.Company.IsDeleted))
        //        .ForMember(dest => dest.LogRecordColor, opt => opt.MapFrom(src => string.Format("#{0}", src.LogType.Color)));
        //}

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

        [HttpGet]
        public JsonResult GetFilterData(int filterId)
        {
            var filter = _logFilterRepository.FindById(filterId);

            return Json(new
            {
                fromDate = filter.FromDate.HasValue ? string.Format("{0} {1}:{2}", filter.FromDate.Value.ToString("dd.MM.yyyy"), filter.FromDate.Value.Hour.ToString("D2"), filter.FromDate.Value.Minute.ToString("D2")) : "",
                toDate = filter.ToDate.HasValue ? string.Format("{0} {1}:{2}", filter.ToDate.Value.ToString("dd.MM.yyyy"), filter.ToDate.Value.Hour.ToString("D2"), filter.FromDate.Value.Minute.ToString("D2")) : "",
                building = filter.Building,
                node = filter.Node,
                companyId = filter.CompanyId.HasValue ? filter.CompanyId.Value.ToString() : "",
                userName = filter.UserName,
                name = filter.Name,
                activity = string.IsNullOrWhiteSpace(filter.Activity) ? "" : filter.Activity.TrimEnd(),
                id = filter.Id,
                showDefaultLog = filter.IsShowDefaultLog.HasValue ? filter.IsShowDefaultLog.Value : true
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EditCreateFilter(LogFilterItem filter)
        {
            string mesage = string.Empty;

            if (string.IsNullOrWhiteSpace(filter.Name))
            {
                ModelState.AddModelError("", "Filter name should be entered!");
                mesage = "Filter name should be entered!";
            }

            var id = -1;
            bool isCreate = false;
            DateTime? fromDate = null;

            if (!string.IsNullOrWhiteSpace(filter.FromDate))
            {
                try
                {
                    fromDate = DateTime.ParseExact(filter.FromDate, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    mesage = "From date should be in 'dd.MM.yyyy hh:mm' format ";
                    ModelState.AddModelError("", mesage);
                }
            }

            var host = Request.UserHostAddress;

            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(filter.ToDate))
            {
                try
                {
                    toDate = DateTime.ParseExact(filter.ToDate, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    mesage = "To date should be in 'dd.MM.yyyy hh:mm' format ";
                    ModelState.AddModelError("", mesage);
                }
            }
            if (ModelState.IsValid)
            {
                if (!filter.LogFilterId.HasValue)
                {
                    id = _logFilterService.CreateLogFilter(CurrentUser.Get().Id, filter.UserName, filter.Building, filter.Node, filter.Name,
                                                      filter.CompanyId, fromDate,
                                                      toDate, filter.Activity, host, filter.IsShowDefaultLog);
                    isCreate = true;

                }
                else
                {
                    id = _logFilterService.EditLogFilter(filter.LogFilterId.Value, filter.UserName, filter.Building, filter.Node, filter.Name,
                        filter.CompanyId, fromDate, toDate, filter.Activity, host, filter.IsShowDefaultLog);
                }

                if (id == -1)
                {
                    ModelState.AddModelError("", string.Format("Filter with name '{0}' already exists!", filter.Name));
                    mesage = string.Format("Filter with name '{0}' already exists!", filter.Name);
                }
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? "Save filter" : mesage,
                Id = id,
                IsCreate = isCreate,
                FilterName = filter.Name
            });
        }

        [HttpPost]
        public JsonResult DeleteFilter(int? id)
        {
            string mesage = string.Empty;

            var host = Request.UserHostAddress;
            if (!id.HasValue)
            {
                ModelState.AddModelError("", "Filter is not selected!");
                mesage = "Filter is not selected!";
            }

            if (ModelState.IsValid)
            {
                _logFilterService.DeleteLogFilter(id.Value, host);
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? "Filter deleted" : mesage
            });
        }

        public JsonResult GetFilters()
        {
            StringBuilder result = new StringBuilder();
            result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");

            var filters = _logFilterRepository.FindAll().Where(ff => !ff.IsDeleted).OrderBy(ff => ff.Name).ToList();

            foreach (var ff in filters)
            {
                result.Append("<option value=" + '"' + ff.Id + '"' + ">" + ff.Name + "</option>");
            }

            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByNameAutoComplete(string term)
        {
            var ids = GetAllowedUserIds(term);

            List<string> result = string.IsNullOrWhiteSpace(term) ? new List<string>() : (from user in _userRepository.FindAll(x => ids.Contains(x.Id))
                                                                                          select string.Format("{0} {1}", user.FirstName, user.LastName)).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetDetail(string BuildingName, string BuildingObject)
        {
            FoxSecDBContext db = new FoxSecDBContext();
            // string BuildingName1 = BuildingName.Trim();
            //string BuildingObject1 = BuildingObject.Trim();

            //string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
            //SqlConnection myConnection = new SqlConnection(connectionString);
            //myConnection.Open();
            ////string BuildingName = "Pärnu mnt. 102";
            //int buildingid = 0;
            //int BuildingObjectid = 0;
            //int Cameraid = 0;
            //// Linq query
            //string getBuildingID = @" select ISNULL(ID,0) from Buildings where Name={0} and IsDeleted={1} ";
            //var ResultBuilding = db.Database.SqlQuery<box10>(getBuildingID, BuildingName.Trim(),false).FirstOrDefault();

            //buildingid = ResultBuilding.ID;
            ////SqlCommand myCommand = new SqlCommand("select id from Buildings where Name='" + BuildingName1 + "'", myConnection);
            ////SqlDataReader dr = myCommand.ExecuteReader();
            ////while(dr.Read())
            ////{
            ////   buildingid=int.Parse( dr[0].ToString());

            ////}


            ////myConnection.Close();
            ////myConnection.Open();
            //string getBuildingObjectID = @" select ISNULL(ID,0) from BuildingObjects where BuildingId={0} and Description={1} and IsDeleted={2} ";
            //var ResultBuildingObject = db.Database.SqlQuery<box10>(getBuildingObjectID, buildingid, BuildingObject1.Trim(),false).FirstOrDefault();

            //BuildingObjectid = ResultBuildingObject.ID;

            //SqlCommand myCommand1 = new SqlCommand("select id from BuildingObjects where BuildingId='" + buildingid + "'And Description='" + BuildingObject1 + "'" , myConnection);
            //SqlDataReader dr1 = myCommand1.ExecuteReader();
            //while (dr1.Read())
            //{
            //    BuildingObjectid = int.Parse(dr1[0].ToString());

            //}

            //myConnection.Close();


            //myConnection.Open();
            //string getBuildingObjectCamera = @" select ISNULL(CameraId,0) from FSBuildingObjectCameras where BuildingObjectId={0} and Description={1} and IsDeleted={2} ";
            //var ResultBuildingCamera = db.Database.SqlQuery<box10>(getBuildingObjectCamera, BuildingObjectid, BuildingObject1.Trim(), false).FirstOrDefault();

            // Cameraid = ResultBuildingCamera.CameraId;



            if (BuildingName.Trim() == "web" && BuildingObject.Trim() == "::1")
            {
                return Json("Records Not Matched", JsonRequestBehavior.AllowGet);

            }


            int buildingid = 0;
            int BuildingObjectid = 0;
            int Cameraid = 0;

            string getBuildingID = @" select ISNULL(ID,0) as ID from Buildings where Name={0} and IsDeleted={1}  ";
            var ResultBuilding = db.Database.SqlQuery<box10>(getBuildingID, BuildingName.Trim(), 0).FirstOrDefault();

            buildingid = Convert.ToInt32(ResultBuilding.ID);



            string getBuildingObjectID = @" select ISNULL(ID,0) as ID from BuildingObjects where BuildingId={0} and Description={1} and IsDeleted={2} ";
            var ResultBuildingObject = db.Database.SqlQuery<box10>(getBuildingObjectID, buildingid, BuildingObject.Trim(), "False").FirstOrDefault();
            if (ResultBuildingObject == null)
            {
                return Json("Records Not Matched", JsonRequestBehavior.AllowGet);
            }
            BuildingObjectid = Convert.ToInt32(ResultBuildingObject.ID);



            string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            SqlCommand myCommand2 = new SqlCommand("  select  ISNULL(CameraId,0) as CameraId   from FSBuildingObjectCameras where BuildingObjectId='" + BuildingObjectid + "'and IsDeleted='" + 0 + "'", myConnection);
            SqlDataReader dr2 = myCommand2.ExecuteReader();

            List<string> value = new List<string>();
            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    Cameraid = int.Parse(dr2[0].ToString());
                    value.Add(Cameraid.ToString());

                    SqlConnection c1 = new SqlConnection(connectionString);
                    c1.Open();


                    int servernr = 0;
                    string port = "";
                    string starttime = "";
                    string playtime = "";
                    string height = "";
                    string width = "";
                    string cameranr = "";
                    SqlCommand cmd1 = new SqlCommand("select Port, ServerNr,QuickPreviewSeconds,Delay,ResX,ResY,CameraNr from FSCameras where Id='" + Cameraid + "'", c1);
                    SqlDataReader dr3 = cmd1.ExecuteReader();
                    while (dr3.Read())
                    {
                        port = dr3[0].ToString();
                        servernr = int.Parse(dr3[1].ToString());
                        starttime = dr3[2].ToString();
                        playtime = dr3[3].ToString();
                        height = dr3["ResX"].ToString();
                        width = dr3["ResY"].ToString();
                        cameranr = dr3["CameraNr"].ToString();

                    }

                    value.Add(port);
                    value.Add(starttime);
                    value.Add(playtime);
                    value.Add(height);
                    value.Add(width);
                    value.Add(cameranr);
                    c1.Close();

                    SqlConnection conn2 = new SqlConnection(connectionString);
                    string IP = "";
                    string ServerName = "";
                    string Uname = "";
                    string Password = "";
                    conn2.Open();
                    SqlCommand cmd2 = new SqlCommand("select IP,Name,UID,PWD from FSVideoServers where Id='" + servernr + "'", conn2);
                    SqlDataReader dr5 = cmd2.ExecuteReader();
                    while (dr5.Read())
                    {
                        IP = dr5[0].ToString();
                        ServerName = dr5[1].ToString();
                        Uname = dr5[2].ToString();
                        Password = dr5[3].ToString();
                    }
                    value.Add(IP);
                    value.Add(ServerName);
                    value.Add(Uname);
                    value.Add(Password);
                    conn2.Close();







                }
            }



            myConnection.Close();


            return Json(value, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Getphoto(string username, string company)
        {
            FoxSecDBContext db = new FoxSecDBContext();
            string name = username.Trim();
            string cc = company.Trim();
            if (name == string.Empty && cc == string.Empty)
            {
                return Json("Image Not Found Along This User", JsonRequestBehavior.AllowGet);
            }
            var names = name.Split(' ');
            string firstName = names[0];
            string lastName = names[1];

            string fullname = firstName + "." + lastName;

            string companyname = company.Trim();
            string base64String = "";


            try
            {
                var GetImage = db.User.Where(x => x.LoginName == fullname).Select(x => new { x.Image }).FirstOrDefault();
                string qcont = @"select count(Image) as count from users where LoginName={0}";

                var countImage = db.Database.SqlQuery<box10>(qcont, fullname).FirstOrDefault();



                int count = countImage.count;
                if (count <= 0)
                {


                    return Json("Image Not Found Along This User", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    byte[] imageBytes = GetImage.Image;
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                    ms.Write(imageBytes, 0, imageBytes.Length);
                    Image image = System.Drawing.Image.FromStream(ms, true);
                    byte[] imageBytes1 = ms.ToArray();

                    base64String = Convert.ToBase64String(imageBytes1);

                }

            }
            catch
            {

            }

            return Json(base64String, JsonRequestBehavior.AllowGet);





        }

    }

}