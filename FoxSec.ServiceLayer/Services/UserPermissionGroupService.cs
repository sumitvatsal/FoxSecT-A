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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace FoxSec.ServiceLayer.Services
{
    internal class UserPermissionGroupService : ServiceBase, IUserPermissionGroupService
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);

        private readonly IUserRepository _userRepository;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IUserTimeZonePropertyRepository _userTimeZonePropertyRepository;
        private readonly IUserPermissionGroupRepository _userPermissionGroupRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly ILogService _logService;
        private readonly IControllerUpdateService _controllerUpdateService;
        string flag = "";
        public UserPermissionGroupService(ICurrentUser currentUser,
                                            IUserRepository userRepository,
                                            IDomainObjectFactory domainObjectFactory,
                                            IEventAggregator eventAggregator,
                                            IUserTimeZoneRepository userTimeZoneRepository,
                                            IUserTimeZonePropertyRepository userTimeZonePropertyRepository,
                                            IUserPermissionGroupRepository userPermissionGroupRepository,
                                            IUserPermissionGroupTimeZoneRepository userPermissionGroupTimeZoneRepository,
                                            IControllerUpdateService controllerUpdateService,
                                            ILogService logService) : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _userRepository = userRepository;
            _userTimeZoneRepository = userTimeZoneRepository;
            _userTimeZonePropertyRepository = userTimeZonePropertyRepository;
            _userPermissionGroupRepository = userPermissionGroupRepository;
            _userPermissionGroupTimeZoneRepository = userPermissionGroupTimeZoneRepository;
            _logService = logService;
            _controllerUpdateService = controllerUpdateService;
        }

        public int CreateUserPermissionGroup(string name, int? copyFromPgId, int? defaultTimeZoneId, List<int> buildingObjectIds, List<int> selectedBuildingObjectIds)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserPermissionGroup newUpg = DomainObjectFactory.CreateUserPermissionGroup();

                newUpg.UserId = CurrentUser.Get().Id;
                newUpg.Name = name;
                newUpg.PermissionIsActive = false;
                newUpg.IsDeleted = false;

                if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsBuildingAdmin)
                {
                    newUpg.IsOriginal = true;
                }
                else
                {
                    newUpg.IsOriginal = false;
                }

                if (copyFromPgId.HasValue)
                {
                    newUpg.DefaultUserTimeZoneId = _userPermissionGroupRepository.FindById(copyFromPgId.Value).DefaultUserTimeZoneId;
                }
                else
                if (defaultTimeZoneId.HasValue)
                {
                    newUpg.DefaultUserTimeZoneId = defaultTimeZoneId.Value;
                }

                _userPermissionGroupRepository.Add(newUpg);
                work.Commit();
                newUpg = _userPermissionGroupRepository.FindById(newUpg.Id);
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePermissionGroupCreated", new List<string> { newUpg.Name }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDefaultTimeZoneName", new List<string> { _userTimeZoneRepository.FindById(newUpg.DefaultUserTimeZoneId).Name }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      message.ToString());

                if (defaultTimeZoneId.HasValue)
                {
                    if (selectedBuildingObjectIds == null)
                    {
                        selectedBuildingObjectIds = new List<int>();
                    }

                    if (buildingObjectIds != null)
                    {
                        foreach (int bId in buildingObjectIds)
                        {
                            if (selectedBuildingObjectIds.Contains(bId))
                            {
                                UserPermissionGroupTimeZone upgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();

                                upgtz.UserPermissionGroupId = newUpg.Id;
                                upgtz.UserTimeZoneId = defaultTimeZoneId.Value;
                                upgtz.BuildingObjectId = bId;
                                upgtz.IsArming = true;
                                upgtz.IsDefaultArming = true;
                                upgtz.IsDisarming = true;
                                upgtz.IsDefaultDisarming = true;
                                upgtz.Active = selectedBuildingObjectIds.Contains(bId);
                                upgtz.IsDeleted = false;

                                _userPermissionGroupTimeZoneRepository.Add(upgtz);
                            }
                        }

                        #region old code
                        //foreach (int bId in buildingObjectIds)
                        //{
                        //    UserPermissionGroupTimeZone upgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();

                        //    upgtz.UserPermissionGroupId = newUpg.Id;
                        //    upgtz.UserTimeZoneId = defaultTimeZoneId.Value;
                        //    upgtz.BuildingObjectId = bId;
                        //    upgtz.IsArming = true;
                        //    upgtz.IsDefaultArming = true;
                        //    upgtz.IsDisarming = true;
                        //    upgtz.IsDefaultDisarming = true;
                        //    upgtz.Active = selectedBuildingObjectIds.Contains(bId);
                        //    upgtz.IsDeleted = false;

                        //    _userPermissionGroupTimeZoneRepository.Add(upgtz);
                        //}
                        #endregion
                    }
                }
                else
                if (copyFromPgId.HasValue)
                {
                    //UserPermissionGroup cfPG = _userPermissionGroupRepository.FindById(copyFromPgId.Value);
                    buildingObjectIds = _userPermissionGroupTimeZoneRepository.FindByPGId(copyFromPgId.Value).Select(x => x.BuildingObjectId).ToList();
                    foreach (int bId in buildingObjectIds)
                    {
                        UserPermissionGroupTimeZone upgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();
                        //var cfupgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == copyFromPgId && x.BuildingObjectId == bId).FirstOrDefault();
                        var cfupgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(copyFromPgId.Value).Where(x => !x.IsDeleted && x.BuildingObjectId == bId).FirstOrDefault();
                        upgtz.UserPermissionGroupId = newUpg.Id;
                        upgtz.UserTimeZoneId = cfupgtz.UserTimeZoneId;
                        upgtz.BuildingObjectId = bId;
                        upgtz.IsArming = cfupgtz.IsArming;
                        upgtz.IsDefaultArming = cfupgtz.IsDefaultArming;
                        upgtz.IsDisarming = cfupgtz.IsDisarming;
                        upgtz.IsDefaultDisarming = cfupgtz.IsDefaultDisarming;
                        upgtz.Active = cfupgtz.Active;
                        upgtz.IsDeleted = false;

                        _userPermissionGroupTimeZoneRepository.Add(upgtz);
                    }
                }

                work.Commit();
                /*
				var upgtzs =
					_userPermissionGroupTimeZoneRepository.FindAll().Where(pgtzone => !pgtzone.IsDeleted && buildingObjectIds != null
						&& buildingObjectIds.Contains(pgtzone.BuildingObjectId) && pgtzone.UserPermissionGroupId == newUpg.Id);
                */
                var upgtzs =
                    _userPermissionGroupTimeZoneRepository.FindByPGId(newUpg.Id).Where(pgtzone => !pgtzone.IsDeleted && buildingObjectIds != null
                        && buildingObjectIds.Contains(pgtzone.BuildingObjectId));

                message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGCreated", new List<string> { newUpg.Name, CurrentUser.Get().LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneName", new List<string> { newUpg.UserTimeZone.Name }));
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, newUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Created, name);
                //topelt ei ole vaja	_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, newUpg.Id, UpdateParameter.SpecificTimeZoneChange, ControllerStatus.Created, string.Format("Time zone for permission group '{0}' is '{1}'", name, _userTimeZoneRepository.FindById(newUpg.DefaultUserTimeZoneId).Name));
                //aaa kui kiiruse probleemid ,siis siin ei te for tsyklit
                work.Commit();
                foreach (var pgtz in upgtzs)
                {
                    //message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObject", new List<string> { pgtz.BuildingObject.Description }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsArming", new List<string> { pgtz.IsArming.ToString() }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDisarming", new List<string> { pgtz.IsDisarming.ToString() }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultArming", new List<string> { pgtz.IsDefaultArming.ToString() }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultDisarming", new List<string> { pgtz.IsDefaultDisarming.ToString() }));
                    /* ei ole vaja if (pgtz.Active)
					{
						_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, pgtz.Id,
						                                                UpdateParameter.UserSpecificPermissionChange,
						                                                ControllerStatus.Created,
						                                                string.Format(
						                                                	"Building object '{0}' assigned to permision group '{1}'",
						                                                	pgtz.BuildingObject.BuildingObjectType.Description + " " +
						                                                	pgtz.BuildingObject.Description, name));
						if (pgtz.BuildingObject.TypeId == 2)
							_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, pgtz.Id,
							                                                UpdateParameter.RoomPermissionChange, ControllerStatus.Created,
							                                                string.Format(
							                                                	"Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.",
							                                                	pgtz.IsArming, pgtz.IsDisarming, pgtz.IsDefaultArming,
							                                                	pgtz.IsDefaultDisarming));
					} */
                }

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                        message.ToString());

                return newUpg.Id;
            }
        }

        private bool IsOriginalUserTimeZoneAlreadyExists(int userId, Guid uid)
        {
            return _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserId == userId && x.Uid == uid).FirstOrDefault() != null;
        }

        private UserTimeZone GetOriginalUserTimeZone(int userId, Guid uid)
        {
            return _userTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserId == userId && x.Uid == uid).FirstOrDefault();
        }
        /*
        private UserTimeZone CreateUserOriginalTimeZone(int userId, UserTimeZone origUtz)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserTimeZone newUserTz = DomainObjectFactory.CreateUserTimeZone();
                newUserTz.UserId = userId;
                newUserTz.TimeZoneId = origUtz.TimeZoneId;
                newUserTz.ParentUserTimeZoneId = origUtz.Id;
                newUserTz.Name = origUtz.Name;
                newUserTz.Uid = origUtz.Uid;
                newUserTz.IsOriginal = true;
                newUserTz.IsDeleted = false;
				newUserTz.IsCompanySpecific = false;

                _userTimeZoneRepository.Add(newUserTz);
                work.Commit();

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneCreated", new List<string> { newUserTz.Name, CurrentUser.Get().LoginName }));


                _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                  message.ToString());

                var utzProperties = _userTimeZonePropertyRepository.FindAll(x => x.UserTimeZoneId == origUtz.Id).ToList();

                foreach (var utzp in utzProperties)
                {
                    UserTimeZoneProperty newUtzp = DomainObjectFactory.CreateUserTimeZoneProperty();

                    newUtzp.UserTimeZoneId = newUserTz.Id;
                    newUtzp.TimeZoneId = utzp.TimeZoneId;
                    newUtzp.OrderInGroup = utzp.OrderInGroup;
                    newUtzp.ValidFrom = utzp.ValidFrom;
                    newUtzp.ValidTo = utzp.ValidTo;
                    newUtzp.IsMonday = utzp.IsMonday;
                    newUtzp.IsTuesday = utzp.IsTuesday;
                    newUtzp.IsWednesday = utzp.IsWednesday;
                    newUtzp.IsThursday = utzp.IsThursday;
                    newUtzp.IsFriday = utzp.IsFriday;
                    newUtzp.IsSaturday = utzp.IsSaturday;
                    newUtzp.IsSunday = utzp.IsSunday;

                    _userTimeZonePropertyRepository.Add(newUtzp);
                    work.Commit();

					message = new XElement(XMLLogLiterals.LOG_MESSAGE);
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUTZPropertyforUTZCreated", new List<string> { newUserTz.Name, CurrentUser.Get().LoginName }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTimeZoneId", new List<string> { newUtzp.TimeZoneId.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageOrderInGroup", new List<string> { newUtzp.OrderInGroup.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom", new List<string> { newUtzp.ValidFrom.HasValue ? newUtzp.ValidFrom.Value.ToString("HH:mm") : "" }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo", new List<string> { newUtzp.ValidTo.HasValue ? newUtzp.ValidTo.Value.ToString("HH:mm") : "" }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsMonday", new List<string> { newUtzp.IsMonday.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsTuesday", new List<string> { newUtzp.IsTuesday.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsWednesday", new List<string> { newUtzp.IsWednesday.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsThursday", new List<string> { newUtzp.IsThursday.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsFriday", new List<string> { newUtzp.IsFriday.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSaturday", new List<string> { newUtzp.IsSaturday.ToString() }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsSunday", new List<string> { newUtzp.IsSunday.ToString() }));
                    
                    _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                  message.ToString());
                }

                return _userTimeZoneRepository.FindById(newUserTz.Id);
            }
        }
        */
        public int AssignUserPermissionGroup(int userId, int upgId)
        {/*
            if (_userRepository.IsSuperAdmin(userId) || _userRepository.IsBuildingAdmin(userId))
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {
                    var userPermissionGroups = _userPermissionGroupRepository.FindAll(x => x.UserId == userId && !x.IsDeleted && x.Id != upgId).ToList();
                    foreach (var upg in userPermissionGroups)
                    {
      
                        upg.PermissionIsActive = false;
                    }

                    var newActivePermissionGroup = _userPermissionGroupRepository.FindById(upgId);
                    newActivePermissionGroup.PermissionIsActive = true;

                    work.Commit();
                    return upgId;
                }
            }
            else*/
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {
                    //var userPermissionGroups = _userPermissionGroupRepository.FindAll(x => x.UserId == userId && !x.IsDeleted).ToList();
                    var userPermissionGroups = _userPermissionGroupRepository.FindByUserId(userId).Where(x => !x.IsDeleted).ToList();

                    foreach (var upg in userPermissionGroups)
                    {
                        upg.PermissionIsActive = false;
                    }

                    UserPermissionGroup origUpg = _userPermissionGroupRepository.FindById(upgId);
                    UserPermissionGroup newUpg = DomainObjectFactory.CreateUserPermissionGroup();
                    /* UserTimeZone origDefaultUserTz = _userTimeZoneRepository.FindById(origUpg.DefaultUserTimeZoneId);

                     if (!IsOriginalUserTimeZoneAlreadyExists(userId, origDefaultUserTz.Uid))
                     {
                         UserTimeZone newDefaultUtz = CreateUserOriginalTimeZone(userId, origDefaultUserTz);
                         newUpg.DefaultUserTimeZoneId = newDefaultUtz.Id;
                     }
                     else
                     {
                         newUpg.DefaultUserTimeZoneId = GetOriginalUserTimeZone(userId, origDefaultUserTz.Uid).Id;
                     }
                     */
                    newUpg.DefaultUserTimeZoneId = origUpg.DefaultUserTimeZoneId;
                    newUpg.UserId = userId;
                    newUpg.ParentUserPermissionGroupId = upgId;
                    newUpg.Name = origUpg.Name;
                    newUpg.IsOriginal = true;
                    newUpg.PermissionIsActive = true;
                    newUpg.IsDeleted = false;

                    _userPermissionGroupRepository.Add(newUpg);
                    work.Commit();

                    newUpg = _userPermissionGroupRepository.FindById(newUpg.Id);

                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUPGForUserCreated", new List<string> { newUpg.Name, newUpg.User.LoginName }));

                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      message.ToString());

                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, newUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Created, string.Format("created user permission group '{0}' for user '{1}'", newUpg.Name, newUpg.User.LoginName));

                    var upgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.Active == true).ToList();

                    foreach (var upgtz in upgTimeZones)
                    {
                        UserPermissionGroupTimeZone newUpgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();
                        /* UserTimeZone originalUtz = _userTimeZoneRepository.FindById(upgtz.UserTimeZoneId);

                         if (!IsOriginalUserTimeZoneAlreadyExists(userId, originalUtz.Uid))
                         {
                             UserTimeZone newUserTimeZone = CreateUserOriginalTimeZone(userId, originalUtz);
                             newUpgtz.UserTimeZoneId = newUserTimeZone.Id;
                         }
                         else
                         {
                             newUpgtz.UserTimeZoneId = GetOriginalUserTimeZone(userId, originalUtz.Uid).Id;
                         }
                         */
                        newUpgtz.UserTimeZoneId = upgtz.UserTimeZoneId; //newUpg.DefaultUserTimeZoneId;
                        newUpgtz.UserPermissionGroupId = newUpg.Id;
                        newUpgtz.BuildingObjectId = upgtz.BuildingObjectId;
                        newUpgtz.IsArming = upgtz.IsArming;
                        newUpgtz.IsDefaultArming = upgtz.IsDefaultArming;
                        newUpgtz.IsDisarming = upgtz.IsDisarming;
                        newUpgtz.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                        newUpgtz.Active = upgtz.Active;
                        newUpgtz.IsDeleted = false;

                        _userPermissionGroupTimeZoneRepository.Add(newUpgtz);
                        work.Commit();
                        /*
                        newUpgtz = _userPermissionGroupTimeZoneRepository.FindById(newUpgtz.Id);

                        message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFUPGCreated", new List<string> { newUpg.Name, newUpg.User.LoginName }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneName", new List<string> { newUpg.UserTimeZone.Name }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObject", new List<string> { newUpgtz.BuildingObject.Description }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsArming", new List<string> { newUpgtz.IsArming.ToString() }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDisarming", new List<string> { newUpgtz.IsDisarming.ToString() }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultArming", new List<string> { newUpgtz.IsDefaultArming.ToString() }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultDisarming", new List<string> { newUpgtz.IsDefaultDisarming.ToString() }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActive", new List<string> { newUpgtz.Active.ToString() }));

                        _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      message.ToString());

						_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, newUpgtz.Id, UpdateParameter.UpgBuildingObjectTimeZoneChange, ControllerStatus.Created, string.Format(" Time zone {0} assigned for user permission group {1} and building object '{2} {3}'", newUpgtz.UserTimeZone.Name, newUpgtz.UserPermissionGroup.Name, newUpgtz.BuildingObject.BuildingObjectType.Name, newUpgtz.BuildingObject.Description ));
                   
                         */
                    }
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, newUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Created, string.Format("created user permission group '{0}' for user '{1}'", newUpg.Name, newUpg.User.LoginName));

                    work.Commit();
                    return newUpg.Id;
                }
            }
        }

        public bool IsUserHasActivePermission(int userId)
        {

            //return _userPermissionGroupRepository.FindAll(x => x.UserId == userId && !x.IsDeleted && x.PermissionIsActive) != null;

            return _userPermissionGroupRepository.FindByUserId(userId).Where(x => !x.IsDeleted && x.PermissionIsActive) != null;
        }

        public int ChangeUserPermissionGroup(int userId, int newUpgId) //zzzz
        {
            //if (_userRepository.IsSuperAdmin(userId) || _userRepository.IsBuildingAdmin(userId))
            /* {
                 using (IUnitOfWork work = UnitOfWork.Begin())
                 {
                     var userPermissionGroups = _userPermissionGroupRepository.FindAll(x => x.UserId == userId && !x.IsDeleted && x.Id != newUpgId).ToList();
                     foreach (var upg in userPermissionGroups)
                     {
                         upg.PermissionIsActive = false;
                     }

                     var newActivePermissionGroup = _userPermissionGroupRepository.FindById(newUpgId);
                     newActivePermissionGroup.PermissionIsActive = true;

                     var message = new XElement(XMLLogLiterals.LOG_MESSAGE); //i19.08.2012
                     message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneChanged", new List<string> { newActivePermissionGroup.Name, newActivePermissionGroup.User.LoginName }));
                     message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNewTimeZoneName", new List<string> { newActivePermissionGroup.UserTimeZone.Name }));

                     _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, newActivePermissionGroup.Id, UpdateParameter.PermissionGroupChange, ControllerStatus.Edited, newActivePermissionGroup.Name);
                     _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                     message.ToString()); 

                     work.Commit();

                     return newUpgId;
                 }
             }*/
            //else
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {
                    //UserPermissionGroup origUpg = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.UserId == userId).FirstOrDefault();
                    UserPermissionGroup origUpg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => !x.IsDeleted).FirstOrDefault();
                    UserPermissionGroup newUpg = _userPermissionGroupRepository.FindById(newUpgId);
                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE); //i19.08.2012
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneChanged", new List<string> { origUpg.Name, origUpg.User.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserOldTimeZoneName", new List<string> { origUpg.UserTimeZone.Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNewTimeZoneName", new List<string> { newUpg.UserTimeZone.Name }));

                    origUpg.DefaultUserTimeZoneId = newUpg.DefaultUserTimeZoneId;
                    origUpg.ParentUserPermissionGroupId = newUpgId;
                    origUpg.Name = newUpg.Name;
                    origUpg.IsOriginal = true;
                    work.Commit();
                    //illi 25.12.1012 v16 change User permission 
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, newUpg.Name);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                     message.ToString());


                    var oldUpgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();

                    foreach (var oldUpgTz in oldUpgTimeZones)
                    {
                        oldUpgTz.Active = false;
                        oldUpgTz.IsDeleted = true;
                        /* var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                         message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFUPGDeleted", new List<string> { oldUpgTz.UserPermissionGroup.Name, oldUpgTz.UserPermissionGroup.User.LoginName }));
                         _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                         message.ToString()); */
                    }
                    var newUpgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGId(newUpg.Id).Where(x => !x.IsDeleted && x.Active == true).ToList();

                    foreach (var upgtz in newUpgTimeZones)
                    {
                        UserPermissionGroupTimeZone EUPGTZ = oldUpgTimeZones.FindAll(x => x.BuildingObjectId == upgtz.BuildingObjectId).FirstOrDefault();

                        if (EUPGTZ != null)
                        {
                            EUPGTZ.UserPermissionGroupId = origUpg.Id;
                            //EUPGTZ.BuildingObjectId = upgtz.BuildingObjectId;
                            EUPGTZ.IsArming = upgtz.IsArming;
                            EUPGTZ.IsDefaultArming = upgtz.IsDefaultArming;
                            EUPGTZ.IsDisarming = upgtz.IsDisarming;
                            EUPGTZ.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                            EUPGTZ.Active = true;
                            EUPGTZ.IsDeleted = false;
                            EUPGTZ.UserTimeZoneId = upgtz.UserTimeZoneId;//origUpg.DefaultUserTimeZoneId; 1.14.2013

                        }
                        else
                        {
                            UserPermissionGroupTimeZone newUpgtz;
                            newUpgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();
                            newUpgtz.UserTimeZoneId = upgtz.UserTimeZoneId;//origUpg.DefaultUserTimeZoneId; 1.14.2013 
                            newUpgtz.UserPermissionGroupId = origUpg.Id;
                            newUpgtz.BuildingObjectId = upgtz.BuildingObjectId;
                            newUpgtz.IsArming = upgtz.IsArming;
                            newUpgtz.IsDefaultArming = upgtz.IsDefaultArming;
                            newUpgtz.IsDisarming = upgtz.IsDisarming;
                            newUpgtz.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                            newUpgtz.Active = true;
                            newUpgtz.IsDeleted = false;

                            _userPermissionGroupTimeZoneRepository.Add(newUpgtz);
                        }
                        /*         var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFUPGCreated", new List<string> { origUpg.Name, origUpg.User.LoginName }));
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneName", new List<string> { upgtz.UserTimeZone.Name }));
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObject", new List<string> { upgtz.BuildingObject.Description }));
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsArming", new List<string> { upgtz.IsArming.ToString() }));
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDisarming", new List<string> { upgtz.IsDisarming.ToString() }));
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultArming", new List<string> { upgtz.IsDefaultArming.ToString() }));
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultDisarming", new List<string> { upgtz.IsDefaultDisarming.ToString() }));
                                 message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActive", new List<string> { upgtz.Active.ToString() }));

                                 _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                                 message.ToString()); */
                    }
                    work.Commit();
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, newUpg.Name);
                    work.Commit();
                    return origUpg.Id;

                }
            }
        }

        public int DelUserPermissionGroup(int userId, int newUpgId)
        {
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {

                    UserPermissionGroup origUpg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => !x.IsDeleted).FirstOrDefault();
                    UserPermissionGroup newUpg = _userPermissionGroupRepository.FindById(newUpgId);
                    if (!origUpg.Name.Contains(newUpg.Name)) { return origUpg.Id; }

                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneChanged", new List<string> { origUpg.Name, origUpg.User.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserOldTimeZoneName", new List<string> { origUpg.UserTimeZone.Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNewTimeZoneName", new List<string> { newUpg.UserTimeZone.Name }));

                    //origUpg.DefaultUserTimeZoneId = newUpg.DefaultUserTimeZoneId;
                    //origUpg.ParentUserPermissionGroupId = newUpgId;

                    origUpg.Name = origUpg.Name.Replace("++" + newUpg.Name, "");


                    origUpg.IsOriginal = true;
                    work.Commit();
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, newUpg.Name);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                     message.ToString());


                    var oldUpgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();
                    /*
                    foreach (var oldUpgTz in oldUpgTimeZones)
                    {
                        oldUpgTz.Active = false;
                        oldUpgTz.IsDeleted = true;
                     }
                     * */
                    var newUpgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGId(newUpg.Id).Where(x => !x.IsDeleted && x.Active == true).ToList();

                    foreach (var upgtz in newUpgTimeZones)
                    {
                        UserPermissionGroupTimeZone EUPGTZ = oldUpgTimeZones.FindAll(x => x.BuildingObjectId == upgtz.BuildingObjectId).FirstOrDefault();

                        if (EUPGTZ != null)
                        {

                            EUPGTZ.Active = false;
                            EUPGTZ.IsDeleted = true;


                        }

                    }
                    work.Commit();
                    return origUpg.Id;
                }

            }
        }

        //DeletePermissionFromUsers
        public int DelPermissionGroupFromUsers(int userId, int dltGroup_Id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                //get the user permission Group all details
                UserPermissionGroup origUpg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => !x.IsDeleted).FirstOrDefault();

                // UserPermissionGroup detailsOfGroupToDlt = _userPermissionGroupRepository.FindById(dltGroup_Id);
                UserPermissionGroup detailsOfGroupToDlt = _userPermissionGroupRepository.FindAll(x => x.Id == dltGroup_Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault();

                //if deleted group is not exist in the users group		
                if (!origUpg.Name.Contains(detailsOfGroupToDlt.Name)) { return origUpg.Id; }

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneChanged", new List<string> { origUpg.Name, origUpg.User.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserOldTimeZoneName", new List<string> { origUpg.UserTimeZone.Name }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNewTimeZoneName", new List<string> { detailsOfGroupToDlt.UserTimeZone.Name }));

                //origUpg.DefaultUserTimeZoneId = newUpg.DefaultUserTimeZoneId;
                //origUpg.ParentUserPermissionGroupId = newUpgId;
                // origUpg.Name = origUpg.Name.Replace("++" + newUpg.Name, "");

                var userCurrentPermissionName = origUpg.Name;
                var dltGroup_Name = detailsOfGroupToDlt.Name;

                if (origUpg.Name.Contains("++"))
                {
                    //Main group to be deleted... 
                    string[] sep = { "++" };
                    string[] allPermissionGroups = userCurrentPermissionName.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    var userNewPermissionName = " ";

                    if (origUpg.ParentUserPermissionGroupId == detailsOfGroupToDlt.Id)
                    {
                        //Get New PermissionGroup Name after delete
                        for (int i = 1; i <= allPermissionGroups.Length - 1; i++)
                        {
                            userNewPermissionName = userNewPermissionName + allPermissionGroups[i];
                            if (i != allPermissionGroups.Length - 1)
                            {
                                userNewPermissionName = userNewPermissionName + "++";
                            }
                        }

                        var newParentPermissionName = allPermissionGroups.ElementAt(1);
                        UserPermissionGroup parentIdRecord = _userPermissionGroupRepository.FindAll(x => x.Name == newParentPermissionName && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault();

                        //UPDATE Records in UserPermissionGroup TABLE
                        if (parentIdRecord != null)
                        {
                            origUpg.Name = userNewPermissionName;
                            origUpg.DefaultUserTimeZoneId = parentIdRecord.DefaultUserTimeZoneId;
                            origUpg.ParentUserPermissionGroupId = parentIdRecord.Id;
                            origUpg.Timestamp = parentIdRecord.Timestamp;
                            origUpg.IsOriginal = true;
                        }
                        // origUpg.IsDeleted = parentIdRecord.IsDeleted;
                        //  origUpg.PermissionIsActive = parentIdRecord.PermissionIsActive;

                        //UPDATE logs

                        work.Commit();
                        _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
                        _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                         message.ToString());

                        //UPDATE UserPermissionGroupTimeZones TABLE

                        // User's All BuildingObjId
                        var UserAllBuildingObjIDs = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();

                        //building Object ID of group to be delete
                        //var dlt_grpID = _userPermissionGroupRepository.FindAll(x => x.Name == detailsOfGroupToDlt.Name && x.Id == detailsOfGroupToDlt.Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault().Id;

                        var dlt_grpID = detailsOfGroupToDlt.Id;
                        var dltgrp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == dlt_grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList(); //1,2,3

                        foreach (var buildingObjId in dltgrp_buildingObjIDs)
                        {
                            UserPermissionGroupTimeZone EUPGTZ = UserAllBuildingObjIDs.FindAll(x => x.BuildingObjectId == buildingObjId).FirstOrDefault();

                            if (EUPGTZ != null)
                            {
                                EUPGTZ.Active = false;
                                EUPGTZ.IsDeleted = true;
                            }
                        }
                    }
                    else
                    {
                        var length = allPermissionGroups.Length;
                        if (allPermissionGroups[length - 1] == dltGroup_Name)
                        {
                            for (int i = 0; i <= allPermissionGroups.Length - 2; i++)
                            {
                                userNewPermissionName = userNewPermissionName + allPermissionGroups[i];
                                if (i != allPermissionGroups.Length - 2)
                                {
                                    userNewPermissionName = userNewPermissionName + "++";
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i <= allPermissionGroups.Length - 1; i++)
                            {
                                if (allPermissionGroups[i] != dltGroup_Name)
                                {
                                    userNewPermissionName = userNewPermissionName + allPermissionGroups[i];

                                    if (i != allPermissionGroups.Length - 1)
                                    {
                                        userNewPermissionName = userNewPermissionName + "++";
                                    }
                                }
                            }
                        }
                        //UPDATE Records in UserPermissionGroup TABLE

                        origUpg.Name = userNewPermissionName;
                        origUpg.IsOriginal = true;
                        //origUpg.IsDeleted = true;

                        work.Commit();
                        _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
                        _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                         message.ToString());

                        //UPDATE UserPermissionGroupTimeZones TABLE

                        //User's All BuildingObjId
                        var UserAllBuildingObjIDs = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();

                        //get dltdgrpId's building object id details 

                        //var dlt_grpID = _userPermissionGroupRepository.FindAll(x => x.Name == dltGroup_Name && x.Id == dltGroup_Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault().Id;
                        var dlt_grpID = detailsOfGroupToDlt.Id;
                        var dltgrp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == dlt_grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList();

                        foreach (var buildingObjId in dltgrp_buildingObjIDs)
                        {
                            UserPermissionGroupTimeZone EUPGTZ = UserAllBuildingObjIDs.FindAll(x => x.BuildingObjectId == buildingObjId).FirstOrDefault();

                            if (EUPGTZ != null)
                            {
                                EUPGTZ.Active = false;
                                EUPGTZ.IsDeleted = true;
                            }
                        }
                    }
                }//close  outer if
                else
                {
                    //User have  only 1 Permission Group
                    //UPDATE Records in UserPermissionGroup TABLE

                    origUpg.IsOriginal = true;
                    origUpg.IsDeleted = true;
                    origUpg.PermissionIsActive = false;

                    work.Commit();
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                     message.ToString());

                    //UPDATE logs

                    work.Commit();
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                     message.ToString());

                    //UPDATE UserPermissionGroupTimeZones TABLE

                    // User's All BuildingObjId
                    var UserAllBuildingObjIDs = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();

                    //building Object ID of group to be delete
                    //var dlt_grpID = _userPermissionGroupRepository.FindAll(x => x.Name == detailsOfGroupToDlt.Name && x.Id == detailsOfGroupToDlt.Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault().Id;

                    var dlt_grpID = detailsOfGroupToDlt.Id;
                    var dltgrp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == dlt_grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList(); //1,2,3

                    foreach (var buildingObjId in dltgrp_buildingObjIDs)
                    {
                        UserPermissionGroupTimeZone EUPGTZ = UserAllBuildingObjIDs.FindAll(x => x.BuildingObjectId == buildingObjId).FirstOrDefault();

                        if (EUPGTZ != null)
                        {
                            EUPGTZ.Active = false;
                            EUPGTZ.IsDeleted = true;
                        }
                    }
                }
                work.Commit();
                return origUpg.Id;
            }
        }
        // End Of DelPermissionGroupFromUsers function

        //public int DelPermissionGroupFromUsers(int userId, int dltGroup_Id)
        //{
        //    using (IUnitOfWork work = UnitOfWork.Begin())
        //    {
        //        string LoginName = "";
        //        string UserTimeZoneName = "";
        //        //get the user permission Group all details
        //        con.Open();
        //        string str1 = "select top 1 Id,UserId,DefaultUserTimeZoneId,ParentUserPermissionGroupId,Name,IsOriginal,PermissionIsActive,IsDeleted,(select LoginName from Users where id=pg.UserId) as LoginName,(select name from UserTimeZones where id=pg.DefaultUserTimeZoneId) as UserTimeZones from UserPermissionGroups pg where IsDeleted=0 and UserId='" + userId + "'";
        //        SqlDataAdapter da = new SqlDataAdapter(str1, con);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        UserPermissionGroup origUpg = new UserPermissionGroup();
        //        if (dt.Rows.Count > 0)
        //        {
        //            origUpg.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
        //            origUpg.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
        //            origUpg.DefaultUserTimeZoneId = Convert.ToInt32(dt.Rows[0]["DefaultUserTimeZoneId"]);
        //            if (dt.Rows[0]["ParentUserPermissionGroupId"] != null)
        //            {
        //                origUpg.ParentUserPermissionGroupId = Convert.ToInt32(dt.Rows[0]["ParentUserPermissionGroupId"]);
        //            }
        //            else
        //            {
        //                origUpg.ParentUserPermissionGroupId = null;
        //            }
        //            origUpg.Name = Convert.ToString(dt.Rows[0]["Name"]);
        //            origUpg.IsOriginal = Convert.ToBoolean(dt.Rows[0]["IsOriginal"]);
        //            origUpg.PermissionIsActive = Convert.ToBoolean(dt.Rows[0]["PermissionIsActive"]);
        //            origUpg.IsDeleted = Convert.ToBoolean(dt.Rows[0]["IsDeleted"]);

        //            LoginName = Convert.ToString(dt.Rows[0]["LoginName"]);
        //            UserTimeZoneName = Convert.ToString(dt.Rows[0]["UserTimeZones"]);
        //        }

        //        //UserPermissionGroup origUpg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => !x.IsDeleted).FirstOrDefault();

        //        // UserPermissionGroup detailsOfGroupToDlt = _userPermissionGroupRepository.FindById(dltGroup_Id);
        //        //UserPermissionGroup detailsOfGroupToDlt = _userPermissionGroupRepository.FindAll(x => x.Id == dltGroup_Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault();

        //        string LoginNamen = "";
        //        string UserTimeZoneNamen = "";
        //        string str2 = "select top 1 Id,UserId,DefaultUserTimeZoneId,ParentUserPermissionGroupId,Name,IsOriginal,PermissionIsActive,IsDeleted,(select LoginName from Users where id=pg.UserId) as LoginName,(select name from UserTimeZones where id=pg.DefaultUserTimeZoneId) as UserTimeZones from UserPermissionGroups pg where IsDeleted=0 and Id='" + dltGroup_Id + "' and ParentUserPermissionGroupId is null and PermissionIsActive=0";
        //        SqlDataAdapter da1 = new SqlDataAdapter(str2, con);
        //        DataTable dt1 = new DataTable();
        //        da1.Fill(dt1);
        //        UserPermissionGroup detailsOfGroupToDlt = new UserPermissionGroup();
        //        if (dt1.Rows.Count > 0)
        //        {
        //            detailsOfGroupToDlt.Id = Convert.ToInt32(dt1.Rows[0]["Id"]);
        //            detailsOfGroupToDlt.UserId = Convert.ToInt32(dt1.Rows[0]["UserId"]);
        //            detailsOfGroupToDlt.DefaultUserTimeZoneId = Convert.ToInt32(dt1.Rows[0]["DefaultUserTimeZoneId"]);

        //            detailsOfGroupToDlt.Name = Convert.ToString(dt1.Rows[0]["Name"]);
        //            detailsOfGroupToDlt.IsOriginal = Convert.ToBoolean(dt1.Rows[0]["IsOriginal"]);
        //            detailsOfGroupToDlt.PermissionIsActive = Convert.ToBoolean(dt1.Rows[0]["PermissionIsActive"]);
        //            detailsOfGroupToDlt.IsDeleted = Convert.ToBoolean(dt1.Rows[0]["IsDeleted"]);
        //            LoginNamen = Convert.ToString(dt1.Rows[0]["LoginName"]);
        //            UserTimeZoneNamen = Convert.ToString(dt1.Rows[0]["UserTimeZones"]);
        //        }
        //        con.Close();


        //        //if deleted group is not exist in the users group		
        //        if (!origUpg.Name.Contains(detailsOfGroupToDlt.Name)) { return origUpg.Id; }

        //        var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
        //        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneChanged", new List<string> { origUpg.Name, LoginName }));
        //        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserOldTimeZoneName", new List<string> { UserTimeZoneName }));
        //        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNewTimeZoneName", new List<string> { UserTimeZoneNamen }));

        //        //origUpg.DefaultUserTimeZoneId = newUpg.DefaultUserTimeZoneId;
        //        //origUpg.ParentUserPermissionGroupId = newUpgId;
        //        // origUpg.Name = origUpg.Name.Replace("++" + newUpg.Name, "");

        //        var userCurrentPermissionName = origUpg.Name;
        //        var dltGroup_Name = detailsOfGroupToDlt.Name;

        //        if (origUpg.Name.Contains("++"))
        //        {
        //            //Main group to be deleted... 
        //            string[] sep = { "++" };
        //            string[] allPermissionGroups = userCurrentPermissionName.Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //            var userNewPermissionName = " ";

        //            if (origUpg.ParentUserPermissionGroupId == detailsOfGroupToDlt.Id)
        //            {
        //                //Get New PermissionGroup Name after delete

        //                for (int i = 1; i <= allPermissionGroups.Length - 1; i++)
        //                {
        //                    userNewPermissionName = userNewPermissionName + allPermissionGroups[i];
        //                    if (i != allPermissionGroups.Length - 1)
        //                    {
        //                        userNewPermissionName = userNewPermissionName + "++";
        //                    }
        //                }

        //                var newParentPermissionName = allPermissionGroups.ElementAt(1);
        //                UserPermissionGroup parentIdRecord = _userPermissionGroupRepository.FindAll(x => x.Name == newParentPermissionName && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault();

        //                //UPDATE Records in UserPermissionGroup TABLE
        //                if (parentIdRecord != null)
        //                {
        //                    origUpg.Name = userNewPermissionName;
        //                    origUpg.DefaultUserTimeZoneId = parentIdRecord.DefaultUserTimeZoneId;
        //                    origUpg.ParentUserPermissionGroupId = parentIdRecord.Id;
        //                    origUpg.Timestamp = parentIdRecord.Timestamp;
        //                    origUpg.IsOriginal = true;
        //                }
        //                // origUpg.IsDeleted = parentIdRecord.IsDeleted;
        //                //  origUpg.PermissionIsActive = parentIdRecord.PermissionIsActive;

        //                //UPDATE logs

        //                work.Commit();
        //                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
        //                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
        //                 message.ToString());

        //                //UPDATE UserPermissionGroupTimeZones TABLE

        //                // User's All BuildingObjId
        //                var UserAllBuildingObjIDs = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();

        //                //building Object ID of group to be delete
        //                //var dlt_grpID = _userPermissionGroupRepository.FindAll(x => x.Name == detailsOfGroupToDlt.Name && x.Id == detailsOfGroupToDlt.Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault().Id;

        //                var dlt_grpID = detailsOfGroupToDlt.Id;
        //                var dltgrp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == dlt_grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList(); //1,2,3

        //                foreach (var buildingObjId in dltgrp_buildingObjIDs)
        //                {
        //                    UserPermissionGroupTimeZone EUPGTZ = UserAllBuildingObjIDs.FindAll(x => x.BuildingObjectId == buildingObjId).FirstOrDefault();

        //                    if (EUPGTZ != null)
        //                    {
        //                        EUPGTZ.Active = false;
        //                        EUPGTZ.IsDeleted = true;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var length = allPermissionGroups.Length;
        //                if (allPermissionGroups[length - 1] == dltGroup_Name)
        //                {
        //                    for (int i = 0; i <= allPermissionGroups.Length - 2; i++)
        //                    {
        //                        userNewPermissionName = userNewPermissionName + allPermissionGroups[i];
        //                        if (i != allPermissionGroups.Length - 2)
        //                        {
        //                            userNewPermissionName = userNewPermissionName + "++";
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    for (int i = 0; i <= allPermissionGroups.Length - 1; i++)
        //                    {
        //                        if (allPermissionGroups[i] != dltGroup_Name)
        //                        {
        //                            userNewPermissionName = userNewPermissionName + allPermissionGroups[i];

        //                            if (i != allPermissionGroups.Length - 1)
        //                            {
        //                                userNewPermissionName = userNewPermissionName + "++";
        //                            }
        //                        }
        //                    }
        //                }
        //                //UPDATE Records in UserPermissionGroup TABLE

        //                origUpg.Name = userNewPermissionName;
        //                origUpg.IsOriginal = true;
        //                //origUpg.IsDeleted = true;

        //                work.Commit();
        //                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
        //                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
        //                 message.ToString());

        //                //UPDATE UserPermissionGroupTimeZones TABLE

        //                //User's All BuildingObjId
        //                var UserAllBuildingObjIDs = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();

        //                //get dltdgrpId's building object id details 

        //                //var dlt_grpID = _userPermissionGroupRepository.FindAll(x => x.Name == dltGroup_Name && x.Id == dltGroup_Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault().Id;
        //                var dlt_grpID = detailsOfGroupToDlt.Id;
        //                var dltgrp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == dlt_grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList();

        //                foreach (var buildingObjId in dltgrp_buildingObjIDs)
        //                {
        //                    UserPermissionGroupTimeZone EUPGTZ = UserAllBuildingObjIDs.FindAll(x => x.BuildingObjectId == buildingObjId).FirstOrDefault();

        //                    if (EUPGTZ != null)
        //                    {
        //                        EUPGTZ.Active = false;
        //                        EUPGTZ.IsDeleted = true;
        //                    }
        //                }
        //            }
        //        }//close  outer if

        //        else
        //        {
        //            //User have  only 1 Permission Group
        //            //UPDATE Records in UserPermissionGroup TABLE

        //            origUpg.IsOriginal = true;
        //            origUpg.IsDeleted = true;
        //            origUpg.PermissionIsActive = false;

        //            work.Commit();
        //            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
        //            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
        //             message.ToString());

        //            //UPDATE logs

        //            work.Commit();
        //            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, detailsOfGroupToDlt.Name);
        //            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
        //             message.ToString());

        //            //UPDATE UserPermissionGroupTimeZones TABLE

        //            // User's All BuildingObjId
        //            var UserAllBuildingObjIDs = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();

        //            //building Object ID of group to be delete
        //            //var dlt_grpID = _userPermissionGroupRepository.FindAll(x => x.Name == detailsOfGroupToDlt.Name && x.Id == detailsOfGroupToDlt.Id && x.ParentUserPermissionGroupId == null && x.IsDeleted == false && x.PermissionIsActive == false).FirstOrDefault().Id;

        //            var dlt_grpID = detailsOfGroupToDlt.Id;
        //            var dltgrp_buildingObjIDs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == dlt_grpID && !x.IsDeleted && x.Active).Select(x => x.BuildingObjectId).ToList(); //1,2,3

        //            foreach (var buildingObjId in dltgrp_buildingObjIDs)
        //            {
        //                UserPermissionGroupTimeZone EUPGTZ = UserAllBuildingObjIDs.FindAll(x => x.BuildingObjectId == buildingObjId).FirstOrDefault();

        //                if (EUPGTZ != null)
        //                {
        //                    EUPGTZ.Active = false;
        //                    EUPGTZ.IsDeleted = true;
        //                }
        //            }
        //        }
        //        work.Commit();
        //        return origUpg.Id;

        //    }
        //} // End Of DelPermissionGroupFromUsers function

        public void AddPermissionsToAdditionalGroups(int UpgId, List<int> selectedBuildingObjectIds, List<int> oldObjets) //illi31.10.2019
        {
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {
                    UserPermissionGroup pg = _userPermissionGroupRepository.FindById(UpgId);

                    var Ids = new List<int>(); //illi31.10.2019
                    Ids.AddRange(selectedBuildingObjectIds); //illi31.10.2019

                    foreach (int tt in oldObjets) //illi31.10.2019
                    {
                        if (!Ids.Contains(tt)) //illi31.10.2019
                        {
                            Ids.Add(tt);//illi31.10.2019
                        }
                    }

                    var pgtz = _userPermissionGroupTimeZoneRepository.FindAll().Where(x => x.UserPermissionGroupId == pg.Id && Ids.Contains(x.BuildingObjectId)).ToList(); //illi31.10.2019 only active and deactivated(previous active) will owerwrited 

                    // _userPermissionGroupRepository.FindAll().Where(x => !x.IsDeleted && x.Name.Trim().Contains("++" + pg.Name.Trim())).ToList();
                    List<UserPermissionGroup> Upgs = _userPermissionGroupRepository.FindAll().Where(x => !x.IsDeleted && x.Name.Trim().Contains("++" + pg.Name.Trim() + "++") || x.Name.Trim().Contains("++") && x.Name.Trim().Split('+').Last() == pg.Name.Trim()).ToList();


                    foreach (var newUpg in Upgs)
                    {

                        var newUpgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGId(newUpg.Id).ToList();
                        var origPG = _userPermissionGroupRepository.FindById(newUpg.ParentUserPermissionGroupId.GetValueOrDefault());
                        if (origPG != null)
                        {
                            foreach (var upgtz in pgtz)
                            {
                                UserPermissionGroupTimeZone EUPGTZ = newUpgTimeZones.FindAll(x => x.BuildingObjectId == upgtz.BuildingObjectId).FirstOrDefault();
                                bool act = upgtz.Active;
                                int tz = upgtz.UserTimeZoneId;
                                if (!act)
                                {
                                    bool p = _userPermissionGroupTimeZoneRepository.FindByPGId(origPG.Id).Where(x => x.BuildingObjectId == upgtz.BuildingObjectId).Any();
                                    if (p)
                                    {
                                        tz = _userPermissionGroupTimeZoneRepository.FindByPGId(origPG.Id).Where(x => x.BuildingObjectId == upgtz.BuildingObjectId).First().UserTimeZoneId;
                                        act = _userPermissionGroupTimeZoneRepository.FindByPGId(origPG.Id).Where(x => x.BuildingObjectId == upgtz.BuildingObjectId).First().Active;
                                    }
                                }
                                if (EUPGTZ != null)
                                {
                                    EUPGTZ.IsArming = upgtz.IsArming;
                                    EUPGTZ.IsDefaultArming = upgtz.IsDefaultArming;
                                    EUPGTZ.IsDisarming = upgtz.IsDisarming;
                                    EUPGTZ.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                                    EUPGTZ.Active = act;
                                    EUPGTZ.IsDeleted = false;
                                    EUPGTZ.UserTimeZoneId = tz;

                                }
                                else
                                {
                                    UserPermissionGroupTimeZone newUpgtz;
                                    newUpgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();
                                    newUpgtz.UserTimeZoneId = upgtz.UserTimeZoneId;//origUpg.DefaultUserTimeZoneId; 1.14.2013 
                                    newUpgtz.UserPermissionGroupId = newUpg.Id;
                                    newUpgtz.BuildingObjectId = upgtz.BuildingObjectId;
                                    newUpgtz.IsArming = upgtz.IsArming;
                                    newUpgtz.IsDefaultArming = upgtz.IsDefaultArming;
                                    newUpgtz.IsDisarming = upgtz.IsDisarming;
                                    newUpgtz.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                                    newUpgtz.Active = act;
                                    newUpgtz.IsDeleted = upgtz.IsDeleted;

                                    _userPermissionGroupTimeZoneRepository.Add(newUpgtz);
                                }
                            }
                        }
                    }
                    work.Commit();
                }
            }
        }

        public int AddUserPermissionGroup(int userId, int newUpgId)
        {
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {
                    UserPermissionGroup origUpg = _userPermissionGroupRepository.FindByUserId(userId).Where(x => !x.IsDeleted).FirstOrDefault();
                    UserPermissionGroup newUpg = _userPermissionGroupRepository.FindById(newUpgId);
                    if (origUpg.Name.Contains(newUpg.Name)) { return origUpg.Id; }

                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneChanged", new List<string> { origUpg.Name, origUpg.User.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserOldTimeZoneName", new List<string> { origUpg.UserTimeZone.Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNewTimeZoneName", new List<string> { newUpg.UserTimeZone.Name }));

                    //origUpg.DefaultUserTimeZoneId = newUpg.DefaultUserTimeZoneId;
                    //origUpg.ParentUserPermissionGroupId = newUpgId;

                    origUpg.Name = origUpg.Name + "++" + newUpg.Name;
                    origUpg.IsOriginal = true;
                    work.Commit();
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, origUpg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, newUpg.Name);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                     message.ToString());


                    var oldUpgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGIdDel(origUpg.Id).ToList();
                    /*
                    foreach (var oldUpgTz in oldUpgTimeZones)
                    {
                        oldUpgTz.Active = false;
                        oldUpgTz.IsDeleted = true;
                     }
                     * */
                    var newUpgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGId(newUpg.Id).Where(x => !x.IsDeleted && x.Active == true).ToList();

                    foreach (var upgtz in newUpgTimeZones)
                    {
                        UserPermissionGroupTimeZone EUPGTZ = oldUpgTimeZones.FindAll(x => x.BuildingObjectId == upgtz.BuildingObjectId).FirstOrDefault();

                        if (EUPGTZ != null)
                        {
                            EUPGTZ.UserPermissionGroupId = origUpg.Id;
                            EUPGTZ.IsArming = upgtz.IsArming;
                            EUPGTZ.IsDefaultArming = upgtz.IsDefaultArming;
                            EUPGTZ.IsDisarming = upgtz.IsDisarming;
                            EUPGTZ.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                            EUPGTZ.Active = true;
                            EUPGTZ.IsDeleted = false;
                            EUPGTZ.UserTimeZoneId = upgtz.UserTimeZoneId;

                        }
                        else
                        {
                            UserPermissionGroupTimeZone newUpgtz;
                            newUpgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();
                            newUpgtz.UserTimeZoneId = upgtz.UserTimeZoneId;//origUpg.DefaultUserTimeZoneId; 1.14.2013 
                            newUpgtz.UserPermissionGroupId = origUpg.Id;
                            newUpgtz.BuildingObjectId = upgtz.BuildingObjectId;
                            newUpgtz.IsArming = upgtz.IsArming;
                            newUpgtz.IsDefaultArming = upgtz.IsDefaultArming;
                            newUpgtz.IsDisarming = upgtz.IsDisarming;
                            newUpgtz.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                            newUpgtz.Active = true;
                            newUpgtz.IsDeleted = false;

                            _userPermissionGroupTimeZoneRepository.Add(newUpgtz);
                        }
                    }
                    work.Commit();
                    return origUpg.Id;
                }
            }
        }

        public void SaveUserPermissionGroup(int upgId, List<int> buildingObjectIds, List<int> selectedBuildingObjectIds, bool isGroupOriginal = true, bool savelog = true)
        {

            var upg = _userPermissionGroupRepository.FindById(upgId);
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                List<UserPermissionGroupTimeZone> PGTZs = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).ToList();

                //List<UserPermissionGroupTimeZone> PGTZs = _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == upgId).ToList();
                foreach (var pg in PGTZs)
                {
                    pg.Active = false;
                    // pg.IsDeleted = true;
                }
                IEnumerable<int> Ids = selectedBuildingObjectIds.Distinct();
                //foreach (var bId in buildingObjectIds)
                foreach (var bId in Ids)
                {
                    //    if (bId == 581)           illi2015veeb???
                    //   { var pio = selectedBuildingObjectIds.Count(); } illi2015veeb???
                    //var bo_changed = false;
                    UserPermissionGroupTimeZone pgtz =
                                               // _userPermissionGroupTimeZoneRepository.FindAll(x => x.UserPermissionGroupId == upgId && x.BuildingObjectId == bId).FirstOrDefault();
                                               _userPermissionGroupTimeZoneRepository.FindGpTZbyBuilding(upgId, bId);

                    if (pgtz != null)
                    {
                        //bo_changed = pgtz.Active != selectedBuildingObjectIds.Contains(bId) && isGroupOriginal;
                        pgtz.Active = selectedBuildingObjectIds.Contains(bId);
                        pgtz.IsDeleted = false;
                    }
                    else //if (selectedBuildingObjectIds.Contains(bId))
                    {
                        pgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();
                        pgtz.BuildingObjectId = bId;
                        pgtz.IsArming = true;
                        pgtz.IsDefaultArming = true;
                        pgtz.IsDisarming = true;
                        pgtz.IsDefaultDisarming = true;
                        pgtz.UserPermissionGroupId = upgId;
                        pgtz.UserTimeZoneId = upg.DefaultUserTimeZoneId;
                        pgtz.IsDeleted = false;
                        pgtz.Active = true;
                        _userPermissionGroupTimeZoneRepository.Add(pgtz);
                    }
                    /* i19.08.2012 aaa tulevikus võib olla vaja see tagasi võtta vähendab salvestust kontrollerisse aga pole ka kindel
					if (bo_changed)
					{
						var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
						message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGCreated",
																	  new List<string>
			                                                                {
			                                                                    pgtz.UserPermissionGroup.Name,
			                                                                    pgtz.UserPermissionGroup.User.LoginName
			                                                                }));
						message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActive", new List<string> { pgtz.Active.ToString() }));

						_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
											  message.ToString());

						_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, pgtz.Id,
                                                                        UpdateParameter.UserSpecificPermissionChange, //i19.08.2012
																		ControllerStatus.Created,
																		string.Format("building object '{0}' for user permission group '{1}' '{2}'",
																			            pgtz.BuildingObject.Name, pgtz.UserPermissionGroup.Name,
																			            pgtz.Active ? "activated" : "deactivated"));
					} */
                    //*i19.08.2012

                }

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE); //i19.08.2012
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupChanged", new List<string> { upg.Name, upg.User.LoginName }));
                //illi 25.12.1012 v16 change User permission change
                if (savelog == true)
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upg.Id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, upg.Name);

                }
                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                    message.ToString());
                work.Commit();

                // add copy new BuildingObjects here
                // var affectedUpgs = _userPermissionGroupRepository.FindAll(x => GetAffectedUserPermissionGroupIds(upgId).Contains(x.Id)).ToList();
            }
        }

        public void ChangeUserPermissionGroupBuildingTimeZone(int upgId, int buildingObjectId, int newUtzId, bool savelog = true)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                //int userId = _userPermissionGroupRepository.FindById(upgId).UserId;
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindGpTZbyBuilding(upgId, buildingObjectId);//.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                if (upgtz != null)
                {
                    //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                    /*
                    if (_userRepository.IsCompanyManager(userId) || _userRepository.IsCommonUser(userId))
                    {
                        UserTimeZone originalUtz = _userTimeZoneRepository.FindById(newUtzId);

                        if (!IsOriginalUserTimeZoneAlreadyExists(userId, originalUtz.Uid))
                        {
                            UserTimeZone newUserTimeZone = CreateUserOriginalTimeZone(userId, originalUtz);
                            upgtz.UserTimeZoneId = newUserTimeZone.Id;
                        }
                        else
                        {
                            upgtz.UserTimeZoneId = GetOriginalUserTimeZone(userId, originalUtz.Uid).Id;
                        }
                    }
                    else*/

                    upgtz.UserTimeZoneId = newUtzId;

                    upgtz.Active = true;
                    upgtz.IsDeleted = false;
                    work.Commit();
                }
                else
                {
                    UserPermissionGroupTimeZone pgtz;
                    pgtz = DomainObjectFactory.CreateUserPermissionGroupTimeZone();
                    pgtz.BuildingObjectId = buildingObjectId;
                    pgtz.IsArming = true;
                    pgtz.IsDefaultArming = true;
                    pgtz.IsDisarming = true;
                    pgtz.IsDefaultDisarming = true;
                    pgtz.UserPermissionGroupId = upgId;
                    pgtz.UserTimeZoneId = newUtzId;
                    pgtz.IsDeleted = false;
                    pgtz.Active = true;
                    _userPermissionGroupTimeZoneRepository.Add(pgtz);


                    work.Commit();
                    upgtz = pgtz;
                }

                upgtz = _userPermissionGroupTimeZoneRepository.FindById(upgtz.Id);
                if (savelog) //illi 25.12.1012 v16 
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.UpgBuildingObjectTimeZoneChange, ControllerStatus.Edited, string.Format("Time zone '{0}' assigned for User permission group '{1}' building object '{2} {3}'", upgtz.UserTimeZone.Name, upgtz.UserPermissionGroup.Name, upgtz.BuildingObject.BuildingObjectType.Name, upgtz.BuildingObject.Description));
                }
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFUPGChanged", new List<string> { upgtz.UserPermissionGroup.Name, upgtz.UserPermissionGroup.User.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActive", new List<string> { upgtz.Active.ToString() }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }

        }

        public void ChangeUserPermissionGroupBuildingTimeZoneToDefault(int upgId, int buildingObjectId, bool savelog = true)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                int userId = _userPermissionGroupRepository.FindById(upgId).UserId;
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                int oldUserTimeZoneId = upgtz.UserTimeZoneId;

                upgtz.UserTimeZoneId = _userPermissionGroupRepository.FindById(upgId).DefaultUserTimeZoneId;
                upgtz.Active = true;
                work.Commit();

                if (_userRepository.IsCompanyManager(userId) || _userRepository.IsCommonUser(userId))
                {
                    var upgtZones = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.BuildingObjectId == buildingObjectId).ToList();
                    if (!upgtZones.Any(x => !x.IsDeleted && x.UserTimeZoneId == oldUserTimeZoneId))
                    {
                        UserTimeZone utz = _userTimeZoneRepository.FindById(oldUserTimeZoneId);
                        utz.IsDeleted = true;
                        work.Commit();
                    }
                }

                upgtz = _userPermissionGroupTimeZoneRepository.FindById(upgtz.Id);

                string utzName = upgtz.UserTimeZone.Name;

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGChanged", new List<string> { upgtz.UserPermissionGroup.Name, upgtz.UserPermissionGroup.User.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActive", new List<string> { upgtz.Active.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserTimeZoneNameChanged", new List<string> { utzName, upgtz.UserTimeZone.Name }));
                if (savelog) //illi 25.12.1012 v16 
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.UpgBuildingObjectTimeZoneChange, ControllerStatus.Edited,
                        string.Format("Time zone '{0}' assigned for User permission group '{1}' building object '{2} {3}' ",
                        upgtz.UserTimeZone.Name, upgtz.UserPermissionGroup.Name, upgtz.BuildingObject.BuildingObjectType.Name, upgtz.BuildingObject.Description));
                }
                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public int GetUserDefaultTimeZoneId(int upgId)
        {
            return _userPermissionGroupRepository.FindById(upgId).DefaultUserTimeZoneId;
        }

        public int GetUserActiveTimeZoneIdByBuildingObjectId(int upgId, int buildingObjectId)
        {// dodelatj
            UserPermissionGroupTimeZone bo = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => x.BuildingObjectId == buildingObjectId).FirstOrDefault();
            if (bo == null)
            {
                return _userPermissionGroupRepository.FindById(upgId).DefaultUserTimeZoneId;
            }
            else
            {
                return _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => x.BuildingObjectId == buildingObjectId).FirstOrDefault().UserTimeZoneId;
            }
        }

        public IEnumerable<int> GetUserTimeZonesIds(int upgId)
        {
            return (from c in _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(pg => !pg.IsDeleted) select c.UserTimeZoneId);
        }

        public IEnumerable<int> GetUserBuildingObjectIds(int upgId)
        {
            var test = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId);
            return (from c in test select c.BuildingObjectId);
            //return (from c in _userPermissionGroupTimeZoneRepository.FindAll(pg => !pg.IsDeleted && pg.Active && pg.UserPermissionGroupId == upgId) select c.BuildingObjectId);
        }

        public bool IsDefaultUserTimeZone(int buildingObjectId, int upgId)
        {
            var up = _userPermissionGroupRepository.FindById(upgId);
            if (up != null)
            {/*
                int defaultUserTimeZoneId = up.DefaultUserTimeZoneId;
                var upgtz = up.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                if (upgtz != null)
                {
                    return upgtz.UserTimeZoneId == defaultUserTimeZoneId;
                }*/
                int a1 = _userPermissionGroupRepository.FindById(upgId).DefaultUserTimeZoneId;
                var b2 = _userPermissionGroupTimeZoneRepository.FindGpTZbyBuilding(upgId, buildingObjectId);
                if (b2 == null)
                {
                    return true;
                }
                int a2 = b2.UserTimeZoneId;
                return a1 == a2;
            }
            return true;
        }

        public bool ToggleUserArming(int buildingObjectId, int upgId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted &&/* x.UserPermissionGroupId == upgId &&*/ x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                upgtz.IsArming = !upgtz.IsArming;

                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                //message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGChanged", new List<string> { upgtz.UserPermissionGroup.Name, upgtz.UserPermissionGroup.User.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGChanged", new List<string> { upgtz.UserPermissionGroupId.ToString(), CurrentUser.Get().Id.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObject", new List<string> { upgtz.BuildingObjectId.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsArming", new List<string> { upgtz.IsArming.ToString() }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                              message.ToString());

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                        string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));

                return upgtz.IsArming;
            }
        }

        public bool ToggleUserDefaultArming(int buildingObjectId, int upgId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted/* && x.UserPermissionGroupId == upgId*/ && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                upgtz.IsDefaultArming = !upgtz.IsDefaultArming;

                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGChanged", new List<string> { upgtz.UserPermissionGroupId.ToString(), CurrentUser.Get().Id.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObject", new List<string> { upgtz.BuildingObjectId.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultArming", new List<string> { upgtz.IsDefaultArming.ToString() }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                              message.ToString());
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                        string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));

                return upgtz.IsDefaultArming;
            }
        }

        public bool ToggleUserDisarming(int buildingObjectId, int upgId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                upgtz.IsDisarming = !upgtz.IsDisarming;

                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGChanged", new List<string> { upgtz.UserPermissionGroupId.ToString(), CurrentUser.Get().Id.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObject", new List<string> { upgtz.BuildingObjectId.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDisarming", new List<string> { upgtz.IsDisarming.ToString() }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                              message.ToString());

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                        string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));

                return upgtz.IsDisarming;
            }
        }

        public bool ToggleUserDefaultDisarming(int buildingObjectId, int upgId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                //                UserPermissionGroupTimeZone upgtzC = _userPermissionGroupTimeZoneRepository.FindGpTZbyBuilding(upgId, buildingObjectId);
                upgtz.IsDefaultDisarming = !upgtz.IsDefaultDisarming;

                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneFPGChanged", new List<string> { upgtz.UserPermissionGroupId.ToString(), CurrentUser.Get().Id.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObject", new List<string> { upgtz.BuildingObjectId.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageIsDefaultDisarming", new List<string> { upgtz.IsDefaultDisarming.ToString() }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                              message.ToString());

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                        string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));

                return upgtz.IsDefaultDisarming;
            }
        }

        public void DeleteUserPermissionGroup(int id)
        {
            UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                upg.IsDeleted = true;
                work.Commit();

            }

            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupDeleted", new List<string> { upg.Name }));
            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                  message.ToString());
            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Deleted, string.Format("Permission group '{0}' deleted", upg.Name));

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                foreach (var item in _userPermissionGroupTimeZoneRepository.FindByPGId(id).Where(x => !x.IsDeleted)/*_userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == id)*/)
                {
                    // DeleteUserPermissionGroupTimeZone(item.Id);

                    item.IsDeleted = true;

                }
                work.Commit();

                /*
            var message1 = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneDeleted", new List<string> { item.UserPermissionGroup.Name }));
            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, item.Id, UpdateParameter.UserSpecificPermissionChange, ControllerStatus.Deleted, "");

            _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                  message.ToString());
                */


            }

        }

        private void DeleteUserPermissionGroupTimeZone(int id)
        {
            UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindById(id);

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                upgtz.IsDeleted = true;
                work.Commit();
            }
            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserPermissionGroupTimeZoneDeleted", new List<string> { upgtz.UserPermissionGroup.Name }));
            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserSpecificPermissionChange, ControllerStatus.Deleted, "");

            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                  message.ToString());

        }

        public void ChangeUserDefaultTimeZone(int userPermissionGroupId, int zoneId, bool savelog = true)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {

                UserPermissionGroup upg = _userPermissionGroupRepository.FindById(userPermissionGroupId);

                int oldDefaultUtzId = upg.DefaultUserTimeZoneId;
                upg.DefaultUserTimeZoneId = zoneId;

                work.Commit();

                upg = _userPermissionGroupRepository.FindById(userPermissionGroupId);

                //  illi 25.12.1012 v16 pg Save canges
                if (savelog == true)
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upg.Id, UpdateParameter.PermissionGroupChangeAll, ControllerStatus.Edited, string.Format("Time zone is '{0}'", upg.UserTimeZone.Name));
                }
                var upgTimeZones = _userPermissionGroupTimeZoneRepository.FindByPGId(userPermissionGroupId).Where(x => !x.IsDeleted && x.UserPermissionGroupId == userPermissionGroupId && x.UserTimeZoneId == oldDefaultUtzId).ToList();

                foreach (var upgtz in upgTimeZones)
                {
                    upgtz.UserTimeZoneId = zoneId;
                    // i19.08.2012 if( upgtz.Active )
                    //_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.UserSpecificPermissionChange, ControllerStatus.Edited, string.Format("Time zone is '{0}'", upg.UserTimeZone.Name));
                }

                work.Commit();      //  illi 25.12.1012 v16 probleem ei jõua salvestada kõike kui on suur grupp
            }
        }

        public void UpdateAddUserPermission(int id, string name, string oldname)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
                var newname = upg.Name.Replace(oldname, name);
                upg.Name = newname;
                work.Commit();
            }
        }

        //      public void UpdateUserPermission(int id, string name)
        //{
        //	using (IUnitOfWork work = UnitOfWork.Begin())
        //	{
        //		UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
        //		var old_name = upg.Name;
        //		upg.Name = name;
        //		work.Commit();

        //		var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
        //		message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePermissionGroupChanged", new List<string> { old_name, name }));

        //		_logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
        //							  message.ToString());
        //              //illi 25.12.1012 v16 pg name change
        //             //ei ole vaja nime muutus _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, name);
        //	}
        //}
        public void UpdateUserPermission(int id, string name, string oldnamegroupename) //illi  16.10.2017 bug
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserPermissionGroup upg = _userPermissionGroupRepository.FindById(id);
                var old_name = upg.Name;
                //illi  16.10.2017 bug found ver 145 crash groupe names ++               
                if (old_name.Contains("++") && old_name.Contains(oldnamegroupename)) //illi  16.10.2017 bug
                {
                    var newname = upg.Name.Replace(oldnamegroupename, name);
                    upg.Name = newname;
                }
                else
                {
                    upg.Name = name;
                }
                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePermissionGroupChanged", new List<string> { old_name, name }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      message.ToString());
                //illi 25.12.1012 v16 pg name change
                //ei ole vaja nime muutus _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPermissionGroupChange, ControllerStatus.Edited, name);
            }
        }

        #region Group Changes

        public void UpdateGroupFromTime(int utzId, DateTime fromTime, int orderInGroup)
        {
            var userTimeZoneProperty =
                _userTimeZonePropertyRepository.FindAll().Where(
                    x => x.UserTimeZoneId == utzId && x.OrderInGroup == orderInGroup).FirstOrDefault();
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                var userTimeZone = _userTimeZoneRepository.FindById(utzId);
                var utzs =
                    _userTimeZoneRepository.FindAll().Where(x => !x.IsDeleted && x.TimeZoneId == userTimeZone.TimeZoneId);
                /*if( CurrentUser.Get().IsCompanyManager )
				{
					utzs = userTimeZone.TimeZone.IsDefault ? utzs.Where(x => x.User.CompanyId == CurrentUser.Get().CompanyId) : utzs.Where(x => x.User.CompanyId == CurrentUser.Get().CompanyId || (_userRepository.IsBuildingAdmin(x.UserId) || _userRepository.IsSuperAdmin(x.UserId)));
				}*/
                var affectedUtzIds = (from utz in utzs select utz.Id).ToArray();

                var affectedUtzps = _userTimeZonePropertyRepository.FindAll(x => x.OrderInGroup == orderInGroup && affectedUtzIds.Contains(x.UserTimeZoneId)).ToList();

                foreach (var utzp in affectedUtzps)
                {
                    utzp.ValidFrom = fromTime;
                    utzp.IsMonday = userTimeZoneProperty.IsMonday;
                    utzp.IsTuesday = userTimeZoneProperty.IsTuesday;
                    utzp.IsWednesday = userTimeZoneProperty.IsWednesday;
                    utzp.IsThursday = userTimeZoneProperty.IsThursday;
                    utzp.IsFriday = userTimeZoneProperty.IsFriday;
                    utzp.IsSaturday = userTimeZoneProperty.IsSaturday;
                    utzp.IsSunday = userTimeZoneProperty.IsSunday;
                }

                work.Commit();
            }
        }

        public void UpdateGroupToTime(int utzId, DateTime toTime, int orderInGroup)
        {
            var userTimeZoneProperty =
                _userTimeZonePropertyRepository.FindAll().Where(
                    x => x.UserTimeZoneId == utzId && x.OrderInGroup == orderInGroup).FirstOrDefault();
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                var userTimeZone = _userTimeZoneRepository.FindById(utzId);
                var utzs =
                    _userTimeZoneRepository.FindAll().Where(x => !x.IsDeleted && x.TimeZoneId == userTimeZone.TimeZoneId);
                /*if (CurrentUser.Get().IsCompanyManager)
				{
					utzs = userTimeZone.TimeZone.IsDefault ? utzs.Where(x => x.User.CompanyId == CurrentUser.Get().CompanyId) : utzs.Where(x => x.User.CompanyId == CurrentUser.Get().CompanyId || (_userRepository.IsBuildingAdmin(x.UserId) || _userRepository.IsSuperAdmin(x.UserId)));
				}*/
                var affectedUtzIds = (from utz in utzs select utz.Id).ToArray();

                var affectedUtzps = _userTimeZonePropertyRepository.FindAll(x => x.OrderInGroup == orderInGroup && affectedUtzIds.Contains(x.UserTimeZoneId)).ToList();

                foreach (var utzp in affectedUtzps)
                {
                    utzp.ValidTo = toTime;
                    utzp.IsMonday = userTimeZoneProperty.IsMonday;
                    utzp.IsTuesday = userTimeZoneProperty.IsTuesday;
                    utzp.IsWednesday = userTimeZoneProperty.IsWednesday;
                    utzp.IsThursday = userTimeZoneProperty.IsThursday;
                    utzp.IsFriday = userTimeZoneProperty.IsFriday;
                    utzp.IsSaturday = userTimeZoneProperty.IsSaturday;
                    utzp.IsSunday = userTimeZoneProperty.IsSunday;
                }

                work.Commit();
            }
        }

        private int[] GetAffectedUserPermissionGroupIds(int upgId)
        {
            var permgr = _userPermissionGroupRepository.FindById(upgId).Name;
            int[] pgtz = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.User.Active && x.PermissionIsActive && (x.Name.Trim().Contains("++" + permgr.Trim()) || x.ParentUserPermissionGroupId == upgId || (x.UserId == CurrentUser.Get().Id && x.ParentUserPermissionGroupId != null && x.Id == upgId))).Select(x => x.Id).ToArray();
            return pgtz;

            /*
               int[] affectedUpgIdsLevel1 = (from upg in _userPermissionGroupRepository.FindByPupgHasValue().Where(x => x.ParentUserPermissionGroupId.Value == upgId) select upg.Id).ToArray();
               int[] affectedUpgIdsLevel2 = (from upg in _userPermissionGroupRepository.FindByPupgHasValue().Where(x => affectedUpgIdsLevel1.Contains(x.ParentUserPermissionGroupId.Value)) select upg.Id).ToArray();
               int[] affectedUpgIdsLevel3 = (from upg in _userPermissionGroupRepository.FindByPupgHasValue().Where(x => affectedUpgIdsLevel2.Contains(x.ParentUserPermissionGroupId.Value)) select upg.Id).ToArray();
               return affectedUpgIdsLevel1.Union(affectedUpgIdsLevel2).Union(affectedUpgIdsLevel3).ToArray();
           */
            /*
            int[] affectedUpgIdsLevel1 = (from upg in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId.HasValue && x.ParentUserPermissionGroupId.Value == upgId) select upg.Id).ToArray();
            int[] affectedUpgIdsLevel2 = (from upg in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId.HasValue && affectedUpgIdsLevel1.Contains(x.ParentUserPermissionGroupId.Value)) select upg.Id).ToArray();
            int[] affectedUpgIdsLevel3 = (from upg in _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.ParentUserPermissionGroupId.HasValue && affectedUpgIdsLevel2.Contains(x.ParentUserPermissionGroupId.Value)) select upg.Id).ToArray();
            return affectedUpgIdsLevel1.Union(affectedUpgIdsLevel2).Union(affectedUpgIdsLevel3).ToArray();
        
        */
        }

        public void GroupSaveUserPermissionGroup(int upgId, List<int> buildingObjectIds, List<int> selectedBuildingObjectIds, List<int> ownObjets)
        {
            /*    using (IUnitOfWork work = UnitOfWork.Begin())
                {
               //illi 25.12.1012 v16 change User permission change
                    var upg = _userPermissionGroupRepository.FindById(upgId);
                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE); 
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePermissionGroupChanged", new List<string> { upg.Name, upg.User.LoginName }));

                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgId, UpdateParameter.PermissionGroupChangeAll, ControllerStatus.Edited, upg.Name);

                    _logService.CreateLog(CurrentUser.Get().Id, "web", CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                     message.ToString()); 

                    work.Commit();

                } */
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                var affIds = GetAffectedUserPermissionGroupIds(upgId);
                var affectedUpgs = _userPermissionGroupRepository.FindAll(x => affIds.Contains(x.Id)).ToList();
                var usrpg = _userPermissionGroupRepository.FindById(upgId);
                foreach (var upg in affectedUpgs)
                {
                    var Ids = new List<int>();
                    Ids.AddRange(selectedBuildingObjectIds);
                    var t = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => x.Active && !x.IsDeleted).Select(x => x.BuildingObjectId);
                    foreach (int tt in t)
                    {
                        if (!ownObjets.Contains(tt))
                        {
                            Ids.Add(tt);
                        }
                    }
                    SaveUserPermissionGroup(upg.Id, buildingObjectIds, Ids, false, false);
                }
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgId, UpdateParameter.PermissionGroupChangeAll, ControllerStatus.Edited, usrpg.Name);
                work.Commit();
            }
        }

        public void GroupChangeUserPermissionGroupBuildingTimeZone(int upgId, int buildingObjectId, int newUtzId)
        {
            var affIds = GetAffectedUserPermissionGroupIds(upgId);
            var affectedUpgs = _userPermissionGroupRepository.FindAll(x => affIds.Contains(x.Id)).ToList();

            foreach (var upg in affectedUpgs)
            {
                //UserTimeZone origUserTz = _userTimeZoneRepository.FindById(newUtzId);
                /*
                if (!IsOriginalUserTimeZoneAlreadyExists(upg.UserId, origUserTz.Uid))
                {
                    UserTimeZone newUtz = CreateUserOriginalTimeZone(upg.UserId, origUserTz);
                    ChangeUserPermissionGroupBuildingTimeZone(upg.Id, buildingObjectId, newUtz.Id);
                }
                else*/
                {
                    ChangeUserPermissionGroupBuildingTimeZone(upg.Id, buildingObjectId, newUtzId, false);//GetOriginalUserTimeZone(upg.UserId, origUserTz.Uid).Id);
                }

            }
        }

        public void GroupChangeUserPermissionGroupBuildingTimeZoneToDefault(int upgId, int buildingObjectId)
        {
            var affectedUpgs = _userPermissionGroupRepository.FindAll(x => GetAffectedUserPermissionGroupIds(upgId).Contains(x.Id)).ToList();

            foreach (var upg in affectedUpgs)
            {
                ChangeUserPermissionGroupBuildingTimeZoneToDefault(upg.Id, buildingObjectId, false);
            }
        }

        public void GroupChangeUserDefaultTimeZone(int upgId, int zoneId)
        {
            var Aupg = GetAffectedUserPermissionGroupIds(upgId);
            var affectedUpgs = _userPermissionGroupRepository.FindAll(x => Aupg.Contains(x.Id)).ToList();

            foreach (var upg in affectedUpgs)
            {
                UserTimeZone origUserTz = _userTimeZoneRepository.FindById(zoneId);
                /*
                if (!IsOriginalUserTimeZoneAlreadyExists(upg.UserId, origUserTz.Uid))
                {
                    UserTimeZone newUtz = CreateUserOriginalTimeZone(upg.UserId, origUserTz);
                    ChangeUserDefaultTimeZone(upg.Id, newUtz.Id);
                }
                else*/
                {
                    ChangeUserDefaultTimeZone(upg.Id, zoneId, false);// GetOriginalUserTimeZone(upg.UserId, origUserTz.Uid).Id);
                }
            }
        }

        public void GroupToggleUserArming(int buildingObjectId, int upgId)
        {
            var affectedIds = GetAffectedUserPermissionGroupIds(upgId);
            var affectedUpgs = _userPermissionGroupRepository.FindAll(x => affectedIds.Contains(x.Id)).ToList();
            //UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
            UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                foreach (var upg in affectedUpgs)
                {
                    UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                    upgtz.IsArming = origUpgtz.IsArming;
                }

                work.Commit();
            }

            foreach (var upg in affectedUpgs)
            {
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                //illibug 10.10.14 upgtz.IsDisarming = origUpgtz.IsDisarming;
                upgtz.IsArming = origUpgtz.IsArming;
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                    string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));
            }
        }

        public void GroupToggleUserDefaultArming(int buildingObjectId, int upgId)
        {
            var affectedIds = GetAffectedUserPermissionGroupIds(upgId);
            var affectedUpgs = _userPermissionGroupRepository.FindAll(x => affectedIds.Contains(x.Id)).ToList();
            //UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
            UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                foreach (var upg in affectedUpgs)
                {
                    //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                    UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                    upgtz.IsDefaultArming = origUpgtz.IsDefaultArming;
                }

                work.Commit();
            }

            foreach (var upg in affectedUpgs)
            {
                //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                //illibug 10.10.14    upgtz.IsDisarming = origUpgtz.IsDisarming;
                upgtz.IsDefaultArming = origUpgtz.IsDefaultArming;

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                    string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));
            }
        }

        public void GroupToggleUserDisarming(int buildingObjectId, int upgId)
        {
            var affectedIds = GetAffectedUserPermissionGroupIds(upgId);
            var affectedUpgs = _userPermissionGroupRepository.FindAll(x => affectedIds.Contains(x.Id)).ToList();
            //UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
            UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                foreach (var upg in affectedUpgs)
                {
                    //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                    UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                    upgtz.IsDisarming = origUpgtz.IsDisarming;
                }

                work.Commit();
            }

            foreach (var upg in affectedUpgs)
            {
                //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                upgtz.IsDisarming = origUpgtz.IsDisarming;

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                    string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));
            }
        }

        public void GroupToggleUserDefaultDisarming(int buildingObjectId, int upgId)
        {
            var affectedIds = GetAffectedUserPermissionGroupIds(upgId);
            var affectedUpgs = _userPermissionGroupRepository.FindAll(x => affectedIds.Contains(x.Id)).ToList();
            //UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
            UserPermissionGroupTimeZone origUpgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upgId).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upgId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                foreach (var upg in affectedUpgs)
                {
                    //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                    UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                    upgtz.IsDefaultDisarming = origUpgtz.IsDefaultDisarming;
                }

                work.Commit();
            }

            foreach (var upg in affectedUpgs)
            {
                //UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
                UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(upg.Id).Where(x => !x.IsDeleted && x.UserPermissionGroupId == upg.Id && x.BuildingObjectId == buildingObjectId).FirstOrDefault();

                //illibug 10.10.14  upgtz.IsDisarming = origUpgtz.IsDisarming;
                upgtz.IsDefaultDisarming = origUpgtz.IsDefaultDisarming;
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, upgtz.Id, UpdateParameter.RoomPermissionChange, ControllerStatus.Edited,
                    string.Format("Arming is {0}. Disarming is {1}. Default arming is {2}. Default disarming is {3}.", upgtz.IsArming, upgtz.IsDisarming, upgtz.IsDefaultArming, upgtz.IsDefaultDisarming));
            }
        }

        public int[] GetAddAffectedUserPermissionGroupIds(string name)
        {
            int[] groups = (from upg in _userPermissionGroupRepository.FindByPupgHasValue().Where(x => x.Name.Trim().Contains("++" + name.Trim())) select upg.Id).ToArray();
            return groups;
        }

        //public void GroupUpdateUserPermission(int upgId, string name, string oldname)
        //{
        //    //var affectedUpgs = _userPermissionGroupRepository.FindAll(x => GetAffectedUserPermissionGroupIds(upgId).Contains(x.Id)).ToList();
        //    var affIds = GetAffectedUserPermissionGroupIds(upgId);
        //    //List<UserPermissionGroupTimeZone> affectedUpgs = 
        //    //var affectedUpgs = _userPermissionGroupRepository.FindById(affIds)
        //    foreach (var upg in affIds)
        //    {
        //        UpdateUserPermission(upg, name);
        //    }

        //    var addaffIds = GetAddAffectedUserPermissionGroupIds(oldname);
        //    foreach (var upg in addaffIds)
        //    {
        //        UpdateAddUserPermission(upg, name, oldname);
        //    }

        //}
        public void GroupUpdateUserPermission(int upgId, string name, string oldname)
        {
            //var affectedUpgs = _userPermissionGroupRepository.FindAll(x => GetAffectedUserPermissionGroupIds(upgId).Contains(x.Id)).ToList();
            var affIds = GetAffectedUserPermissionGroupIds(upgId);
            //List<UserPermissionGroupTimeZone> affectedUpgs = 
            //var affectedUpgs = _userPermissionGroupRepository.FindById(affIds)
            foreach (var upg in affIds)
            {
                UpdateUserPermission(upg, name, oldname); //illi  16.10.2017 bug
            }

            var addaffIds = GetAddAffectedUserPermissionGroupIds(oldname);
            foreach (var upg in addaffIds)
            {
                UpdateAddUserPermission(upg, name, oldname);
            }

        }

        public void UpdateUserPermission(int id, string name)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}