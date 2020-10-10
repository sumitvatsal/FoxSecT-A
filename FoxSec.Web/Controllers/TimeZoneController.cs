using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FoxSec.Authentication;
using System.Globalization;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Resources;
using ViewResources;

namespace FoxSec.Web.Controllers
{
    public class TimeZoneController : PaginatorControllerBase<UserTimeZone>
    {
        private readonly IUserTimeZoneService _userTimeZoneService;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUserTimeZonePropertyRepository _userTimeZonePropertyRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly IUserPermissionGroupService _userPermissionGroupService;
        private readonly IClassificatorValueRepository _classificatorValueRepository;

        public TimeZoneController(ICurrentUser currentUser,
                                     ILogger logger,
                                     IUserTimeZoneService userTimeZoneService,
                                     IClassificatorValueRepository classificatorValueRepository,
                                     IUserTimeZoneRepository userTimeZoneRepository,
                                     IUserTimeZonePropertyRepository userTimeZonePropertyRepository,
                                     IUserPermissionGroupTimeZoneRepository userPermissionGroupTimeZoneRepository,
                                     IUserPermissionGroupService userPermissionGroupService) : base(currentUser, logger)
        {
            _userTimeZoneService = userTimeZoneService;
            _userTimeZoneRepository = userTimeZoneRepository;
            _classificatorValueRepository = classificatorValueRepository;
            _userTimeZonePropertyRepository = userTimeZonePropertyRepository;
            _userPermissionGroupTimeZoneRepository = userPermissionGroupTimeZoneRepository;
            _userPermissionGroupService = userPermissionGroupService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserRole()
        {
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsBuildingAdmin)
            {
                return null;
            }
            return JavaScript(" $('#button_add_time_zone').hide();");
        }

        public ActionResult Search(string name, string start, int? nav_page, int? rows)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            var tzlvm = CreateViewModel<TimeZoneListViewModel>();
            List<UserTimeZone> zones = new List<UserTimeZone>();
            //Create LIST
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

            IEnumerable<UserTimeZone> z = zones.OrderBy(y => y.TimeZoneId);
            tzlvm.Paginator = SetupPaginator(ref z, nav_page, rows);
            tzlvm.Paginator.DivToRefresh = "TimeZoneList";
            tzlvm.Paginator.Prefix = "TimeZone";

            Mapper.Map(z, tzlvm.TimeZones);

            foreach (var zone in tzlvm.TimeZones)
            {
                zone.IsInUse = _userPermissionGroupTimeZoneRepository.FindActiveTZ(zone.Id).Any();
                //zone.IsInUse = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserTimeZoneId == zone.Id)/*.ToList()*/.Any();
            }
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

        [HttpGet]
        public ActionResult Create()
        {
            var tzevm = CreateViewModel<TimeZoneEditViewModel>();
            return PartialView(tzevm);
        }

        [HttpPost]
        public ActionResult Create(TimeZoneEditViewModel item)
        {
            int id = 0;
            var out_tz = CreateViewModel<TimeZoneEditViewModel>();
            out_tz.TimeZone = item.TimeZone;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    id = _userTimeZoneService.CreateUserTimeZone(item.TimeZone.Name);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }
            if (id < 0)
            {
                return Json(new { IsSucceed = false });
            }
            else
            {
                return Json(new
                {
                    IsSucceed = ModelState.IsValid,
                    Msg = ModelState.IsValid ? ViewResources.SharedStrings.TimeZonesEditMessage : err_msg,
                    DisplayMessage = !string.IsNullOrEmpty(err_msg),
                    viewData = ModelState.IsValid ? null : this.RenderPartialView("Create", out_tz)
                });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tzevm = CreateViewModel<TimeZoneEditViewModel>();
            Mapper.Map(_userTimeZoneRepository.FindById(id), tzevm.TimeZone);
            return PartialView(tzevm);
        }

        [HttpPost]
        public ActionResult Edit(TimeZoneEditViewModel item)
        {
            var out_tz = CreateViewModel<TimeZoneEditViewModel>();
            out_tz.TimeZone = item.TimeZone;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _userTimeZoneService.UpdateUserTimeZone(item.TimeZone.Id, item.TimeZone.Name);
                    _userTimeZoneService.GroupUpdateName(item.TimeZone.Id, item.TimeZone.Name);
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
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.TimeZonesEditMessage : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = ModelState.IsValid ? null : this.RenderPartialView("Edit", out_tz)
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _userTimeZoneService.DeleteUserTimeZone(id);
            return null;
        }
        public ActionResult ToggleZone(int id, int day, bool CreateNew)
        {
            UserTimeZone utz = new UserTimeZone();
            if (CurrentUser.Get().IsCompanyManager)
            {
                var utzp = _userTimeZonePropertyRepository.FindById(id);
                var utzz = _userTimeZoneRepository.FindById(utzp.UserTimeZoneId);
                if (utzz.CompanyId != CurrentUser.Get().CompanyId)
                {
                    if (CreateNew == true)
                    {
                        var CompanyTimeZoneIsDeleted = _userTimeZoneRepository.FindAll(x => x.TimeZoneId == utzz.TimeZoneId /*&& x.IsDeleted*/ && x.CompanyId == CurrentUser.Get().CompanyId);
                        if (CompanyTimeZoneIsDeleted.Count() == 0)
                        {
                            int NUTZId = _userTimeZoneService.CreateUserTimeZone(utzz.Name);
                            return null;
                        }
                        else
                        {
                            UserTimeZone DUTZ = CompanyTimeZoneIsDeleted.First();
                            _userTimeZoneService.RecoveryUserTimeZone(DUTZ.Id);
                            return null;
                        }
                    }
                    return Json(new { html = '1' });
                }
            }
            _userTimeZoneService.ToggleUserZone(id, day);
            return null;
        }

        public JsonResult TimeFromChange(int id, string time)
        {
            if (time.Trim() == string.Empty) return Json("empty", JsonRequestBehavior.AllowGet);

            try
            {
                DateTime fromTime = DateTime.ParseExact(time.Trim(), "HH:mm", CultureInfo.InvariantCulture);
                UserTimeZoneProperty newUtzp = _userTimeZonePropertyRepository.FindById(id);
                _userTimeZoneService.UpdateUserFromTime(id, fromTime);
                _userPermissionGroupService.UpdateGroupFromTime(newUtzp.UserTimeZoneId, fromTime, newUtzp.OrderInGroup);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("not", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult TimeToChange(int id, string time)
        {
            if (time.Trim() == string.Empty) return Json("empty", JsonRequestBehavior.AllowGet);

            try
            {
                DateTime toTime = DateTime.ParseExact(time.Trim(), "HH:mm", CultureInfo.InvariantCulture);
                UserTimeZoneProperty newUtzp = _userTimeZonePropertyRepository.FindById(id);

                _userTimeZoneService.UpdateUserToTime(id, toTime);
                _userPermissionGroupService.UpdateGroupToTime(newUtzp.UserTimeZoneId, toTime, newUtzp.OrderInGroup);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("not", JsonRequestBehavior.AllowGet);
            }
        }
    }
}