using FoxSec.DomainModel.DomainObjects;
using System;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IClassificatorService
    {
        void CreateClassificator(string name, string comment, string host);
        void EditClassificator(int id, string name, string comment, string host);
        void DeleteClassificator(int id, string host);
        void CreateClassificatorValue(int classificatorId, string value, string host);
        void DeleteClassificatorValue(int id, string host);
        void EditClassificatorValue(int id, string value, string host);

        int CheckLicenseLessValidation(string type, int value);
        void InsertNewLicense(string type, int value, string remainhashcode, int id, string HostName, string validto,int remaining,string encrypkey);
        void UpdateLicenseValidTo(int id, string host, string validto);

        void InsertLicencePathintbl(int id);
    }
}