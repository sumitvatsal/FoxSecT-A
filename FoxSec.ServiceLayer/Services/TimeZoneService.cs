using System;
using System.Collections.Generic;
using System.Xml.Linq;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;
using TimeZone = FoxSec.DomainModel.DomainObjects.TimeZone;

namespace FoxSec.ServiceLayer.Services
{
    internal class TimeZoneService : ServiceBase, ITimeZoneService
    {
        private readonly ITimeZoneRepository _timeZoneRepository;
        private readonly ITimeZonePropertyRepository _timeZonePropertyRepository;
    	private readonly ILogService _logService;

        public TimeZoneService( ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory,
                                IEventAggregator eventAggregator, ITimeZoneRepository timeZoneRepository,
                                ILogService logService,
                                ITimeZonePropertyRepository timeZonePropertyRepository) : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _timeZoneRepository = timeZoneRepository;
            _timeZonePropertyRepository = timeZonePropertyRepository;
        	_logService = logService;
        }


        public int CreateTimeZone(string name)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZone timeZone = DomainObjectFactory.CreateTimeZone();

                timeZone.Name = name;
                timeZone.IsActive = true;
                timeZone.IsDeleted = false;

                _timeZoneRepository.Add(timeZone);

                work.Commit();

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneCreated", new List<string> { timeZone.Name }));
            	_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
            	                      message.ToString());
                return timeZone.Id;
            }
        }

        public void UpdateTimeZone(int id, string name)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZone timeZone = _timeZoneRepository.FindById(id);
            	var old_name = timeZone.Name;

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneNameChanged", new List<string> { old_name, name }));
            	timeZone.Name = name;

				_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  message.ToString());
                work.Commit();
            }
        }

        public void ActivateTimeZone(int id)
        {
            SetTimeZoneSate(id, true);
        }

        public void DeactivateTimeZone(int id)
        {
            SetTimeZoneSate(id, false);
        }

        public void SetTimeZoneSate(int id, bool state)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZone timeZone = _timeZoneRepository.FindById(id);

                timeZone.IsActive = state;

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            	message.Add(state
            	            	? XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneChangedActivated",
            	            	                                    new List<string> {timeZone.Name})
            	            	: XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneChangedDeActivated",
            	            	                                    new List<string> {timeZone.Name}));

            	work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void DeleteTimeZone(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZone timeZone = _timeZoneRepository.FindById(id);

                timeZone.IsDeleted = true;

                work.Commit();

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneDeleted", new List<string> { timeZone.Name }));

				_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public int CreateTimeZoneProperty(int timeZoneId, int order)
        {
        	var validFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
			var validTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
			return CreateTimeZoneProperty(timeZoneId, order, validFrom, validTo, true, false, false, false, false, false, false);
        }

        public int CreateTimeZoneProperty(int timeZoneId, int order, DateTime? validFrom, DateTime? validTo, bool isMonday, bool isTuesday, bool isWednesday, bool isThursday, bool isFriday, bool isSaturday, bool isSunday)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZoneProperty timeZoneProperty = DomainObjectFactory.CreateTimeZoneProperty();

                timeZoneProperty.TimeZoneId = timeZoneId;
                timeZoneProperty.OrderInGroup = order;
                timeZoneProperty.ValidFrom = validFrom;
                timeZoneProperty.ValidTo = validTo;
                timeZoneProperty.IsMonday = isMonday;
                timeZoneProperty.IsTuesday = isTuesday;
                timeZoneProperty.IsWednesday = isWednesday;
                timeZoneProperty.IsThursday = isThursday;
                timeZoneProperty.IsFriday = isFriday;
                timeZoneProperty.IsSaturday = isSaturday;
                timeZoneProperty.IsSunday = isSunday;

                _timeZonePropertyRepository.Add(timeZoneProperty);

                work.Commit();

            	var tzProperty = _timeZonePropertyRepository.FindById(timeZoneProperty.Id);

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZonePropertyCreated", new List<string> { tzProperty.TimeZone.Name }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageOrderInGroup", new List<string> { order.ToString() }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom", new List<string> { tzProperty.ValidFrom == null ? "empty" : tzProperty.ValidFrom.Value.ToString("HH:mm") }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo", new List<string> { tzProperty.ValidTo == null ? "empty" : tzProperty.ValidTo.Value.ToString("HH:mm") }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsMonday", new List<string> { tzProperty.IsMonday.ToString() }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsTuesday", new List<string> { tzProperty.IsTuesday.ToString() }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsWednesday", new List<string> { tzProperty.IsWednesday.ToString() }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsThursday", new List<string> { tzProperty.IsThursday.ToString() }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsFriday", new List<string> { tzProperty.IsFriday.ToString() }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSaturday", new List<string> { tzProperty.IsSaturday.ToString() }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSunday", new List<string> { tzProperty.IsSunday.ToString() }));

				_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());

                return timeZoneProperty.Id;
			}
        }

        public void ToggleZone(int id, int day)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZoneProperty timeZoneProperty = _timeZonePropertyRepository.FindById(id);
            	bool dayVal = false;
            	string dayName = string.Empty;
                switch (day)
                {
                    case 1:
                		dayVal = timeZoneProperty.IsMonday;
						timeZoneProperty.IsMonday = !timeZoneProperty.IsMonday;
                		dayName = "LogMessageMonday";
                        break;
                    case 2:
						dayVal = timeZoneProperty.IsTuesday;
                        timeZoneProperty.IsTuesday = !timeZoneProperty.IsTuesday;
						dayName = "LogMessageTuesday";
                        break;
                    case 3:
						dayVal = timeZoneProperty.IsWednesday;
                        timeZoneProperty.IsWednesday = !timeZoneProperty.IsWednesday;
						dayName = "LogMessageWednesday";
                        break;
                    case 4:
						dayVal = timeZoneProperty.IsThursday;
                        timeZoneProperty.IsThursday = !timeZoneProperty.IsThursday;
						dayName = "LogMessageThursday";
                        break;
                    case 5:
						dayVal = timeZoneProperty.IsFriday;
                        timeZoneProperty.IsFriday = !timeZoneProperty.IsFriday;
						dayName = "LogMessageFriday";
                        break;
                    case 6:
						dayVal = timeZoneProperty.IsSaturday;
                        timeZoneProperty.IsSaturday = !timeZoneProperty.IsSaturday;
						dayName = "LogMessageSaturday";
                        break;
                    case 7:
						dayVal = timeZoneProperty.IsSunday;
                        timeZoneProperty.IsSunday = !timeZoneProperty.IsSunday;
						dayName = "LogMessageSunday";
                        break;
                }

            	var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZonePropertyChanged", new List<string> { timeZoneProperty.TimeZone.Name }));
				message.Add(XMLLogMessageHelper.TemplateToXml(dayName, null));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCommonIsChanged", new List<string> { dayVal.ToString(), (!dayVal).ToString() }));
				
                work.Commit();
				_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void UpdateFromTime(int id, DateTime? time)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZoneProperty timeZoneProperty = _timeZonePropertyRepository.FindById(id);
            	var oldTime = timeZoneProperty.ValidFrom;
				timeZoneProperty.ValidFrom = time;

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZonePropertyChanged", new List<string> { timeZoneProperty.TimeZone.Name }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFromChanged", new List<string> { oldTime == null ? "empty" : oldTime.Value.ToString("HH:mm"),
            	                        timeZoneProperty.ValidFrom == null
            	                        	? "empty"
            	                        	: timeZoneProperty.ValidFrom.Value.ToString("HH:mm") }));
				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void UpdateToTime(int id, DateTime? time)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TimeZoneProperty timeZoneProperty = _timeZonePropertyRepository.FindById(id);
				var oldTime = timeZoneProperty.ValidTo;
                timeZoneProperty.ValidTo = time;

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZonePropertyChanged", new List<string> { timeZoneProperty.TimeZone.Name }));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidToChanged", new List<string> { oldTime == null ? "empty" : oldTime.Value.ToString("HH:mm"),
            	                        timeZoneProperty.ValidTo == null
            	                        	? "empty"
            	                        	: timeZoneProperty.ValidTo.Value.ToString("HH:mm") }));
				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }
        }
    }
}