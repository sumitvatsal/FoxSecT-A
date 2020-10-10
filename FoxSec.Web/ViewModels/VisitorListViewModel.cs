using FoxSec.DomainModel.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class VisitorListViewModel : PaginatorViewModelBase
    {

        public VisitorListViewModel()
        {
            Visitors = new List<VisitorItem>();
        }

        public IEnumerable<VisitorItem> Visitors { get; set; }

        public int FilterCriteria { get; set; }
        //public List<CustomiseVisitor> VisitorNew { get; set; }
        public bool IsDispalyColumn1 { get; set; }

        public bool IsDispalyColumn2 { get; set; }

        public bool IsDispalyColumn3 { get; set; }

        public bool IsDispalyColumn4 { get; set; }

        public bool IsDispalyColumn5 { get; set; }

        public bool IsDispalyColumn6 { get; set; }

        public bool IsDispalyColumn7 { get; set; }

        public bool Comment { get; set; }

        public int StaticId { get; set; }
    }

    public class CustomiseVisitor
    {
        //public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string PersonalCode { get; set; }
        // public string Name { get; set; }
    }
    public class VisitorItem
    {
        public IEnumerable<SelectListItem> RoleItems { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        public IEnumerable<SelectListItem> BuildingItems { get; set; }
        public IEnumerable<UsersAccessUnit> UsersAccessUnits { get; set; }
        public IEnumerable<UserDepartmentItem> UserDepartments { get; set; }
        public List<UserBuildingItem> UserBuildingObjects { get; set; }
        public UserAccessUnitListViewModel UserCards { get; set; }
        public PermissionTreeViewModel UserPermissionTree { get; set; }
        public VisitorItem FoxSecVisitor { get; set; }
        public int? UserDepartmentItemId { get; set; }
        public UserRoleModel UserRoleItems { get; set; }
        public IEnumerable<SelectListItem> UserDepartmentsList { get; set; }
        public VisitorItem()
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
        }

        public string CompanyName { get; set; }

        public int Id { get; set; }
        public string CarNr { get; set; }
        public Nullable<int> UserId { get; set; }
        public string FirstName { get; set; }
        public string CarType { get; set; }
        // public Nullable<System.DateTime> StartDateTime { get; set; }
        // public Nullable<System.DateTime> StopDateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? StopDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        //public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public DateTime? UpdateDatetime { get; set; }
        //public System.DateTime LastChange { get; set; }
        public DateTime? LastChange { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public byte[] Timestamp { get; set; }
        public bool Accept { get; set; }
        public Nullable<int> AcceptUserId { get; set; }

        //public Nullable<System.DateTime> AcceptDateTime { get; set; }
        public DateTime? AcceptDateTime { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public string Company { get; set; }
        public Nullable<int> ParentVisitorsId { get; set; }
        public string Comment { get; set; }
        //public Nullable<System.DateTime> ReturnDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCarNrAccessUnit { get; set; }
        public bool IsPhoneNrAccessUnit { get; set; }
        public Nullable<int> ResponsibleUserId { get; set; }
        public bool CardNeedReturn { get; set; }

        public string UserPermissionGroupName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateReturn { get; set; }
        public string ChangeLast { get; set; }
        public string BarCodeImage { get; set; }
        public string UserStatus
        {
            get
            {
                if (StopDateTime >= DateTime.Now)
                    return "A";
                else
                    return "D";
            }
        }
        public string PersonalCode { get; set; }
        public bool CardBackFlag { get; set; }
        public string CardFirstName { get; set; }
        public string CardLastName { get; set; }

    }
}