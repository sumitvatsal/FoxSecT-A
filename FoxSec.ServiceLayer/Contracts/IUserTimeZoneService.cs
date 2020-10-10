using System;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IUserTimeZoneService
    {
       // void SynchonizeUserTimeZones(int userId);
        int CreateUserTimeZone(string name);
        int CreateUserTimeZoneProperty(int timeZoneId, int order);
        int CreateUserTimeZoneProperty(int userTimeZoneId, int order, DateTime? validFrom, DateTime? validTo, bool isMonday, bool isTuesday, bool isWednesday, bool isThursday, bool isFriday, bool isSaturday, bool isSunday);
        void UpdateUserTimeZone(int id, string name);
        void DeleteUserTimeZone(int id);
        void ToggleUserZone(int id, int day);
        void UpdateUserFromTime(int id, DateTime? time);
        void UpdateUserToTime(int id, DateTime? time);
        void GroupUpdateName(int utzId, string Name);

        void RecoveryUserTimeZone(int p);
    }
}