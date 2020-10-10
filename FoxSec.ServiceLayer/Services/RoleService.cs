using System;
using System.Collections.Generic;
using System.Linq;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Common.Extensions;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.ServiceResults;

namespace FoxSec.ServiceLayer.Services
{
	internal class RoleService : ServiceBase, IRoleService
	{
		private readonly IRoleRepository _roleRepository;
	    private readonly ILogService _logService;
        private readonly IControllerUpdateService _conrtollerCreateService;
        private string flag = "";
        // _controllerUpdateService

        public RoleService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory, IEventAggregator eventAggregator, IRoleRepository roleRepository, ILogService logService, IControllerUpdateService createupdateservice)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
			_roleRepository = roleRepository;

		    _logService = logService;
            _conrtollerCreateService = createupdateservice;

        }

		public RoleCreateResult CreateRole(string name, string description, string createdBy, bool active, int roleTypeId, IEnumerable<RoleBuildingDto> buildings, IEnumerable<int> permissionIds = null, IEnumerable<int> menuIds = null)
		{
			var result = new RoleCreateResult { ErrorCode = RoleServiceErrorCode.Ok };

            if (_roleRepository.FindByName(name).Count() != 0)
            {
                result.ErrorCode = RoleServiceErrorCode.RoleAlreadyExists;
                return result;
            }

			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				Role role = DomainObjectFactory.CreateRole();

			    role.Name = name;
				role.Description = description;
				role.RoleTypeId = roleTypeId;

				role.AssignPermissions(permissionIds.NullSafe());                
                role.AssignMenues(menuIds.NullSafe());

			    role.ModifiedLast = DateTime.Now;
                role.ModifiedBy = createdBy;
			    role.Active = active;
                role.IsDeleted = false;
			    role.StaticId = -1;
				role.UserId = CurrentUser.Get().Id;
				foreach (var building in buildings)
				{
					if( building.IsChecked )
					{
						var roleBuilding = DomainObjectFactory.CreateRoleBuilding();
						roleBuilding.RoleId = role.Id;
						roleBuilding.BuildingId = building.BuildingId.Value;
						roleBuilding.IsDeleted = false;
						role.RoleBuildings.Add(roleBuilding);
					}
				}

				_roleRepository.Add(role);

				work.Commit();

				IFoxSecIdentity identity = CurrentUser.Get();
				var e = new RoleCreatedEventArgs(role, identity.LoginName, identity.FirstName, identity.LastName, DateTime.Now);

				

                var logRoleEntity = new RoleEventEntity(_roleRepository.FindById(role.Id));

			    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
			                          logRoleEntity.GetCreateMessage());
       // _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, CurrentUser.Get().Id, UpdateParameter.UserStatusChanged, ControllerStatus.Created, "Active");
                EventAggregator.GetEvent<RoleCreatedEvent>().Publish(e);
                _conrtollerCreateService.CreateControllerUpdate(CurrentUser.Get().Id, CurrentUser.Get().Id, UpdateParameter.UserRoleManagement, ControllerStatus.Created, "Active");

            }

			return result;
		}

        public RoleCreateResult EditRole(int id, string name, string description, string modifiedBy, bool active, int roleTypeId, IEnumerable<RoleBuildingDto> buildings, IEnumerable<int> permissionIds = null, IEnumerable<int> menuIds = null)
		{
			var result = new RoleCreateResult { ErrorCode = RoleServiceErrorCode.Ok };
			if( _roleRepository.FindAll().Where(x=>!x.IsDeleted && x.Name == name && x.Id != id).Count() > 0 )
			{
				result.ErrorCode = RoleServiceErrorCode.RoleAlreadyExists;
				return result;
			}

			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				Role role = _roleRepository.FindById(id);
                var logRoleEntity = new RoleEventEntity(role);

			    role.Name = name;
			    role.Description = description;
				role.RoleTypeId = roleTypeId;
                role.AssignPermissions(permissionIds.NullSafe());
                role.AssignMenues(menuIds.NullSafe());

                role.ModifiedLast = DateTime.Now;
                role.ModifiedBy = modifiedBy;
			    role.Active = active;



				IFoxSecIdentity identity = CurrentUser.Get();
				var e = new RoleEditedEventArgs(role, identity.LoginName, identity.FirstName, identity.LastName, DateTime.Now);

				foreach (var building in buildings)
				{
					if (building.IsChecked)
					{
						var roleBuilding =
							role.RoleBuildings.Where(x => x.RoleId == id && x.BuildingId == building.BuildingId.Value).FirstOrDefault();
						if(roleBuilding == null)
						{
							roleBuilding = DomainObjectFactory.CreateRoleBuilding();
						}
						roleBuilding.RoleId = role.Id;
						roleBuilding.BuildingId = building.BuildingId.Value;
						roleBuilding.IsDeleted = false;
						role.RoleBuildings.Add(roleBuilding);
					}
					else
					{
						var roleBuilding =
							role.RoleBuildings.Where(x => x.RoleId == id && x.BuildingId == building.BuildingId.Value).FirstOrDefault();
						if( roleBuilding != null )
						{
							roleBuilding.IsDeleted = true;
						}
					}
				}


				work.Commit();
				
				e.SetNewRole(role);
				EventAggregator.GetEvent<RoleEditedEvent>().Publish(e);

                logRoleEntity.SetNewRole(role);
                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      logRoleEntity.GetEditMessage());
                _conrtollerCreateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserRoleManagement, ControllerStatus.Created, "Active");

            }

        	return result;
		}

        public void DeleteRole(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Role role = _roleRepository.FindById(id);
                var logRoleEntity = new RoleEventEntity(role);

                IFoxSecIdentity identity = CurrentUser.Get();
                var e = new RoleDeletedEventArgs(role, identity.LoginName, identity.FirstName, identity.LastName, DateTime.Now);

                role.IsDeleted = true;
                work.Commit();

                EventAggregator.GetEvent<RoleDeletedEvent>().Publish(e);

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, logRoleEntity.GetDeleteMessage());
            }
        }
	}

	public enum RoleServiceErrorCode
	{
		Ok = 0,
		RoleAlreadyExists = 1
	}

	public class RoleBuildingDto
	{
		public int? Id { get; set; }

		public int? RoleId { get; set; }

		public int? BuildingId { get; set; }

		public bool IsChecked { get; set; }

	}
}