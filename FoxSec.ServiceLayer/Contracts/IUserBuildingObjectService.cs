using System;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface IUserBuildingObjectService
	{
		int CreateUserBuildingObject(int userId, int buildingObjectId, string host);
		void DeleteUserBuildingObjects(int userId, string host);
	}
}