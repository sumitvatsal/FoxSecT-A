using System;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.Common.Enums;
using FoxSec.ServiceLayer.Services;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.ServiceResults;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.DomainModel;
using DevExpress.Web.Mvc;

namespace FoxSec.Web.Controllers
{
    public class CardController : PaginatorControllerBase<UserAccessUnitListItem>
    {
        private readonly IUserAccessUnitTypeService _cardTypeService;
        private readonly IUsersAccessUnitService _cardService;
        private readonly IUserAccessUnitTypeRepository _cardTypeRepository;
        private readonly IUsersAccessUnitRepository _usersAccessUnitRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserDepartmentRepository _userDepartmentRepository;
        private readonly IUserService _userService;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserBuildingRepository _userBuildingRepository;
        private string errormessage = "";
       
        public CardController(IUserAccessUnitTypeService cardTypeService,
                                IUsersAccessUnitService cardService,
                                ICompanyRepository companyRepository,
                                IUsersAccessUnitRepository usersAccessUnitRepository,
                                IClassificatorValueRepository classificatorValueRepository,
                                ICurrentUser currentUser,
                                IUserAccessUnitTypeRepository cardTypeRepository,
                                IUserDepartmentRepository userDepartmentRepository,
                                IUserRepository userRepository,
                                IUserService userService,
                                IBuildingRepository buildingRepository,
                                IUserBuildingRepository userBuildingRepository,
                                IRoleRepository roleRepository,
                                ILogger logger
                               ) : base(currentUser, logger)
        {
            _cardTypeService = cardTypeService;
            _cardService = cardService;
            _companyRepository = companyRepository;
            _cardTypeRepository = cardTypeRepository;
            _classificatorValueRepository = classificatorValueRepository;
            _usersAccessUnitRepository = usersAccessUnitRepository;
            _userDepartmentRepository = userDepartmentRepository;
            _userRepository = userRepository;
            _userService = userService;
            _buildingRepository = buildingRepository;
            _userBuildingRepository = userBuildingRepository;
            _roleRepository = roleRepository;
            
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        FoxSecDBContext db = new FoxSecDBContext();
        #region Card Types

        [HttpGet]
        public ActionResult TypeList()
        {
            var ctlvm = CreateViewModel<UserAccessUnitTypeListViewModel>();
            Mapper.Map(_cardTypeRepository.FindAll().OrderBy(x => x.Name.ToLower()), ctlvm.CardTypes);
            return PartialView(ctlvm);
        }

        [HttpGet]
        public ActionResult NewType()
        {
            var ctem = CreateViewModel<UserAccessUnitTypeEditViewModel>();
            return PartialView(ctem);
        }

        [HttpPost]
        public ActionResult CreateNewType(UserAccessUnitTypeEditViewModel item)
        {
            var ctevm = CreateViewModel<UserAccessUnitTypeEditViewModel>();
            ctevm.CardType = item.CardType;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _cardTypeService.CreateCardType(item.CardType.Name, item.CardType.IsCardCode, item.CardType.IsSerDK, item.CardType.Description);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = this.RenderPartialView("NewType", ctevm)
            });
        }

        [HttpGet]
        public ActionResult EditType(int id)
        {
            var ctevm = CreateViewModel<UserAccessUnitTypeEditViewModel>();
            Mapper.Map(_cardTypeRepository.FindById(id), ctevm.CardType);
            return PartialView(ctevm);
        }

