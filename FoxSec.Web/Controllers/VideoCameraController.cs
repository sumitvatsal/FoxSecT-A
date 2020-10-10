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
using FoxSec.Infrastructure.EF.Repositories.Interfaces;
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
using static FoxSec.DomainModel.DomainObjects.CameraPermissions;
using FoxSec.Web.ListModel;

namespace FoxSec.Web.Controllers
{
    public class VideoCameraController : PaginatorControllerBase<value>
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
        private readonly ICameraAccessRepository _ICameraAccessRepository;
        private readonly IVideoAccessRepository _IVideoAccessRepository;

        private ResourceManager _resourceManager;
        public VideoCameraController(IUserService userService,
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
                                  ICameraAccessRepository CameraAccessRepository,
                                 IVideoAccessRepository VideoAccessRepository,

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
            _ICameraAccessRepository = CameraAccessRepository;
            _IVideoAccessRepository = VideoAccessRepository;
            _userBuildingService = userBuildingService;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
        }

        #region Tree
        public ActionResult Camera()
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

            foreach (var items1 in Camera)
            {
                Cameras.Add(new Node() { ParentId = items1.CompanyId, MyId = items1.Id, Name = items1.CameraName });
            }
            ctvm.Cameras = Cameras;
            //ctvm.Cameras =
            //           from Camera in
            //               _IVideoAccessRepository.FindAll(x => companyIds.Contains(Convert.ToInt32(x.CompanyId)))
            //           select
            //               new Node
            //               {
            //                   ParentId = Camera.CompanyId,
            //                   MyId = Camera.Id,
            //                   Name = Camera.CameraName
            //               };
            return PartialView("Camera", ctvm);
        }

        FoxSecDBContext db = new FoxSecDBContext();
        [HttpGet]
        public ActionResult ByCompany(int Id)
        {
            try
            {
                Session["companyid"] = Id;
                var uvm = CreateViewModel<LiveVideoListViewModel>();
                List<FSCameras> FSCameras = UsersByCamera();
                IEnumerable<value> list = new List<value>();
                //  List<CameraPermissions> VideoAccess = ByCamera(Id);
                //IEnumerable<value> list = new List<value>();
                Mapper.Map(FSCameras, list);
                uvm.Paginator = SetupPaginator(ref list, 0, 10);
                uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
                uvm.Paginator.Prefix = "Usersss";

                uvm.Users1 = list;
                uvm.FilterCriteria = 1;
                return PartialView("List", uvm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Search(string name, string comment, string cardSer, string cardDk, string cardCode, string company, string title, int filter, int departmentId, int? nav_page, int? rows, int? sort_field, int? sort_direction,
           int countryId, int locationId, int buildingId, int companyId, int floorId)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            FoxSecDBContext db = new FoxSecDBContext();
            List<FSCameras> FSCameras = UsersByCamera();
            IEnumerable<item> camaras = new List<item>();

            //Mapper.Map(FSCameras, camaras);
            IEnumerable<value> filt_camras = new List<value>();
            var gridshort = CreateViewModel<LiveVideoListViewModel>();
            //var uvm = CreateViewModel<LiveVideoListViewModel>();
            //IEnumerable<item> Camralist = new List<item>();
            if (comment != "")
            {
                gridshort.Comment = true;
                //return PartialView("List", uvm);
            }
            else
            {
                var Live_camera = db.FSCameras;
            }

            var uvm = CreateViewModel<LiveVideoListViewModel>();
            FSCameras = ApplyUserStatusFilter(FSCameras, filter).ToList();

            //IEnumerable<value> list = new List<value>();
            Mapper.Map(FSCameras, filt_camras);

            if (sort_field.HasValue && sort_direction.HasValue)
            {

                if (sort_direction.Value == 0)
                {
                    filt_camras = filt_camras.OrderBy(x => x.Name).ToList();
                }
                else filt_camras = filt_camras.OrderByDescending(x => x.Name).ToList();

            }
            //IEnumerable<value> list = new List<value>();
            //Mapper.Map(FSCameras, list);
            uvm.Paginator = SetupPaginator(ref filt_camras, nav_page, rows);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResults";
            uvm.Paginator.Prefix = "Usersss";
            uvm.Users1 = filt_camras;
            uvm.FilterCriteria = 1;
            return PartialView("List", uvm);
        }

        private IEnumerable<FSCameras> ApplyUserStatusFilter(IEnumerable<FSCameras> filt_camras, int filter)
        {
            if (filter == 1)
            {
                filt_camras = filt_camras.ToList();
            }
            else if (filter == 0)
            {
                filt_camras = filt_camras.ToList();
            }

            return filt_camras;
        }

        private List<FSCameras> UsersByCamera()
        {
            int cmp_id = Convert.ToInt32(Session["companyid"]);
            int roleid = Convert.ToInt32(Session["roll_id"]);
            if (roleid == 0)
            {
                roleid = 5;
            }

            string connectionString1 = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
            SqlConnection myConnection1 = new SqlConnection(connectionString1);
            myConnection1.Open();

            SqlCommand myCommand1;
            //if (roleid == 5)
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsBuildingAdmin)
            {
                myCommand1 = new SqlCommand("select Id,Name from FSCameras where Deleted=0", myConnection1);            
            }
            else
            {
                myCommand1 = new SqlCommand("select CameraID as Id, (select name from FSCameras where id=cp.CameraID) as Name from CameraPermissions cp where CompanyID=@CompanyID and Access=1 and CameraID in (select id from FSCameras where Deleted=0 )", myConnection1);
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

        private List<CameraPermissions> ByCamera(int Id)
        {
            try
            {
                List<CameraPermissions> VAccess = _ICameraAccessRepository.FindAll(x => Id == x.CompanyID && x.Access == true).ToList();
                return VAccess;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        #endregion
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
    }
}


