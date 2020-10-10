using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FoxSec.Common.Enums;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Visitor:Entity
    {
        public new int Id { get; set; }
        public virtual string CarNr { get; set; }

      /* [Key, ForeignKey("User")] */
        public virtual Nullable<int> UserId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string CarType { get; set; }
        public virtual Nullable<System.DateTime> StartDateTime { get; set; }
        public virtual Nullable<System.DateTime> StopDateTime { get; set; }
        public virtual bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        //public virtual Nullable<System.DateTime> UpdateDatetime { get; set; }
        public virtual DateTime? UpdateDatetime { get; set; }
       // public virtual System.DateTime LastChange { get; set; }
        public virtual DateTime? LastChange { get; set; }
        public virtual Nullable<int> CompanyId { get; set; }
        public virtual new byte[] Timestamp { get; set; }
        public virtual bool Accept { get; set; }
        public virtual  Nullable<int> AcceptUserId { get; set; }
        // public virtual Nullable<System.DateTime> AcceptDateTime { get; set; }
       // public virtual DateTime? AcceptDateTime { get; set; }
        public virtual DateTime? AcceptDateTime { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool Active { get; set; }
        public virtual string Company { get; set; }
        public virtual Nullable<int> ParentVisitorsId { get; set; }
        public virtual string Comment { get; set; }
        // public virtual Nullable<System.DateTime> ReturnDate { get; set; }
        public virtual DateTime? ReturnDate { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool IsCarNrAccessUnit { get; set; }
        public virtual bool IsPhoneNrAccessUnit { get; set; }
        public virtual Nullable<int> ResponsibleUserId { get; set; }
        public virtual bool CardNeedReturn { get; set; }

        //realtions
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateReturn { get; set; }

        public string ChangeLast { get; set; }

        public string PersonalCode { get; set; }

        public virtual User User { get; set; }

        /*Added*/

        public virtual ICollection<UserRole> VisitorRoles { get; set; }

        public virtual ICollection<UsersAccessUnit> VisitorsAccessUnits { get; set; }

        public virtual ICollection<UserDepartment> VisitorDepartments { get; set; }

        public virtual ICollection<UserPermissionGroup> VisitorPermissionGroups { get; set; }

        public virtual ICollection<CompanyManager> CompanyManagers { get; set; }

        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<LogFilter> LogFilters { get; set; }

        public virtual ClassificatorValue ClassificatorValue { get; set; }

        public virtual ICollection<UserTimeZone> VisitorTimeZones { get; set; }

        public virtual ICollection<UserBuilding> VisitorBuildings { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
