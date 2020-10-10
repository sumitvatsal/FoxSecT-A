using System;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface ICompanyBuildingObjectService
    {
        int CreateCompanyBuildingObject(int companyId, int buildingObjectId, string host);
        void DeleteCompanyBuildingObjects(int companyId, string host);
    }
}