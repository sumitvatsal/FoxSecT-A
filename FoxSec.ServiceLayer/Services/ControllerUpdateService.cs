using System;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.DomainModel;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
	internal class ControllerUpdateService : ServiceBase, IControllerUpdateService
	{
		private readonly IControllerUpdateRepository _controllerUpdateRepository;
		
		public ControllerUpdateService(ICurrentUser currentUser,
										IDomainObjectFactory domainObjectFactory,
										IEventAggregator eventAggregator,
										IControllerUpdateRepository controllerUpdateRepository)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
			_controllerUpdateRepository = controllerUpdateRepository;
		}


		public void CreateControllerUpdate(int userId, int entityId, UpdateParameter parameterId, ControllerStatus statusId, string value)
		{
			ControllerUpdate cu =
				_controllerUpdateRepository.FindAll().Where(x => x.ParameterId == (int)parameterId && x.EntityId == entityId).
					FirstOrDefault();

			if (cu == null)
			{
				using (IUnitOfWork work = UnitOfWork.Begin())
				{
				    cu = new ControllerUpdate {DateAdded = DateTime.Now};
				    cu.DateLastChanged = cu.DateAdded;
					cu.ParameterId = (int) parameterId;
					cu.Status = (int) statusId;
					cu.EntityId = entityId;
					cu.Value = string.IsNullOrEmpty(value) ? "" : value;
					cu.IsDeleted = statusId == ControllerStatus.Deleted;
					_controllerUpdateRepository.Add(cu);

					/*var removedEntities =
                        _controllerUpdateRepository.FindAll().Where(x => x.DateLastChanged.AddDays(_configurationSettings.ControllerUpdateRecordLife) < DateTime.Now);
                    foreach (var controllerUpdate in removedEntities)
                    {
                        _controllerUpdateRepository.Delete(controllerUpdate);
                    }
                    */

					work.Commit();
				}
			}
			else
			{
				if( statusId == ControllerStatus.Deleted )
				{
					DeleteControllerUpdate(cu.Id, userId, entityId, parameterId, statusId, value);
				}
				else
				{
					EditControllerUpdate(cu.Id, userId, entityId, parameterId, ControllerStatus.Edited, value);
				}
			}
		}

		public void EditControllerUpdate(int id, int userId, int entityId, UpdateParameter parameterId, ControllerStatus statusId, string value)
		{
			using (IUnitOfWork work = UnitOfWork.Begin())
			{
				var cu = _controllerUpdateRepository.FindById(id);
				cu.DateLastChanged = DateTime.Now;
				cu.ParameterId = (int)parameterId;
				cu.Status = (int)statusId;
				cu.EntityId = entityId;
				cu.Value = string.IsNullOrEmpty(value) ? "" : value;
				cu.IsDeleted = false;
				work.Commit();
			}
		}

		public void DeleteControllerUpdate(int id, int userId, int entityId, UpdateParameter parameterId, ControllerStatus statusId, string value)
		{ 
			using (IUnitOfWork work = UnitOfWork.Begin()) 
			{
				var cu = _controllerUpdateRepository.FindById(id);
				cu.DateLastChanged = DateTime.Now;
				cu.ParameterId = (int)parameterId;
				cu.Status = (int)statusId;
				cu.EntityId = entityId;
				cu.IsDeleted = true;
				if( !string.IsNullOrEmpty(value) )
				{
					cu.Value = value;
				}
				work.Commit();
			}
		}
	}
}