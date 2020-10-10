using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class UserListViewModel : PaginatorViewModelBase
    {
        public UserListViewModel()
        {
            Users = new List<UserItem>();
        }

        public IEnumerable<UserItem> Users { get; set; }

        public List<CustomiseUser> UserNew { get; set; }
        public int FilterCriteria { get; set; }

        public bool IsDispalyColumn1 { get; set; }

        public bool IsDispalyColumn2 { get; set; }

        public bool IsDispalyColumn3 { get; set; }

        public bool IsDispalyColumn4 { get; set; }

        public bool IsDispalyColumn5 { get; set; }

        public bool IsDispalyColumn6 { get; set; }

        public bool Comment { get; set; }
    }

    public class CustomiseUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalCode { get; set; }
        public string Name { get; set; }
    }

    public class UserItem
    {
        public UserItem()
        {
            RoleItems = new List<SelectListItem>();
            UserBuildingObjects = new List<UserBuildingItem>();
            BuildingItems = new List<SelectListItem>();
            CompanyName = "-";
            UserCards = new UserAccessUnitListViewModel();
            UserPermissionTree = new PermissionTreeViewModel();
            UserDepartmentsList = new List<SelectListItem>();
            UserDepartments = new List<UserDepartmentItem>();
            UserRoleItems = new UserRoleModel();
            UsersAccessUnit = new List<UsersAccessUnit>();
        }

        public int? Id { get; set; }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PersonalId { get; set; }

        public IEnumerable<SelectListItem> RoleItems { get; set; }

        public UserRoleModel UserRoleItems { get; set; }

        public bool Active { get; set; }

        public string Comment { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedLast { get; set; }

        public string OccupationName { get; set; }

        public string PhoneNumber { get; set; }

        public Int16? WorkHours { get; set; }

        public int? GroupId { get; set; }

        public byte[] Image { get; set; }

        public DateTime? Birthday { get; set; }

        public string Birthplace { get; set; }

        public string FamilyState { get; set; }

        public string Citizenship { get; set; }

        public string Residence { get; set; }

        public string Nation { get; set; }

        public string CardNumber { get; set; }

        public string DepartmentName { get; set; }

        public string ValidToStr { get; set; }

        public string Roles { get; set; }

        public int? TitleId { get; set; }

        public string TitleName { get; set; }

        public string ContractNum { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public DateTime? PermitOfWork { get; set; }

        public bool? PermissionCallGuests { get; set; }

        public bool? MillitaryAssignment { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }

        public IEnumerable<UsersAccessUnit> UsersAccessUnits { get; set; }

        public String PersonalCode { get; set; }

        public String ExternalPersonalCode { get; set; }

        public int? LanguageId { get; set; }

        public DateTime RegistredStartDate { get; set; }

        public DateTime RegistredEndDate { get; set; }

        public int? TableNumber { get; set; }

        public virtual bool? WorkTime { get; set; }

        public bool EServiceAllowed { get; set; }

        public bool IsVisitor { get; set; }

        public bool? CardAlarm { get; set; }

        public bool? IsShortTermVisitor { get; set; }

        public bool? ApproveTerminals { get; set; }

        public bool? ApproveVisitor { get; set; }



        // custom:

        public DateTime? ValidTo { get; set; }

        public bool IsInCreateMode { get; set; }

        public string BirthDayStr { get; set; }

        public string ContractStartDateStr { get; set; }

        public string ContractEndDateStr { get; set; }

        public string PermitOfWorkStr { get; set; }

        public int? CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string DepartmentStartDateStr { get; set; }

        public string DepartmentEndDateStr { get; set; }

        public IEnumerable<SelectListItem> UserDepartmentsList { get; set; }

        public IEnumerable<UserDepartmentItem> UserDepartments { get; set; }

        public List<UsersAccessUnit> UsersAccessUnit { get; set; }

        public List<UserBuildingItem> UserBuildingObjects { get; set; }

        public int? UserDepartmentItemId { get; set; }

        public int RoleId { get; set; }

        public string RegistredDateStr { get; set; }

        public string PIN1 { get; set; }

        public string PIN2 { get; set; }

        public IEnumerable<SelectListItem> BuildingItems { get; set; }

        public bool IsCompanyManager { get; set; }

        public bool IsBuildingAdmin { get; set; }

        public bool IsSuperAdmin { get; set; }

        public UserAccessUnitListViewModel UserCards { get; set; }

        public PermissionTreeViewModel UserPermissionTree { get; set; }

        public string UserPermissionGroupName { get; set; } //Added
        public string UserStatus
        {
            get
            {
                return Active ? ViewResources.SharedStrings.FilterActiveShort : ViewResources.SharedStrings.FilterDeactivatedShort;
            }
        }

        public int? buildingID { get; set; }
        public string buildingName { get; set; }

        public List<UserBuildingObjects> UserBuildingObjectsItems { get; set; }
    }

    public class UserBuildingItem
    {
        public UserBuildingItem()
        {
            FloorItems = new List<SelectListItem>();
            IsBuildingAvailable = true;
        }

        public int? UserId { get; set; }

        public int? BuildingObjectId { get; set; }

        public bool IsDeleted { get; set; }

        public int? BuildingId { get; set; }

        public IEnumerable<SelectListItem> FloorItems { get; set; }

        public bool IsBuildingAvailable { get; set; }

        public string BuildingName { get; set; }

        public string FloorName { get; set; }
    }

    public class UserRoleModel
    {

        public int UserId { get; set; }

        public List<UserRoleItem> Roles { get; set; }

        public bool IsCurrentUser { get; set; }

        public virtual bool? WorkTime { get; set; }

        public bool? EServiceAllowed { get; set; }

        public bool? IsVisitor { get; set; }

        public bool? IsShortTermVisitor { get; set; }

        public bool? ApproveTerminals { get; set; }

        public bool? ApproveVisitor { get; set; }


        public bool? CardAlarm { get; set; }

        public bool? CannotAddUsersAndCard { get; set; }

        public UserRoleModel()
        {
            Roles = new List<UserRoleItem>();
        }

    }

    public class UserRoleItem
    {
        public int? Id { get; set; }

        public int RoleId { get; set; }

        public bool IsSelected { get; set; }

        [RegularExpression("(0?[1-9]|[12][0-9]|3[01]).(0?[1-9]|1[0-2]).(20[0-9]{2}|[2][0-9][0-9]{2})", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "CommonDateFormat")]
        public string ValidFrom { get; set; }

        [RegularExpression("(0?[1-9]|[12][0-9]|3[01]).(0?[1-9]|1[0-2]).(20[0-9]{2}|[2][0-9][0-9]{2})", ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "CommonDateFormat")]
        public string ValidTo { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }
    }

    public class UserBuildingObjects
    {
        public int Id { get; set; }

        public string BuildingObjectName { get; set; }

        public int BuildingId { get; set; }
        
    }
}