using System;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
	internal class HolidayService : ServiceBase, IHolidayService
	{
		private readonly IHolidayRepository _holidayRepository;
	    private readonly ILogService _logService;
		private readonly IControllerUpdateService _controllerUpdateService;
        private string flag = "";
        public HolidayService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory, IEventAggregator eventAggregator, IHolidayRepository holidayRepository, ILogService logService, IControllerUpdateService controllerUpdateService)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
			_holidayRepository = holidayRepository;
        	_controllerUpdateService = controllerUpdateService;
            _logService = logService;
		}

        public void CreateHoliday(string name, string createdBy, DateTime eventStart, DateTime eventEnd, bool holidayMoving)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
                Holiday holiday = DomainObjectFactory.CreateHoliday();

                holiday.Name = name;
			    holiday.EventStart = eventStart;
                holiday.EventEnd = eventStart; //eventEnd;
			    holiday.ModifiedLast = DateTime.Now;
			    holiday.ModifiedBy = createdBy;
			    holiday.MovingHoliday = holidayMoving;
                holiday.IsDeleted = false;

                _holidayRepository.Add(holiday);

				work.Commit();

			    var holidayLogEntity = new HolidayEventEntity(holiday);

			    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
			                          holidayLogEntity.GetCreateMessage());

				string holiday_value = string.Format("{0} {1} {1}", name, eventStart.ToString("dd.MM.yyyy"));

				_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, holiday.Id, UpdateParameter.HolidayChange, ControllerStatus.Created, holiday_value);

			}
		}

        public void DeleteHoliday(int id)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
                Holiday holiday = _holidayRepository.FindById(id);
				var holiday_id = holiday.Id;
                var holidayLogEntity = new HolidayEventEntity(holiday);

                //_holidayRepository.Delete(holiday);
                holiday.IsDeleted = true;

				work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      holidayLogEntity.GetDeleteMessage());

				_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, holiday_id, UpdateParameter.HolidayChange, ControllerStatus.Deleted, string.Empty);
			}
		}

	    public void EditHoliday(int id, string name, string modifiedBy, DateTime eventStart, DateTime eventEnd)//, bool holidayMoving)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
                Holiday holiday = _holidayRepository.FindById(id);

                var holidayLogEntity = new HolidayEventEntity(holiday);
                holiday.Name = name;
			    holiday.EventStart = eventStart;
                holiday.EventEnd = eventStart; //eventEnd;
			    holiday.ModifiedLast = DateTime.Now;
                holiday.ModifiedBy = modifiedBy;
                //holiday.MovingHoliday = holidayMoving;

				work.Commit();

                holidayLogEntity.SetNewHoliday(holiday);

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      holidayLogEntity.GetEditMessage());

				string holiday_value = string.Format("{0} {1} {2}", holiday.Name, holiday.EventStart.ToString("dd.MM.yyyy"), holiday.EventEnd.ToString("dd.MM.yyyy"));
				_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, holiday.Id, UpdateParameter.HolidayChange, ControllerStatus.Edited, holiday_value);
			}
		}

        public void SaveMoving(int id, bool isChecked)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Holiday holiday = _holidayRepository.FindById(id);
                var holidayLogEntity = new HolidayEventEntity(holiday);

                holiday.MovingHoliday = isChecked;

                work.Commit();

                holidayLogEntity.SetNewHoliday(holiday);

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      holidayLogEntity.GetMovingMessage());

				string holiday_value = string.Format("{0} {1} - {2}, moving is '{3}'", holiday.Name, holiday.EventStart.ToString("dd.MM.yyyy"), holiday.EventEnd.ToString("dd.MM.yyyy"), isChecked);
				_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, holiday.Id, UpdateParameter.HolidayChange, ControllerStatus.Edited, holiday_value);
            }
        }
	}
}