        [HttpPost]
        public ActionResult UpdateType(UserAccessUnitTypeEditViewModel item)
        {
            var ctevm = CreateViewModel<UserAccessUnitTypeEditViewModel>();
            ctevm.CardType = item.CardType;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _cardTypeService.EditCardType((int)item.CardType.Id, item.CardType.Name, item.CardType.IsCardCode, item.CardType.IsSerDK, item.CardType.Description);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? string.Format("{0} \"{1}\"...", ViewResources.SharedStrings.CardTypeSaving, item.CardType.Name) : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = this.RenderPartialView("EditType", ctevm)
            });
        }

        [HttpPost]
        public ActionResult DeleteType(int id)
        {
            _cardTypeService.DeleteCardType((int)id);
            return null;
        }

        # endregion

        #region Card List / Search

        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            hmv.CardTypes = _cardTypeRepository.FindAll().OrderBy(x => x.Name);
            hmv.ClassificatorValues = _classificatorValueRepository.FindAll().Where(y => y.ClassificatorId == 5 || y.ClassificatorId == 6).OrderBy(x => x.Value.ToLower());
            return PartialView(hmv);
        }

        public ActionResult TypeTabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        public ActionResult Search(Int32? reasonId, string cardSer, string cardDk, string cardNo, string cardName, string company, string building, string validation, int filter, Int32 type, int? nav_page, int? rows, int? sort_field, int? sort_direction, string flagc)
        {
            //int companyid = 0;

            if (nav_page < 0)
            {
                nav_page = 0;
            }
            if (CurrentUser.Get().CompanyId != null && !CurrentUser.Get().IsSuperAdmin)
            {
                company = _companyRepository.FindAll().Where(cc => cc.Id == (CurrentUser.Get().CompanyId)).First().Name;
                //companyid = _companyRepository.FindAll().Where(cc => cc.Id == (CurrentUser.Get().CompanyId)).First().Id;
            }
            var uaulvm = CreateViewModel<UserAccessUnitListViewModel>();
            var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted);
            foreach (var obj in cards)
            {
                if (obj.Free == true)
                {
                    obj.ClassificatorValueId = null;
                }
            }
            var building_ids = GetRoleBuildings(_buildingRepository, _roleRepository);
            var user_building_ids = GetUserBuildings(_buildingRepository, _userRepository);
            cards = cards.Where(x => building_ids.Contains(x.BuildingId) && user_building_ids.Contains(x.BuildingId));

            if (reasonId.HasValue && reasonId.Value > 0)
            {
                cards = cards.Where(x => x.ClassificatorValueId != null && x.ClassificatorValueId.Equals(reasonId.Value)).ToList();
            }
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
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    var compids = _companyRepository.FindAll().Where(x => x.Name.ToLower().Contains(company.Trim().ToLower())).Select(x => x.Id);
                    var compall = _companyRepository.FindAll().Where(x => (compids.Contains(x.Id) || compids.Contains(Convert.ToInt32(x.ParentId == null ? 0 : x.ParentId)))).Select(x => x.Id);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("select ParentCompanieId from CompanieSubCompanies where IsDeleted=0 and CompanyId='" + CurrentUser.Get().CompanyId + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    List<int> subcompanyIds = new List<int>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        subcompanyIds.Add(Convert.ToInt32(dr["ParentCompanieId"]));
                    }
                    compall = compall.Concat(subcompanyIds).ToList();

                    //cards = cards.Where(x => x.CompanyId.HasValue && x.Company.Name.ToLower().Contains(company.Trim().ToLower())).ToList();
                    cards = cards.Where(x => compall.Contains(Convert.ToInt32(x.CompanyId == null ? 0 : x.CompanyId))).ToList();
                    //cards = cards.Where(x => x.CompanyId.HasValue && (x.CompanyId == CurrentUser.Get().CompanyId || x.Company.ParentId == CurrentUser.Get().CompanyId)).ToList();
                }
                else
                {
                    cards = cards.Where(x => x.CompanyId.HasValue && x.Company.Name.ToLower().Contains(company.Trim().ToLower())).ToList();
                }
            }
            else
            {
                if (CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
                {
                    //cards = cards.Where(x => x.CompanyId.HasValue && (x.CompanyId == CurrentUser.Get().CompanyId || x.Company.ParentId == CurrentUser.Get().CompanyId)).ToList();
                    var compids = _companyRepository.FindAll().Where(x => x.Name.ToLower().Contains(company.Trim().ToLower())).Select(x => x.Id);
                    var compall = _companyRepository.FindAll().Where(x => (compids.Contains(x.Id) || compids.Contains(Convert.ToInt32(x.ParentId == null ? 0 : x.ParentId)))).Select(x => x.Id);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("select ParentCompanieId from CompanieSubCompanies where IsDeleted=0 and CompanyId='" + CurrentUser.Get().CompanyId + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    List<int> subcompanyIds = new List<int>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        subcompanyIds.Add(Convert.ToInt32(dr["ParentCompanieId"]));
                    }
                    compall = compall.Concat(subcompanyIds).ToList();

                    //cards = cards.Where(x => x.CompanyId.HasValue && x.Company.Name.ToLower().Contains(company.Trim().ToLower())).ToList();
                    cards = cards.Where(x => compall.Contains(Convert.ToInt32(x.CompanyId == null ? 0 : x.CompanyId))).ToList();
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
            if (type != 0)
            {
                // type card 7
                cards = cards.Where(x => x.TypeId.HasValue && x.TypeId.Value.Equals(type));
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

            uaulvm.Paginator = SetupPaginator(ref cards_list, nav_page, rows);
            uaulvm.Paginator.DivToRefresh = "CardsList";
            uaulvm.Paginator.Prefix = "Card";

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

            return PartialView("List", uaulvm);
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

        #endregion

        #region Create / Edit Card

        [HttpGet]
        public ActionResult Create(int? cardId)
        {
            var uauevm = CreateViewModel<UserAccessUnitEditViewModel>();
            IEnumerable<Company> companies = _companyRepository.FindAll().Where(cc => !cc.IsDeleted && cc.Active).OrderBy(x => x.Name.ToLower());
            var user_buildings = GetUserBuildings(_buildingRepository, _userRepository);
            var company_ids =
                    from c in
                        companies.Where(
                            x =>
                            x.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && user_buildings.Contains(cbo.BuildingObject.BuildingId)))
                    select c.Id;

            if (!CurrentUser.Get().IsCompanyManager)
            {
                companies =
                    companies.Where(x => company_ids.Contains(x.Id) || (x.ParentId != null && company_ids.Contains(x.ParentId.Value)));
            }

            if (cardId != null)
            {
                var card = _usersAccessUnitRepository.FindById(cardId.Value);
                Mapper.Map(card, uauevm.Card);
                if (card.CompanyId.HasValue && !company_ids.Contains(card.CompanyId.Value))
                {
                    card.CompanyId = company_ids.FirstOrDefault();
                }
                uauevm.Card.CardTypes = new SelectList(_cardTypeRepository.FindAll().OrderBy(ct => ct.Name.ToLower()), "Id", "Name", uauevm.Card.TypeId);
                uauevm.Card.Companies = new SelectList(companies, "Id", "Name", uauevm.Card.CompanyId);
                var building_ids = GetCardBuildings(uauevm.Card.CompanyId);
                Mapper.Map(_buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id)),
                           uauevm.Card.Buildings);
            }
            else
            {
                uauevm.Card.CardTypes = new SelectList(_cardTypeRepository.FindAll().OrderBy(x => x.Name), "Id", "Name");
                uauevm.Card.Companies = new SelectList(companies, "Id",
                                                       "Name");
                var company_id = int.Parse(uauevm.Card.Companies.First().Value);
                if (CurrentUser.Get().IsCompanyManager)
                {
                    var us_comp = _userRepository.FindById(CurrentUser.Get().Id).Company;
                    company_id = us_comp.ParentId == null ? us_comp.Id : us_comp.ParentId.Value;
                }
                var building_ids = GetCardBuildings(company_id);
                Mapper.Map(_buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id)).OrderBy(b => b.Name),
                           uauevm.Card.Buildings);
            }
            uauevm.isCompanyManager = CurrentUser.Get().RoleTypeId == (int)FixedRoleType.CompanyManager;
            return PartialView(uauevm);
        }

        [HttpPost]
        public JsonResult CreateCard(UserAccessUnitItem card)
        {
            int? tc = 0;
            int id = 0;
            int tc1 = 0;
            DateTime? validto = null;

            IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue("Users");
            foreach (var obj in cv)
            {
                tc = obj.Legal;
                id = obj.Id;
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
                tc1 = _userRepository.FindAll(x => !x.IsDeleted && x.Active == true).ToList().Count();
                int remaining = Convert.ToInt32(tc) - tc1;
                remaining = remaining < 0 ? 0 : remaining;
                if (remaining > 0 && validto > DateTime.Now)
                {
                    int? companyId = CurrentUser.Get().CompanyId;
                    var uauevm = CreateViewModel<UserAccessUnitEditViewModel>();
                    uauevm.Card = card;
                    uauevm.Card.CardTypes = new SelectList(_cardTypeRepository.FindAll().OrderBy(x => x.Name.ToLower()), "Id", "Name", uauevm.Card.TypeId);
                    uauevm.Card.Companies = new SelectList(_companyRepository.FindAll().Where(cc => !cc.IsDeleted && cc.Active), "Id", "Name", uauevm.Card.CompanyId);
                    var building_ids = GetCardBuildings(uauevm.Card.CompanyId);
                    Mapper.Map(_buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id)),
                               uauevm.Card.Buildings);

                    var err = string.Empty;
                    ValidateCardSerial(card);

                    if (ModelState.IsValid)
                    {
                        var valid_from = DateTime.ParseExact(card.ValidFromStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        var valid_to = DateTime.ParseExact(card.ValidToStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        if (!companyId.HasValue)
                        {
                            companyId = card.CompanyId;
                        }
                        var host = Request.UserHostAddress;

                        UserCreateResult result = _userService.CreateUser(card.FirstName, card.LastName,
                                                                          string.Format("{0}.{1}", card.FirstName.Trim(), card.LastName.Trim()), "foxsec", string.Empty,
                                                                          string.Empty, card.PersonalCode, null, null,
                                                                          companyId, null, null, null, host, null, 0);

                        if (result.ErrorCode.Equals(UserServiceErrorCode.LoginAlreadyExists))
                        {
                            ModelState.AddModelError("LastName",
                                string.Format("Login name '{0}' already exists!", string.Format("{0}.{1}", card.FirstName.Trim(), card.LastName.Trim())));
                        }
                        if (result.Id == 0)
                        {
                            err = "Error creating user!";
                            ModelState.AddModelError("", err);
                        }

                        if (ModelState.IsValid)
                        {
                            _cardService.CreateCard(result.Id, card.TypeId, companyId, card.BuildingId, card.Serial, card.Dk, card.Code, false, valid_from,
                                                    valid_to, card.IsMainUnit);
                        }
                        return Json(new
                        {
                            IsSucceed = ModelState.IsValid,
                            Msg = ModelState.IsValid ? "Creating user" : err,
                            viewData = this.RenderPartialView("Create", uauevm)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            IsSucceed = ModelState.IsValid,
                            Msg = errormessage,
                        });
                    }

                }
                else
                {
                    return Json(new
                    {
                        IsSucceed = false,
                        Msg = "licence error",
                        Count = Convert.ToInt32(tc),
                    });
                }
            }
        }

        private void ValidateCardSerial(UserAccessUnitItem card)
        {
            bool cardqq = _usersAccessUnitRepository.FindAll(x => x.Dk == card.Dk && x.Serial == card.Serial && !x.IsDeleted && !x.Free).Any();//illiv67


            UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindAll(x => x.Dk == card.Dk && x.Serial == card.Serial && !x.IsDeleted && !x.Active && x.Free).FirstOrDefault();
            if (usersAccessUnit!= null && usersAccessUnit.UserId.HasValue && usersAccessUnit.UserId != card.UserId)
            {
                var difference = DateTime.Now - usersAccessUnit.Closed.Value;
                if (difference.Days < 7)
                {
                    ModelState.AddModelError("Serial", "Cannot give free card to new user before 7 days.The card is available from " +usersAccessUnit.Closed.Value.AddDays(7));
                    errormessage = "Cannot give free card to new user before 7 days.The card is available from " +usersAccessUnit.Closed.Value.AddDays(7);
                    return;
                }
            }

            if (cardqq && card.Dk != null && card.Serial != null)
            {
                var usrdet = _usersAccessUnitRepository.FindAll(x => x.Dk == card.Dk && x.Serial == card.Serial && !x.IsDeleted && !x.Free).FirstOrDefault();
                string firstname = "";
                string lastname = "";
                int usrid = 0;
                if (usrdet != null)
                {
                    usrid = Convert.ToInt32(usrdet.UserId);
                    firstname = _userRepository.FindById(usrid).FirstName;
                    lastname = _userRepository.FindById(usrid).LastName;
                }
                ModelState.AddModelError("Serial", "Card alredy in use! Owner name: " + firstname + " " + lastname);
                errormessage = "Card alredy in use! Owner name: " + firstname + " " + lastname;
                return;
            }
            bool cardq = _usersAccessUnitRepository.FindAll(x => x.Code == card.Code && !x.IsDeleted && !x.Free).Any();//illiv67
            if (cardq && card.Code != null)
            {
                var usrdet = _usersAccessUnitRepository.FindAll(x => x.Code == card.Code && !x.IsDeleted && !x.Free).FirstOrDefault();
                string firstname = "";
                string lastname = "";
                int usrid = 0;
                if (usrdet != null)
                {
                    usrid = Convert.ToInt32(usrdet.UserId);
                    firstname = _userRepository.FindById(usrid).FirstName;
                    lastname = _userRepository.FindById(usrid).LastName;
                }

                ModelState.AddModelError("Code", "Card alredy in use! Owner name: " + firstname + " " + lastname);
                errormessage = "Card alredy in use! Owner name: " + firstname + " " + lastname;
                return;
            }


            if (string.IsNullOrWhiteSpace(card.Serial) && string.IsNullOrWhiteSpace(card.Dk) && string.IsNullOrWhiteSpace(card.Code))
            {
                ModelState.AddModelError("Serial", "Serial should be entered!");
                ModelState.AddModelError("Dk", "Dk should be entered!");
                ModelState.AddModelError("Code", "Code Should be entered!");
                errormessage = "Serial and DK or Code one should be entered!";
                return;
            }

            if (!string.IsNullOrWhiteSpace(card.Serial) && !string.IsNullOrWhiteSpace(card.Code))
            {
                ModelState.AddModelError("Code", "Only Code or serial can be entered!");
                errormessage = "Only Code or serial can be entered!";
                return;
            }

            if ((!string.IsNullOrWhiteSpace(card.Serial) || !string.IsNullOrWhiteSpace(card.Dk)) && string.IsNullOrWhiteSpace(card.Code))
            {
                if (string.IsNullOrWhiteSpace(card.Serial) || string.IsNullOrWhiteSpace(card.Dk))
                {
                    ModelState.AddModelError("Serial", "Serial and DK must be entered!");
                    errormessage = "Serial and DK must be entered!";
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(card.Serial) && card.Serial.Length != 3)
            {
                ModelState.AddModelError("Serial", "Serial should contain 3 symbols!");
                errormessage = "Serial should contain 3 symbols!";
            }

            if (!string.IsNullOrWhiteSpace(card.Dk) && card.Dk.Length != 5)
            {
                ModelState.AddModelError("Dk", "DK should contain 5 symbols!");
                errormessage = "DK should contain 5 symbols!";
            }

            if (!string.IsNullOrWhiteSpace(card.Serial) && string.IsNullOrWhiteSpace(card.Dk))
            {
                ModelState.AddModelError("Dk", "Dk should be entered!");
                errormessage = "Dk should be entered!";
            }

            if (!string.IsNullOrWhiteSpace(card.Dk) && string.IsNullOrWhiteSpace(card.Serial))
            {
                ModelState.AddModelError("Serial", "Serial should be entered!");
                errormessage = "Serial should be entered!";
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
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

        public JsonResult GetBuildingsByCompany(int companyId)
        {
            StringBuilder result = new StringBuilder();

            var building_ids = GetCardBuildings(companyId);

            var buildings = _buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id));

            foreach (var b in buildings)
            {
                result.Append(string.Format("<option value=\"{0}\" >{1}</option>", b.Id, b.Name));
            }
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public string EditCard(UserAccessUnitItem card)
        {
            try
            {
                DateTime? validFrom = null;
                DateTime? validTo = null;

                //if (card.UserId.HasValue)
                //{
                //    User user = _userRepository.FindById(card.UserId.Value);
                //    validFrom = user.RegistredStartDate;
                //    validTo = user.RegistredEndDate;
                //}
                //else
                //{
                if (!String.IsNullOrEmpty(card.ValidFromStr))
                {
                    bool validFromParsedDate = DateTime.TryParseExact(card.ValidFromStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime result);
                    if (validFromParsedDate)
                    {
                        validFrom = result;
                    }
                    else
                    {
                        return "Valid from date error";
                    }
                }
                if (!String.IsNullOrEmpty(card.ValidToStr))
                {
                    bool validToParsedValue = DateTime.TryParseExact(card.ValidToStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime result);
                    if (validToParsedValue)
                    {
                        validTo = result;
                    }
                    else
                    {
                        return "Valid to date error";
                    }
                }
                //}

                bool done = _cardService.EditCard(card.Id, card.UserId, card.TypeId, card.CompanyId, card.BuildingId, card.Serial, card.Dk, card.Code, card.Free, validFrom, validTo, card.Comment, card.IsMainUnit);
                if (done == false)
                {
                    if (string.IsNullOrEmpty(card.Code))
                    {
                        var usrdet = _usersAccessUnitRepository.FindAll(x => x.Dk == card.Dk && x.Serial == card.Serial && !x.IsDeleted && !x.Free).FirstOrDefault();

                        string firstname = "";
                        string lastname = "";
                        string fullname = "";
                        int usrid = 0;
                        if (usrdet != null)
                        {
                            usrid = Convert.ToInt32(usrdet.UserId);
                            firstname = _userRepository.FindById(usrid).FirstName;
                            lastname = _userRepository.FindById(usrid).LastName;
                        }
                        fullname = firstname + " " + lastname;
                        return fullname;
                    }
                    else
                    {
                        var usrdet = _usersAccessUnitRepository.FindAll(x => x.Code == card.Code && !x.IsDeleted && !x.Free).FirstOrDefault();
                        string firstname = "";
                        string lastname = "";
                        string fullname = "";
                        int usrid = 0;
                        if (usrdet != null)
                        {
                            usrid = Convert.ToInt32(usrdet.UserId);
                            firstname = _userRepository.FindById(usrid).FirstName;
                            lastname = _userRepository.FindById(usrid).LastName;
                        }
                        fullname = firstname + " " + lastname;
                        return fullname;
                    }

                }
                else
                {
                    return "True";
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult MoveToCompany()
        {
            var mtcvm = CreateViewModel<MoveToCompanyViewModel>();

            IEnumerable<Company> companies = _companyRepository.FindAll(x => !x.IsDeleted && x.Active).OrderBy(x => x.Name.ToLower());
            if (CurrentUser.Get().IsBuildingAdmin)
            {
                var user_buildings = _userBuildingRepository.FindByUserId(CurrentUser.Get().Id);
                var buildIds = user_buildings.Select(ub => ub.BuildingId).ToList();
                companies = companies.Where(x => x.CompanyBuildingObjects.Any(y => !y.IsDeleted && buildIds.Contains(y.BuildingObject.BuildingId)));
            }
            mtcvm.Companies = new SelectList(companies, "Id", "Name", "");
            return PartialView(mtcvm);
        }
        public bool CardIsBack(int id)
        {
            return _cardService.CardIsBack(id);
        }
        public ActionResult DoMoveToCompany(int companyId, List<int> cardIds)
        {
            var err_msg = string.Empty;
            var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && cardIds.Contains(x.Id));

            foreach (var usersAccessUnit in cards)
            {
                var building_ids = GetCardBuildings(companyId);
                if (!building_ids.Contains(usersAccessUnit.BuildingId))
                {
                    ModelState.AddModelError("", err_msg);
                    err_msg = ViewResources.SharedStrings.CardsCardsNotMovedError;
                }
            }
            if (string.IsNullOrEmpty(err_msg))
            {
                foreach (var card in cards)
                {
                    _cardService.EditCard(card.Id, card.UserId, card.TypeId, companyId, card.BuildingId, card.Serial, card.Dk,
                                          card.Code, card.Free, null, null, card.Comment, card.IsMainUnit);
                }
            }

            return Json(new
            {
                IsSucceed = string.IsNullOrWhiteSpace(err_msg),
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.CardsMoved : err_msg
            });
        }

        #endregion

        #region User Cards

        public ActionResult NewUserCard(int userId)
        {
            var uauevm = CreateViewModel<UserAccessUnitEditViewModel>();
            uauevm.Card.CardTypes = new SelectList(_cardTypeRepository.FindAll(x => !x.IsDeleted).OrderBy(x => x.Name.ToLower()), "Id", "Name");
            uauevm.Card.TypeId = 7;
            uauevm.Card.UserId = userId;
            var user = _userRepository.FindById(userId);
            uauevm.Card.FirstName = user.FirstName;
            uauevm.Card.LastName = user.LastName;
            uauevm.Card.PersonalCode = user.PersonalCode;
            uauevm.Card.ValidFromStr = DateTime.Now.ToString("dd.MM.yyyy");
            uauevm.Card.ValidToStr = DateTime.Now.AddYears(2).ToString("dd.MM.yyyy");
            //uauevm.Card.ValidFromStr = DateTime.Now().ParseExact(uauevm.Card.ValidFromStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            //uauevm.Card.ValidFrom = DateTime.ParseExact(uauevm.Card.ValidFromStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var building_ids = GetCardBuildings(user.CompanyId);
            var user_building_ids = GetUserBuildings(_buildingRepository, _userRepository, userId);
            Mapper.Map(_buildingRepository.FindAll().Where(x => building_ids.Contains(x.Id) && user_building_ids.Contains(x.Id)), uauevm.Card.Buildings);
            return PartialView(uauevm);
        }

        public JsonResult AddNewUserCard(UserAccessUnitItem card)
        {
            var userId = ((FoxSecIdentity)System.Web.HttpContext.Current.User.Identity).Id;
            List<UserBuilding> building = _userBuildingRepository.FindAll().Where(x => !x.IsDeleted && x.UserId == userId).ToList();

            if (CurrentUser.Get().IsCompanyManager)
            {

                if (building.Count == 0)
                {
                    return Json(new
                    {
                        IsSucceed = false,
                        Msg = "NobuildingSelected",
                    });
                }
            }
            if (card.Serial == "255" && card.Dk == "65535")
            {
                return Json(new
                {
                    IsSucceed = false,
                    Msg = "serdkerror",
                });
            }
            else
            {
                int? tc = 0;
                int id = 0;
                int tc1 = 0;
                DateTime? validto = null;

                int chkflag = 0;
                bool canUseOwnCards = false;
                int? companyId = CurrentUser.Get().CompanyId;
                string err_msg = string.Empty;
                ValidateCardSerial(card);
                ModelState.Remove("PersonalCode");
                int legaltc = 0;
                if (ModelState.IsValid)
                {
                    var validFrom = DateTime.ParseExact(card.ValidFromStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    var validTo = DateTime.ParseExact(card.ValidToStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    if (companyId.HasValue)
                    {
                        canUseOwnCards = _companyRepository.FindById(companyId.Value).IsCanUseOwnCards;
                    }

                    List<UsersAccessUnit> inSystem = new List<UsersAccessUnit>();

                    if (card.Code != null)
                    {
                        inSystem =
                            _usersAccessUnitRepository.FindAll(
                                x => !x.IsDeleted && x.Free && x.Code != null && x.Code.ToLower() == card.Code.Trim().ToLower()).ToList();
                    }
                    else
                    {
                        inSystem =
                            _usersAccessUnitRepository.FindAll(
                                x =>
                                !x.IsDeleted && x.Free && x.Serial != null && x.Serial.ToLower() == card.Serial.Trim().ToLower() &&
                                x.Dk != null && x.Dk.ToLower() == card.Dk.Trim().ToLower()).ToList();
                    }

                    bool isCardAssigned = false;

                    if (card.Code != null)
                    {
                        isCardAssigned =
                            _usersAccessUnitRepository.FindAll(
                                x => !x.IsDeleted && x.Active && x.UserId != null && x.Code != null && x.Code.ToLower() == card.Code.Trim().ToLower()).FirstOrDefault() != null;
                    }
                    else
                    {
                        isCardAssigned =
                            _usersAccessUnitRepository.FindAll(
                                x =>
                                !x.IsDeleted && x.Active && x.UserId != null && x.Serial != null && x.Serial.ToLower() == card.Serial.Trim().ToLower() &&
                                x.Dk != null && x.Dk.ToLower() == card.Dk.Trim().ToLower()).FirstOrDefault() != null;
                    }

                    if (isCardAssigned)
                    {
                        err_msg = ViewResources.SharedStrings.CardsErrorMessageCardUsed;
                        ModelState.AddModelError("", err_msg);
                    }

                    if (inSystem.Count == 0 && !isCardAssigned && !CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin && !canUseOwnCards)
                    {
                        err_msg = ViewResources.SharedStrings.CardsMessageCardNotFree;
                        ModelState.AddModelError("", err_msg);
                    }

                    if (ModelState.IsValid)
                    {
                        User user = _userRepository.FindById(card.UserId.Value);

                        if (inSystem.Count != 0)
                        {
                            UsersAccessUnit insystem_card = null;
                            foreach (var usersAccessUnit in inSystem)
                            {
                                var buildingIds = GetCardBuildings(usersAccessUnit.CompanyId);
                                if (buildingIds.Contains(card.BuildingId))
                                {
                                    insystem_card = usersAccessUnit;
                                    break;
                                }
                            }
                            if (insystem_card == null)
                            {
                                err_msg = ViewResources.SharedStrings.CardsNotSuitableCardMessage;
                            }
                            if (ModelState.IsValid)
                            {
                                _cardService.EditCard(insystem_card.Id, card.UserId, card.TypeId, user.CompanyId, card.BuildingId,
                                                      card.Serial, card.Dk, card.Code, false, validFrom,
                                                      validTo, null, true, true);
                                chkflag = 1;
                            }
                        }
                        else
                        {
                            IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue("Users");
                            foreach (var obj in cv)
                            {
                                tc = obj.Legal;
                                id = obj.Id;

                                validto = obj.ValidTo;
                            }
                            if (validto == null && tc == null)
                            {
                                chkflag = 2; //licence error
                                legaltc = 0;
                            }
                            else
                            {
                                tc1 = _userRepository.FindAll(x => !x.IsDeleted && x.Active == true).ToList().Count();
                                int remaining = Convert.ToInt32(tc) - tc1;
                                remaining = remaining < 0 ? 0 : remaining;
                                if (remaining > 0 && validto > DateTime.Now)
                                {
                                    _cardService.CreateCard(user.Id, card.TypeId, user.CompanyId, card.BuildingId, card.Serial,
                                                        card.Dk, card.Code, false, validFrom, validTo, card.IsMainUnit);
                                    chkflag = 1;
                                }
                                else
                                {
                                    chkflag = 2; //licence error
                                    legaltc = Convert.ToInt32(tc);
                                }
                            }
                        }
                    }

                    var tmp = CreateViewModel<UserAccessUnitEditViewModel>();
                    var card_user = _userRepository.FindById(card.UserId.Value);
                    card.CardTypes = new SelectList(_cardTypeRepository.FindAll(x => !x.IsDeleted).OrderBy(x => x.Name.ToLower()), "Id", "Name");
                    var building_ids = GetCardBuildings(card_user.CompanyId);
                    var user_building_ids = GetUserBuildings(_buildingRepository, _userRepository, card.UserId);
                    Mapper.Map(_buildingRepository.FindAll().Where(x => building_ids.Contains(x.Id) && user_building_ids.Contains(x.Id)), card.Buildings);
                    tmp.Card = card;
                    if (chkflag == 2)
                    {
                        return Json(new
                        {
                            IsSucceed = false,
                            Msg = "licence error",
                            Count = legaltc
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            IsSucceed = ModelState.IsValid,
                            IsMessageEmpty = string.IsNullOrWhiteSpace(err_msg),
                            Msg = ModelState.IsValid ? ViewResources.SharedStrings.CardsMessageAddNewCardToUser : err_msg,
                            viewData = this.RenderPartialView("NewUserCard", tmp)
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        IsSucceed = ModelState.IsValid,
                        IsMessageEmpty = string.IsNullOrWhiteSpace(err_msg),
                        Msg = ModelState.IsValid ? ViewResources.SharedStrings.CardsMessageAddNewCardToUser : errormessage,
                    });
                }
            }
        }

        public ActionResult UserCardsList(int id, int filter)
        {
            var uaulvm = CreateViewModel<UserAccessUnitListViewModel>();
            var cards = ApplyCardStatusFilter(_usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == id), filter);
            Mapper.Map(cards, uaulvm.Cards);
            uaulvm.FilterCriteria = filter;
            return PartialView(uaulvm);
        }

        #endregion

        #region Deactivate / Activate / Delete Card

        [HttpGet]
        public ActionResult Activate()
        {
            var dcvm = CreateViewModel<DeactivateCardViewModel>();
            dcvm.Reasons = new SelectList(_classificatorValueRepository.FindAll(cv => cv.ClassificatorId == 6).OrderBy(cv => cv.Value.ToLower()), "Id", "Value");
            return PartialView("ActivateDeactivate", dcvm);
        }

        [HttpGet]
        public ActionResult Deactivate()
        {
            var dcvm = CreateViewModel<DeactivateCardViewModel>();
            dcvm.Reasons = new SelectList(_classificatorValueRepository.FindAll(cv => cv.ClassificatorId == 5).OrderBy(cv => cv.Value.ToLower()), "Id", "Value");
            dcvm.IsDeactivateDialog = true;
            return PartialView("ActivateDeactivate", dcvm);
        }

        [HttpPost]
        public ActionResult DeactivateCards(int[] cardIds, int reasonId, bool isMoveToFree)
        {
            if (isMoveToFree)
            {
                foreach (var cardId in cardIds)
                {
                    _cardService.SetFreeState(cardId, reasonId);
                    _cardService.SetValidTo(cardId, DateTime.Now);
                }
            }
            else
            {
                foreach (var cardId in cardIds)
                {
                    _cardService.Deactivate(cardId, reasonId);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult ActivateCards(int[] cardIds, int reasonId)
        {
            foreach (var cardId in cardIds)
            {
                _cardService.Activate(cardId, reasonId);
            }
            return null;
        }
        [HttpPost]
        public ActionResult FreeCards(int[] cardIds)
        {
            foreach (int id in cardIds)
            {
                _cardService.SetFreeState(id, null);
            }
            return null;
        }
        [HttpPost]
        public ActionResult DeleteCards(int[] cardIds)
        {
            foreach (var cardId in cardIds)
            {
                _cardService.Delete(cardId);
            }
            return null;
        }

        #endregion

        [HttpGet]
        public ActionResult IsCompanyCanUseOwnCards()
        {
            IFoxSecIdentity cuser = CurrentUser.Get();
            int? companyId = _userRepository.FindById(cuser.Id).CompanyId;
            if (companyId.HasValue)
            {
                return _companyRepository.FindById(companyId.Value).IsCanUseOwnCards ? Content("True") : Content("False");
            }
            return Content(cuser.IsSuperAdmin || cuser.IsBuildingAdmin ? "True" : "False");
        }

        [HttpGet]
        public ActionResult CardNotFound()
        {
            var cnfvm = CreateViewModel<CardNotFoundViewModel>();
            if (CurrentUser.Get().IsCompanyManager)
            {
                cnfvm.CanCreateCard = _userRepository.FindById(CurrentUser.Get().Id).Company.IsCanUseOwnCards;
            }
            var user = _userRepository.FindById(CurrentUser.Get().Id);
            var buildings = GetCardBuildings(user.CompanyId);
            Mapper.Map(_buildingRepository.FindAll().Where(x => !x.IsDeleted && buildings.Contains(x.Id)), cnfvm.Buildings);
            Mapper.Map(_cardTypeRepository.FindAll().Where(x => !x.IsDeleted).OrderBy(x => x.Name.ToLower()), cnfvm.CardTypes);
            cnfvm.TypeId = 7;
            return PartialView(cnfvm);
        }

        [HttpPost]
        public JsonResult CreateFreeCard(string serial, string dk, string code, int? buildingId, int? cardTypeId, string validFrom, string validTo)
        {
            IFoxSecIdentity cuser = CurrentUser.Get();
            var err = string.Empty;
            DateTime? valid_from = null;
            DateTime? valid_to = null;
            valid_from = DateTime.Now.Date;
            valid_from = DateTime.Now.Date;
            try
            {
                valid_from = DateTime.ParseExact(validFrom.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                valid_to = DateTime.ParseExact(validTo.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                err = ViewResources.SharedStrings.CardsDateValidationMessage;
                ModelState.AddModelError("", err);
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(serial) && serial.Length != 3)
                {
                    err = "Serial should contain 3 symbols!";
                    ModelState.AddModelError("", err);
                }
            }
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(dk) && dk.Length != 5)
            {
                err = "DK should contain 5 symbols!";
                ModelState.AddModelError("", err);
            }

            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(serial) && string.IsNullOrWhiteSpace(dk))
            {
                err = "Dk should be entered!";
                ModelState.AddModelError("", err);
            }

            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(dk) && string.IsNullOrWhiteSpace(serial))
            {
                err = "Serial should be entered!";
                ModelState.AddModelError("", err);
            }
            if (ModelState.IsValid)
            {
                var card = _usersAccessUnitRepository.FindAll().Where(x => x.Serial == serial && x.Dk == dk && !x.Free);
                if (card.Count() != 0)
                {
                    err = "Card already exist";
                    ModelState.AddModelError("", err);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    int? companyId = _userRepository.FindById(cuser.Id).CompanyId;
                    _cardService.CreateCard(null, cardTypeId, companyId, buildingId.Value, serial, dk, code, true, valid_from, valid_to, null);
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    ModelState.AddModelError("", err);
                }
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? "Added new free card" : string.Format("Adding new free card failed : {0}", err)
            });
        }


        #region give card back
        public ActionResult GiveCardBack(List<int> cardIds)
        {
            try
            {
                bool done = _cardService.GiveCardBack(cardIds);

                return Json(new
                {
                    IsSucceed = done
                });
            }
            catch
            {
                return Json(new
                {
                    IsSucceed = false
                });
            }
        }
        #endregion
        
        public ActionResult CommentedCardsList()
        {
            var users = db.User.Where(x => !x.IsDeleted && x.CompanyId != null).ToList();
            var commentedCardList = new List<ComentedCardListModel>();
            //users.ForEach(x => { var commentecard = new ComentedCardListModel { FirstName = x.FirstName, LastName = x.LastName, ValidFrom = x.ModifiedLast, ValidTo = x.ModifiedLast, Comment = x.Comment }; commentedCardList.Add(commentecard); });
            //commentedCardList.ForEach(x => { if (x.Comment == null) { x.Comment = ""; } });
            if(CurrentUser.Get().IsCompanyManager && CurrentUser.Get().CompanyId.HasValue)
            {
                var company = _companyRepository.FindAll().Where(cc => cc.Id == (CurrentUser.Get().CompanyId)).First().Name;
                // var cards = _usersAccessUnitRepository.FindAll(x => x.CompanyId == CurrentUser.Get().CompanyId.Value && x.Comment != null && x.Comment.Length > 2).ToList();
                var cards = _usersAccessUnitRepository.FindAll(x => x.Comment != null && x.Comment.Trim().Length > 2).ToList();

                var compids = _companyRepository.FindAll().Where(x => x.Name.ToLower().Contains(company.Trim().ToLower())).Select(x => x.Id);
                var compall = _companyRepository.FindAll().Where(x => (compids.Contains(x.Id) || compids.Contains(Convert.ToInt32(x.ParentId == null ? 0 : x.ParentId)))).Select(x => x.Id);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select ParentCompanieId from CompanieSubCompanies where IsDeleted=0 and CompanyId='" + CurrentUser.Get().CompanyId + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                List<int> subcompanyIds = new List<int>();
                foreach (DataRow dr in dt.Rows)
                {
                    subcompanyIds.Add(Convert.ToInt32(dr["ParentCompanieId"]));
                }
                compall = compall.Concat(subcompanyIds).ToList();

               
                cards = cards.Where(x => compall.Contains(Convert.ToInt32(x.CompanyId == null ? 0 : x.CompanyId))).ToList();
                if(cards != null)
                {
                    cards.ForEach(x => { if (x.User != null) { var commentedCard = new ComentedCardListModel { Id = x.Id, FirstName = x.User.FirstName, LastName = x.User.LastName, ValidFrom = x.ValidFrom.Value, ValidTo = x.ValidTo.Value, Comment = x.Comment }; commentedCardList.Add(commentedCard); } });
                }
                
                //var usersAccessUnit = _usersAccessUnitRepository.FindAll(x => x.CompanyId == CurrentUser.Get().CompanyId.Value && x.Comment != null && x.Comment.Length > 2).ToList();
                //usersAccessUnit.ForEach(x => { var commentedCard = new ComentedCardListModel { Id = x.Id, FirstName = x.User.FirstName, LastName = x.User.LastName, ValidFrom = x.ValidFrom.Value, ValidTo = x.ValidTo.Value, Comment = x.Comment }; commentedCardList.Add(commentedCard); });
            }
            else if(CurrentUser.Get().IsCompanyManager && !CurrentUser.Get().CompanyId.HasValue)
            {
                var usersAccessUnit = _usersAccessUnitRepository.FindAll(x => x.UserId == CurrentUser.Get().Id && x.Comment != null && x.Comment.Trim().Length > 2).ToList();
                if(usersAccessUnit != null)
                {

                usersAccessUnit.ForEach(x => { if (x.User != null) { var commentedCard = new ComentedCardListModel { Id = x.Id, FirstName = x.User.FirstName, LastName = x.User.LastName, ValidFrom = x.ValidFrom.Value, ValidTo = x.ValidTo.Value, Comment = x.Comment }; commentedCardList.Add(commentedCard); } });

                }
            }
            else
            {
                var usersAccessUnit = _usersAccessUnitRepository.FindAll(x => x.Comment != null && x.Comment.Trim().Length > 2).ToList();
                if(usersAccessUnit != null)
                {

                usersAccessUnit.ForEach(x => { if (x.User != null) { var commentedCard = new ComentedCardListModel { Id = x.Id, FirstName = x.User.FirstName, LastName = x.User.LastName, ValidFrom = x.ValidFrom.Value, ValidTo = x.ValidTo.Value, Comment = x.Comment }; commentedCardList.Add(commentedCard); } });

                }
            }
           
            
            //usersAccessUnit.ForEach(x => { var commentedCard = new ComentedCardListModel { Id = x.Id, FirstName = users.Where(y => y.Id == x.UserId.Value).Select(u => u.FirstName).FirstOrDefault(), LastName = users.Where(z => z.Id == x.UserId.Value).Select(i => i.LastName).FirstOrDefault(), ValidFrom = x.ValidFrom.Value, ValidTo = x.ValidTo.Value, Comment = x.Comment }; commentedCardList.Add(commentedCard); });
           


            return PartialView("CommentedCardList",commentedCardList);
        }

        public string CardEditingUpdateComment(MVCxGridViewBatchUpdateValues<UsersAccessUnit, object> updateValues)
        {
           
            foreach(var usersUnit in updateValues.Update)
            {
               
               var result = _cardService.EditCommentCard(usersUnit.Id, usersUnit.ValidTo.Value, usersUnit.ValidFrom.Value, usersUnit.Comment);
                
                
               

            }

            return "";
        }
    }
}