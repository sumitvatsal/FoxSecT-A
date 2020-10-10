using System;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IBuildingObjectService
    {
        int CreateOrFindBuildingFloorId(int buildingId, int floorNr, string description, int? onjectNr = null);

    	void SetComment(int id, string comment, string host);

        int EditBuilding(int id, int? Global, string host);
    }
}