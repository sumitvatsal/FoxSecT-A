using AutoMapper;
using FoxSec.Authentication;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EF.Repositories.Interfaces;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.ServiceResults;
using FoxSec.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ViewResources;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using QRCoder;
using System.Net;
using System.Net.Mail;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI;
using iTextSharp.tool.xml;
using FoxSec.DomainModel;
using FoxSec.Core.SystemEvents;
using FoxSec.Common.Enums;
using Winnovative.WnvHtmlConvert;
using Rectangle = iTextSharp.text.Rectangle;
using FoxSec.Core.Infrastructure.UnitOfWork;
using System.IO;
using Image = System.Drawing.Image;
using System.Drawing.Drawing2D;

namespace FoxSec.Web.Controllers
{
    [System.Web.Script.Services.ScriptService]
    public class VisitorsController : PaginatorControllerBase<VisitorItem>
    {
        // GET: Visitors

        FoxSecDBContext db = new FoxSecDBContext();
        private readonly IVisitorService _visitorService;
        private readonly IVisitorRepository _visitorRepository;
        private readonly IUserTimeZoneService _userTimeZoneService;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUserTimeZonePropertyRepository _userTimeZonePropertyRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly IUserPermissionGroupService _userPermissionGroupService;
        private readonly IUserPermissionGroupRepository _userPermissionGroupRepository;
        private readonly IUserService _userService;
        private readonly IUserAccessUnitTypeRepository _cardTypeRepository;
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
        private readonly IUsersAccessUnitService _userAccessUnitService;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IUserBuildingService _userBuildingService;
        private readonly IUsersAccessUnitService _cardService;
        private readonly IUsersAccessUnitRepository _usersAccessUnitRepository;
        private readonly IControllerUpdateService _controllerUpdateService;
        private readonly IFSINISettingRepository _FSINISettingsRepository;
        private ResourceManager _resourceManager;
        private readonly ILogService _logService;
        private readonly IUserRoleRepository _userrolerepository;

        public object DBConnectionSettings { get; private set; }

        public VisitorsController(IVisitorService visitorService,
                                IVisitorRepository visitorRepository,
                                IUserService userService,
                                IUserAccessUnitTypeRepository cardTypeRepository,
                                IUserTimeZoneService userTimeZoneService,
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
                                ILogService logService,
                                 IUserRoleRepository userrolerepository,
                                ILogger logger) : base(currentUser, logger)
        {
            _visitorService = visitorService;
            _visitorRepository = visitorRepository;
            _userTimeZoneService = userTimeZoneService;
            _FSINISettingsRepository = FSINISettingsRepository;
            _controllerUpdateService = controllerUpdateService;
            _cardTypeRepository = cardTypeRepository;
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
            _userrolerepository = userrolerepository;
            _resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        string flag = "";
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
        public ActionResult Index()
        {
            Session["CapturedImage"] = "";
            return View();
        }
        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            con.Open();
            SqlCommand cmdg = new SqlCommand("select StaticId from Roles where id=(select top 1 roleid from UserRoles where userid='" + CurrentUser.Get().Id + "' and IsDeleted=0)", con);
            hmv.StaticId = Convert.ToInt32(cmdg.ExecuteScalar()); ;
            con.Close();
            return PartialView(hmv);
        }

