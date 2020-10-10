using FoxSec.DomainModel.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.Core.SystemEvents.DTOs
{
   public class VisitorEntity:LogEntity
    {
        //public int Id { get; set; }
        public string CarNr { get; set; }
        public Nullable<int> UserId { get; set; }
        public string FirstName { get; set; }
        public string CarType { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> StopDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public System.DateTime LastChange { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public byte[] Timestamp { get; set; }
        public bool Accept { get; set; }
        public Nullable<int> AcceptUserId { get; set; }
        public Nullable<System.DateTime> AcceptDateTime { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public string Company { get; set; }
        public Nullable<int> ParentVisitorsId { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCarNrAccessUnit { get; set; }
        public bool IsPhoneNrAccessUnit { get; set; }
        public Nullable<int> ResponsibleUserId { get; set; }
        public bool CardNeedReturn { get; set; }

        /*relations*/
         public virtual User User { get; set; }
        public string Image { get; set; }
    }
}
