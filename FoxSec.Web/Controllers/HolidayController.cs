using System;
using System.Globalization;
using AutoMapper;
using System.Linq;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EF.Repositories.Interfaces;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoxSec.Web.Controllers
{
    public class HolidayController : BusinessCaseController
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IHolidayBuildingRepository _HolidayBuildingRepository;
        private readonly IHolidayService _holidayService;
        private readonly IHolidayBuildingService _holidayBuildingService;
        private readonly IUserRepository _userRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IBuildingObjectRepository _buildingObjectRepository;

        public HolidayController(IHolidayService holidayService,
                                    IHolidayBuildingRepository holidayBuildingRepository,
                                    IHolidayBuildingService holidayBuildingService,
                                    IHolidayRepository holidayRepository,
                                    ICurrentUser currentUser,
                                    IUserRepository userRepository,
                                    IBuildingRepository buildingRepository,
                                    IBuildingObjectRepository buildingObjectRepository,
                                    ILogger logger)
            : base(currentUser, logger)
        {
            _holidayRepository = holidayRepository;
            _HolidayBuildingRepository = holidayBuildingRepository;
            _holidayBuildingService = holidayBuildingService;
            _holidayService = holidayService;
            _userRepository = userRepository;
            _buildingRepository = buildingRepository;
            _buildingObjectRepository = buildingObjectRepository;
            _currentUser = currentUser;
        }

        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        [HttpGet]
        public ActionResult List()
        {
            var hlvm = CreateViewModel<HolidayListViewModel>();
            Mapper.Map(_holidayRepository.FindAll(h => !h.IsDeleted), hlvm.Holidays);
            return PartialView(hlvm);
        }

        public List<int> GetUserHolidayBuildings(int id)
        {
            List<int> Ids = (from cbo in _HolidayBuildingRepository.FindAll().Where(x => x.HoliDayId == id && !x.IsDeleted) select cbo.BuildingId).ToList();
            return Ids;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var hevm = CreateViewModel<HolidayEditViewModel>();
            var Holiday = _holidayRepository.FindById(id);
            Mapper.Map(Holiday, hevm.Holiday);

            var building_ids = GetUserBuildings(_buildingRepository, _userRepository);
            var buildings = _buildingRepository.FindAll().Where(x => !x.IsDeleted && building_ids.Contains(x.Id));
            Mapper.Map(buildings, hevm.BuildingItems);

            var ActiveBuildings = GetUserHolidayBuildings(id);
            foreach (int BuildId in ActiveBuildings)
            {
                hevm.BuildingItems.Where(x => x.Value == BuildId.ToString()).First().Selected = true;
            }

            if (!hevm.BuildingItems.Where(x => x.Selected).Any()) { hevm.AllBuildings = true; }
            hevm.Holiday.EventStartStr = hevm.Holiday.EventStart.ToString("dd.MM.yyyy");
            return PartialView(hevm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var rvm = CreateViewModel<HolidayEditViewModel>();
            return View(rvm);
        }

        [HttpPost]
        public ActionResult Create(HolidayEditViewModel hevm)
        {
            var err_msg = string.Empty;
            var res_hevm = CreateViewModel<HolidayEditViewModel>();
            res_hevm.Holiday = hevm.Holiday;
            if (ModelState.IsValid)
            {
                try
                {
                    hevm.Holiday.EventStart = DateTime.ParseExact(hevm.Holiday.EventStartStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Holiday.EventStartStr", ViewResources.SharedStrings.CommonDateFormat);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _holidayService.CreateHoliday(hevm.Holiday.Name, _currentUser.Get().LoginName, hevm.Holiday.EventStart,
                                              hevm.Holiday.EventEnd, false); //hevm.Holiday.MovingHoliday);
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
                viewData = ModelState.IsValid ? null : this.RenderPartialView("Create", res_hevm)
            });
        }

        [HttpPost]
        public ActionResult Edit(HolidayEditViewModel hevm)
        {
            var err_msg = string.Empty;
            var res_hevm = CreateViewModel<HolidayEditViewModel>();
            res_hevm.Holiday = hevm.Holiday;

            if (ModelState.IsValid)
            {
                try
                {
                    hevm.Holiday.EventStart = DateTime.ParseExact(hevm.Holiday.EventStartStr.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Holiday.EventStartStr", ViewResources.SharedStrings.CommonDateFormat);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _holidayService.EditHoliday((int)hevm.Holiday.Id, hevm.Holiday.Name, _currentUser.Get().LoginName, hevm.Holiday.EventStart, hevm.Holiday.EventEnd);//, holiday.MovingHoliday);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }
            if (ModelState.IsValid)
            {
                if (hevm.AllBuildings)
                {
                    var list = _HolidayBuildingRepository.FindAll().Where(x => x.HoliDayId == hevm.Holiday.Id).Select(x => x.BuildingId);
                    foreach (int ids in list)
                    {
                        _holidayBuildingService.DeleteHolidayBuilding(hevm.Holiday.Id.GetValueOrDefault(), ids);
                    }
                }
                else
                {
                    foreach (var build in hevm.BuildingItems.Where(x => x.Selected))
                    {
                        _holidayBuildingService.CreateHolidayBuilding(hevm.Holiday.Id.GetValueOrDefault(), Convert.ToInt32(build.Value));
                    }
                    foreach (var build in hevm.BuildingItems.Where(x => !x.Selected))
                    {
                        _holidayBuildingService.DeleteHolidayBuilding(hevm.Holiday.Id.GetValueOrDefault(), Convert.ToInt32(build.Value));
                    }
                }
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? string.Format("{0} \"{1}\" ...", ViewResources.SharedStrings.HolidaysHolidaySaving, hevm.Holiday.Name) : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = ModelState.IsValid ? null : this.RenderPartialView("Edit", res_hevm)
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _holidayService.DeleteHoliday(id);
            }
            catch (Exception e)
            {
                Logger.Write("Error deleting Holiday Id=" + id, e);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult SaveMoving(int holidayId, bool isChecked)
        {
            _holidayService.SaveMoving(holidayId, isChecked);
            return null;
        }
    }
}