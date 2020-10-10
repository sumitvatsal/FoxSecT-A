using System;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface IUserBuildingService
	{
		int CreateUserBuilding(int userId, int buildingId, int? buildingObjectId, string host);
		void DeleteUserBuildings(int userId, string host);
	}
}