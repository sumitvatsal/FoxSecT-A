using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.DomainModel.DomainObjects
{
    public class TAReport : EntityName //      LookupEntity
    {

        //  public new int Id { get; set; }
        public int UserId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public System.DateTime ReportDate { get; set; }
        public short Day { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Shift { get; set; }
        public Nullable<byte> Status { get; set; }
        public bool Completed { get; set; }

        //public byte[] Timestamp { get; set; }
        public System.DateTime ModifiedLast { get; set; }
        public int ModifiedId { get; set; }
        public int? BuildingId { get; set; }

        // relations:
        public virtual User User { get; set; }
        public virtual Department Department { get; set; }
        public virtual Building Building { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string UserName
        {
            get
            {
                var usr = User;
                if (usr != null)
                {
                    return User.LastName + " " + User.FirstName;
                }
                else
                {
                    return null;
                }
            }
            set { }
        }

        public string UserID { get; set; }
        public string FullName { get; set; }
    }

}
/*
        public int Id { get; set; }
        public int UserId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string Name { get; set; }
        public System.DateTime ReportDate { get; set; }
        public short Day { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Shift { get; set; }
        public Nullable<byte> Status { get; set; }
        public bool Completed { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Timestamp { get; set; }
        public System.DateTime ModifiedLast { get; set; }
        public int ModifiedId { get; set; }
        public Nullable<int> BuildingId { get; set; }
    
        public virtual Building Building { get; set; }
        public virtual Department Department { get; set; }
        public virtual User User { get; set; }

     
     */
