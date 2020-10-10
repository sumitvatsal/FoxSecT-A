using System;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IUserAccessUnitTypeService
    {
        void EditCardType(int id, string name, bool isCardCode, bool isSerDK, string description);
        void DeleteCardType(int id);
        int CreateCardType(string name, bool isCardCode, bool isSerDK, string description);
    }
}