using System;
using System.Collections.Generic;
using System.Linq;
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
//using TimeZone = FoxSec.DomainModel.DomainObjects.TimeZone;

namespace FoxSec.ServiceLayer.Services
{

    internal class UserTimeZoneService : ServiceBase, IUserTimeZoneService
    {
        string flag = "";
        // private readonly ITimeZoneRepository _timeZoneRepository;
        // private readonly ITimeZonePropertyRepository _timeZonePropertyRepository;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUserTimeZonePropertyRepository _userTimeZonePropertyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IControllerUpdateService _controllerUpdateService;
        private readonly ILogService _logService;

        public UserTimeZoneService(ICurrentUser currentUser,
                                    IDomainObjectFactory domainObjectFactory,
                                    IEventAggregator eventAggregator,
                                    ILogService logService,
                                    IUserTimeZoneRepository userTimeZoneRepository,
                                    IUserRepository userRepository,
                                    IControllerUpdateService controllerUpdateService,
                                    IUserTimeZonePropertyRepository userTimeZonePropertyRepository) : base(currentUser, domainObjectFactory, eventAggregator)
        {
            //  _timeZoneRepository = timeZoneRepository;
            // _timeZonePropertyRepository = timeZonePropertyRepository;
            _userTimeZoneRepository = userTimeZoneRepository;
            _userTimeZonePropertyRepository = userTimeZonePropertyRepository;
            _userRepository = userRepository;
            _logService = logService;
            _controllerUpdateService = controllerUpdateService;
        }

        /*   public void SynchonizeUserTimeZones(int userId)
           {
               int[] userTimeZoneIds = (from utz in _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserId == userId && x.TimeZoneId.HasValue) select utz.TimeZoneId.Value).ToArray();
               int[] commonTimeZoneIds = (from tz in _timeZoneRepository.FindAll(x => !x.IsDeleted) select tz.Id).ToArray();
               int[] copyTimeZoneIds = (from id in commonTimeZoneIds where !userTimeZoneIds.Contains(id) select id).ToArray();
               string loginName = _userRepository.FindById(userId).LoginName;

               foreach (int id in copyTimeZoneIds)
               {
                   var tz = _timeZoneRepository.FindById(id);
                   var createTz = true;
                   if( CurrentUser.Get().IsCompanyManager )
                   {
                       var utz = tz.UserTimeZones.Where(x => !x.IsDeleted && x.IsCompanySpecific).FirstOrDefault();

                       if( utz != null )
                       {
                           var user = _userRepository.FindById(utz.UserId);
                           if( user.CompanyId != CurrentUser.Get().CompanyId )
                           {
                               createTz = false;
                           }
                       }
                   }

                   if( createTz )
                   {
                       using (IUnitOfWork work = UnitOfWork.Begin())
                       {
                           UserTimeZone userTimeZone = DomainObjectFactory.CreateUserTimeZone();
                           //TimeZone timeZone = new //_timeZoneRepository.FindById(id);

                           userTimeZone.UserId = userId;
                           //userTimeZone.TimeZoneId = timeZone.Id;           Edit Later
                           userTimeZone.ParentUserTimeZoneId = null;
                           //userTimeZone.Name = timeZone.Name;
                           userTimeZone.Uid = Guid.NewGuid();
                           userTimeZone.IsOriginal = true;
                           userTimeZone.IsDeleted = false;
                           userTimeZone.IsCompanySpecific = false;

                           var tzProperties = _timeZonePropertyRepository.FindAll(x => x.TimeZoneId == timeZone.Id).ToList();

                           foreach (var tzp in tzProperties)
                           {
                               UserTimeZoneProperty utzp = DomainObjectFactory.CreateUserTimeZoneProperty();

                               utzp.UserTimeZoneId = userTimeZone.Id;
                               utzp.TimeZoneId = tzp.TimeZoneId;
                               utzp.OrderInGroup = tzp.OrderInGroup;
                               utzp.ValidFrom = tzp.ValidFrom;
                               utzp.ValidTo = tzp.ValidTo;
                               utzp.IsMonday = tzp.IsMonday;
                               utzp.IsTuesday = tzp.IsTuesday;
                               utzp.IsWednesday = tzp.IsWednesday;
                               utzp.IsThursday = tzp.IsThursday;
                               utzp.IsFriday = tzp.IsFriday;
                               utzp.IsSaturday = tzp.IsSaturday;
                               utzp.IsSunday = tzp.IsSunday;

                               userTimeZone.UserTimeZoneProperties.Add(utzp);

                               var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUTZPropertyforUTZCreated",
                                                                             new List<string> {userTimeZone.Name, loginName}));


                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneId",
                                                                             new List<string> {utzp.TimeZoneId.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageOrderInGroup",
                                                                             new List<string> {utzp.OrderInGroup.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom",
                                                                             new List<string>
                                                                               {
                                                                                   utzp.ValidFrom.HasValue
                                                                                       ? utzp.ValidFrom.Value.ToString("HH:mm")
                                                                                       : ""
                                                                               }));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo",
                                                                             new List<string>
                                                                               {
                                                                                   utzp.ValidTo.HasValue ? utzp.ValidTo.Value.ToString("HH:mm") : ""
                                                                               }));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsMonday", new List<string> {utzp.IsMonday.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsTuesday", new List<string> {utzp.IsTuesday.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsWednesday",
                                                                             new List<string> {utzp.IsWednesday.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsThursday",
                                                                             new List<string> {utzp.IsThursday.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsFriday", new List<string> {utzp.IsFriday.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSaturday",
                                                                             new List<string> {utzp.IsSaturday.ToString()}));
                               message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSunday", new List<string> {utzp.IsSunday.ToString()}));

                               _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                                     message.ToString());
                           }

                           _userTimeZoneRepository.Add(userTimeZone);

                           work.Commit();

                           var mesage = new XElement(XMLLogLiterals.LOG_MESSAGE);
                           mesage.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneCommonCreated",
                                                                        new List<string> {userTimeZone.Name, loginName}));
                           _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, mesage.ToString());
                           _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, userTimeZone.Id, UpdateParameter.UserTimeZoneUpdate, ControllerStatus.Created, userTimeZone.Name);
                       }
                   }
               }
           }
           */

