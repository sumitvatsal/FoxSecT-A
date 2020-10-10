using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.DomainModel.DomainObjects
{
    public class TAMove :EntityName// EntityName  //      LookupEntity
    {
       
        // public int Id { get; set; }
       
       //public Int16 PIN { get; set; }
        public int UserId { get; set; }
        public Nullable<System.Int32> DepartmentId { get; set; }
        public string Label { get; set; }
        public string Remark { get; set; }
        public System.DateTime Started { get; set; }
        public Nullable<System.DateTime> Finished { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Schedule { get; set; }
        public Nullable<byte> Status { get; set; }
        public bool JobNotMove { get; set; }
        public bool Completed { get; set; }
        // public bool IsDeleted { get; set; }
        //public byte[] Timestamp { get; set; }
        public System.DateTime ModifiedLast { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.Int32> StartedBoId { get; set; }
        public Nullable<System.Int32> FinishedBoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual User User { get; set; }
        public virtual Department Department { get; set; }
        //public virtual TAReport TAreport { get; set; } //manoranjan
        public string UserName
        {
            get { return User.FirstName + " " + User.LastName; }
            set { }
        }  
        //public int BuildingId { get; set; }

        public DateTime ReportDate { get; set; }

        public int SrNo { get; set; }
        public string PersonalCode { get; set; }
        public DateTime? Birthday { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNo { get; set; }
        public string ContractNo { get; set; }
        public string RprtDate { get; set; }
    }

}

