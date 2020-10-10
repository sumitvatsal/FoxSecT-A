using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FoxSec.Authentication;
using FoxSec.Common.Enums;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
    internal class UserDepartmentService : ServiceBase, IUserDepartmentService
    {
        private readonly IUserDepartmentRepository _userDepartmentRepository;
        private readonly IUserRoleRepository _userRoleRepository;
    	private readonly ILogService _logService;
    	private readonly IUserRepository _userRepository;
    	private readonly IDepartmentRepository _departmentRepository;
    	private readonly IRoleTypeRepository _roleTypeRepository;
        string flag = "";

        public UserDepartmentService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory,
                                     IEventAggregator eventAggregator,
                                     IUserDepartmentRepository userDepartmentRepository,
									 ILogService logService,
                                     IUserRoleRepository userRoleRepository,
									 IUserRepository userRepository,
									 IDepartmentRepository departmentRepository,
									 IRoleTypeRepository roleTypeRepository)
            : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _userDepartmentRepository = userDepartmentRepository;
            _userRoleRepository = userRoleRepository;
        	_logService = logService;
        	_userRepository = userRepository;
        	_departmentRepository = departmentRepository;
        	_roleTypeRepository = roleTypeRepository;
        }

		public void DeleteUserDepartment(int Id)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				UserDepartment userDepartment = _userDepartmentRepository.FindById(Id);

				userDepartment.IsDeleted = true;

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserRemovedFromDepartment", new List<string> { userDepartment.User.LoginName,
											userDepartment.Department.Name }));
				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  message.ToString());
			}
		}

		public void DeleteUserDepartment(int Id, int departmentId)
		{
			using (IUnitOfWork work = UnitOfWork.Begin())
			{
				UserDepartment userDepartment = _userDepartmentRepository.FindByUserId(Id).Where(ud => ud.DepartmentId == departmentId).Single();

				userDepartment.IsDeleted = true;

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserRemovedFromDepartment", new List<string> { userDepartment.User.LoginName,
											userDepartment.Department.Name }));

				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag ,CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  message.ToString());
			}
		}

        public void AddUserDepartment(bool currentDep,
                                      int departmentId,
                                      bool isDeleted,
                                      bool isDepartmentManager,
                                      int userId,
                                      DateTime validFrom,
                                      DateTime validTo)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserDepartment ud = _userDepartmentRepository.FindByUserId(userId).Where(d => d.DepartmentId == departmentId).SingleOrDefault();

                if(ud == null)
                {
                    ud = DomainObjectFactory.CreateUserDepartment();
                    _userDepartmentRepository.Add(ud);
                }

                ud.CurrentDep = currentDep;
                ud.DepartmentId = departmentId;
                ud.IsDeleted = isDeleted;
                ud.IsDepartmentManager = isDepartmentManager;
                ud.UserId = userId;
                ud.ValidFrom = validFrom;
                ud.ValidTo = validTo;

                work.Commit();

            	var userDepLogEntity = new UserDepartmentEventEntity(_userDepartmentRepository.FindById(ud.Id));

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  userDepLogEntity.GetCreateMessage());
            }
        }

        public void AddDepartmentManager(int userId, int departmentId, DateTime validFrom, DateTime validTo)
        {
            AddUserDepartment(false, departmentId, false, true, userId, validFrom, validTo);
        }

        public void SetCurrentDepartament(int userId, bool value)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserDepartment userDepartment = _userDepartmentRepository.FindByUserId(userId).FirstOrDefault();
                if (userDepartment != null)
                {
                    userDepartment.CurrentDep = value;

                    work.Commit();

					var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserDepartmentChanged", new List<string> { userDepartment.Department.Name, userDepartment.User.LoginName }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUDCurrentDepartment", new List<string> { value.ToString() }));
                	
					_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  message.ToString());
                }
            }
        }

        public void UpdateUserDepartment(int userId, int departmentId, DateTime validFrom, DateTime validTo)
        {
            UpdateUserDepartment(userId, departmentId, validFrom, validTo, false);
        }

        public void UpdateUserDepartment(int userId, int departmentId, DateTime validFrom, DateTime validTo, bool isDelete)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserDepartment userDepartment = _userDepartmentRepository.FindByUserId(userId).Where(ud => ud.DepartmentId == departmentId).SingleOrDefault();
            	
				if (userDepartment == null)
                {
                    userDepartment = DomainObjectFactory.CreateUserDepartment();
                    _userDepartmentRepository.Add(userDepartment);
					userDepartment.UserId = userId;
					userDepartment.DepartmentId = departmentId;
					userDepartment.ValidFrom = validFrom;
					userDepartment.ValidTo = validTo;
					userDepartment.CurrentDep = false;
					userDepartment.IsDepartmentManager = userDepartment.IsDepartmentManager && !isDelete;
					userDepartment.IsDeleted = isDelete;

					work.Commit();

                	var userDepLogEntity =
                		new UserDepartmentEventEntity(_userDepartmentRepository.FindById(userDepartment.Id));

					_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  userDepLogEntity.GetCreateMessage());
                }
				else
				{
					var userDepLogEntity =
						new UserDepartmentEventEntity(userDepartment);
					userDepartment.UserId = userId;
					userDepartment.DepartmentId = departmentId;
					userDepartment.ValidFrom = validFrom;
					userDepartment.ValidTo = validTo;
					userDepartment.CurrentDep = false;
					userDepartment.IsDepartmentManager = userDepartment.IsDepartmentManager && !isDelete;
					userDepartment.IsDeleted = isDelete;
					
					work.Commit();

					userDepLogEntity.SetNewUserDepartment(_userDepartmentRepository.FindById(userDepartment.Id));
					_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  userDepLogEntity.GetEditMessage());
				}
            }
        }

        public IEnumerable<UserDepartment> GetDepartmentManagers(int departmentId)
        {
            IEnumerable<UserDepartment> userDepartments =
                _userDepartmentRepository.FindAll(
                    ud => !ud.IsDeleted && ud.DepartmentId == departmentId);

            var managers = new List<UserDepartment>();
            foreach (var userDepartment in userDepartments)
            {
                foreach (UserRole role in _userRoleRepository.FindByUserId(userDepartment.UserId))
                {
                    if (role.Role.RoleTypeId == (int)FixedRoleType.DepartmentManager) 
                    {
                        managers.Add(userDepartment);
                        break;
                    }
                }
            }

            return managers;
        }

        public void MoveToDepartment(int userId, int oldDepartmentId, int departmentId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserDepartment oldUserDepartment =
                    _userDepartmentRepository.FindAll().Where(
                        ud => ud.UserId == userId && ud.DepartmentId == oldDepartmentId && !ud.IsDeleted).
                        SingleOrDefault();

                UserDepartment newUserDepartment =
                    _userDepartmentRepository.FindAll().Where(
                        ud => ud.UserId == userId && ud.DepartmentId == departmentId && !ud.IsDeleted).
                        SingleOrDefault();

                if (oldUserDepartment != null)
                {
                    if (newUserDepartment != null)
                    {
                            oldUserDepartment.IsDeleted = true;
                    }
                    else
                    {
                            oldUserDepartment.DepartmentId = departmentId;
                    }

                    work.Commit();

                	var user = _userRepository.FindById(userId);
                	var dep1 = _departmentRepository.FindById(oldDepartmentId);
                	var dep2 = _departmentRepository.FindById(departmentId);

                	var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDepartmentForUserChangedFromTo", new List<string> { user.LoginName,
                	                         dep1.Name, dep2.Name }));

					_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  message.ToString());
                }
            }
        }

		public void DeleteUserFromDepartments(int userId)
		{
			using (IUnitOfWork work = UnitOfWork.Begin())
			{
				IEnumerable<UserDepartment> userDepartments = _userDepartmentRepository.FindAll().Where(
						ud => ud.UserId == userId && !ud.IsDeleted);

				var user = _userRepository.FindById(userId);

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUDDepartmentsForUserRemoved", new List<string> { user.LoginName }));
				foreach (var ud in userDepartments)
				{
					ud.IsDeleted = true;
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUDDepartmentRemoved", new List<string> { ud.Department.Name }));
				}

				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  message.ToString());
			}
		}

		public void DeleteDepartmentUserWithRole(int departmentId, int roleTypeId)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				List<UserDepartment> userDepartments = _userDepartmentRepository.FindAll().Where(
						ud => !ud.IsDeleted && ud.DepartmentId == departmentId).ToList();
				var roleType = _roleTypeRepository.FindById(roleTypeId);

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDepartmentsWithRoleRemoved", new List<string> { roleType.Name }));

				foreach( var user in userDepartments )
				{
					if( user.User.UserRoles.Where(ur => ur.Role.RoleTypeId == roleTypeId).FirstOrDefault() != null )
					{
						user.IsDeleted = true;
						message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserDeleted", new List<string> { user.User.LoginName }));
					}
				}

				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag , CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  message.ToString());
			}
		}
    }
}