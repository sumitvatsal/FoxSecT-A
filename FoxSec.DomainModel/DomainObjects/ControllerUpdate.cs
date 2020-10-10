using System;

namespace FoxSec.DomainModel.DomainObjects
{
    public class ControllerUpdate : Entity
    {
        public virtual int? ControllerId { get; set; }

        public virtual int EntityId { get; set; }

        public virtual int ParameterId { get; set; }

        public virtual string Value { get; set; }

        public virtual string MemoryStartAddress { get; set; }

        public virtual int Status { get; set; }

        public virtual DateTime DateAdded { get; set; }

        public virtual DateTime DateLastChanged { get; set; }

        public virtual bool IsDeleted { get; set; }
    }

    public enum UpdateParameter
    {
        UserNameChanged = 1,

        UserPin1Changed = 2,

        UserPin2Changed = 3,

        UserRoleValidationPeriodChanged = 4,

        UserStatusChanged = 5,

        UserCardChange = 6,

        CardStatusChange = 7,

        HolidayChange = 8,

        TimeZoneChange = 9,

        PermissionGroupChangeAll = 10,

        SpecificTimeZoneChange = 11,

        UserPermissionGroupChange = 12,

        UserSpecificPermissionChange = 13,

        RoomPermissionChange = 14,

        UpgBuildingObjectTimeZoneChange = 15,

        UserTimeZoneUpdate = 16,

        PictureUpdate = 17,

        TAReportChange = 18,

        UserRoleManagement = 19,
    }

    public enum ControllerStatus
    {
        Created = 1,

        Deleted = 2,

        Edited = 3
    }
}