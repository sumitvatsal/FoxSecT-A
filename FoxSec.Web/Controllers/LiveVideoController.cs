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
//using System.Linq;
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
using static FoxSec.DomainModel.DomainObjects.VideoAccess;
using System.Data;
using FoxSec.Web.ListModel;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;

namespace FoxSec.Web.Controllers
{
    public class LiveVideoController : PaginatorControllerBase<item>
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
        private readonly IVideoAccessRepository _IVideoAccessRepository;
        private readonly ILogService _logservice;
        //private readonly IVideoCameraService _videocameraService;

        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);

        private ResourceManager _resourceManager;
        public LiveVideoController(IUserService userService,
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
                                IVideoAccessRepository VideoAccessRepository,
                                  IClassificatorValueRepository classificatorValueRepository,
                                  ILogService logservice,
                                  //IVideoCameraService videocameraService,
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
            _IVideoAccessRepository = VideoAccessRepository;
            _userBuildingService = userBuildingService;
            _logservice = logservice;
            //_videocameraService = videocameraService;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
        }

        FoxSecDBContext db = new FoxSecDBContext();
        public int uid;
        List<string> mylist = new List<string>();

        #region Tree
        public ActionResult LiveCamera()
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
            //offices is buildings
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
            List<Node> Cameras = new List<Node>();
            string query = @"select FSC.Name as CameraName , FSC.Id,cmpbo.CompanyId from CompanyBuildingObjects as cmpbo inner join FSBuildingObjectCameras fsboc on cmpbo.BuildingObjectId= fsboc.BuildingObjectId inner join FSCameras FSC ON FSBOC.CameraId= FSC.Id ";
            var Camera = db.Database.SqlQuery<CameraDetals>(query);
            //ViewBag.CDtails= db.Database.SqlQuery<CameraDetals>(query).All(x => companyIds.Contains(Convert.ToInt32(x.CompanyId)));
            foreach (var items1 in Camera)
            {
                Cameras.Add(new Node() { ParentId = items1.CompanyId, MyId = items1.Id, Name = items1.CameraName });
            }
            ctvm.Cameras = Cameras;
            //ctvm.Cameras = Cameras;
            //ctvm.Cameras =
            //               new Node
            //               {
            //                   ParentId = Camera.,
            //                   MyId = Camera.Id,
            //                   Name = Camera.CameraName
            //               };
            //ctvm.Cameras =
            //          from Camera in
            //              _IVideoAccessRepository.FindAll(x => companyIds.Contains(Convert.ToInt32(x.CompanyId)))
            //          select
            //              new Node
            //              {
            //                  ParentId = Camera.CompanyId,
            //                  MyId = Camera.Id,
            //                  Name = Camera.CameraName
            //              };
            return PartialView("LiveCamera", ctvm);
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
            return PartialView("Tree", ctvm);
        }


        [HttpPost]
        public ActionResult Unassigncamera(int[] uniqueNames, int companyid, string[] uniqueNames1)
        {
            string cameraname = "";
            for (int i = 0; i < uniqueNames.Length; i++)
            {
                int id = uniqueNames[i];

                for (int j = i; j < uniqueNames1.Length;)
                {
                    cameraname = uniqueNames1[i];
                    break;
                }
                string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection1 = new SqlConnection(connectionString1);
                myConnection1.Open();
                SqlCommand myCommand1 = new SqlCommand("select * from CameraPermissions where CameraID ='" + id + "'and CompanyID='" + companyid + "'and Access='" + true + "'", myConnection1);
                SqlDataReader dr = myCommand1.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        uid = Convert.ToInt32(dr["id"]);
                        cameraname = dr[2].ToString();
                    }
                    myConnection1.Close();
                    SqlConnection myConnection2 = new SqlConnection(connectionString1);
                    myConnection2.Open();
                    try
                    {
                        SqlCommand myCommand2 = new SqlCommand("update CameraPermissions set Access ='" + false + "' where id='" + uid + "'", myConnection2);
                        myCommand2.ExecuteNonQuery();
                        myConnection2.Close();
                    }
                    catch (Exception ee)
                    {
                        throw ee;
                    }
                }
                else
                {
                    mylist.Add(cameraname);
                }
            }
            return Json(mylist);
        }

        [HttpPost]
        public ActionResult Deactivate(int[] uniqueNames, int companyid, string[] uniqueNames1)
        {
            string camera = "";
            for (int i = 0; i < uniqueNames.Length; i++)
            {
                int id = uniqueNames[i];
                string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection1 = new SqlConnection(connectionString1);
                myConnection1.Open();
                //new SqlCommand("select * from CameraPermissions where CameraID ='" + id + "'and CompanyID='" + companyid + "'and Access='" + true + "'", myConnection1);
                SqlCommand myCommand1 = new SqlCommand("select * from CameraPermissions where CameraID ='" + id + "'and CompanyID='" + companyid + "'and Access='" + false + "'", myConnection1);
                SqlDataReader dr = myCommand1.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        uid = Convert.ToInt32(dr["id"]);
                        camera = dr[2].ToString();
                        try
                        {
                            SqlConnection con = new SqlConnection(connectionString1);
                            con.Open();
                            SqlCommand cmd2 = new SqlCommand("update CameraPermissions set Access ='" + true + "' where id='" + uid + "'", con);
                            int k = cmd2.ExecuteNonQuery();
                            con.Close();
                        }
                        catch (Exception ee)
                        {
                            throw ee;
                        }
                    }
                }
                else
                {
                    SqlConnection conn = new SqlConnection(connectionString1);
                    conn.Open();
                    SqlCommand cm = new SqlCommand("select * from CameraPermissions where CameraID ='" + id + "'and CompanyID='" + companyid + "'and Access='" + true + "'", conn);
                    SqlDataReader data = cm.ExecuteReader();
                    if (data.HasRows)
                    {
                        try
                        {
                            while (data.Read())
                            {
                                camera = data[2].ToString();
                                mylist.Add(camera);
                            }
                        }
                        catch (Exception ee)
                        {
                            throw ee;
                        }
                    }
                    else
                    {
                        uid = 8;
                        camera = "";
                    }
                }

                myConnection1.Close();
                if (uid == 8)
                {
                    for (int j = i; j < uniqueNames1.Length;)
                    {
                        string name = uniqueNames1[i];
                        string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                        SqlConnection myConnection = new SqlConnection(connectionString);
                        myConnection.Open();
                        SqlCommand myCommand = new SqlCommand("insert  into  CameraPermissions(cameraID,CompanyID,CameraName,Access) values('" + id + "', '" + companyid + "', '" + name + "', '" + true + "')", myConnection);
                        myCommand.ExecuteNonQuery();
                        myConnection.Close();
                        break;
                    }
                }
            }
            return Json(mylist);
        }


        [HttpGet]
        public ActionResult allcamera(int CompnyId)
        {
            Session["allcamera"] = "All";
            try
            {
                Session["companyid"] = CompnyId;
                var grid = CreateViewModel<LiveVideoListViewModel>();
                List<FSCameras> FSCameras = UsersByCamera();
                IEnumerable<item> list = new List<item>();
                Mapper.Map(FSCameras, list);
                // Mapper.Map(VideoAccess, new List<UserItems>());
                grid.Paginator = SetupPaginator(ref list, 0, 10);
                grid.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
                grid.Paginator.Prefix = "LiveVideo";
                grid.Users = list;
                grid.FilterCriteria = 1;
                return PartialView("List", grid);
            }
            catch (Exception ee)
            {
                throw (ee);
            }
        }
        private IEnumerable<FSCameras> ApplyUserStatusFilter(IEnumerable<FSCameras> filt_camras, int filter)
        {
            if (filter == 1)
            {
                filt_camras = filt_camras.Where(x => !x.Deleted).ToList();
            }
            else if (filter == 0)
            {
                filt_camras = filt_camras.Where(x => !x.Deleted).ToList();
            }

            return filt_camras;
        }
        public ActionResult Search(string name, string comment, string cardSer, string cardDk, string cardCode, string company, string title, int filter, int departmentId, int? nav_page, int? rows, int? sort_field, int? sort_direction,
            int countryId, int locationId, int buildingId, int companyId, int floorId, string status)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            FoxSecDBContext db = new FoxSecDBContext();
            List<FSCameras> FSCameras = UsersByCamera(companyId, status);
            IEnumerable<item> camaras = new List<item>();
            Mapper.Map(FSCameras, camaras);
            IEnumerable<item> filt_camras = new List<item>();
            var gridshort = CreateViewModel<LiveVideoListViewModel>();
            //var uvm = CreateViewModel<LiveVideoListViewModel>();
            //IEnumerable<item> Camralist = new List<item>();
            if (comment != "")
            {
                // filt_camras = SearchByComment(comment);
                /*
                Mapper.Map(users, filt_users);
                uvm.Paginator = SetupPaginator(ref filt_users, nav_page, rows);
                uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
                uvm.Paginator.Prefix = "User";

                uvm.Users = filt_users;
                uvm.FilterCriteria = filter;*/
                gridshort.Comment = true;
                //return PartialView("List", uvm);
            }
            else
            {
                var Live_camera = db.FSCameras;
                // var user_priority = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
                //users = _userRepository.FindAll(x => !x.IsDeleted && user_priority <= x.RolePriority()).ToList();
                //camaras = Live_camera.ToList();           
            }
            //camaras = ApplyUserStatusFilter(filt_camras, filter).ToList();
            FSCameras = ApplyUserStatusFilter(FSCameras, filter).ToList();
            Mapper.Map(FSCameras, filt_camras);
            if (sort_field.HasValue && sort_direction.HasValue)
            {
                if (sort_direction.Value == 0)
                {
                    filt_camras = filt_camras.OrderBy(x => x.Name).ToList();
                }
                else filt_camras = filt_camras.OrderByDescending(x => x.Name).ToList();

            }
            //  Mapper.Map(FSCameras, filt_camras);
            gridshort.Paginator = SetupPaginator(ref filt_camras, nav_page, rows);
            gridshort.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            gridshort.Paginator.Prefix = "LiveVideo";
            // gridshort.User = camaras;
            gridshort.Users = filt_camras;
            gridshort.FilterCriteria = filter;
            return PartialView("List", gridshort);
        }
        private List<FSCameras> UsersByCamera()
        {
            int cmp_id = Convert.ToInt32(Session["companyid"]);
            int roleid = Convert.ToInt32(Session["roll_id"]);
            string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
            SqlConnection myConnection1 = new SqlConnection(connectionString1);
            myConnection1.Open();
            SqlCommand myCommand1;
            if (roleid == 0)
            {
                roleid = 5;
            }
            if (roleid == 5)
            {
                myCommand1 = new SqlCommand("select Id,Name from FSCameras where Deleted=0", myConnection1);
            }
            else
            {
                myCommand1 = new SqlCommand("select CameraID as Id, (select name from FSCameras where id=cp.CameraID) as Name from CameraPermissions cp where CompanyID=@CompanyID and Access=1", myConnection1);
                myCommand1.Parameters.AddWithValue("@CompanyID", cmp_id);
            }
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


        [HttpGet]
        public ActionResult ByCompany(int cameraId, int companyId, string status)
        {
            Session["companyId"] = companyId;
            Session["allcamera"] = "test";
            string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand("select Name,Id From Companies where Id='" + companyId + "'", myConnection);
            SqlDataReader dr = myCommand.ExecuteReader();
            while (dr.Read())
            {
                Session["companyName"] = dr[0].ToString();
                Session["comp_id"] = companyId;
            }
            try
            {
                var grid = CreateViewModel<LiveVideoListViewModel>();
                List<FSCameras> FSCameras = UsersByCamera(companyId, status);
                IEnumerable<item> list = new List<item>();
                Mapper.Map(FSCameras, list);
                // Mapper.Map(VideoAccess, new List<UserItems>());
                grid.Paginator = SetupPaginator(ref list, 0, 10);
                grid.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
                grid.Paginator.Prefix = "LiveVideo";
                grid.Users = list;
                grid.FilterCriteria = 1;
                return PartialView("List", grid);
            }
            catch (Exception ee)
            {
                throw (ee);
            }
        }

        [HttpGet]
        public ActionResult SearchCamera(int cameraId)
        {
            string query_camera = @"select FSC.Name as CameraName , FSC.Id,cmpbo.CompanyId from CompanyBuildingObjects as cmpbo inner join FSBuildingObjectCameras fsboc on cmpbo.BuildingObjectId= fsboc.BuildingObjectId inner join FSCameras FSC ON FSBOC.CameraId= FSC.Id where where Fsc.Id={0} ";
            var result = db.Database.SqlQuery<CameraDetals>(query_camera, cameraId).SingleOrDefault();
            //var result = db.CameraIPs.Where(x => x.CameraId == cameraId).SingleOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<FSCameras> UsersByCamera(int companyId, string status)
        {
            if (status == "Unassigned")
            {
                string connectionString2 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection2 = new SqlConnection(connectionString2);
                myConnection2.Open();
                string query2 = @"select DISTINCT fc.Id as camid, Name,case when  Access = 1 then  'UnAssigned'  when Access=0 then'UnAssigned' else 'UnAssigned' end  Status  from FSCameras fc   left outer join CameraPermissions cp on cp.CameraID = fc.Id     where   fc.Deleted=0 and fc.Id not in(select fc.Id from FSCameras fc   left outer join CameraPermissions cp on cp.CameraID = fc.Id  where cp.CompanyID = " + companyId + " and cp.Access=1 )";

                //   string query2 = @"SELECT FSCameras.Id, FSCameras.Name, CameraPermissions.Access FROM CameraPermissions INNER JOIN FSCameras ON CameraPermissions.Id = FSCameras.Id WHERE(CameraPermissions.Access = 0)and CameraPermissions.CompanyID = " + companyId + "";
                SqlCommand myCommand2 = new SqlCommand(query2, myConnection2);
                SqlDataAdapter adap2 = new SqlDataAdapter(myCommand2);
                DataTable dt2 = new DataTable();
                adap2.Fill(dt2);
                List<FSCameras> list2 = new List<FSCameras>();
                foreach (DataRow dr in dt2.Rows)
                {
                    FSCameras usr = new FSCameras();
                    usr.Id = Convert.ToInt32(dr["camid"]);
                    usr.Name = dr["Name"].ToString();
                    usr.status = dr["Status"].ToString();
                    list2.Add(usr);
                }
                List<FSCameras> FSCameras2 = list2.ToList();
                return FSCameras2;
                //List<FSCameras> list2 = new List<FSCameras>();
                //string unassigneddatas = @"select CameraName as Name,CameraID as Id,case when Access=1 then 'Assigned' else 'UnAssigned' end status  from  CameraPermissions   where CompanyID={0} and  Access={1}  ";
                //var data1 = db.Database.SqlQuery<FSCameras>(unassigneddatas, companyId, 0).ToList();
                //foreach (var itemsx in data1)
                //{

                //    list2.Add(new FSCameras() { Id = itemsx.Id, Name = itemsx.Name, status = itemsx.status });
                //}
                //return list2;
            }
            else
            {
                string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection1 = new SqlConnection(connectionString1);
                myConnection1.Open();
                string query1 = @"select CameraID, Name,case when  Access=1 then 'Assigned' else 'UnAssigned' end Status  from FSCameras fc 
                                                    left outer join CameraPermissions cp on cp.CameraID = fc.Id
                                                    where cp.CompanyID = " + companyId + " and cp.Access=1 and fc.Deleted=0";
                SqlCommand myCommand1 = new SqlCommand(query1, myConnection1);

                SqlDataAdapter adap1 = new SqlDataAdapter(myCommand1);
                DataTable dt1 = new DataTable();
                adap1.Fill(dt1);

                List<FSCameras> list1 = new List<FSCameras>();
                foreach (DataRow dr in dt1.Rows)
                {
                    FSCameras usr = new FSCameras();
                    usr.Id = Convert.ToInt32(dr["CameraID"]);
                    usr.Name = dr["Name"].ToString();
                    usr.status = dr["Status"].ToString();
                    list1.Add(usr);
                }
                List<FSCameras> FSCameras1 = list1.ToList();
                return FSCameras1;
            }
        }

        //private List<FSCameras> UsersByCamera(int companyId, string status)
        //{
        //    if (status == "Assigned")
        //    {
        //        List<FSCameras> list1 = new List<FSCameras>();
        //        string camaradatas = @"select CameraName as Name,CameraID as Id,case when Access=1 then 'Assigned' else 'UnAssigned' end status  from  CameraPermissions   where CompanyID={0} and  Access={1}  ";
        //        var data1 = db.Database.SqlQuery<FSCameras>(camaradatas, companyId, 1).ToList();
        //        foreach (var itemsx in data1)
        //        {
        //            list1.Add(new FSCameras() { Id = itemsx.Id, Name = itemsx.Name, status = itemsx.status });
        //        }
        //        //string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
        //        //SqlConnection myConnection1 = new SqlConnection(connectionString1);
        //        //myConnection1.Open();

        //        //string query1 = @"select CameraID, Name,case when  Access=1 then 'Assigned' else 'UnAssigned' end Status  from FSCameras fc 
        //        //                                    left outer join CameraPermissions cp on cp.CameraID = fc.Id
        //        //                                    where cp.CompanyID = " + companyId + " and cp.Access=1";

        //        //SqlCommand myCommand1 = new SqlCommand(query1, myConnection1);

        //        //SqlDataAdapter adap1 = new SqlDataAdapter(myCommand1);
        //        //DataTable dt1 = new DataTable();
        //        //adap1.Fill(dt1);
        //        //List<FSCameras> list1 = new List<FSCameras>();
        //        //foreach (DataRow dr in dt1.Rows)
        //        //{
        //        //    FSCameras usr = new FSCameras();
        //        //    usr.Id = Convert.ToInt32(dr["CameraID"]);
        //        //    usr.Name = dr["Name"].ToString();
        //        //    usr.status = dr["Status"].ToString();
        //        //    list1.Add(usr);
        //        //}
        //        //List<FSCameras> FSCameras1 = list1.ToList();
        //        return list1;
        //    }
        //    else if (status == "Unassigned")
        //    {
        //        string connectionString2 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
        //        SqlConnection myConnection2 = new SqlConnection(connectionString2);
        //        myConnection2.Open();
        //        string query2 = @"select DISTINCT fc.Id as camid, Name,case when  Access = 1 then  'UnAssigned'  when Access=0 then'UnAssigned' else 'UnAssigned' end  Status  from FSCameras fc   left outer join CameraPermissions cp on cp.CameraID = fc.Id     where fc.Id not in(select fc.Id from FSCameras fc   left outer join CameraPermissions cp on cp.CameraID = fc.Id  where cp.CompanyID = " + companyId + " and cp.Access=1 )";

        //        //   string query2 = @"SELECT FSCameras.Id, FSCameras.Name, CameraPermissions.Access FROM CameraPermissions INNER JOIN FSCameras ON CameraPermissions.Id = FSCameras.Id WHERE(CameraPermissions.Access = 0)and CameraPermissions.CompanyID = " + companyId + "";
        //        SqlCommand myCommand2 = new SqlCommand(query2, myConnection2);
        //        SqlDataAdapter adap2 = new SqlDataAdapter(myCommand2);
        //        DataTable dt2 = new DataTable();
        //        adap2.Fill(dt2);
        //        List<FSCameras> list2 = new List<FSCameras>();
        //        foreach (DataRow dr in dt2.Rows)
        //        {
        //            FSCameras usr = new FSCameras();
        //            usr.Id = Convert.ToInt32(dr["camid"]);
        //            usr.Name = dr["Name"].ToString();
        //            usr.status = dr["Status"].ToString();
        //            list2.Add(usr);
        //        }
        //        List<FSCameras> FSCameras2 = list2.ToList();
        //        return FSCameras2;
        //        //List<FSCameras> list2 = new List<FSCameras>();
        //        //string unassigneddatas = @"select CameraName as Name,CameraID as Id,case when Access=1 then 'Assigned' else 'UnAssigned' end status  from  CameraPermissions   where CompanyID={0} and  Access={1}  ";
        //        //var data1 = db.Database.SqlQuery<FSCameras>(unassigneddatas, companyId, 0).ToList();
        //        //foreach (var itemsx in data1)
        //        //{

        //        //    list2.Add(new FSCameras() { Id = itemsx.Id, Name = itemsx.Name, status = itemsx.status });
        //        //}
        //        //return list2;
        //    }
        //    else
        //    {
        //        string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
        //        SqlConnection myConnection1 = new SqlConnection(connectionString1);
        //        myConnection1.Open();
        //        string query1 = @"select CameraID, Name,case when  Access=1 then 'Assigned' else 'UnAssigned' end Status  from FSCameras fc 
        //                                            left outer join CameraPermissions cp on cp.CameraID = fc.Id
        //                                            where cp.CompanyID = " + companyId + " and cp.Access=1";
        //        SqlCommand myCommand1 = new SqlCommand(query1, myConnection1);

        //        SqlDataAdapter adap1 = new SqlDataAdapter(myCommand1);
        //        DataTable dt1 = new DataTable();
        //        adap1.Fill(dt1);

        //        List<FSCameras> list1 = new List<FSCameras>();
        //        foreach (DataRow dr in dt1.Rows)
        //        {
        //            FSCameras usr = new FSCameras();
        //            usr.Id = Convert.ToInt32(dr["CameraID"]);
        //            usr.Name = dr["Name"].ToString();
        //            usr.status = dr["Status"].ToString();
        //            list1.Add(usr);
        //        }
        //        List<FSCameras> FSCameras1 = list1.ToList();
        //        return FSCameras1;
        //    }
        //}

        private List<CamPermission> UsersByCamPermission()
        {
            List<CamPermission> CamPermission = db.CamPermissions.Where(x => x.CameraName != null).ToList();
            return CamPermission;
        }

        [HttpPost]
        public JsonResult GetDetail(string CameraName)
        {
            string BuildingName1 = CameraName.Trim();
            // string BuildingObject1 = BuildingObject.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();

            int cameraid = 0;
            int servernr = 0;
            string port = "";
            string regwidth = "";
            string regheight = "";
            string cameranr = "";
            SqlCommand myCommand = new SqlCommand("select id,ServerNr,Port,ResX,ResY,CameraNr from FSCameras where Name='" + BuildingName1 + "'", myConnection);

            SqlDataReader dr = myCommand.ExecuteReader();
            while (dr.Read())
            {
                cameraid = int.Parse(dr[0].ToString());
                servernr = int.Parse(dr[1].ToString());
                port = dr[2].ToString();
                regheight = dr["ResX"].ToString();
                regwidth = dr["ResY"].ToString();
                cameranr = dr["CameraNr"].ToString();
            }
            string IP = "";
            string ServerName = "";
            string Uname = "";
            string Password = "";
            myConnection.Close();

            SqlConnection myConnection1 = new SqlConnection(connectionString);
            myConnection1.Open();
            SqlCommand myCommand1 = new SqlCommand("select IP,Name,UID,PWD from FSVideoServers where Id='" + servernr + "'", myConnection1);
            SqlDataReader dr1 = myCommand1.ExecuteReader();
            while (dr1.Read())
            {
                IP = dr1[0].ToString();
                ServerName = dr1["Name"].ToString();
                Uname = dr1[2].ToString();
                Password = dr1[3].ToString();
            }

            List<string> cameradetail = new List<string>();
            cameradetail.Add(cameraid.ToString());
            cameradetail.Add(IP);
            cameradetail.Add(port);
            cameradetail.Add(regheight);
            cameradetail.Add(regwidth);
            cameradetail.Add(cameranr);
            cameradetail.Add(ServerName);
            cameradetail.Add(Uname);
            cameradetail.Add(Password);
            return Json(cameradetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult bingAssignCameras(string companyId)
        {
            try
            {
                List<assignCamera> list = new List<assignCamera>();
                string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection = new SqlConnection(connectionString);
                myConnection.Open();
                string query = @"select Name,case when  Access=1 then 'Assigned' else 'UnAssigned' end Status  from FSCameras fc 
                                                    left outer join CameraPermissions cp on cp.CameraID = fc.Id
                                                    where cp.CompanyID = " + companyId + @"
                                                    union all
                    select  Name,case when  Access = 1 then 'Assigned' else 'UnAssigned' end Status  from FSCameras fc   left outer join CameraPermissions cp on cp.CameraID = fc.Id     where fc.Id not in(select fc.Id from FSCameras fc   left outer join CameraPermissions cp on cp.CameraID = fc.Id    where cp.CompanyID = " + companyId + ")";
                SqlCommand myCommand = new SqlCommand(query, myConnection);

                SqlDataAdapter adap = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable();
                adap.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    assignCamera usr = new assignCamera();
                    usr.Name = dr["Name"].ToString();
                    usr.Status = dr["Status"].ToString();
                    list.Add(usr);
                }
                return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return PartialView("Edit");
        }
        [HttpGet]
        public JsonResult EditDetails(int id)
        {
            myConnection.Open();

            int cameraid = 0;
            string servernr = "";
            string CameraNr = "";
            string Name = "";
            string Port = "";
            string ResX = "";
            string ResY = "";
            string Skip = "";
            string Delay = "";
            string EnableLiveControls = "";
            string QuickPreviewSeconds = "";
            SqlCommand myCommand = new SqlCommand("select id,ServerNr,CameraNr,Name,Port,ResX,ResY,Skip,Delay,EnableLiveControls,QuickPreviewSeconds from FSCameras where id='" + id + "'", myConnection);

            SqlDataReader dr = myCommand.ExecuteReader();
            while (dr.Read())
            {
                cameraid = id;
                servernr = dr["ServerNr"].ToString();
                CameraNr = dr["CameraNr"].ToString();
                Name = dr["Name"].ToString();
                Port = dr["Port"].ToString();
                ResX = dr["ResX"].ToString();
                ResY = dr["ResY"].ToString();
                Skip = dr["Skip"].ToString();
                Delay = dr["Delay"].ToString();
                EnableLiveControls = dr["EnableLiveControls"].ToString();
                QuickPreviewSeconds = dr["QuickPreviewSeconds"].ToString();
            }

            myConnection.Close();

            List<string> cameradetail = new List<string>();
            cameradetail.Add(cameraid.ToString());
            cameradetail.Add(servernr);
            cameradetail.Add(CameraNr);
            cameradetail.Add(Name);
            cameradetail.Add(Port);
            cameradetail.Add(ResX);
            cameradetail.Add(ResY);
            cameradetail.Add(Skip);
            cameradetail.Add(Delay);
            cameradetail.Add(EnableLiveControls);
            cameradetail.Add(QuickPreviewSeconds);
            return Json(cameradetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult fetchFSVideoServers()
        {
            FSVideoServers tm = new FSVideoServers();

            var fsserver = db.FSVideoServers.Where(s => s.Deleted == false).OrderBy(c => c.Name).ToList();
            //            var terminal_list = db.Database.SqlQuery<Terminal>("Select * from Terminal where Id=" + id).SingleOrDefault();
            //          tm.term = terminal_list;
            //return Json(userss, JsonRequestBehavior.AllowGet); // this is old code

            return new JsonResult() // this is new code
            {
                Data = fsserver,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public JsonResult GetAllFSVideoServers()
        {
            FSVideoServers tm = new FSVideoServers();

            var fsserver = db.FSVideoServers.Where(s => s.Deleted == false).OrderBy(c => c.Name).ToList();
            //            var terminal_list = db.Database.SqlQuery<Terminal>("Select * from Terminal where Id=" + id).SingleOrDefault();
            //          tm.term = terminal_list;
            //return Json(userss, JsonRequestBehavior.AllowGet); // this is old code
            var fslist = new List<FSVideoServersDetails>();
            foreach (var obj in fsserver)
            {
                var objn = new FSVideoServersDetails();
                objn.Id = obj.Id;
                objn.ProjectId = obj.ProjectId;
                objn.Name = obj.Name;
                objn.IP = obj.IP;
                objn.UID = obj.UID;
                objn.PWD = obj.PWD;
                var fsproname = db.FSProjects.Where(s => s.Id == obj.ProjectId).FirstOrDefault().Name;
                objn.ProjectName = fsproname;
                fslist.Add(objn);
            }

            return new JsonResult() // this is new code
            {
                Data = fslist,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public JsonResult fetchFSProjects()
        {
            FSProjects tm = new FSProjects();

            var fsserver = db.FSProjects.Where(s => s.active == true).OrderBy(c => c.Name).ToList();
            //            var terminal_list = db.Database.SqlQuery<Terminal>("Select * from Terminal where Id=" + id).SingleOrDefault();
            //          tm.term = terminal_list;
            //return Json(userss, JsonRequestBehavior.AllowGet); // this is old code

            return new JsonResult() // this is new code
            {
                Data = fsserver,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        //[HttpPost]
        public string SaveUpdateCameraDetails(string Name, string ServerNr, string CameraNr, string Port, string ResX, string ResY, string Skip, string Delay, string QuickPreviewSeconds, string EnableLiveControls, int? Id, int? type)
        {
            string msg = "";
            try
            {
                myConnection.Open();
                IFoxSecIdentity identity = CurrentUser.Get();
                if (type == 2)
                {
                    int ocameraid = 0;
                    string oservernr = "";
                    string oCameraNr = "";
                    string oName = "";
                    string oPort = "";
                    string oResX = "";
                    string oResY = "";
                    string oSkip = "";
                    string oDelay = "";
                    string oEnableLiveControls = "";
                    string oQuickPreviewSeconds = "";

                    SqlCommand myCommand = new SqlCommand("select id,ServerNr,CameraNr,Name,Port,ResX,ResY,Skip,Delay,EnableLiveControls,QuickPreviewSeconds from FSCameras where id='" + Id + "'", myConnection);

                    SqlDataReader dr = myCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        ocameraid = Convert.ToInt32(Id);
                        oservernr = dr["ServerNr"].ToString();
                        oCameraNr = dr["CameraNr"].ToString();
                        oName = dr["Name"].ToString();
                        oPort = dr["Port"].ToString();
                        oResX = dr["ResX"].ToString();
                        oResY = dr["ResY"].ToString();
                        oSkip = dr["Skip"].ToString();
                        oDelay = dr["Delay"].ToString();
                        oEnableLiveControls = dr["EnableLiveControls"].ToString();
                        oQuickPreviewSeconds = dr["QuickPreviewSeconds"].ToString();
                    }


                    SqlCommand cmd = new SqlCommand("update FSCameras set ServerNr='" + ServerNr + "',CameraNr='" + CameraNr + "',Name='" + Name + "',Port='" + Port + "',ResX='" + ResX + "',ResY='" + ResY + "',Skip='" + Skip + "',Delay='" + Delay + "',EnableLiveControls='" + EnableLiveControls + "',QuickPreviewSeconds='" + QuickPreviewSeconds + "' where id='" + Id + "'", myConnection);
                    cmd.ExecuteNonQuery();


                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdated", new List<string> { Name, identity.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedNameChanged", new List<string> { oName, Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedServerNrChanged", new List<string> { oservernr, ServerNr }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedCameraNrChanged", new List<string> { oCameraNr, CameraNr }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedPortChanged", new List<string> { oPort, Port }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedResXChanged", new List<string> { oResX, ResX }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedResYChanged", new List<string> { oResY, ResY }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedSkipChanged", new List<string> { oSkip, Skip }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedDelayChanged", new List<string> { oDelay, Delay }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedEnableLiveControlsChanged", new List<string> { oEnableLiveControls, EnableLiveControls }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedQuickPreviewSecondsChanged", new List<string> { oQuickPreviewSeconds, QuickPreviewSeconds }));
                    int id = CurrentUser.Get().Id;
                    _logservice.CreateLog(CurrentUser.Get().Id, "web", "", HostName, CurrentUser.Get().CompanyId, message.ToString());
                }
                if (type == 1)
                {
                    SqlCommand cmd = new SqlCommand("insert into FSCameras(ServerNr,CameraNr,Name,Port,ResX,ResY,Skip,Delay,EnableLiveControls,QuickPreviewSeconds,Deleted) values('" + ServerNr + "','" + CameraNr + "','" + Name + "','" + Port + "','" + ResX + "','" + ResY + "','" + Skip + "','" + Delay + "','" + EnableLiveControls + "','" + QuickPreviewSeconds + "','0')", myConnection);
                    cmd.ExecuteNonQuery();

                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCameras", new List<string> { Name, identity.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasName", new List<string> { Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasServerNr", new List<string> { ServerNr }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasCameraNr", new List<string> { CameraNr }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasPort", new List<string> { Port }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasResX", new List<string> { ResX }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasResY", new List<string> { ResY }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasSkip", new List<string> { Skip }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasDelay", new List<string> { Delay }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasEnableLiveControls", new List<string> { EnableLiveControls }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSCamerasQuickPreviewSeconds", new List<string> { QuickPreviewSeconds }));
                    int id = CurrentUser.Get().Id;
                    _logservice.CreateLog(CurrentUser.Get().Id, "web", "", HostName, CurrentUser.Get().CompanyId, message.ToString());
                }
                myConnection.Close();
                //msg = _videocameraService.SaveUpdateCameraDetails(Name, ServerNr, CameraNr, Port, ResX,  ResY,  Skip,  Delay,  QuickPreviewSeconds,  EnableLiveControls, Id, type);
                msg = "1";
                return msg;
            }
            catch (Exception ex)
            {
                myConnection.Close();
                return ex.Message;
            }
        }

        public string SaveUpdateFSServerDetails(string Name, int? Project, string IP, string UID, string PWD, int? id, string flgtype)
        {
            string msg = "";
            try
            {
                myConnection.Open();
                IFoxSecIdentity identity = CurrentUser.Get();
                if (flgtype == "1")
                {
                    SqlCommand cmd = new SqlCommand("update FSVideoServers set ProjectId='" + Project + "',Name='" + Name + "',IP='" + IP + "',UID='" + UID + "',PWD='" + PWD + "' where id='" + id + "'", myConnection);
                    cmd.ExecuteNonQuery();

                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUpdatedFSVideoServers", new List<string> { Name, identity.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUpdatedFSVideoServersName", new List<string> { Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUpdatedFSVideoServersProjectId", new List<string> { Convert.ToString(Project) }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUpdatedFSVideoServersIP", new List<string> { IP }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUpdatedFSVideoServersUID", new List<string> { UID }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUpdatedFSVideoServersPWD", new List<string> { PWD }));

                    _logservice.CreateLog(CurrentUser.Get().Id, "web", "", HostName, CurrentUser.Get().CompanyId, message.ToString());
                }
                if (flgtype == "")
                {
                    SqlCommand cmd = new SqlCommand("insert into FSVideoServers(ProjectId,Name,IP,UID,PWD,Deleted) values('" + Project + "','" + Name + "','" + IP + "','" + UID + "','" + PWD + "','0')", myConnection);
                    cmd.ExecuteNonQuery();

                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSVideoServers", new List<string> { Name, identity.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSVideoServersName", new List<string> { Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSVideoServersProjectId", new List<string> { Convert.ToString(Project) }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSVideoServersIP", new List<string> { IP }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSVideoServersUID", new List<string> { UID }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewFSVideoServersPWD", new List<string> { PWD }));

                    _logservice.CreateLog(CurrentUser.Get().Id, "web", "", HostName, CurrentUser.Get().CompanyId, message.ToString());
                }
                myConnection.Close();
                //msg = _videocameraService.SaveUpdateCameraDetails(Name, ServerNr, CameraNr, Port, ResX,  ResY,  Skip,  Delay,  QuickPreviewSeconds,  EnableLiveControls, Id, type);
                msg = "1";
                return msg;
            }
            catch (Exception ex)
            {
                myConnection.Close();
                return ex.Message;
            }
        }

        public JsonResult GetSelFSVideoServers(int id)
        {
            FSVideoServers tm = new FSVideoServers();
            var serdet = db.FSVideoServers.Where(s => s.Id == id).FirstOrDefault();
            if (serdet != null)
            {
                tm.ProjectId = serdet.ProjectId;
                tm.Name = serdet.Name;
                tm.IP = serdet.IP;
                tm.UID = serdet.UID;
                tm.PWD = serdet.PWD;
                tm.Id = serdet.Id;
            }
            return new JsonResult() // this is new code
            {
                Data = tm,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };          
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpGet]
        public ActionResult VideoServer()
        {
            return PartialView("VideoServer");
        }
    }
}


public class assignCamera
{
    public string Name { get; set; }
    public string Status { get; set; }
}


#endregion