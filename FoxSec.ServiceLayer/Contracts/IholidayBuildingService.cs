using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IHolidayBuildingService
    {
        void CreateHolidayBuilding(int holidayId, int BuildingId);
        void DeleteHolidayBuilding(int holidayId, int BuildingId);
        void EditHolidayBuilding(int id, int holidayId, int BuildingId, Boolean isDeleted);
    }

}
