using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.Core.SystemEvents.DTOs
{
    public class TAReportEntity : LogEntity
    {
//      public int Id { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public DateTime ReportDate{ get; set; }
        public Int16 Day { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Shift { get; set; }
        public Nullable<byte> Status { get; set; }
        public bool Completed { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Timestamp { get; set; }
        public System.DateTime ModifiedLast { get; set; }
        public int ModifiedId { get; set; }

        // references
        // relations:
//      public virtual User User { get; set; }
//      public virtual Department Department { get; set; }

    }
}
