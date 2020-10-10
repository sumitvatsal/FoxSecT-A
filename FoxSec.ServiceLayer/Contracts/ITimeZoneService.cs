using System;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface ITimeZoneService
    {
        int CreateTimeZone(string name);
        void UpdateTimeZone(int id, string name);
        void ActivateTimeZone(int id);
        void DeactivateTimeZone(int id);
        void SetTimeZoneSate(int id, bool state);
        void DeleteTimeZone(int id);
        void ToggleZone(int id, int day);
        void UpdateFromTime(int id, DateTime? time);
        void UpdateToTime(int id, DateTime? time);

        int CreateTimeZoneProperty(int timeZoneId, int order);
        int CreateTimeZoneProperty(int timeZoneId,
                                   int order,
                                   DateTime? validFrom,
                                   DateTime? validTo,
                                   bool isMonday,
                                   bool isTuesday,
                                   bool isWednesday,
                                   bool isThursday,
                                   bool isFriday,
                                   bool isSaturday,
                                   bool isSunday);
    }
}