        [HttpGet]
        public ActionResult CreateVisitor()
        {
            var vem = CreateViewModel<VisitorEditViewModel>();

            var companyId = CurrentUser.Get().CompanyId.HasValue ? CurrentUser.Get().CompanyId.Value : -1;
            IEnumerable<Company> companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active).OrderBy(x => x.Name.ToLower());
            if (CurrentUser.Get().IsBuildingAdmin)
            {
                var buildIds = GetUserBuildings(_buildingRepository, _userRepository);

                companies = companies.Where(x => x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildIds.Contains(y.BuildingObject.BuildingId)));
            }

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                companies = companies.Where(cc => cc.Id == companyId || (cc.ParentId != null && cc.ParentId.Value == companyId));
            }

            vem.Companies = new SelectList(companies, "Id", "Name", vem.FoxSecVisitor.Company);
            return PartialView("CreateVisitor", vem);
        }
        public ActionResult CreatePersonalData(VisitorItem visitor)
        {
            var IsSucceed = false;
            var resId = -1;
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(visitor.StartDateTime.ToString()))
                    {
                        visitor.StartDateTime = null;
                    }
                    else
                    {
                        if (visitor.StartDateTime != null)
                            visitor.StartDateTime = DateTime.ParseExact(visitor.StartDateTime.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    if (string.IsNullOrWhiteSpace(visitor.StopDateTime.ToString()))
                    {
                        visitor.StopDateTime = null;
                    }
                    else
                    {
                        if (visitor.StopDateTime != null)
                            visitor.StartDateTime = DateTime.ParseExact(visitor.StopDateTime.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    }

                    if (string.IsNullOrWhiteSpace(visitor.ReturnDate.ToString()))
                    {
                        visitor.ReturnDate = null;
                    }
                    else
                    {
                        if (visitor.ReturnDate != null)
                            visitor.ReturnDate = DateTime.ParseExact(visitor.ReturnDate.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception e) { Console.WriteLine("Error:" + e); }
                
                var companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active).OrderBy(x => x.Name.ToLower());
                visitor.Company = companies.Where(x => x.Id == visitor.CompanyId).Select(x => x.Name).ToString();
                
                VisitorCreateResult result = _visitorService.CreateVisitor(
                    visitor.CarNr,
                    visitor.UserId,
                    visitor.FirstName,
                    visitor.CarType,
                    visitor.StartDateTime,
                    visitor.StopDateTime,
                    visitor.CompanyId,
                    visitor.LastName,
                    visitor.Company,
                    visitor.Email,
                    HostName,
                    visitor.PhoneNumber,
                    visitor.IsPhoneNrAccessUnit,
                    visitor.IsCarNrAccessUnit,
                    visitor.ReturnDate,
                    visitor.CardNeedReturn, visitor.PersonalCode, visitor.Comment);

                Session["NewVisitorId"] = result.Id;
                resId = result.Id;
                IsSucceed = true;
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                            Console.WriteLine("error:" + modelError.ErrorMessage);
                        }
                    }
                    return Json(modelErrors);
                }

            }
            return Json(new
            {
                IsSucceed,
                Id = resId
            });

        }

        public ActionResult CreatePersonalData_2(string Company, int? CompanyId, int? UserID, string FirstName, string LastName, string CarNumber, string CarType, string From, string To, string PhoneNumber, string Email, bool IsPhoneNrAccessUnit, bool IsCarNrAccessUnit, string ReturnDate, bool CardNeedReturn, string PersonalCode, string Comment)
        {
            var IsSucceed = false;
            var resId = -1;
            VisitorItem visitor = new VisitorItem();

            visitor.CompanyId = CompanyId;
            visitor.Company = Company;
            visitor.UserId = Convert.ToInt32(UserID);
            visitor.FirstName = FirstName;
            visitor.LastName = LastName;
            visitor.CarNr = CarNumber;
            visitor.CarType = CarType;
            visitor.PhoneNumber = PhoneNumber;
            visitor.Email = Email;
            visitor.IsPhoneNrAccessUnit = IsPhoneNrAccessUnit;
            visitor.IsCarNrAccessUnit = IsCarNrAccessUnit;
            visitor.CardNeedReturn = CardNeedReturn;
            visitor.PersonalCode = PersonalCode;
            visitor.Comment = Comment;
            
            try
            {
                visitor.StartDateTime = DateTime.ParseExact(From.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                visitor.StopDateTime = DateTime.ParseExact(To.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                visitor.ReturnDate = DateTime.ParseExact(ReturnDate.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Console.WriteLine("Visitor Create:Date Conversion error-" + e); //for test
            }

            VisitorCreateResult result = _visitorService.CreateVisitor(
                visitor.CarNr,
                visitor.UserId,
                visitor.FirstName,
                visitor.CarType,
                visitor.StartDateTime,
                visitor.StopDateTime,
                visitor.CompanyId,
                visitor.LastName,
                visitor.Company,
                visitor.Email,
                HostName,
                visitor.PhoneNumber,
                visitor.IsPhoneNrAccessUnit,
                visitor.IsCarNrAccessUnit,
                visitor.ReturnDate,
                visitor.CardNeedReturn, visitor.PersonalCode, visitor.Comment);

            //Session["NewUserId"] = result.Id;

            resId = result.Id;
            if (resId == 0)
            {
                IsSucceed = false;
                return Json(new
                {
                    IsSucceed,
                    Count = 0,
                });
            }
            else if (resId < 0)
            {
                IsSucceed = false;
                return Json(new
                {
                    IsSucceed,
                    Count = -1 * resId,
                });
            }
            else
            {
                Session["NewVisitorId"] = result.Id;
                IsSucceed = true;
                return Json(new
                {
                    IsSucceed,
                    Id = resId
                });
            }

        }

        [ValidateInput(false)]
        public ActionResult VisitorUserExportCallBack()
        {
            try
            {
                var ulvm = CreateViewModel<UserListViewModel>();
                ulvm = (UserListViewModel)Session["TypedListModel"];
                return PartialView("VisitorsExportUsersDetails", ulvm.Users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ValidateInput(false)]
        public ActionResult SelectedUserIdParam(string PermissionName)
        {
            Session["VisitorPermissionName"] = PermissionName;
            return Json(PermissionName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectedUserIdParam2(int? userId)
        {
            Session["VisitorUserId"] = userId;
            return Json(userId, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public bool CheckVisitorAlreadyExist(string firstName, string lastname, string phoneNumber, string email)
        {
            var visitorExist = false;
            List<Visitor> visitor = _visitorRepository.FindAll().Where(x => x.FirstName == firstName.Trim() && x.LastName == lastname.Trim() && x.PersonalCode == phoneNumber).ToList();
            if (visitor.Count > 0)
            {
                visitorExist = true;
            }
            else
            {
                List<Visitor> visitorname = _visitorRepository.FindAll().Where(x => x.FirstName.ToUpper() == firstName.Trim().ToUpper() 
                && x.LastName.ToUpper() == lastname.Trim().ToUpper()
                 && x.PersonalCode != phoneNumber && phoneNumber!="").ToList();
               
               
                if (visitorname.Count > 0)
                {
                    visitorExist = false;
                }
                else
                {
                    //string firstname = _userRepository.FindAll().Where(x => x.Id == obj.UserId).Select(y => y.FirstName + " " + y.LastName).FirstOrDefault();

                    List<Visitor> visit = _visitorRepository.FindAll().Where(x => x.FirstName.ToUpper() == firstName.Trim().ToUpper() && x.LastName.ToUpper() == lastname.Trim().ToUpper()
                    && x.PersonalCode == null).ToList();
                    if (visit.Count > 0)
                    {
                        visitorExist = true;
                    }
                    else
                    {
                        List<Visitor> visitcheck = _visitorRepository.FindAll().Where(x => x.FirstName.ToUpper() != firstName.Trim().ToUpper() && x.LastName.ToUpper() != lastname.Trim().ToUpper()
                       ).ToList();
                        if (visitcheck.Count > 0)
                        {
                            visitorExist = false;
                        }
                    }
              
                }
            }

           
          
            
            return visitorExist;
        }

        [HttpGet]
        public bool CheckVisitorAlreadyExistEdit(string firstName, string lastname, string phoneNumber, string email, int vid)
        {
            var visitorExist = false;
            List<Visitor> visitor = _visitorRepository.FindAll().Where(x => x.FirstName == firstName.Trim() && x.LastName == lastname.Trim() && x.PersonalCode == phoneNumber  
            && x.Id != vid).ToList();
          
            if (visitor.Count > 0)
            {
                visitorExist = true;
            }
            else
            {
                List<Visitor> visitorname = _visitorRepository.FindAll().Where(x => x.FirstName.ToUpper() == firstName.Trim().ToUpper() && x.LastName.ToUpper() == lastname.Trim().ToUpper()
                 && x.PersonalCode != phoneNumber && phoneNumber != "" && x.Id != vid).ToList();


                if (visitorname.Count > 0)
                {
                    visitorExist = false;
                }
                else
                {
                    //string firstname = _userRepository.FindAll().Where(x => x.Id == obj.UserId).Select(y => y.FirstName + " " + y.LastName).FirstOrDefault();

                    List<Visitor> visit = _visitorRepository.FindAll().Where(x => x.FirstName.ToUpper() == firstName.Trim().ToUpper() && x.LastName.ToUpper() == lastname.Trim().ToUpper()
                    && x.PersonalCode == null && x.Id != vid).ToList();
                    if (visit.Count > 0)
                    {
                        visitorExist = true;
                    }
                    else
                    {
                        List<Visitor> visitcheck = _visitorRepository.FindAll().Where(x => x.FirstName.ToUpper() != firstName.Trim().ToUpper() && x.LastName.ToUpper() != lastname.Trim().ToUpper()
                        && x.Id != vid).ToList();
                        if (visitcheck.Count > 0)
                        {
                            visitorExist = false;
                        }
                    }

                }
            }

            return visitorExist;
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

        [HttpGet]
        public JsonResult SearchByNameAutoComplete(string term, int filter)
        {
            term = term.ToLower();

            //IEnumerable<Visitor> users = _visitorRepository.FindAll(x => !x.IsDeleted && x.FirstName.ToLower().Contains(term) || x.LastName.ToLower().Contains(term) && !x.IsDeleted);

            IEnumerable<Visitor> users = _visitorRepository.FindAll(x => !x.IsDeleted).ToList();
            users = ApplyUserStatusFilter(users, filter);
            foreach (var obj in users)
            {
                string joinname = "";
                DateTime? validtodt = _visitorRepository.FindById(obj.Id).StopDateTime;
                if (validtodt != null)
                {
                    if (validtodt > DateTime.Now)
                    {
                        if (obj.UserId != null)
                        {
                            //string firstname = _userRepository.FindAll().Where(x => x.Id == obj.UserId).Select(y => y.FirstName + " " + y.LastName).FirstOrDefault();
                            con.Open();
                            SqlCommand cmd = new SqlCommand("select FirstName+' '+LastName from Users where id='" + obj.UserId + "'", con);
                            string firstname = Convert.ToString(cmd.ExecuteScalar());
                            con.Close();
                            if (!string.IsNullOrEmpty(firstname))
                            {
                                joinname = "(" + firstname + ")";
                            }
                            obj.LastName = obj.LastName + " " + joinname;
                        }
                    }
                }
            }
            users = users.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(term.ToLower())).ToList();

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                int cmp_id = CurrentUser.Get().CompanyId.Value;
                List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmp_id || x.ParentId == cmp_id)) select c.Id).ToList();
                users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
            }

            List<string> result = users.Select(user => user.FirstName + " " + user.LastName).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<Visitor> ApplyUserStatusFilter(IEnumerable<Visitor> users, int filter)
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
                users = users.ToList();
            }

            return users;
        }

        public ActionResult Search(string name, string company, int filter, int? nav_page, int? rows, int? sort_field, int? sort_direction,
               int countryId, int locationId, int buildingId, int companyId, int floorId)
        {
            string[] arr = name.Split('(');
            if (arr.Length > 1)
            {
                name = arr[0].Trim();
            }
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            IEnumerable<VisitorItem> filt_users = new List<VisitorItem>();
            var uvm = CreateViewModel<VisitorListViewModel>();
            List<Visitor> users = _visitorRepository.FindAll(x => !x.IsDeleted).ToList();

            if (countryId != 0)
            {
                users = VisitorByCountry(countryId);
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
                users = UsersByCompany(companyId, buildingId);
            }
            users = ApplyUserStatusFilter(users, filter).ToList();
            foreach (var obj in users)
            {
                string joinname = "";
                DateTime? validtodt = _visitorRepository.FindById(obj.Id).StopDateTime;
                if (validtodt != null)
                {
                    if (validtodt > DateTime.Now)
                    {
                        if (obj.UserId != null)
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("select FirstName+' '+LastName from Users where id='" + obj.UserId + "'", con);
                            string firstname = Convert.ToString(cmd.ExecuteScalar());
                            con.Close();
                            //string firstname = _userRepository.FindAll().Where(x => x.Id == obj.UserId).Select(y => y.FirstName + " " + y.LastName).FirstOrDefault();
                            if (!string.IsNullOrEmpty(firstname))
                            {
                                joinname = "(" + firstname + ")";
                            }
                            obj.LastName = obj.LastName + " " + joinname;
                        }
                    }
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
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select name from Companies where id='" + obj.CompanyId + "'", con);
                    obj.Company = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            if (name != String.Empty)
            {

                string[] split = name.ToLower().Trim().Split(' ');
                users = users.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(name.ToLower())).ToList();
                //switch (split.Count())
                {
                    //case 1:
                    //    users = users.Where(x => (x.FirstName.ToLower().Contains(split[0]) || x.LastName.ToLower().Contains(split[0]))).ToList();
                    //    break;
                    //case 2:
                    //    users = users.Where(x => x.FirstName.ToLower().Contains(split[0]) && x.LastName.ToLower().Contains(split[1])).ToList();
                    //    break;
                    //default:
                    //    users = users.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(name.ToLower())).ToList();
                    //    break;
                }
            }

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                int cmp_id = CurrentUser.Get().CompanyId.Value;
                List<int> companyIds = (from c in _companyRepository.FindAll(x => !x.IsDeleted && (x.Id == cmp_id || x.ParentId == cmp_id)) select c.Id).ToList();
                users = users.Where(x => x.CompanyId.HasValue && companyIds.Contains(x.CompanyId.Value)).ToList();
            }
            else
            {
                if (company != null && company != String.Empty)
                {
                    users = users.Where(x => x.CompanyId == Convert.ToInt32(company)).ToList();
                }
            }

            Mapper.CreateMap<Visitor, VisitorItem>();
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

                        if (sort_direction.Value == 0) filt_users = filt_users.OrderBy(x => x.Company).ToList();
                        else filt_users = filt_users.OrderByDescending(x => x.Company).ToList();
                        break;
                    case 5:
                        filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.StartDateTime) : filt_users.OrderByDescending(x => x.StartDateTime);
                        break;
                    case 6:
                        filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.DateReturn) : filt_users.OrderByDescending(x => x.DateReturn);
                        break;
                    case 7:
                        filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.LastChange) : filt_users.OrderByDescending(x => x.LastChange);
                        break;
                    case 4:
                        filt_users = sort_direction.Value == 0 ? filt_users.OrderBy(x => x.StopDateTime) : filt_users.OrderByDescending(x => x.StopDateTime);
                        break;
                    default:
                        filt_users = filt_users.OrderByDescending(x => x.StopDateTime);
                        break;
                }
            }
            else
            {
                filt_users = filt_users.OrderByDescending(x => x.StopDateTime);
            }

            uvm.Paginator = SetupPaginator(ref filt_users, nav_page, rows);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResultsVisitor";
            uvm.Paginator.Prefix = "Visitor";
            uvm.Visitors = filt_users;
            uvm.FilterCriteria = filter;
            con.Open();
            SqlCommand cmdg = new SqlCommand("select StaticId from Roles where id=(select top 1 roleid from UserRoles where userid='" + CurrentUser.Get().Id + "' and IsDeleted=0)", con);
            uvm.StaticId = Convert.ToInt32(cmdg.ExecuteScalar()); ;
            con.Close();


            return PartialView("ListUser", uvm);
        }

        [HttpGet]
        public ActionResult VisitorReadOnly(int id)
        {
            Session["VisitorId"] = id;
            DateTime? validtodt = _visitorRepository.FindById(id).StopDateTime;
            if (validtodt == null) { }
            else
            {
                if (validtodt < DateTime.Now)
                {
                    using (IUnitOfWork work = UnitOfWork.Begin())
                    {
                        Visitor vis = _visitorRepository.FindById(id);
                        if (vis.UserId == null) { }
                        else
                        {
                            vis.UserId = null;
                            work.Commit();
                        }
                    }
                }
            }

            Visitor user = _visitorRepository.FindById(id);
            var uvm = CreateViewModel<VisitorEditViewModel>();
            List<int> userPermissionGroup = new List<int>();
            Mapper.CreateMap<Visitor, VisitorItem>();
            Mapper.Map(user, uvm.FoxSecVisitor);
            uvm.LanguageItems = GetLanguages();
            List<UserPermissionGroup> userPermissionGroups = new List<UserPermissionGroup>();
            userPermissionGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == uvm.FoxSecVisitor.UserId).OrderBy(x => x.User).ToList();

            foreach (var up in userPermissionGroups)
            {
                if (uvm.FoxSecVisitor.UserId == up.UserId)
                {
                    uvm.FoxSecVisitor.UserPermissionGroupName = up.Name;
                    User det = _userRepository.FindById(up.UserId);
                    uvm.FoxSecVisitor.CardFirstName = det.FirstName;
                    uvm.FoxSecVisitor.CardLastName = det.LastName;
                }
            }

            uvm.FoxSecVisitor.FromDate = !String.IsNullOrEmpty(uvm.FoxSecVisitor.StartDateTime.ToString()) ? Convert.ToDateTime(uvm.FoxSecVisitor.StartDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
            uvm.FoxSecVisitor.ToDate = !String.IsNullOrEmpty(uvm.FoxSecVisitor.StopDateTime.ToString()) ? Convert.ToDateTime(uvm.FoxSecVisitor.StopDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
            uvm.FoxSecVisitor.DateReturn = !String.IsNullOrEmpty(uvm.FoxSecVisitor.ReturnDate.ToString()) ? Convert.ToDateTime(uvm.FoxSecVisitor.ReturnDate.ToString()).ToString("dd.MM.yyyy HH:mm") : Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd.MM.yyyy HH:mm");
            DateTime validto = Convert.ToDateTime(uvm.FoxSecVisitor.StopDateTime);

            if (uvm.FoxSecVisitor.UserId == null)
            {
                uvm.FoxSecVisitor.CardBackFlag = false;
            }
            else
            {
                uvm.FoxSecVisitor.DateReturn = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd.MM.yyyy HH:mm");
                uvm.FoxSecVisitor.CardBackFlag = true;
            }
            var companyId = CurrentUser.Get().CompanyId.HasValue ? CurrentUser.Get().CompanyId.Value : -1;
            IEnumerable<Company> companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active).OrderBy(x => x.Name.ToLower());
            if (CurrentUser.Get().IsBuildingAdmin)
            {
                companies = companies.Where(x => x.CompanyBuildingObjects.Any(y => !y.IsDeleted));
            }

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                companies =
                    companies.Where(cc => cc.Id == companyId || (cc.ParentId != null && cc.ParentId.Value == companyId));
            }
            ViewBag.ImageData1 = null;
            uvm.Companies = new SelectList(companies, "Id", "Name", uvm.FoxSecVisitor.CompanyId);

            string imgname = "/Uploads/VisitorsPhoto/" + id.ToString() + ".png";

            if (System.IO.File.Exists(Server.MapPath(imgname)))
            {
                ViewBag.VisitorImage = "../../Uploads/VisitorsPhoto/" + id.ToString() + ".png" + "?time=" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            }
            else
            {
                ViewBag.VisitorImage = "../../Uploads/VisitorsPhoto/no-image.png" + "?time=" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            }

            return PartialView("VisitorReadOnly", uvm);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Session["VisitorId"] = id;
            DateTime? validtodt = _visitorRepository.FindById(id).StopDateTime;
            if (validtodt == null) { }
            else
            {
                if (validtodt < DateTime.Now)
                {
                    using (IUnitOfWork work = UnitOfWork.Begin())
                    {
                        Visitor vis = _visitorRepository.FindById(id);
                        if (vis.UserId == null) { }
                        else
                        {
                            vis.UserId = null;
                            work.Commit();
                        }
                    }
                }
            }

            Visitor user = _visitorRepository.FindById(id);
            var uvm = CreateViewModel<VisitorEditViewModel>();
            List<int> userPermissionGroup = new List<int>();
            Mapper.CreateMap<Visitor, VisitorItem>();
            Mapper.Map(user, uvm.FoxSecVisitor);
            uvm.LanguageItems = GetLanguages();
            List<UserPermissionGroup> userPermissionGroups = new List<UserPermissionGroup>();
            userPermissionGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == uvm.FoxSecVisitor.UserId).OrderBy(x => x.User).ToList();

            foreach (var up in userPermissionGroups)
            {
                if (uvm.FoxSecVisitor.UserId == up.UserId)
                {
                    uvm.FoxSecVisitor.UserPermissionGroupName = up.Name;
                }
            }

            uvm.FoxSecVisitor.FromDate = !String.IsNullOrEmpty(uvm.FoxSecVisitor.StartDateTime.ToString()) ? Convert.ToDateTime(uvm.FoxSecVisitor.StartDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
            uvm.FoxSecVisitor.ToDate = !String.IsNullOrEmpty(uvm.FoxSecVisitor.StopDateTime.ToString()) ? Convert.ToDateTime(uvm.FoxSecVisitor.StopDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
            uvm.FoxSecVisitor.DateReturn = !String.IsNullOrEmpty(uvm.FoxSecVisitor.ReturnDate.ToString()) ? Convert.ToDateTime(uvm.FoxSecVisitor.ReturnDate.ToString()).ToString("dd.MM.yyyy HH:mm") : Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd.MM.yyyy HH:mm");
            DateTime validto = Convert.ToDateTime(uvm.FoxSecVisitor.StopDateTime);

            //if (!String.IsNullOrEmpty(uvm.FoxSecVisitor.StopDateTime.ToString()))
            //{
            //   if (uvm.FoxSecVisitor.ReturnDate == null)
            //    {
            //        uvm.FoxSecVisitor.DateReturn = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd.MM.yyyy HH:mm");
            //        uvm.FoxSecVisitor.CardBackFlag = true;
            //    }
            //    else if (uvm.FoxSecVisitor.ReturnDate == (validto).AddDays(1))
            //    {
            //        uvm.FoxSecVisitor.DateReturn = Convert.ToDateTime(uvm.FoxSecVisitor.ReturnDate.ToString()).ToString("dd.MM.yyyy HH:mm");
            //        uvm.FoxSecVisitor.CardBackFlag = false;
            //    }
            //    else
            //    {
            //        uvm.FoxSecVisitor.DateReturn = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd.MM.yyyy HH:mm");
            //        uvm.FoxSecVisitor.CardBackFlag = true;
            //    }
            //}
            //else
            //{
            //    uvm.FoxSecVisitor.DateReturn = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd.MM.yyyy HH:mm");
            //    uvm.FoxSecVisitor.CardBackFlag = true;
            //}

            if (uvm.FoxSecVisitor.UserId == null)
            {
                uvm.FoxSecVisitor.CardBackFlag = false;
            }
            else
            {
                uvm.FoxSecVisitor.DateReturn = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd.MM.yyyy HH:mm");
                uvm.FoxSecVisitor.CardBackFlag = true;
            }
            var companyId = CurrentUser.Get().CompanyId.HasValue ? CurrentUser.Get().CompanyId.Value : -1;
            IEnumerable<Company> companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active).OrderBy(x => x.Name.ToLower());
            if (CurrentUser.Get().IsBuildingAdmin)
            {
                companies = companies.Where(x => x.CompanyBuildingObjects.Any(y => !y.IsDeleted));
            }

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                companies =
                    companies.Where(cc => cc.Id == companyId || (cc.ParentId != null && cc.ParentId.Value == companyId));
            }
            ViewBag.ImageData1 = null;
            uvm.Companies = new SelectList(companies, "Id", "Name", uvm.FoxSecVisitor.CompanyId);

            string imgname = "/Uploads/VisitorsPhoto/" + id.ToString() + ".png";

            if (System.IO.File.Exists(Server.MapPath(imgname)))
            {
                ViewBag.VisitorImage = "../../Uploads/VisitorsPhoto/" + id.ToString() + ".png" + "?time=" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            }
            else
            {
                ViewBag.VisitorImage = "../../Uploads/VisitorsPhoto/no-image.png" + "?time=" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            }

            return PartialView("Edit", uvm);
        }

        [HttpPost]
        public ActionResult EditPersonalData(int Id, int? CompanyId, int? UserId, string FirstName, string LastName, string CarNr, string CarType, string StartDateTime, string StopDateTime, string PhoneNumber, string Email, bool IsPhoneNrAccessUnit, bool IsCarNrAccessUnit, string ReturnDate, bool CardNeedReturn, string PersonalCode, string Comment)
        {
            DateTime? sdate = null;
            DateTime? stdate = null;
            DateTime? rdate = null;

            if (!String.IsNullOrEmpty(StartDateTime))
            {
                sdate = DateTime.ParseExact(StartDateTime.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            if (!String.IsNullOrEmpty(StopDateTime))
            {
                stdate = DateTime.ParseExact(StopDateTime.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            if (!String.IsNullOrEmpty(ReturnDate))
            {
                rdate = DateTime.ParseExact(ReturnDate.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            }

            if (ModelState.IsValid)
            {

                Visitor oldUserData = _visitorRepository.FindById((int)Id);


                _visitorService.EditUserPersonalData((int)Id, FirstName, LastName, CompanyId,
                                                  PhoneNumber, IsPhoneNrAccessUnit, Email, CarNr,
                                                  IsCarNrAccessUnit, CarType, sdate,
                                                  stdate, UserId, CardNeedReturn, rdate, PersonalCode, Comment);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetUserData(int userId)
        {
            var userItem = new VisitorItem();

            Mapper.Map(_visitorRepository.FindById(userId), userItem);
            string joinname = "";
            if (userItem.UserId != null)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select FirstName+' '+LastName from Users where id='" + userItem.UserId + "'", con);
                string firstname = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
                if (!string.IsNullOrEmpty(firstname))
                {
                    joinname = "(" + firstname + ")";
                }
                userItem.LastName = userItem.LastName + " " + joinname;
            }

            userItem.ToDate = userItem.StopDateTime.ToString();
            if (!String.IsNullOrEmpty(userItem.ToDate))
            {
                userItem.ToDate = Convert.ToDateTime(userItem.StopDateTime).ToString("dd.MM.yyyy HH:mm");
            }

            userItem.FromDate = userItem.StartDateTime.ToString();
            if (!String.IsNullOrEmpty(userItem.FromDate))
            {
                userItem.FromDate = Convert.ToDateTime(userItem.StartDateTime).ToString("dd.MM.yyyy HH:mm");
            }

            if (!String.IsNullOrEmpty(userItem.StopDateTime.ToString()))
            {
                DateTime validto = Convert.ToDateTime(userItem.StopDateTime);
                if (userItem.ReturnDate == null)
                {
                    userItem.DateReturn = null;
                }
                if (userItem.ReturnDate == (validto).AddDays(1))
                {
                    userItem.DateReturn = Convert.ToDateTime(userItem.ReturnDate.ToString()).ToString("dd.MM.yyyy HH:mm");
                }
                else
                {
                    userItem.DateReturn = null;
                }
            }
            else
            {
                userItem.DateReturn = null;
            }

            userItem.ChangeLast = userItem.LastChange.ToString();
            if (!String.IsNullOrEmpty(userItem.ChangeLast))
            {
                userItem.ChangeLast = Convert.ToDateTime(userItem.LastChange).ToString("dd.MM.yyyy HH:mm");
            }

            if (userItem.CompanyId == null)
            {
                userItem.Company = "";
            }
            else
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select name from Companies where id='" + userItem.CompanyId + "'", con);
                userItem.Company = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }

            return Json(new
            {
                Id = userItem.Id,
                Name = string.Format("{0} {1}",
                userItem.FirstName, userItem.LastName),
                userItem.Company,
                userItem.UserStatus,
                userItem.ToDate,
                userItem.FromDate,
                userItem.DateReturn,
                userItem.ChangeLast
            }, JsonRequestBehavior.AllowGet);
        }

        #region qrcode

        #region qrcode generation
        /// <summary>
        /// this method is used to get qr code string
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string getqrcodetxt(int vid, int? userid)
        {
            try
            {
                //getting visitor card code string and valid from-to directly from foxsec databas
                string qrcode = "";
                Visitor user = _visitorRepository.FindById(vid);
                int uid = (int)userid;

                string validfrom = !String.IsNullOrEmpty(user.StartDateTime.ToString()) ? Convert.ToDateTime(user.StartDateTime.ToString()).ToString("MMddyyyyHHmm") : null;
                string validto = !String.IsNullOrEmpty(user.StopDateTime.ToString()) ? Convert.ToDateTime(user.StopDateTime.ToString()).ToString("MMddyyyyHHmm") : null;

                var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == uid && x.TypeId == 7 && x.Active == true).ToList();
                //int? cardid = _visitorRepository.FindById(vid).ParentVisitorsId;
                //var cards = _usersAccessUnitRepository.FindAll(x => x.Id == cardid);

                if (cards.ToList().Count > 0)
                {
                    UsersAccessUnit det = cards.ToList()[0];
                    string accessunitcodehexstring = det.Code;
                    if (!String.IsNullOrEmpty(det.Serial) && !String.IsNullOrEmpty(det.Dk))
                    {
                        int ser1 = 255;
                        int dk1 = 0;
                        bool success = Int32.TryParse(det.Serial, out ser1);
                        success = Int32.TryParse(det.Dk, out dk1);
                        accessunitcodehexstring = ser1.ToString("X2") + dk1.ToString("X4");
                    }

                    bool chkhex = OnlyHexInString(accessunitcodehexstring);
                    if (chkhex == true)
                    {
                        /// accessunitcodehexstring -hex decimal(1234567890ABCDEF only allowed) card code string minimum lenght 6
                        /// 
                        byte rndnr = GetRandomNumber();
                        string hexval = rndnr.ToString("X2") + "01" + validfrom + validto + accessunitcodehexstring + "00";//get hex of string
                        byte[] arrayOfBytes = StringtoByteArray(hexval);//converting hex value to byte array


                        byte crc8b = crc8(arrayOfBytes);
                        int arrysize = arrayOfBytes.Length;
                        arrayOfBytes[arrysize - 1] = crc8b;

                        byte[] output = CryptPacket(rndnr, arrayOfBytes);//crypting input array

                        string outputstr = ByteArrayToHexString(output);
                        qrcode = (char)2 + outputstr + (char)3;
                        return qrcode;
                    }
                    else
                    {
                        return ViewResources.SharedStrings.HexCardCodeError + ".";
                    }
                }
                else
                {
                    return ViewResources.SharedStrings.NoActiveCard;
                }

            }
            catch (Exception ex)
            {
                return ex.Message + ".";
            }
        }
        bool OnlyHexInString(String text)
        {
            char[] charArray = text.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
                if (!Uri.IsHexDigit(charArray[i]))
                    return false;
            return true;
        }

        public static byte[] CryptPacket(byte key, byte[] packet)
        {
            try
            {
                // create a new instance
                byte[] output = new byte[packet.Length];
                output[0] = packet[0];
                // process ALL array items
                for (int i = 1; i < packet.Length; i++)
                {
                    var sum = key ^ packet[i];
                    sum = sum - key;
                    output[i] = BitConverter.GetBytes(sum)[0];
                }
                return output;
            }
            catch
            {
                return null;
            }
        }

        public string ByteArrayToHexString(byte[] packet)
        {
            try
            {
                // create a new instance
                string output = "";

                // process ALL array items
                for (int i = 0; i < packet.Length; i++)
                {
                    output = output + packet[i].ToString("X2");
                }
                return output;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static byte crc8(byte[] packet)
        {
            try
            {
                // create a new instance 
                int output = 0;

                // process ALL array items
                for (int i = 0; i < packet.Length; i++)
                {
                    output = (output + packet[i]);
                }
                return (byte)(output & 255);
            }
            catch
            {
                return (byte)0;
            }
        }

        private byte[] StringtoByteArray(string str)
        {
            try
            {
                Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
                for (int i = 0; i <= 255; i++)
                    hexindex.Add(i.ToString("X2"), (byte)i);

                List<byte> hexres = new List<byte>();
                for (int i = 0; i < str.Length; i += 2)
                    hexres.Add(hexindex[str.Substring(i, 2)]);

                return hexres.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// this method is used to get random hex number from 1-255
        /// </summary>
        /// <returns></returns>
        public static byte GetRandomNumber()
        {
            Random rdm = new Random();
            int num = rdm.Next(1, 255);
            return (byte)num;
        }
        #endregion qrcode generation

        public string CheckPrintPreviewVisitorCard(int visitorid, int? userid)
        {
            try
            {
                Visitor user = _visitorRepository.FindById(visitorid);

                if (user.CompanyId == null)
                {
                    user.Company = "";
                }
                else
                {
                    List<Company> companies = _companyRepository.FindAll(x => x.Id == (int)user.CompanyId).ToList();
                    if (companies.Count == 0)
                    {
                        user.Company = "";
                    }
                    else
                    {
                        foreach (var cc in companies)
                        {
                            user.Company = cc.Name;
                        }
                    }
                }

                string startdate = !String.IsNullOrEmpty(user.StartDateTime.ToString()) ? Convert.ToDateTime(user.StartDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
                string enddate = !String.IsNullOrEmpty(user.StopDateTime.ToString()) ? Convert.ToDateTime(user.StopDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
                string qrcodetxt = getqrcodetxt(visitorid, user.UserId);
                return qrcodetxt;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult PrintPreviewVisitorCard(int visitorid, int? userid, string qrcodetxt)
        {
            try
            {
                Visitor user = _visitorRepository.FindById(visitorid);

                if (user.CompanyId == null)
                {
                    user.Company = "";
                }
                else
                {
                    List<Company> companies = _companyRepository.FindAll(x => x.Id == (int)user.CompanyId).ToList();
                    if (companies.Count == 0)
                    {
                        user.Company = "";
                    }
                    else
                    {
                        foreach (var cc in companies)
                        {
                            user.Company = cc.Name;
                        }
                    }
                }

                string startdate = !String.IsNullOrEmpty(user.StartDateTime.ToString()) ? Convert.ToDateTime(user.StartDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
                string enddate = !String.IsNullOrEmpty(user.StopDateTime.ToString()) ? Convert.ToDateTime(user.StopDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;

                GenerateQRCode(qrcodetxt);
                using (MemoryStream ms = new MemoryStream())
                {
                    Document pdfDoc = new Document(new iTextSharp.text.Rectangle(242, 156), 55f, 30f, 4f, 0f);
                    MemoryStream output = new MemoryStream();

                    pdfDoc.Open();

                    FontFactory.RegisterDirectories();

                    StringBuilder sb = new StringBuilder();

                    foreach (string fontname in FontFactory.RegisteredFonts)
                    {
                        sb.Append(fontname + "\n");
                    }

                    pdfDoc.Add(new Paragraph("All Fonts:\n" + sb.ToString()));

                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, output);

                    string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
                    int namelnght = user.FirstName.Length + user.LastName.Length;

                    iTextSharp.text.Font font1 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 11);
                    iTextSharp.text.Font font = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10);
                    font.SetColor(32, 32, 32);
                    iTextSharp.text.Font font5 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 11);
                    font5.SetColor(32, 32, 32);
                    iTextSharp.text.Font font6 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 9);
                    font5.SetColor(32, 32, 32);
                    iTextSharp.text.Font font2 = FontFactory.GetFont("arial narrow", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 19);
                    iTextSharp.text.Font font3 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8);
                    font3.SetColor(32, 32, 32);
                    iTextSharp.text.Font font4 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10);

                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(ViewBag.imageBytes);
                    jpg.ScaleAbsolute(70f, 67f);
                    jpg.BorderColor = BaseColor.BLACK;
                    PdfPCell logocell = new PdfPCell(jpg, true);

                    byte[] file;

                    string imgname = "/Uploads/VisitorsPhoto/" + visitorid.ToString() + ".png";

                    if (System.IO.File.Exists(Server.MapPath(imgname)))
                    {
                        file = System.IO.File.ReadAllBytes(Server.MapPath(imgname));
                    }
                    else
                    {
                        file = System.IO.File.ReadAllBytes(Server.MapPath("~/Uploads/VisitorsPhoto/no-image.png"));
                    }

                    iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(file);
                    jpg1.ScaleAbsolute(75f, 78f);
                    jpg1.Border = Rectangle.BOX;
                    jpg1.BorderColor = BaseColor.DARK_GRAY;
                    jpg1.BorderWidth = 2f;
                    PdfPCell logocell1 = new PdfPCell(jpg1, true);

                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 165f;
                    PdfPTable table1 = new PdfPTable(1);
                    table1.WidthPercentage = 165f;
                    PdfPTable table2 = new PdfPTable(1);
                    table2.WidthPercentage = 70f;
                    PdfPCell cell;

                    if (user.Company.Length > 40)
                    {
                        cell = new PdfPCell(new Phrase((user.Company).Substring(0, 40) + "...", font5));
                        cell.PaddingTop = 4f;
                    }
                    else if (user.Company.Length < 40)
                    {
                        cell = new PdfPCell(new Phrase(user.Company, font));
                        cell.PaddingTop = 4f;
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(user.Company, font5));
                        cell.PaddingTop = 8f;
                    }
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell);

                    PdfPCell blankCell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    blankCell.Border = PdfPCell.NO_BORDER;
                    table1.AddCell(blankCell);

                    cell = new PdfPCell(jpg1);
                    cell.BorderColor = BaseColor.WHITE;
                    cell.PaddingLeft = 9f;
                    table1.AddCell(cell);

                    cell = new PdfPCell(jpg);
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table2.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ViewResources.SharedStrings.Visitor, font2));
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table2.AddCell(cell);

                    if (namelnght > 15)
                    {
                        cell = new PdfPCell(new Phrase(user.FirstName + "\n" + user.LastName, font4));
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(user.FirstName + "\n" + user.LastName, font1));
                    }

                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table2.AddCell(cell);

                    cell = new PdfPCell(table1);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);

                    cell = new PdfPCell(table2);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ViewResources.SharedStrings.CardsValidFrom + ": " + startdate, font3));
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 9f;
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ViewResources.SharedStrings.CardsValidTo + ": " + enddate, font3));
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 9f;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    pdfDoc.Open();
                    pdfDoc.Add(table);

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    output.Position = 0;

                    byte[] outputPDF = output.ToArray();

                    string base64PDF = System.Convert.ToBase64String(outputPDF, 0, outputPDF.Length);

                    ViewBag.imageBytes1 = output.ToArray();

                    string imgBase64Data = Convert.ToBase64String(output.ToArray());
                    string imgDataURL = string.Format("data:image/png;base64,{0}", base64PDF);
                    byte[] bytes = Convert.FromBase64String(imgBase64Data);
                    ViewBag.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                    ViewBag.ImageData1 = imgBase64Data;
                    return PartialView("PrintVisitorCard");
                }
            }

            catch
            {
                return null;
            }
        }

        public ActionResult GenerateQRCode(string qrcodetxt)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcodetxt, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Height = 150;
            imgBarCode.Width = 150;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ViewBag.imageBytes = ms.ToArray();

                    string imreBase64Data = Convert.ToBase64String(ms.ToArray());
                    string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                    ViewBag.ImageData = imgDataURL;
                }
            }
            return View();
        }

        public string SendQRCode(int vid, int? uid, string qrcodetxt)
        {
            try
            {
                Visitor user = _visitorRepository.FindById(vid);

                if (user.CompanyId == null)
                {
                    user.Company = "";
                }
                else
                {
                    List<Company> companies = _companyRepository.FindAll(x => x.Id == (int)user.CompanyId).ToList();
                    if (companies.Count == 0)
                    {
                        user.Company = "";
                    }
                    else
                    {
                        foreach (var cc in companies)
                        {
                            user.Company = cc.Name;
                        }
                    }
                }

                string startdate = !String.IsNullOrEmpty(user.StartDateTime.ToString()) ? Convert.ToDateTime(user.StartDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;
                string enddate = !String.IsNullOrEmpty(user.StopDateTime.ToString()) ? Convert.ToDateTime(user.StopDateTime.ToString()).ToString("dd.MM.yyyy HH:mm") : null;

                GenerateQRCode(qrcodetxt);
                using (MemoryStream ms = new MemoryStream())
                {
                    Document pdfDoc = new Document(new iTextSharp.text.Rectangle(242, 156), 55f, 30f, 4f, 0f);

                    MemoryStream output = new MemoryStream();

                    pdfDoc.Open();

                    FontFactory.RegisterDirectories();

                    StringBuilder sb = new StringBuilder();

                    foreach (string fontname in FontFactory.RegisteredFonts)
                    {
                        sb.Append(fontname + "\n");
                    }

                    pdfDoc.Add(new Paragraph("All Fonts:\n" + sb.ToString()));

                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, output);

                    string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
                    int namelnght = user.FirstName.Length + user.LastName.Length;

                    iTextSharp.text.Font font1 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 11);
                    iTextSharp.text.Font font = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10);
                    font.SetColor(32, 32, 32);
                    iTextSharp.text.Font font5 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 11);
                    font5.SetColor(32, 32, 32);
                    iTextSharp.text.Font font6 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 9);
                    font5.SetColor(32, 32, 32);
                    iTextSharp.text.Font font2 = FontFactory.GetFont("arial narrow", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 19);
                    iTextSharp.text.Font font3 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8);
                    font3.SetColor(32, 32, 32);
                    iTextSharp.text.Font font4 = FontFactory.GetFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10);

                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(ViewBag.imageBytes);
                    jpg.ScaleAbsolute(70f, 67f);
                    jpg.BorderColor = BaseColor.BLACK;
                    PdfPCell logocell = new PdfPCell(jpg, true);

                    byte[] file;

                    string imgname = "/Uploads/VisitorsPhoto/" + vid.ToString() + ".png";
                    if (System.IO.File.Exists(Server.MapPath(imgname)))
                    {
                        file = System.IO.File.ReadAllBytes(Server.MapPath(imgname));
                    }
                    else
                    {
                        file = System.IO.File.ReadAllBytes(Server.MapPath("~/Uploads/VisitorsPhoto/no-image.png"));
                    }

                    iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(file);
                    jpg1.ScaleAbsolute(75f, 78f);
                    jpg1.Border = Rectangle.BOX;
                    jpg1.BorderColor = BaseColor.DARK_GRAY;
                    jpg1.BorderWidth = 2f;
                    PdfPCell logocell1 = new PdfPCell(jpg1, true);

                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 165f;
                    PdfPTable table1 = new PdfPTable(1);
                    table1.WidthPercentage = 165f;
                    PdfPTable table2 = new PdfPTable(1);
                    table2.WidthPercentage = 70f;
                    PdfPCell cell;

                    if (user.Company.Length > 40)
                    {
                        cell = new PdfPCell(new Phrase((user.Company).Substring(0, 40) + "...", font5));
                        cell.PaddingTop = 4f;
                    }
                    else if (user.Company.Length < 40)
                    {
                        cell = new PdfPCell(new Phrase(user.Company, font));
                        cell.PaddingTop = 4f;
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(user.Company, font5));
                        cell.PaddingTop = 8f;
                    }
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell);

                    PdfPCell blankCell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    blankCell.Border = PdfPCell.NO_BORDER;
                    table1.AddCell(blankCell);

                    cell = new PdfPCell(jpg1);
                    cell.BorderColor = BaseColor.WHITE;
                    cell.PaddingLeft = 9f;
                    table1.AddCell(cell);

                    cell = new PdfPCell(jpg);
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table2.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ViewResources.SharedStrings.Visitor, font2));
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table2.AddCell(cell);

                    if (namelnght > 15)
                    {
                        cell = new PdfPCell(new Phrase(user.FirstName + "\n" + user.LastName, font4));
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(user.FirstName + "\n" + user.LastName, font1));
                    }

                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.NoWrap = false;
                    table2.AddCell(cell);

                    cell = new PdfPCell(table1);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);

                    cell = new PdfPCell(table2);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ViewResources.SharedStrings.CardsValidFrom + ": " + startdate, font3));
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 9f;
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ViewResources.SharedStrings.CardsValidTo + ": " + enddate, font3));
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 9f;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    pdfDoc.Open();
                    pdfDoc.Add(table);

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    output.Position = 0;

                    EmailViewModel mailobj = ReadServerEmailParameters();
                    if (mailobj.ThisserveremailsparamOK == true)
                    {
                        using (StringWriter sw = new StringWriter())
                        {
                            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                            {
                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                                    mailobj.VisitorMailText = (mailobj.VisitorMailText).Replace(";", "<br/>");
                                    MailMessage mail = new MailMessage(mailobj.ThisServerMailFrom, user.Email, mailobj.ThisServerMailSubject, mailobj.VisitorMailText);
                                    mail.IsBodyHtml = true;
                                    mail.BodyEncoding = Encoding.Default;
                                    Attachment data = new Attachment(output, "VisitorCard.pdf", "application/pdf");
                                    mail.Attachments.Add(data);
                                    SmtpClient SMTPServerClient = new SmtpClient(mailobj.ThisServerSmtpServer, mailobj.ThisServerSmtpPort);
                                    SMTPServerClient.Credentials = new System.Net.NetworkCredential(mailobj.ThisServerSmtpUser, mailobj.ThisServerSmtpPsw);
                                    SMTPServerClient.EnableSsl = true;
                                    SMTPServerClient.Send(mail);

                                    var entity = new VisitorEventEntity(_visitorRepository.FindById(vid));
                                    var message = entity.GetCreateSendMailMessage();

                                    _visitorService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
                                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, vid, UpdateParameter.UserStatusChanged, ControllerStatus.Created, "Active");

                                }
                            }
                        }

                    }
                }
                return "1";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public EmailViewModel ReadServerEmailParameters()
        {
            try
            {
                EmailViewModel obj = new EmailViewModel();
                var emailparameters = (from c in _FSINISettingsRepository.FindAll(c => !c.IsDeleted && c.SoftType == 2) select c).ToList();

                var allparams = 0;
                if (emailparameters.Count == 8)
                {
                    foreach (var emailparameter in emailparameters)
                    {
                        switch (emailparameter.Name)
                        {
                            case "VisitorMailSubject":
                                {
                                    allparams = allparams | 1;
                                    obj.ThisServerMailSubject = emailparameter.Value;
                                    break;
                                }

                            case "VisitorMailText":
                                {
                                    allparams = allparams | 1;
                                    obj.VisitorMailText = emailparameter.Value;
                                    break;
                                }

                            case "MailFrom":
                                {
                                    allparams = allparams | 2;
                                    obj.ThisServerMailFrom = emailparameter.Value;
                                    break;
                                }

                            case "SmtpServer":
                                {
                                    allparams = allparams | 4;
                                    obj.ThisServerSmtpServer = emailparameter.Value;
                                    break;
                                }

                            case "SmtpPort":
                                {
                                    allparams = allparams | 8;
                                    obj.ThisServerSmtpPort = Convert.ToInt32(emailparameter.Value);
                                    break;
                                }

                            case "SmtpUser":
                                {
                                    allparams = allparams | 16;
                                    obj.ThisServerSmtpUser = emailparameter.Value;
                                    if (String.IsNullOrEmpty(obj.ThisServerSmtpUser))
                                        obj.ThisServerSmtpUser = "User";
                                    break;
                                }

                            case "SmtpPsw":
                                {
                                    allparams = allparams | 32;
                                    obj.ThisServerSmtpPsw = emailparameter.Value;
                                    if (String.IsNullOrEmpty(obj.ThisServerSmtpPsw))
                                        obj.ThisServerSmtpPsw = "1234";
                                    break;
                                }
                        }
                    }
                }
                if (allparams == 63)
                    obj.ThisserveremailsparamOK = true;
                return obj;
            }
            catch
            {
                return null;
            }
        }

        #endregion qrcode

        [HttpGet]
        public ActionResult ByCountry(int id)
        {
            var uvm = CreateViewModel<VisitorListViewModel>();
            List<Visitor> users = VisitorByCountry(id);

            users = ApplyUserStatusFilter(users, 1).ToList();
            //users = GetUsersByBuildingInRole(users, _buildingRepository, _userRepository);

            users = users.OrderByDescending(x => x.StopDateTime).ToList();
            foreach (var obj in users)
            {
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
            IEnumerable<VisitorItem> list = new List<VisitorItem>();
            Mapper.CreateMap<Visitor, VisitorItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResultsVisitor";
            uvm.Paginator.Prefix = "Visitor";

            uvm.Visitors = list;
            uvm.FilterCriteria = 1;
            return PartialView("ListUser", uvm);
        }

        private List<Visitor> VisitorByCountry(int id)
        {
            List<int> activeCompanyIds =
                (from c in _companyRepository.FindAll(x => !x.IsDeleted) select c.Id).ToList();
            List<int> locationIds =
                (from l in _locationRepository.FindAll(x => !x.IsDeleted && x.CountryId == id) select l.Id).ToList();
            List<int> buildingIds =
                (from b in _buildingRepository.FindAll(x => !x.IsDeleted && locationIds.Contains(x.LocationId))
                 select b.Id).ToList();
            List<int> companyIds =
                (from b in
                     _companyBuildingObjectRepository.FindAll(x => !x.IsDeleted && buildingIds.Contains(x.BuildingObject.BuildingId))
                 select b.CompanyId).ToList();
            List<Visitor> users = _visitorRepository.FindAll(x => !x.IsDeleted).ToList();
            users = users.Where(
                    x =>
                    !x.IsDeleted && x.Active && x.CompanyId.HasValue && companyIds.Contains((int)x.CompanyId) &&
                    activeCompanyIds.Contains((int)x.CompanyId)).ToList();

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                users = users.Where(x => x.CompanyId.HasValue && x.CompanyId.Value == CurrentUser.Get().CompanyId.Value).ToList();
            }

            return users;
        }

        [HttpGet]
        public ActionResult ByLocation(int id)
        {
            var uvm = CreateViewModel<VisitorListViewModel>();
            List<Visitor> users = UsersByLocation(id);

            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                users = users.Where(x => x.CompanyId.HasValue && x.CompanyId.Value == CurrentUser.Get().CompanyId.Value).ToList();
            }
            users = ApplyUserStatusFilter(users, 1).ToList();
            users = users.OrderByDescending(x => x.StopDateTime).ToList();

            foreach (var obj in users)
            {
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
            IEnumerable<VisitorItem> list = new List<VisitorItem>();
            Mapper.CreateMap<Visitor, VisitorItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResultsVisitor";
            uvm.Paginator.Prefix = "Visitor";

            uvm.Visitors = list;
            uvm.FilterCriteria = 1;
            return PartialView("ListUser", uvm);
        }

        private List<Visitor> UsersByLocation(int id)
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

            List<Visitor> users = _visitorRepository.FindAll(x => !x.IsDeleted).ToList();
            users = users.Where(
                    x =>
                    !x.IsDeleted && x.Active && x.CompanyId.HasValue && companyIds.Contains((int)x.CompanyId) &&
                    activeCompanyIds.Contains((int)x.CompanyId)).ToList();

            return users;
        }

        [HttpGet]
        public ActionResult ByBuilding(int id)
        {
            var uvm = CreateViewModel<VisitorListViewModel>();

            List<Visitor> users = UsersByBuilding(id);

            if (!CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin)
            {
                users = users.Where(us => us.CompanyId != null).ToList();
            }
            if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                users = users.Where(x => x.CompanyId.HasValue && x.CompanyId.Value == CurrentUser.Get().CompanyId.Value).ToList();
            }
            users = ApplyUserStatusFilter(users, 1).ToList();
            users = users.OrderByDescending(x => x.StopDateTime).ToList();

            foreach (var obj in users)
            {
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
            IEnumerable<VisitorItem> list = new List<VisitorItem>();
            Mapper.CreateMap<Visitor, VisitorItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResultsVisitor";
            uvm.Paginator.Prefix = "Visitor";
            uvm.Visitors = list;
            uvm.FilterCriteria = 1;
            return PartialView("ListUser", uvm);
        }

        private List<Visitor> UsersByBuilding(int id)
        {
            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();

            List<int> userid = new List<int>();
            userid = users.FindAll(x => !x.IsDeleted && x.Active
                && x.UserBuildings != null && x.UserBuildings.Any(ubo => !ubo.IsDeleted && ubo.BuildingId == id)).Select(x => x.Id).ToList();

            List<Visitor> visitor = _visitorRepository.FindAll(x => !x.IsDeleted && x.Active && userid.Contains(Convert.ToInt32(x.UserId))).ToList();

            return visitor;
        }

        [HttpGet]
        public ActionResult ByCompany(int id, int buildingId)
        {
            var uvm = CreateViewModel<VisitorListViewModel>();
            List<Visitor> users = UsersByCompany(id, buildingId);
            users = ApplyUserStatusFilter(users, 1).ToList();
            users = users.OrderByDescending(x => x.StopDateTime).ToList();

            foreach (var obj in users)
            {
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
            IEnumerable<VisitorItem> list = new List<VisitorItem>();
            Mapper.CreateMap<Visitor, VisitorItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResultsVisitor";
            uvm.Paginator.Prefix = "Visitor";

            uvm.Visitors = list;
            uvm.FilterCriteria = 1;
            return PartialView("ListUser", uvm);
        }

        private List<Visitor> UsersByCompany(int id, int buildingId)
        {
            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();

            List<int> userid = new List<int>();
            userid = users.FindAll(x => !x.IsDeleted && x.Active
                && x.UserBuildings != null && x.UserBuildings.Any(ubo => !ubo.IsDeleted && ubo.BuildingId == buildingId)).Select(x => x.Id).ToList();

            List<Visitor> visitor = _visitorRepository.FindAll(x => !x.IsDeleted && x.CompanyId == id && userid.Contains(Convert.ToInt32(x.UserId))).ToList();
            return visitor;
        }

        [HttpGet]
        public ActionResult ByFloor(int floorId, int companyId)
        {
            var uvm = CreateViewModel<VisitorListViewModel>();

            List<Visitor> users = UsersByFloor(floorId, companyId);

            users = ApplyUserStatusFilter(users, 1).ToList();
            users = users.OrderByDescending(x => x.StopDateTime).ToList();

            foreach (var obj in users)
            {
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
            IEnumerable<VisitorItem> list = new List<VisitorItem>();
            Mapper.CreateMap<Visitor, VisitorItem>();
            Mapper.Map(users, list);

            uvm.Paginator = SetupPaginator(ref list, 0, 10);
            uvm.Paginator.DivToRefresh = "AreaTabPeopleSearchResultsVisitor";
            uvm.Paginator.Prefix = "Visitor";
            uvm.Visitors = list;
            uvm.FilterCriteria = 1;
            return PartialView("ListUser", uvm);
        }

        private List<Visitor> UsersByFloor(int floorId, int companyId)
        {
            var bo = _companyBuildingObjectRepository.FindById(floorId);

            var user_pririty = _userRepository.FindById(CurrentUser.Get().Id).RolePriority();
            List<User> users = _userRepository.FindAll(x => !x.IsDeleted && user_pririty <= x.RolePriority()).ToList();
            users = users.Where(x => !x.IsDeleted && x.Active && x.UserBuildings != null && x.UserBuildings.Any(ubo => !ubo.IsDeleted && ubo.BuildingObjectId == bo.BuildingObjectId)).
                    ToList();

            List<int> userid = new List<int>();
            userid = users.Select(x => x.Id).ToList();

            List<Visitor> visitor = _visitorRepository.FindAll(x => !x.IsDeleted && userid.Contains(Convert.ToInt32(x.UserId))).ToList();
            return visitor;
        }

        //Get User Id for Visitor
        [HttpGet]
        public ActionResult GetUserID(string fromdatetime)
        {
            DateTime seldate = DateTime.ParseExact(fromdatetime.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

            List<User> users = new List<User>();

            List<int> usr = new List<int>();

            List<int?> usrAccessUnit = new List<int?>();
            List<int> userPermissionGroup = new List<int>();

            IEnumerable<UserItem> filt_users = new List<UserItem>();

            var uvm = CreateViewModel<UserListViewModel>();

            //List<int> usrrolerepo = new List<int>();
            //usrrolerepo = _userrolerepository.FindAll(x => !x.IsDeleted).Select(x => x.UserId).ToList();

            //users = _userRepository.FindAll(x => !x.IsDeleted && usrrolerepo.Contains(x.Id) && x.IsShortTermVisitor.Equals(true)).OrderBy(x => x.FirstName).ToList();
            users = _userRepository.FindAll(x => !x.IsDeleted && x.IsShortTermVisitor.Equals(true)).OrderBy(x => x.FirstName).ToList();

            List<UserPermissionGroup> userPermissionGroups = new List<UserPermissionGroup>();
            List<UsersAccessUnit> userAccessunit = new List<UsersAccessUnit>();
            List<int?> userAccessUnit = new List<int?>();
            List<int?> visitoruserid = new List<int?>();
            userAccessUnit = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.ValidTo < DateTime.Now  && x.Active == true && x.Free == false).Select(x => x.UserId).ToList();

            List<int> usrperid = new List<int>();
            usrperid = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted).Select(x => x.UserId).ToList();

            //shortTermVisitor=true
            var companyId = CurrentUser.Get().CompanyId.HasValue ? CurrentUser.Get().CompanyId.Value : -1;
            if (companyId == -1)
            {
                users = users.FindAll(x => !x.IsDeleted && userAccessUnit.Contains(x.Id) && x.IsShortTermVisitor.Equals(true)).OrderBy(x => x.FirstName).ToList();
            }
            else
            {
                users = users.FindAll(x => !x.IsDeleted && userAccessUnit.Contains(x.Id) && x.CompanyId == companyId && x.IsShortTermVisitor.Equals(true)).OrderBy(x => x.FirstName).ToList();
            }

            try
            {
                if (users != null)
                {
                    Mapper.Map(users, uvm.Users);
                    Session["TypedListModel"] = uvm;
                }
            }
            catch (Exception) { }

            userPermissionGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted).OrderBy(x => x.UserId).ToList();

            foreach (var uu in uvm.Users.ToList())
            {

                foreach (var up in userPermissionGroups)
                {
                    if (uu.Id == up.UserId)
                    {
                        uu.UserPermissionGroupName = up.Name;
                    }
                }
            }
            Session["TypedListModel"] = uvm;
            return PartialView("Users", Session["TypedListModel"]);
        }


        #region Delete User

        [HttpPost]
        public ActionResult Delete(int vid)
        {

            try
            {
                _visitorService.DeleteVisitor(vid, HostName);
            }
            catch (Exception e)
            {
                Logger.Write("Error deleting User Id=" + vid, e);
            }


            return null;
        }

        #endregion

        public ActionResult VisitorCardsTabContent()
        {
            var hmv = CreateViewModel<CardsTabContentViewModel>();
            hmv.CanAddCard = true;
            return PartialView(hmv);
        }

        public ActionResult VisitorCardsList(int vid, int filter)
        {
            int? id = _visitorRepository.FindById(vid).UserId;
            DateTime? stopdatetime = _visitorRepository.FindById(vid).StopDateTime;
            var uaulvm = CreateViewModel<UserAccessUnitListViewModel>();

            //var cards = _usersAccessUnitRepository.FindAll(x => x.Id == _visitorRepository.FindById(vid).ParentVisitorsId).ToList();
            //var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == id && x.TypeId == 7 && x.Active == true).ToList();
            var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == id && x.Active == true).ToList();
            Mapper.Map(cards, uaulvm.Cards);

            foreach (var obj in uaulvm.Cards)
            {
                DateTime? validfrom = _visitorRepository.FindById(vid).StartDateTime;
                DateTime? validto = _visitorRepository.FindById(vid).StopDateTime;
                obj.ValidFromStr = validfrom == null ? null : Convert.ToDateTime(validfrom).ToString("dd.MM.yyyy HH:mm");
                obj.ValidToStr = validfrom == null ? null : Convert.ToDateTime(validto).ToString("dd.MM.yyyy HH:mm");
            }

            uaulvm.FilterCriteria = filter;
            return PartialView(uaulvm);
        }

        public ActionResult VisitorRightsTabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        public JsonResult GetPermName(int? id)
        {
            if (id == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
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
                string name = upg.FirstOrDefault().Name;
                result.Add(name);
                result.Add(upg.FirstOrDefault().ParentUserPermissionGroupId.ToString());
                string fullname = upg.FirstOrDefault().User.FirstName + " " + upg.FirstOrDefault().User.LastName;
                result.Add(fullname);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetPermissionGroups(int? id)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<option value=" + '"' + '"' + ">" + SharedStrings.PermissionsSelectPermissionGroupOption + "</option>");

            IEnumerable<UserPermissionGroup> permGroups;

            if (!CurrentUser.Get().IsSuperAdmin && !CurrentUser.Get().IsBuildingAdmin)
            {//Tut

                string OwnPermissionGroup = (from c in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null) select c.Name).First();
                var SamePermissionGroupUsers = (from c in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId != null && x.Name == OwnPermissionGroup) select c.UserId).ToArray();
                permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null && SamePermissionGroupUsers.Contains(x.UserId) || !x.IsDeleted && x.UserId == CurrentUser.Get().Id).OrderBy(x => x.Name);
            }
            else
            {
                permGroups = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId == null).OrderBy(x => x.Name);
            }

            var curPermGroup = id.HasValue ? _userPermissionGroupRepository.FindAll().Where(upg => !upg.IsDeleted && upg.UserId == id && upg.PermissionIsActive).FirstOrDefault() : null;

            foreach (var pg in permGroups)
            {
                result.Append(string.Format("<option value=\"{0}\">{1}</option>", pg.ParentUserPermissionGroupId.HasValue ? pg.ParentUserPermissionGroupId : pg.Id, pg.Name));
            }
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditCard(int id)
        {
            var uauevm = CreateViewModel<UserAccessUnitEditViewModel>();

            Mapper.Map(_usersAccessUnitRepository.FindById(id), uauevm.Card);
            uauevm.Card.TypeId = uauevm.Card.TypeId == null ? 7 : uauevm.Card.TypeId;
            uauevm.Card.CardTypes = new SelectList(_cardTypeRepository.FindAll().Where(x => !x.IsDeleted).OrderBy(x => x.Name.ToLower()), "Id", "Name", uauevm.Card.TypeId);

            SelectListItem item = new SelectListItem();
            item.Value = "";
            item.Text = "--Select--";
            SelectList complist = new SelectList(_companyRepository.FindAll().Where(x => !x.IsDeleted && x.Active), "Id", "Name", uauevm.Card.CompanyId);

            uauevm.Card.Companies = AddFirstItem(complist, item);//new SelectList(_companyRepository.FindAll().Where(x=>!x.IsDeleted && x.Active), "Id", "Name", uauevm.Card.CompanyId);

            var building_ids = GetCardBuildings(uauevm.Card.CompanyId);
            if (!building_ids.Contains(uauevm.Card.BuildingId))
            {
                uauevm.Card.BuildingId = building_ids.FirstOrDefault();
            }

            Mapper.Map(_buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id)),
                       uauevm.Card.Buildings);

            return PartialView(uauevm);
        }

        private List<int> GetCardBuildings(int? companyId)
        {
            var building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);

            if (!CurrentUser.Get().IsSuperAdmin)
            {
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId == null) { return building_ids; }
                var user = _userRepository.FindById(CurrentUser.Get().Id);
                var user_building_ids =
                    from ub in
                        user.UserBuildings.Where(x => !x.IsDeleted && building_ids.Contains(x.BuildingId))
                    select ub.BuildingId;

                building_ids = user_building_ids.ToList();
            }

            return building_ids;
        }

        public static SelectList AddFirstItem(SelectList origList, SelectListItem firstItem)
        {
            List<SelectListItem> newList = origList.ToList();
            newList.Insert(0, firstItem);

            var selectedItem = newList.FirstOrDefault(item => item.Selected);
            var selectedItemValue = String.Empty;
            if (selectedItem != null)
            {
                selectedItemValue = selectedItem.Value;
            }
            else
            {
                selectedItemValue = firstItem.Value;
            }

            return new SelectList(newList, "Value", "Text", selectedItemValue);
        }

        [HttpPost]
        public Boolean EditCardDet(UserAccessUnitItem card)
        {
            DateTime? validFrom = null;
            DateTime? validTo = null;
            validFrom = DateTime.ParseExact(card.ValidFromStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            validTo = DateTime.ParseExact(card.ValidToStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

            bool done = _cardService.EditCard(card.Id, card.UserId, card.TypeId, card.CompanyId, card.BuildingId, card.Serial, card.Dk, card.Code, card.Free, validFrom, validTo, card.Comment, card.IsMainUnit);
            return done;
        }

        public Boolean GiveCardBack(int id, string returndate, bool cardneedreturn,int? userID)
        {
            DateTime rdate = DateTime.ParseExact(returndate.ToString().Trim(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            return _visitorService.GiveCardBack(id, rdate, cardneedreturn,Convert.ToInt32(userID));
        }

        #region User Permission Tree

        public ActionResult GetUserPermissionTree(int userId, int? id)
        {
            var ctvm = CreateViewModel<PermissionTreeViewModel>();
            var role_buildingIds = GetUserBuildings(_buildingRepository, _userRepository, CurrentUser.Get().Id);

            if (id.HasValue)
            {
                var upg = _userPermissionGroupRepository.FindById(id.Value);
                var cur_upg = _userPermissionGroupRepository.FindById(id.Value);

                ctvm.IsOriginal = false;
                ctvm.IsCurrentUserAssignedGroup = upg.PermissionIsActive;
                ctvm.IsCurrentUserAssignedGroup = false;
                List<int> buildingObjectsIds = new List<int>();

                buildingObjectsIds = _userRepository.IsCompanyManager(userId) ? (from cbo in upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted /*&& x.Active*/) select cbo.BuildingObjectId).ToList() : (from cbo in upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted) select cbo.BuildingObjectId).ToList();
                var buildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && /*role_buildingIds.Contains(x.BuildingId) &&*/ buildingObjectsIds.Contains(x.Id));

                if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager)
                {
                    var activePg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => x.PermissionIsActive && !x.IsDeleted).FirstOrDefault();
                    var userActiveBuildings = (from cbo in activePg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.Active) select cbo.BuildingObjectId).ToList();
                    buildingObjects =
                        _buildingObjectRepository.FindAll(x => !x.IsDeleted && role_buildingIds.Contains(x.BuildingId) || userActiveBuildings.Contains(x.Id));

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

        #region Capture image

        [System.Web.Services.WebMethod()]
        public void Capture(string reqimg)
        {
            try
            {
                string imageName = Convert.ToString(Session["VisitorId"]);
                string imagePath = string.Format("~/Uploads/VisitorsPhoto/{0}.png", imageName);

                using (FileStream fs = new FileStream(Server.MapPath(imagePath), FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(reqimg);
                        bw.Write(data);
                        bw.Close();
                    }
                }
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();
            }
            catch
            {
            }
        }

        [HttpPost]
        public ContentResult GetCapture()
        {
            string url = Session["CapturedImage"].ToString();
            Session["CapturedImage"] = null;
            return Content(url);
        }
        private static byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
        #endregion

        #region Upload photo

        [HttpGet]
        public ActionResult UploadVisitorImage(int VisitorId)
        {
            return PartialView(VisitorId);
        }

        [HttpPost]
        public string SaveVisitorImage(int Id)
        {
            try
            {
                string fileName = String.Empty;
                string newName = string.Empty;
                int reqid = 0;

                foreach (string inputTagName in Request.Files)
                {
                    HttpPostedFileBase image = Request.Files[inputTagName];

                    if (image.ContentLength > 0)
                    {
                        try
                        {
                            string imageName = Convert.ToString(Id) + ".png";
                            fileName = Path.Combine(HttpContext.Server.MapPath("../Uploads/VisitorsPhoto"), imageName);

                            if (System.IO.File.Exists(fileName))
                            {
                                try
                                {
                                    System.IO.File.Delete(fileName);
                                }
                                catch
                                {
                                }
                            }
                            image.SaveAs(fileName);
                            return "1";
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                return Convert.ToString(reqid);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
    }
}