        public int CreateUserTimeZone(string name)
        {


            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZone userTimeZone = DomainObjectFactory.CreateUserTimeZone();
                if (CurrentUser.Get().IsCompanyManager)
                {
                    UserTimeZone mtz = _userTimeZoneRepository.FindAll(x => x.CompanyId == null && x.Name == name).First();
                    userTimeZone.TimeZoneId = mtz.TimeZoneId;
                    userTimeZone.CompanyId = CurrentUser.Get().CompanyId;
                    userTimeZone.UserId = CurrentUser.Get().Id;
                    userTimeZone.Name = name;
                    userTimeZone.Uid = Guid.NewGuid();
                    userTimeZone.IsOriginal = false;
                    userTimeZone.IsDeleted = false;
                    userTimeZone.IsCompanySpecific = CurrentUser.Get().IsCompanyManager;
                    var timezoneprop = (from tzp in _userTimeZonePropertyRepository.FindAll(x => x.UserTimeZoneId == mtz.Id) select tzp);

                    for (int i = 0; i < 4; i++)
                    {
                        var userTimeZoneProperty = DomainObjectFactory.CreateUserTimeZoneProperty();
                        UserTimeZoneProperty utzp = (from tzp in timezoneprop where tzp.OrderInGroup == i select tzp).First();
                        userTimeZoneProperty.UserTimeZoneId = userTimeZone.Id;
                        userTimeZoneProperty.TimeZoneId = userTimeZone.TimeZoneId;
                        userTimeZoneProperty.OrderInGroup = i;
                        userTimeZoneProperty.ValidFrom = utzp.ValidFrom;
                        userTimeZoneProperty.ValidTo = utzp.ValidTo;
                        userTimeZoneProperty.IsMonday = utzp.IsMonday;
                        userTimeZoneProperty.IsTuesday = utzp.IsTuesday;
                        userTimeZoneProperty.IsWednesday = utzp.IsWednesday;
                        userTimeZoneProperty.IsThursday = utzp.IsThursday;
                        userTimeZoneProperty.IsFriday = utzp.IsFriday;
                        userTimeZoneProperty.IsSaturday = utzp.IsSaturday;
                        userTimeZoneProperty.IsSunday = utzp.IsSunday;

                        userTimeZone.UserTimeZoneProperties.Add(userTimeZoneProperty);
                    }
                }
                else
                {
                    var userTimeZoneIds = (from tz in _userTimeZoneRepository.FindAll(x => x.IsDeleted == true && x.CompanyId == null) select tz);
                    if (userTimeZoneIds.Count() != 0)
                    {
                        userTimeZoneIds.First().Name = name;
                        userTimeZoneIds.First().IsDeleted = false;
                    }
                    else
                    {
                        var maxid = (from tz in _userTimeZoneRepository.FindAll() where tz.CompanyId == null select tz.TimeZoneId).Max();
                        userTimeZone.CompanyId = CurrentUser.Get().IsSuperAdmin ? null : CurrentUser.Get().CompanyId;
                        userTimeZone.UserId = CurrentUser.Get().Id;
                        userTimeZone.Name = name;
                        userTimeZone.Uid = Guid.NewGuid();
                        userTimeZone.IsOriginal = true;
                        userTimeZone.IsDeleted = false;
                        userTimeZone.IsCompanySpecific = CurrentUser.Get().IsSuperAdmin;

                        if (maxid != null)
                        { userTimeZone.TimeZoneId = maxid + 1; }
                        else { userTimeZone.TimeZoneId = 1; }



                    }

                    for (int i = 0; i < 4; i++)
                    {
                        var validFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                        var validTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
                        UserTimeZoneProperty userTimeZoneProperty = DomainObjectFactory.CreateUserTimeZoneProperty();
                        userTimeZoneProperty.UserTimeZoneId = userTimeZone.Id;
                        userTimeZoneProperty.TimeZoneId = userTimeZone.TimeZoneId;
                        userTimeZoneProperty.OrderInGroup = i;
                        userTimeZoneProperty.ValidFrom = validFrom;
                        userTimeZoneProperty.ValidTo = validTo;
                        userTimeZoneProperty.IsMonday = true;
                        userTimeZoneProperty.IsTuesday = false;
                        userTimeZoneProperty.IsWednesday = false;
                        userTimeZoneProperty.IsThursday = false;
                        userTimeZoneProperty.IsFriday = false;
                        userTimeZoneProperty.IsSaturday = false;
                        userTimeZoneProperty.IsSunday = false;
                        userTimeZone.UserTimeZoneProperties.Add(userTimeZoneProperty);
                    }
                }
                _userTimeZoneRepository.Add(userTimeZone);
                work.Commit();
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneCreated", new List<string> { name, CurrentUser.Get().LoginName }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      message.ToString());
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, userTimeZone.Id, UpdateParameter.UserTimeZoneUpdate, ControllerStatus.Created, name);
                return userTimeZone.Id;
            }
        }

        public int CreateUserTimeZoneProperty(int timeZoneId, int order)
        {
            var validFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            var validTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
            return CreateUserTimeZoneProperty(timeZoneId, order, validFrom, validTo, true, false, false, false, false, false, false);
        }

        public int CreateUserTimeZoneProperty(int userTimeZoneId, int order, DateTime? validFrom, DateTime? validTo, bool isMonday, bool isTuesday, bool isWednesday, bool isThursday, bool isFriday, bool isSaturday, bool isSunday)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZoneProperty userTimeZoneProperty = DomainObjectFactory.CreateUserTimeZoneProperty();

                userTimeZoneProperty.UserTimeZoneId = userTimeZoneId;
                userTimeZoneProperty.TimeZoneId = null;
                userTimeZoneProperty.OrderInGroup = order;
                userTimeZoneProperty.ValidFrom = validFrom;
                userTimeZoneProperty.ValidTo = validTo;
                userTimeZoneProperty.IsMonday = isMonday;
                userTimeZoneProperty.IsTuesday = isTuesday;
                userTimeZoneProperty.IsWednesday = isWednesday;
                userTimeZoneProperty.IsThursday = isThursday;
                userTimeZoneProperty.IsFriday = isFriday;
                userTimeZoneProperty.IsSaturday = isSaturday;
                userTimeZoneProperty.IsSunday = isSunday;

                _userTimeZonePropertyRepository.Add(userTimeZoneProperty);

                work.Commit();

                userTimeZoneProperty = _userTimeZonePropertyRepository.FindById(userTimeZoneProperty.Id);

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUTZPropertyforUTZCreated", new List<string> { userTimeZoneProperty.UserTimeZone.Name, userTimeZoneProperty.UserTimeZone.User.LoginName }));

                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageOrderInGroup", new List<string> { userTimeZoneProperty.OrderInGroup.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom", new List<string> { userTimeZoneProperty.ValidFrom.HasValue ? userTimeZoneProperty.ValidFrom.Value.ToString("HH:mm") : "" }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo", new List<string> { userTimeZoneProperty.ValidTo.HasValue ? userTimeZoneProperty.ValidTo.Value.ToString("HH:mm") : "" }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsMonday", new List<string> { userTimeZoneProperty.IsMonday.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsTuesday", new List<string> { userTimeZoneProperty.IsTuesday.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsWednesday", new List<string> { userTimeZoneProperty.IsWednesday.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsThursday", new List<string> { userTimeZoneProperty.IsThursday.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsFriday", new List<string> { userTimeZoneProperty.IsFriday.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSaturday", new List<string> { userTimeZoneProperty.IsSaturday.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSunday", new List<string> { userTimeZoneProperty.IsSunday.ToString() }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());

                return userTimeZoneProperty.Id;
            }
        }

        public void UpdateUserTimeZone(int id, string name)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZone timeZone = _userTimeZoneRepository.FindById(id);
                string old_name = timeZone.Name;
                timeZone.Name = name;
                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneNameChanged", new List<string> { old_name, name }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      message.ToString());

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserTimeZoneUpdate, ControllerStatus.Edited, name);
            }
        }

        public void DeleteUserTimeZone(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZone userTimeZone = _userTimeZoneRepository.FindById(id);



                userTimeZone.IsDeleted = true;
                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneDeleted", new List<string> { userTimeZone.Name }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserTimeZoneUpdate, ControllerStatus.Deleted, userTimeZone.Name);
            }
        }

        public void ToggleUserZone(int id, int day)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZoneProperty userTimeZoneProperty = _userTimeZonePropertyRepository.FindById(id);
                var utz = userTimeZoneProperty.UserTimeZone;

                var utzps =
                    _userTimeZonePropertyRepository.FindAll().Where(
                        x => !x.UserTimeZone.IsDeleted && x.UserTimeZone.TimeZoneId == utz.TimeZoneId);

                if (CurrentUser.Get().IsCompanyManager)
                {
                    if (true/*userTimeZoneProperty.TimeZone.IsDefault*/)
                    {
                        utzps = utzps.Where(x => x.UserTimeZone.User.CompanyId == CurrentUser.Get().CompanyId);
                    }
                    else
                    {
                        //utzps = utzps.Where(x => x.UserTimeZone.User.CompanyId == CurrentUser.Get().CompanyId || (_userRepository.IsBuildingAdmin(x.UserTimeZone.UserId) || _userRepository.IsSuperAdmin(x.UserTimeZone.UserId)));
                    }
                }
                bool dayVal = false;
                string dayName = string.Empty;
                switch (day)
                {
                    case 1:
                        dayVal = userTimeZoneProperty.IsMonday;
                        userTimeZoneProperty.IsMonday = !userTimeZoneProperty.IsMonday;
                        dayName = "LogMessageMonday";
                        break;
                    case 2:
                        dayVal = userTimeZoneProperty.IsTuesday;
                        userTimeZoneProperty.IsTuesday = !userTimeZoneProperty.IsTuesday;
                        dayName = "LogMessageTuesday";
                        break;
                    case 3:
                        dayVal = userTimeZoneProperty.IsWednesday;
                        userTimeZoneProperty.IsWednesday = !userTimeZoneProperty.IsWednesday;
                        dayName = "LogMessageWednesday";
                        break;
                    case 4:
                        dayVal = userTimeZoneProperty.IsThursday;
                        userTimeZoneProperty.IsThursday = !userTimeZoneProperty.IsThursday;
                        dayName = "LogMessageThursday";
                        break;
                    case 5:
                        dayVal = userTimeZoneProperty.IsFriday;
                        userTimeZoneProperty.IsFriday = !userTimeZoneProperty.IsFriday;
                        dayName = "LogMessageFriday";
                        break;
                    case 6:
                        dayVal = userTimeZoneProperty.IsSaturday;
                        userTimeZoneProperty.IsSaturday = !userTimeZoneProperty.IsSaturday;
                        dayName = "LogMessageSaturday";
                        break;
                    case 7:
                        dayVal = userTimeZoneProperty.IsSunday;
                        userTimeZoneProperty.IsSunday = !userTimeZoneProperty.IsSunday;
                        dayName = "LogMessageSunday";
                        break;
                }

                foreach (var timeZoneProperty in utzps)
                {
                    if (timeZoneProperty.Id != id && timeZoneProperty.OrderInGroup == userTimeZoneProperty.OrderInGroup)
                    {
                        timeZoneProperty.IsMonday = userTimeZoneProperty.IsMonday;
                        timeZoneProperty.IsTuesday = userTimeZoneProperty.IsTuesday;
                        timeZoneProperty.IsWednesday = userTimeZoneProperty.IsWednesday;
                        timeZoneProperty.IsThursday = userTimeZoneProperty.IsThursday;
                        timeZoneProperty.IsFriday = userTimeZoneProperty.IsFriday;
                        timeZoneProperty.IsSaturday = userTimeZoneProperty.IsSaturday;
                        timeZoneProperty.IsSunday = userTimeZoneProperty.IsSunday;
                    }
                }
                /*
                                if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsSuperAdmin || (CurrentUser.Get().IsCompanyManager && userTimeZoneProperty.UserTimeZone.IsCompanySpecific))
                                {
                                    var tzp =
                                        _timeZonePropertyRepository.FindAll().Where(
                                            x =>
                                            x.TimeZoneId == userTimeZoneProperty.UserTimeZone.TimeZoneId &&
                                            x.OrderInGroup == userTimeZoneProperty.OrderInGroup).FirstOrDefault();

                                    if (tzp != null && !tzp.TimeZone.IsDefault)
                                    {
                                        tzp.ValidFrom = userTimeZoneProperty.ValidFrom;
                                        tzp.ValidTo = userTimeZoneProperty.ValidTo;
                                        tzp.IsMonday = userTimeZoneProperty.IsMonday;
                                        tzp.IsTuesday = userTimeZoneProperty.IsTuesday;
                                        tzp.IsWednesday = userTimeZoneProperty.IsWednesday;
                                        tzp.IsThursday = userTimeZoneProperty.IsThursday;
                                        tzp.IsFriday = userTimeZoneProperty.IsFriday;
                                        tzp.IsSaturday = userTimeZoneProperty.IsSaturday;
                                        tzp.IsSunday = userTimeZoneProperty.IsSunday;
                                    }
                                }
                                */
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZonePropertyChanged", new List<string> { userTimeZoneProperty.UserTimeZone.Name, userTimeZoneProperty.UserTimeZone.User.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml(dayName, null));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMesageCommonIsChanged", new List<string> { dayVal.ToString(), (!dayVal).ToString() }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, userTimeZoneProperty.UserTimeZoneId, UpdateParameter.UserTimeZoneUpdate, ControllerStatus.Created, "test1"/*userTimeZoneProperty.TimeZone.Name*/);

                work.Commit();
            }
        }

        public void UpdateUserFromTime(int id, DateTime? time)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZoneProperty userTimeZoneProperty = _userTimeZonePropertyRepository.FindById(id);
                var oldTime = userTimeZoneProperty.ValidFrom;
                userTimeZoneProperty.ValidFrom = time;
                /*if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsSuperAdmin || (CurrentUser.Get().IsCompanyManager && userTimeZoneProperty.UserTimeZone.IsCompanySpecific))
				{
					var tzp =
						_timeZonePropertyRepository.FindAll().Where(
							x =>
							x.TimeZoneId == userTimeZoneProperty.UserTimeZone.TimeZoneId &&
							x.OrderInGroup == userTimeZoneProperty.OrderInGroup).FirstOrDefault();

					if (tzp != null && !tzp.TimeZone.IsDefault)
					{
						tzp.ValidFrom = userTimeZoneProperty.ValidFrom;
						tzp.ValidTo = userTimeZoneProperty.ValidTo;
						tzp.IsMonday = userTimeZoneProperty.IsMonday;
						tzp.IsTuesday = userTimeZoneProperty.IsTuesday;
						tzp.IsWednesday = userTimeZoneProperty.IsWednesday;
						tzp.IsThursday = userTimeZoneProperty.IsThursday;
						tzp.IsFriday = userTimeZoneProperty.IsFriday;
						tzp.IsSaturday = userTimeZoneProperty.IsSaturday;
						tzp.IsSunday = userTimeZoneProperty.IsSunday;
					}
				}
                */
                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZonePropertyChanged", new List<string> { userTimeZoneProperty.UserTimeZone.Name, userTimeZoneProperty.UserTimeZone.User.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMesageValidFromChanged", new List<string> {oldTime == null ? "" : oldTime.Value.ToString("HH:mm"),
                                        userTimeZoneProperty.ValidFrom == null ? "" : userTimeZoneProperty.ValidFrom.Value.ToString("HH:mm")}));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, userTimeZoneProperty.UserTimeZoneId, UpdateParameter.UserTimeZoneUpdate, ControllerStatus.Created, userTimeZoneProperty.UserTimeZone.Name);
            }
        }

        public void UpdateUserToTime(int id, DateTime? time)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZoneProperty userTimeZoneProperty = _userTimeZonePropertyRepository.FindById(id);
                var oldTime = userTimeZoneProperty.ValidTo;
                userTimeZoneProperty.ValidTo = time;
                /*
				if (CurrentUser.Get().IsBuildingAdmin || CurrentUser.Get().IsSuperAdmin || (CurrentUser.Get().IsCompanyManager && userTimeZoneProperty.UserTimeZone.IsCompanySpecific))
				{
					var tzp =
						_timeZonePropertyRepository.FindAll().Where(
							x =>
							x.TimeZoneId == userTimeZoneProperty.UserTimeZone.TimeZoneId &&
							x.OrderInGroup == userTimeZoneProperty.OrderInGroup).FirstOrDefault();

					if (tzp != null && !tzp.TimeZone.IsDefault)
					{
						tzp.ValidFrom = userTimeZoneProperty.ValidFrom;
						tzp.ValidTo = userTimeZoneProperty.ValidTo;
						tzp.IsMonday = userTimeZoneProperty.IsMonday;
						tzp.IsTuesday = userTimeZoneProperty.IsTuesday;
						tzp.IsWednesday = userTimeZoneProperty.IsWednesday;
						tzp.IsThursday = userTimeZoneProperty.IsThursday;
						tzp.IsFriday = userTimeZoneProperty.IsFriday;
						tzp.IsSaturday = userTimeZoneProperty.IsSaturday;
						tzp.IsSunday = userTimeZoneProperty.IsSunday;
					}
				}
                */
                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZonePropertyChanged", new List<string> { userTimeZoneProperty.UserTimeZone.Name, userTimeZoneProperty.UserTimeZone.User.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMesageValidToChanged", new List<string> {oldTime == null ? "" : oldTime.Value.ToString("HH:mm"),
                                        userTimeZoneProperty.ValidTo == null ? "" : userTimeZoneProperty.ValidTo.Value.ToString("HH:mm")}));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, userTimeZoneProperty.UserTimeZoneId, UpdateParameter.UserTimeZoneUpdate, ControllerStatus.Created, userTimeZoneProperty.UserTimeZone.Name);
            }
        }

        public void RecoveryUserTimeZone(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZone DUTZ = _userTimeZoneRepository.FindById(id);
                DUTZ.IsDeleted = false;
                work.Commit();
            }
        }

        # region Group Change

        public void GroupUpdateName(int utzId, string Name)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                int[] affectedUtzIdsLevel1 = (from utz in _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.ParentUserTimeZoneId.HasValue && x.ParentUserTimeZoneId.Value == utzId) select utz.Id).ToArray();
                int[] affectedUtzIdsLevel2 = (from utz in _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.ParentUserTimeZoneId.HasValue && affectedUtzIdsLevel1.Contains(x.ParentUserTimeZoneId.Value)) select utz.Id).ToArray();
                int[] affectedUtzIds = affectedUtzIdsLevel1.Union(affectedUtzIdsLevel2).ToArray();

                var affectedUtzs = _userTimeZoneRepository.FindAll(x => affectedUtzIds.Contains(x.Id)).ToList();

                foreach (var utz in affectedUtzs)
                {
                    utz.Name = Name;
                }

                work.Commit();
            }
        }

        #endregion

    }
}