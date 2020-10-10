using System;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface IHolidayService
	{
        void CreateHoliday(string name, string createdBy, DateTime eventStart, DateTime eventEnd, bool holidayMoving);
        void DeleteHoliday(int id);
	    void EditHoliday(int id, string name, string modifiedBy, DateTime eventStart, DateTime eventEnd);//, bool holidayMoving);
	    void SaveMoving(int id, bool isChecked);
	}
}