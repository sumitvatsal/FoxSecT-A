using System;
using System.Collections.Generic;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IUserPermissionGroupService
    {
        int CreateUserPermissionGroup(string name, int? copyFromPgId, int? defaultTimeZoneId, List<int> buildingObjectIds, List<int> selectedBuildingObjectIds);
        int AssignUserPermissionGroup(int userId, int upgId);
        int ChangeUserPermissionGroup(int userId, int newUpgId);
        int AddUserPermissionGroup(int userId, int newUpgId);
        int DelUserPermissionGroup(int userId, int newUpgId);
        int DelPermissionGroupFromUsers(int userId, int dltGroupId);
        bool IsUserHasActivePermission(int userId);
        void AddPermissionsToAdditionalGroups(int userId, List<int> selectedBuildingObjectIds, List<int> ownObjets);
        void SaveUserPermissionGroup(int upgId, List<int> buildingObjectIds, List<int> selectedBuildingObjectIds, bool isGroupOriginal=true ,bool savelog = true);
        void ChangeUserPermissionGroupBuildingTimeZone(int upgId, int buildingObjectId, int newUtzId, bool savelog = true);
        void ChangeUserPermissionGroupBuildingTimeZoneToDefault(int upgId, int buildingObjectId, bool savelog = true);
        int GetUserDefaultTimeZoneId(int upgId);
        int GetUserActiveTimeZoneIdByBuildingObjectId(int upgId, int buildingObjectId);
        IEnumerable<int> GetUserTimeZonesIds(int upgId);
        IEnumerable<int> GetUserBuildingObjectIds(int upgId);
        bool IsDefaultUserTimeZone(int buildingObjectId, int upgId);
        bool ToggleUserArming(int buildingObjectId, int upgId);
		bool ToggleUserDefaultArming(int buildingObjectId, int upgId);
		bool ToggleUserDisarming(int buildingObjectId, int upgId);
		bool ToggleUserDefaultDisarming(int buildingObjectId, int upgId);
        void ChangeUserDefaultTimeZone(int userPermissionGroupId, int zoneId, bool savelog = true);
        void DeleteUserPermissionGroup(int id);
        void UpdateGroupFromTime(int utzId, DateTime fromTime, int orderInGroup);
        void UpdateGroupToTime(int utzId, DateTime toTime, int orderInGroup);
        void UpdateUserPermission(int id, string name);
        void GroupSaveUserPermissionGroup(int upgId, List<int> buildingObjectIds, List<int> selectedBuildingObjectIds, List<int> ownObjets);
        void GroupChangeUserPermissionGroupBuildingTimeZone(int upgId, int buildingObjectId, int newUtzId);
        void GroupChangeUserPermissionGroupBuildingTimeZoneToDefault(int upgId, int buildingObjectId);
        void GroupChangeUserDefaultTimeZone(int upgId, int zoneId);
        void GroupToggleUserArming(int buildingObjectId, int upgId);
	    void GroupToggleUserDefaultArming(int buildingObjectId, int upgId);
	    void GroupToggleUserDisarming(int buildingObjectId, int upgId);
        void GroupToggleUserDefaultDisarming(int buildingObjectId, int upgId);
        void GroupUpdateUserPermission(int upgId, string name, string oldname);
    }
}