using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.ServiceResults;


using System.Data;

namespace FoxSec.ServiceLayer.Services
{
    internal class UserService : ServiceBase, IUserService
    {
        string flag = "";
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ILogService _logservice;
        private readonly ILogService _logservice1;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IUserBuildingRepository _userBuildingRepository;
        private readonly IControllerUpdateService _controllerUpdateService;
        private readonly ICompanyRepository _companyRepository;
        private readonly IControllerUpdateRepository _controllerUpdateRepository;
        private readonly IUsersAccessUnitRepository _usersAccessUnitRepository;
          

        public UserService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory,
                            IEventAggregator eventAggregator, IUserRepository userRepository,
                            ILogService logService,
                            IClassificatorValueRepository classificatorValueRepository,
                            IUserRoleRepository userRoleRepository,
                            IUserBuildingRepository userBuildingRepository,
                            ICompanyRepository companyRepository,
                            IControllerUpdateRepository controllerUpdateRepository,
                            IUsersAccessUnitRepository usersAccessUnitRepository,
                            IControllerUpdateService controllerUpdateService)
            : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _logservice = logService;
            _logservice1 = logService;
            _classificatorValueRepository = classificatorValueRepository;
            _controllerUpdateService = controllerUpdateService;
            _userBuildingRepository = userBuildingRepository;
            _companyRepository = companyRepository;
            _controllerUpdateRepository = controllerUpdateRepository;
            _usersAccessUnitRepository = usersAccessUnitRepository;
        }


        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
       
         int userID = ((FoxSecIdentity)System.Web.HttpContext.Current.User.Identity).Id;
           public UserCreateResult CreateUser(string firstName,
                                           string lastName,
                                           string loginName,
                                           string password,
                                           string personalId,
                                           string email,
                                           String personalCode,
                                           String externalPersonalCode,
                                           DateTime? bithday,
                                           int? companyId,
                                           string PIN1,
                                           string PIN2,
                                           byte[] photo,
                                           string host,
                                           int? languageId, int classificatorid)
        {
            var result = new UserCreateResult { ErrorCode = UserServiceErrorCode.Ok, Id = 0 };

            if (_userRepository.FindByLoginName(loginName) != null)
            {
                result.ErrorCode = UserServiceErrorCode.LoginAlreadyExists;
                return result;
            }

            using (IUnitOfWork work = UnitOfWork.Begin())
            {

                User user = DomainObjectFactory.CreateUser();
                IFoxSecIdentity identity = CurrentUser.Get();

                user.CompanyId = companyId;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.LoginName = loginName;
                user.Password = string.IsNullOrEmpty(password) ? password : EncodePassword.ToMD5(password);
                user.PersonalId = personalId;
                user.Email = string.IsNullOrEmpty(email) ? "" : email;
                user.PersonalCode = personalCode;
                user.ExternalPersonalCode = externalPersonalCode;
                user.Birthday = bithday;
                user.Image = photo;
                user.ModifiedBy = identity.LoginName;
                user.ModifiedLast = DateTime.Now;
                user.Active = true;
                user.PIN1 = !string.IsNullOrEmpty(PIN1) ? Encryption.Encrypt(PIN1) : PIN1;
                user.PIN2 = !string.IsNullOrEmpty(PIN1) ? Encryption.Encrypt(PIN2) : PIN2;
                user.IsDeleted = false;
                user.CreatedBy = identity.LoginName;
                user.EServiceAllowed = false;
                user.LanguageId = languageId;

                user.RegistredStartDate = DateTime.Now;
                user.RegistredEndDate = DateTime.Now.AddYears(50);

                _userRepository.Add(user);
                work.Commit();

                result.Id = user.Id;
                var entity = new UserEventEntity(_userRepository.FindById(result.Id));

                var message = entity.GetCreateMessage();

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message);

                //_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserNameChanged, ControllerStatus.Created, string.Format("{0} {1}", user.FirstName, user.LastName));

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Created, "Active");
                /* i19082012
                                if( !string.IsNullOrEmpty(PIN1) )
                                {
                                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin1Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN1));
                                }

                                if(!string.IsNullOrEmpty(PIN2))
                                {
                                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin2Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN2));
                                }
                */
                if (classificatorid > 0)
                {
                    ClassificatorValue cv1 = _classificatorValueRepository.FindById(Convert.ToInt32(classificatorid));

                    var message1 = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message1.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorValueChanged", new List<string> { cv1.Value, "Users" }));
                    cv1.Value = "Users";
                    work.Commit();

                    _logservice1.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message1.ToString());
                }
                DateTime dttime = DateTime.Now;
                con.Open();
                string ins = "insert into UserLastMoves(UserId,Savestatus,NotFinishedMoveStartTime,NotFinishedJobStartTime,MoveAllowNewWork,FirstComeToWork,DepartureFromWork,LastRecordedTimeAtWork,NextMoveBlockedTo,LocationAtWork,LastMoveTime,LastEnteredExited,EnteredBuilding)values('" + result.Id + "','0','" + dttime + "','" + dttime + "','0','" + dttime + "','" + dttime + "','" + dttime + "','" + dttime + "','NotAtWork','" + dttime + "','1','0')";
                SqlCommand cmd = new SqlCommand(ins, con);
                cmd.ExecuteNonQuery();
                con.Close();
                return result;
            }
        }

        public UserCreateResult ImportCsvUser(string firstName,
                                string lastName,
                                string loginName,
                                 string email,
                                 string personalId,
                                 string externalPersonalCode)
        {
            var result = new UserCreateResult { ErrorCode = UserServiceErrorCode.Ok, Id = 0 };
            string LogName = "";
            if (_userRepository.FindByLoginName(loginName) != null)
            {
                LogName = loginName + DateTime.Now.ToString("mmddyyy");
            }
            else
            {
                LogName = firstName + "." + lastName;
            }

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = DomainObjectFactory.CreateUser();
                IFoxSecIdentity identity = CurrentUser.Get();

                //  user.CompanyId = companyId;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.LoginName = LogName;
                user.ModifiedBy = identity.LoginName;
                user.ModifiedLast = DateTime.Now;
                user.Active = true;
                user.IsDeleted = false;
                user.CreatedBy = identity.LoginName;
                user.EServiceAllowed = false;
                //  user.TableNumber = EmployeeID;
                user.PersonalId = personalId;
                user.ExternalPersonalCode = externalPersonalCode;
                user.RegistredStartDate = DateTime.Now;
                user.RegistredEndDate = DateTime.Now.AddYears(50);

                _userRepository.Add(user);
                work.Commit();

                result.Id = user.Id;
                var entity = new UserEventEntity(_userRepository.FindById(result.Id));

                var message = entity.GetCreateMessage();

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, "host", CurrentUser.Get().CompanyId, message);

                //_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserNameChanged, ControllerStatus.Created, string.Format("{0} {1}", user.FirstName, user.LastName));

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Created, "Active");
                /* i19082012
                if( !string.IsNullOrEmpty(PIN1) )
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin1Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN1));
                }

                if(!string.IsNullOrEmpty(PIN2))
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin2Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN2));
                }
                */
            }

            return result;
        }

        public UserCreateResult ImportUser(int EmployeeID,
                                            string firstName,
                                           string lastName,
                                           int? companyId)
        {
            var result = new UserCreateResult { ErrorCode = UserServiceErrorCode.Ok, Id = 0 };
            string LogName = "";
            if (_userRepository.FindByLoginName(firstName + "." + lastName) != null)
            {
                LogName = firstName + "." + lastName + DateTime.Now.ToString("mmddyyy");
            }
            else
            {
                LogName = firstName + "." + lastName;
            }

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = DomainObjectFactory.CreateUser();
                IFoxSecIdentity identity = CurrentUser.Get();

                user.CompanyId = companyId;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.LoginName = LogName;
                user.ModifiedBy = identity.LoginName;
                user.ModifiedLast = DateTime.Now;
                user.Active = true;
                user.IsDeleted = false;
                user.CreatedBy = identity.LoginName;
                user.EServiceAllowed = false;
                user.TableNumber = EmployeeID;

                user.RegistredStartDate = DateTime.Now;
                user.RegistredEndDate = DateTime.Now.AddYears(50);

                _userRepository.Add(user);
                work.Commit();

                result.Id = user.Id;
                var entity = new UserEventEntity(_userRepository.FindById(result.Id));

                var message = entity.GetCreateMessage();

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, "host", CurrentUser.Get().CompanyId, message);

                //_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserNameChanged, ControllerStatus.Created, string.Format("{0} {1}", user.FirstName, user.LastName));

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Created, "Active");
                /* i19082012
                if( !string.IsNullOrEmpty(PIN1) )
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin1Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN1));
                }

                if(!string.IsNullOrEmpty(PIN2))
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin2Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN2));
                }
                */
            }

            return result;
        }
        public void EditOtherData(int id, string comment)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User usr = _userRepository.FindById(id);
                usr.Comment = comment;
                work.Commit();
            }
        }
        public void EditUserPersonalData(int id,
                                            string firstName,
                                            string lastName,
                                            string loginName,
                                            string password,
                                            string personalId,
                                            string email,
                                            String personalCode,
                                            String externalPersonalCode,
                                            DateTime? bithday,
                                            DateTime registered,
                                            int? companyId,
                                            string PIN1,
                                            string PIN2,
                                            byte[] photo,
                                            string host,
                                            int? languageId
                                        )
        {
            //var name_changed = false;
            //var pin1_changed = false;
            //var pin2_changed = false;
            var pin_regEx = new Regex(@"\d{4}");
            PIN1 = string.IsNullOrWhiteSpace(PIN1) ? PIN1 : pin_regEx.IsMatch(PIN1) ? Encryption.Encrypt(PIN1) : PIN1;
            PIN2 = string.IsNullOrWhiteSpace(PIN2) ? PIN2 : pin_regEx.IsMatch(PIN2) ? Encryption.Encrypt(PIN2) : PIN2;

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                IFoxSecIdentity identity = CurrentUser.Get();
                var old_pin1 = user.PIN1;
                var old_pin2 = user.PIN2;

                var entity = new UserEventEntity(user);/*
				if( user.CompanyId != companyId )
				{
					foreach (var card in user.UsersAccessUnits)
					{
						if( !card.IsDeleted )
						{
							card.Active = false;
							card.UserId = null;
							card.Free = true;
						}
					}
				}*/
                if (companyId != user.CompanyId)
                {
                    var c1 = companyId.HasValue ? _companyRepository.FindById(companyId.Value) : null;
                    if (c1 == null || user.Company == null || (c1.Id != user.Company.ParentId && c1.ParentId != user.Company.ParentId && c1.ParentId != user.CompanyId))
                    {
                        foreach (var ubo in user.UserBuildings)
                        {
                            ubo.IsDeleted = true;
                        }
                    }
                }
                user.CompanyId = companyId;/*
				if( user.FirstName != firstName || user.LastName != lastName )
				{
					name_changed = true;
				}
                */
                if (user.PIN1 != PIN1)
                {
                    //	pin1_changed = true;
                    user.PIN1 = PIN1;
                }

                if (user.PIN2 != PIN2)
                {
                    //	pin2_changed = true;
                    user.PIN2 = PIN2;
                }

                user.FirstName = firstName;
                user.LastName = lastName;
                user.LoginName = loginName;
                if (user.Password != password)
                {
                    user.Password = string.IsNullOrEmpty(password) ? password : EncodePassword.ToMD5(password);
                }
                user.PersonalId = personalId;
                user.Email = email;
                user.PersonalCode = personalCode;
                user.ExternalPersonalCode = externalPersonalCode;
                user.Birthday = bithday;
                //user.Image = photo;
                user.ModifiedBy = identity.LoginName;
                user.ModifiedLast = DateTime.Now;
                user.RegistredStartDate = registered;
                user.RegistredEndDate = registered.AddYears(50);
                user.LanguageId = languageId;

                work.Commit();

                //var e = new UserEditedEventArgs(user, identity.LoginName, identity.FirstName, identity.LastName, DateTime.Now);
                //e.SetNewUser(user);
                //EventAggregator.GetEvent<UserEditedEvent>().Publish(e);

                entity.SetNewUser(_userRepository.FindById(id));

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, entity.GetEditMessage());

                /*i19082012		if (name_changed)
                        {
                            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserNameChanged,
                                                                            ControllerStatus.Edited,
                                                                            string.Format("{0} {1}", user.FirstName, user.LastName));
                        }

                        if( pin1_changed )
                        {
                            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin1Changed, ControllerStatus.Edited, string.Format("PIN1 changed from '{0}' to '{1}'.", string.IsNullOrEmpty(old_pin1) ? " " : Encryption.Decrypt(old_pin1), string.IsNullOrWhiteSpace(PIN1) ? "  " : Encryption.Decrypt(PIN1)));
                        }

                        if( pin2_changed )
                        {
                            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin2Changed, ControllerStatus.Edited, string.Format("PIN2 changed from '{0}' to '{1}'.", string.IsNullOrEmpty(old_pin2) ? " " : Encryption.Decrypt(old_pin2), string.IsNullOrWhiteSpace(PIN2) ? "  " : Encryption.Decrypt(PIN2)));
                        }*/
                //if (pin2_changed || name_changed || pin1_changed)
               
                if (Convert.ToString(user.Id) !=null)
              
                    {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Edited, string.Format("User Save button  "));
                }
                else
                {

                }
            }
        }

        public void UpdateCsv(
                                    string firstName,
                                    string lastName,
                                    string loginName,
                                     string email,
                                     string personalId,
                                     string externalPersonalCode
               )
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {

                User user = _userRepository.FindById(Convert.ToInt32(personalId));
                //   FoxSecIdentity identity = CurrentUser.Get();
                var entity = new UserEventEntity(user);
                user.FirstName = firstName;
                user.LastName = lastName;
                user.LoginName = loginName;
                user.ExternalPersonalCode = externalPersonalCode;
                user.PersonalId = personalId;
                user.IsDeleted = false;
                user.Active = true;
                work.Commit();
                entity.SetNewUser(_userRepository.FindById(Convert.ToInt32(personalId)));
                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, "host", CurrentUser.Get().CompanyId, entity.GetEditMessage());
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Edited, string.Format("User Save button  "));
                }
            }
        }



        public void Update(int id,
                                    string firstName,
                                    string lastName,
                                    string loginName,
                                    int? company)
        {
            var pin_regEx = new Regex(@"\d{4}");
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                IFoxSecIdentity identity = CurrentUser.Get();

                var entity = new UserEventEntity(user);

                user.FirstName = firstName;
                user.LastName = lastName;
                user.LoginName = loginName;
                user.CompanyId = company;
                user.IsDeleted = false;
                user.Active = true;

                work.Commit();

                entity.SetNewUser(_userRepository.FindById(id));

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, "host", CurrentUser.Get().CompanyId, entity.GetEditMessage());

                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Edited, string.Format("User Save button  "));
                }
            }
        }



        public void EditUserRoles(int id, IEnumerable<UserRoleDto> roles, string host, int? newBuilding, bool activeRoleChanged)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                if (activeRoleChanged)
                {
                    foreach (var ubo in user.UserBuildings)
                    {
                        ubo.IsDeleted = true;
                    }

                    if (newBuilding.HasValue)
                    {
                        UserBuilding ub =
                            user.UserBuildings.Where(x => x.BuildingId == newBuilding.Value && x.BuildingObjectId == null).FirstOrDefault();

                        if (ub != null)
                        {
                            ub.IsDeleted = false;
                        }
                        else
                        {
                            ub = DomainObjectFactory.CreateUserBuilding();
                            ub.BuildingId = newBuilding.Value;
                            ub.UserId = user.Id;
                            ub.IsDeleted = false;
                            _userBuildingRepository.Add(ub);
                        }
                    }
                }

                var deleted_user_role_dtos = new List<UserRoleDto>();
                var created_user_role_dtos = new List<UserRoleDto>();
                var edited_user_role_dtos = new List<UserRoleDto>();
                var existing_role_ids = new List<int>();

                //remove unused roles edit existing roles
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserRolesChanged", new List<string> { user.LoginName }));

                foreach (var userRole in user.UserRoles)
                {
                    existing_role_ids.Add(userRole.Id);
                    var edited_role = roles.Where(r => r.RoleId == userRole.RoleId && r.IsSelected).FirstOrDefault();
                    if (edited_role == null)
                    {
                        userRole.IsDeleted = true;
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleRemoved", new List<string> { userRole.Role.Name }));
                        deleted_user_role_dtos.Add(new UserRoleDto() { Id = userRole.Id, RoleName = userRole.Role.Name, ValidFrom = userRole.ValidFrom, ValidTo = userRole.ValidTo });
                    }
                    else
                    {
                        if (!userRole.IsDeleted)
                        {
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleChanged", new List<string> { userRole.Role.Name }));
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFromChanged", new List<string> { userRole.ValidFrom.ToString("dd.MM.yyyy"),
                                                         edited_role.ValidFrom.ToString("dd.MM.yyyy") }));
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidToChanged", new List<string> { userRole.ValidTo.ToString("dd.MM.yyyy"),
                                                         edited_role.ValidTo.ToString("dd.MM.yyyy") }));

                            edited_user_role_dtos.Add(new UserRoleDto() { Id = userRole.Id, RoleName = userRole.Role.Name, ValidFrom = edited_role.ValidFrom, ValidTo = edited_role.ValidTo });
                        }
                        else
                        {
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleAdded", new List<string> { edited_role.RoleName }));
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom", new List<string> { edited_role.ValidFrom.ToString("dd.MM.yyyy") }));
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo", new List<string> { edited_role.ValidTo.ToString("dd.MM.yyyy") }));

                            created_user_role_dtos.Add(new UserRoleDto() { Id = userRole.Id, RoleName = userRole.Role.Name, ValidFrom = edited_role.ValidFrom, ValidTo = edited_role.ValidTo });
                        }
                        userRole.ValidFrom = edited_role.ValidFrom;
                        userRole.ValidTo = edited_role.ValidTo;
                        userRole.IsDeleted = false;
                    }
                }

                foreach (var newRole in roles)
                {
                    if (newRole.IsSelected == true && !user.UserRoles.Any(ur => ur.RoleId == newRole.RoleId))
                    {
                        var new_user_role = DomainObjectFactory.CreateUserRole();
                        new_user_role.UserId = id;
                        new_user_role.RoleId = newRole.RoleId;
                        new_user_role.IsDeleted = false;
                        new_user_role.ValidFrom = newRole.ValidFrom;
                        new_user_role.ValidTo = newRole.ValidTo;
                        _userRoleRepository.Add(new_user_role);

                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleAdded", new List<string> { newRole.RoleName }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom", new List<string> { newRole.ValidFrom.ToString("dd.MM.yyyy") }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo", new List<string> { newRole.ValidTo.ToString("dd.MM.yyyy") }));
                    }
                }

                work.Commit();

                user = _userRepository.FindById(id);
                foreach (var userRole in user.UserRoles)
                {
                    if (!existing_role_ids.Contains(userRole.Id))
                    {
                        created_user_role_dtos.Add(new UserRoleDto() { Id = userRole.Id, RoleName = userRole.Role.Name, ValidFrom = userRole.ValidFrom, ValidTo = userRole.ValidTo });
                    }
                }

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
                /* i19082012
				foreach (var createdUserRoleDto in created_user_role_dtos)
				{
					_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, createdUserRoleDto.Id.Value, UpdateParameter.UserRoleValidationPeriodChanged, ControllerStatus.Created, string.Format("{0}, Validation period from {1} to {2} ", createdUserRoleDto.RoleName, createdUserRoleDto.ValidFrom.ToString("dd.MM.yyyy"), createdUserRoleDto.ValidTo.ToString("dd.MM.yyyy")));
				}

				foreach (var editedUserRoleDto in edited_user_role_dtos)
				{
					_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, editedUserRoleDto.Id.Value, UpdateParameter.UserRoleValidationPeriodChanged, ControllerStatus.Edited, string.Format("{0}, Validation period from {1} to {2} ", editedUserRoleDto.RoleName, editedUserRoleDto.ValidFrom.ToString("dd.MM.yyyy"), editedUserRoleDto.ValidTo.ToString("dd.MM.yyyy")));
				}

				foreach (var deletedUserRoleDto in deleted_user_role_dtos)
				{
					_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, deletedUserRoleDto.Id.Value, UpdateParameter.UserRoleValidationPeriodChanged, ControllerStatus.Deleted, string.Format("{0}, Validation period from {1} to {2} ", deletedUserRoleDto.RoleName, deletedUserRoleDto.ValidFrom.ToString("dd.MM.yyyy"), deletedUserRoleDto.ValidTo.ToString("dd.MM.yyyy")));
				} */
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Edited, string.Format("User role changed"));
            }
        }

        public void UpdateUserRoles(int id, int? companyId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                var roles = user.UserRoles.ToList();

                foreach (var userRole in roles)
                {
                    userRole.CompanyId = companyId;
                }

                work.Commit();
            }
        }

        public void DeleteUserRoles(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                var oldRoles = user.UserRoles.ToList();

                foreach (var oldRole in oldRoles)
                {
                    oldRole.IsDeleted = true;
                }

                work.Commit();
            }
        }

        public void EditUserContactData(int id,
                                            string residence,
                                            string phone,
                                            string host
                                        )
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                var old_residense = user.Residence;
                var old_phone = user.PhoneNumber;

                user.Residence = residence;
                user.PhoneNumber = phone;

                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserChanged", new List<string> { user.LoginName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageResidenceChanged", new List<string> { string.IsNullOrWhiteSpace(old_residense) ? "" : old_residense,
                                             string.IsNullOrWhiteSpace(residence) ? "" : residence }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePhoneNumberChanged", new List<string> { string.IsNullOrWhiteSpace(old_phone) ? "" : old_phone,
                                             string.IsNullOrWhiteSpace(phone) ? "" : phone }));

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void EditUserWorkData(int id,
                                        int? titleId,
                                        string contractNum,
                                        DateTime? contractStartDate,
                                        DateTime? contractEndDate,
                                        DateTime? permitOfWork,
                                        bool? workTime,
                                        int? tableNumber,
                                        bool? eServiceAllowed,
                                        string host
                                     )
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);

                var logEntity = new UserEventEntity(user);

                user.TitleId = titleId;
                user.ContractNum = contractNum;
                user.ContractStartDate = contractStartDate;
                user.ContractEndDate = contractEndDate;
                user.PermitOfWork = permitOfWork;
                user.WorkTime = workTime;
                user.TableNumber = tableNumber;
                //user.EServiceAllowed = eServiceAllowed;

                work.Commit();

                logEntity.SetNewUser(_userRepository.FindById(id));

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId,
                                      logEntity.ChangeWorkDataMessage());
            }
        }

        public void DeleteUser(int id, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);

                var PIN1 = user.PIN1;

                var PIN2 = user.PIN2;

                var logEntity = new UserEventEntity(user);

                IFoxSecIdentity identity = CurrentUser.Get();
                var e = new UserDeletedEventArgs(user, identity.LoginName, identity.FirstName, identity.LastName, DateTime.Now);
                user.LoginName = "";
                user.IsDeleted = true;
                work.Commit();

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, logEntity.GetDeleteMessage());

                //	_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserNameChanged, ControllerStatus.Deleted, string.Empty);

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserStatusChanged, ControllerStatus.Deleted, string.Empty);
                /*
                                if( !string.IsNullOrEmpty(PIN1) )
                                {
                                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPin1Changed, ControllerStatus.Deleted, PIN1);
                                }

                                if( !string.IsNullOrEmpty(PIN2) )
                                {
                                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPin2Changed, ControllerStatus.Deleted, PIN2);
                                } */

                //EventAggregator.GetEvent<UserDeletedEvent>().Publish(e);
            }
        }

        public void Activate(int id, int? classificatorValueId, string host, string cardactivate)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                user.Active = true;
                work.Commit();
                user.ClassificatorValueId = classificatorValueId;
                var reason_str = classificatorValueId.HasValue
                                    ? _classificatorValueRepository.FindById(classificatorValueId.Value).Value
                                    : "Empty";

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserActivated", new List<string> { user.LoginName, reason_str }));

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());

                if (cardactivate == "1")
                {
                    List<UsersAccessUnit> usersAccessUnitlist = _usersAccessUnitRepository.FindByUserIdList(id);
                    foreach (var obj in usersAccessUnitlist)
                    {
                        var card_message = string.Empty;

                        UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(obj.Id);
                        var message1 = new XElement(XMLLogLiterals.LOG_MESSAGE);
                        message1.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardActivated", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));

                        usersAccessUnit.Active = true;
                        if (usersAccessUnit.Free)
                        {
                            usersAccessUnit.Free = false;
                        }
                        work.Commit();

                        card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                            ? string.Format("{0} {1}",
                                                                                                            usersAccessUnit.Serial,
                                                                                                            usersAccessUnit.Dk)
                                                                                                            : usersAccessUnit.Code,
                                                                                                            usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                            usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                        _logservice1.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                                  CurrentUser.Get().CompanyId, message1.ToString());

                    }
                }
                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Edited, "Active");
            }
        }

        public void Deactivate(int id, int? classificatorValueId, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                user.Active = false;
                work.Commit();

                var reason_str = classificatorValueId.HasValue
                                    ? _classificatorValueRepository.FindById(classificatorValueId.Value).Value
                                    : "Empty";

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserDeactivated", new List<string> { user.LoginName, reason_str }));

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());

                List<UsersAccessUnit> usersAccessUnitlist = _usersAccessUnitRepository.FindByUserIdList(id);
                foreach (var obj in usersAccessUnitlist)
                {
                    var card_message = string.Empty;

                    UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(obj.Id);
                    var message1 = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message1.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardDeactivated", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));

                    usersAccessUnit.Active = false;
                    work.Commit();

                    card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                        ? string.Format("{0} {1}",
                                                                                                        usersAccessUnit.Serial,
                                                                                                        usersAccessUnit.Dk)
                                                                                                        : usersAccessUnit.Code,
                                                                                                        usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                        usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                    _logservice1.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                              CurrentUser.Get().CompanyId, message1.ToString());

                }

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Edited, "Deactivated");
            }
        }

        public void Deactivate(int id, int? classificatorValueId, string host, bool isMoveToFree)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                User user = _userRepository.FindById(id);
                user.Active = false;
                work.Commit();

                var reason_str = classificatorValueId.HasValue
                                    ? _classificatorValueRepository.FindById(classificatorValueId.Value).Value
                                    : "Empty";

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserDeactivated", new List<string> { user.LoginName, reason_str }));

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());

                List<UsersAccessUnit> usersAccessUnitlist = _usersAccessUnitRepository.FindByUserIdList(id);
                foreach (var obj in usersAccessUnitlist)
                {
                    var card_message = string.Empty;

                    UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(obj.Id);
                    var message1 = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message1.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardDeactivated", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));

                    usersAccessUnit.Active = false;
                    if (isMoveToFree)
                    {
                        usersAccessUnit.Free = true;
                    }
                    usersAccessUnit.Closed = DateTime.Now;
                    work.Commit();

                    card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                        ? string.Format("{0} {1}",
                                                                                                        usersAccessUnit.Serial,
                                                                                                        usersAccessUnit.Dk)
                                                                                                        : usersAccessUnit.Code,
                                                                                                        usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                        usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                    _logservice1.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                              CurrentUser.Get().CompanyId, message1.ToString());

                }

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Edited, "Deactivated");
            }
        }

        public void SaveAtWorkLeaving(int boid, int userid, int type, string buildingname, string node, string lastlgtype)
        {
            DateTime dttime = DateTime.Now;
            con.Open();
            SqlCommand cmdtc = new SqlCommand("select count(*) from UserLastMoves where userid='" + userid + "'", con);
            int tc = Convert.ToInt32(cmdtc.ExecuteScalar());
            if (tc == 0)
            {              
                string ins = "insert into UserLastMoves(UserId,Savestatus,NotFinishedMoveStartTime,NotFinishedJobStartTime,MoveAllowNewWork,FirstComeToWork,DepartureFromWork,LastRecordedTimeAtWork,NextMoveBlockedTo,LocationAtWork,LastMoveTime,LastEnteredExited,EnteredBuilding)values('" + userid + "','0','"+dttime+ "','" + dttime + "','0','" + dttime + "','" + dttime + "','" + dttime + "','" + dttime + "','NotAtWork','" + dttime + "','1','0')";
                SqlCommand cmd = new SqlCommand(ins, con);
                cmd.ExecuteNonQuery();                
            }
            con.Close();
            int? compid = _userRepository.FindById(userid).CompanyId;
            if (type == 1)//at work
            {
                string message1 = "";
                string message2 = "";
                con.Open();
                SqlCommand cmd2 = new SqlCommand("select FirstName+' '+LastName from Users where id='" + userID + "'", con);
                string firstname = Convert.ToString(cmd2.ExecuteScalar());
                message2 = "Remote Registration done by '" + firstname + "'";
                con.Close();
                if (lastlgtype == "31" || lastlgtype == "")
                {
                    message1 = "started: AtWork " + dttime.ToString("HH:mm") + " " + message2;
                }
                else if (lastlgtype == "30")
                {
                    message1 = "repeated: AtWork " + dttime.ToString("HH:mm")+" "+message2;
                }
               
               
                string evntkey = "TOL00001" + dttime.ToString("yyyyMMddHHmm");

                _logservice1.CreateLogWorkLeave(userid, boid, flag,
                                                 compid, message1.ToString(), dttime, 30, buildingname, node, evntkey);

              
                con.Open();
                string update = "update UserLastMoves set NotFinishedMoveTaReportLabelId='1',NotFinishedMoveStartTime='" + dttime + "',MoveAllowNewWork='1',LastRecordedTimeAtWork='" + dttime + "',LastMoveTime='" + dttime + "',LastMoveBOId='" + boid + "',LastEnteredExited='1',EnteredBuilding='1',TerminalLastEntryBoId='" + boid + "',LocationAtWork='AtWork' where userid='" + userid + "'";
                SqlCommand cmdupd = new SqlCommand(update, con);
                cmdupd.ExecuteNonQuery();
                con.Close();
            }
            if (type == 2)//leaving
            {
                string message2 = "";
                con.Open();
                SqlCommand cmd2 = new SqlCommand("select FirstName+' '+LastName from Users where id='" + userID + "'", con);
                string firstname = Convert.ToString(cmd2.ExecuteScalar());
                message2 = "Remote Registration done by '" + firstname + "'";
                con.Close();
                string message1 = "finished: AtWork " + dttime.ToString("HH:mm")+" "+ message2;
                string evntkey = "TOL00002" + dttime.ToString("yyyyMMddHHmm");
                _logservice1.CreateLogWorkLeave(userid, boid, flag,
                                                 compid, message1.ToString(), dttime, 31, buildingname, node, evntkey);
                
                con.Open();
                string update = "update UserLastMoves set NotFinishedMoveTaReportLabelId='2',MoveAllowNewWork='0',LocationAtWork='NotAtWork',DepartureFromWork='" + dttime + "',LastMoveTime='" + dttime + "',LastMoveBOId='" + boid + "',LastEnteredExited='0',EnteredBuilding='0' where userid='" + userid + "'";
                SqlCommand cmdupd = new SqlCommand(update, con);
                cmdupd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void RemoveUserPhoto(int id)
        {
            if (id != 0)
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {
                    User user = _userRepository.FindById(id);
                    if (user != null)
                    {
                        var str = "Image";
                        user.Image = null;
                        work.Commit();
                        ControllerUpdate cu = _controllerUpdateRepository.FindAll().Where(x => x.EntityId == id).FirstOrDefault();
                        _controllerUpdateService.DeleteControllerUpdate(cu.Id, CurrentUser.Get().Id, id, UpdateParameter.PictureUpdate, ControllerStatus.Created, str);

                    }
                }
            }
        }

        public string Encrypttxt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            //   string key = (string)settingsReader.GetValue(ENCRYPTION_KEY, typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("A456E4DA104F960563A66DDC"));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes("A456E4DA104F960563A66DDC");

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }

    public class UserRoleDto
    {
        public int? Id { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public int RoleId { get; set; }

        public bool IsSelected { get; set; }

        public string RoleName { get; set; }
    }

    public enum UserServiceErrorCode
    {
        Ok = 0,
        LoginAlreadyExists = 1
    }

}