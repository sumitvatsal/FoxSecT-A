using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;
using System.Collections;

namespace FoxSec.ServiceLayer.Services
{
    internal class HolidayBuildingService : ServiceBase, IHolidayBuildingService
    {
        private readonly IHolidayBuildingRepository _HolidayBuildingRepository;
        private readonly ILogService _logService;
        private readonly IControllerUpdateService _controllerUpdateService;
        private string flag = "";
        public HolidayBuildingService(ICurrentUser currentUser,
                               IDomainObjectFactory domainObjectFactory,
                               IEventAggregator eventAggregator,
                               IHolidayBuildingRepository HolidayBuildingRepository,
                               ILogService logService,
                               IControllerUpdateService controllerUpdateService)
            : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _HolidayBuildingRepository = HolidayBuildingRepository;
            _controllerUpdateService = controllerUpdateService;
            _logService = logService;
        }

        public void CreateHolidayBuilding(int holidayId, int BuildingId)
        {

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                if (_HolidayBuildingRepository.FindAll().Where(x => x.HoliDayId == holidayId && x.BuildingId == BuildingId).Any())
                {
                    _HolidayBuildingRepository.FindAll().Where(x => x.HoliDayId == holidayId && x.BuildingId == BuildingId).First().IsDeleted = false;
                }
                else
                {
                    HolidayBuilding hb = DomainObjectFactory.CreateHolidayBuilding();
                    hb.HoliDayId = holidayId;
                    hb.BuildingId = BuildingId;
                    hb.IsDeleted = false;
                    _HolidayBuildingRepository.Add(hb);
                }

                work.Commit();
                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, "Holiday buildings changed");

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, holidayId, UpdateParameter.HolidayChange, ControllerStatus.Edited, string.Empty);

            }
        }

        public void DeleteHolidayBuilding(int holidayId, int BuildingId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                var listHb = _HolidayBuildingRepository.FindAll(x => x.BuildingId == BuildingId && x.HoliDayId == holidayId);
                foreach (var hbitem in listHb)
                {
                    HolidayBuilding hb = _HolidayBuildingRepository.FindById(hbitem.Id);
                    hb.IsDeleted = true;
                }
                work.Commit();
                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, "Holiday buildings changed");

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, holidayId, UpdateParameter.HolidayChange, ControllerStatus.Edited, string.Empty);

            }
        }

        public void EditHolidayBuilding(int id, int holidayId, int BuildingId, Boolean isDeleted)
        {

            using (IUnitOfWork work = UnitOfWork.Begin())
            {

                work.Commit();

            }
        }
    }
}
