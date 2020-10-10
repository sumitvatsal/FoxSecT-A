using System;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface IControllerUpdateService
	{
		void CreateControllerUpdate(int userId, int entityId, UpdateParameter parameterId, ControllerStatus statusId, string value);

		void EditControllerUpdate(int id, int userId, int entityId, UpdateParameter parameterId, ControllerStatus statusId, string value);

		void DeleteControllerUpdate(int id, int userId, int entityId, UpdateParameter parameterId, ControllerStatus statusId, string value);
	}
}