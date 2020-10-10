using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.Core.SystemEvents.DTOs
{
    public class TAMoveEntity  : LogEntity
    {

        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Label { get; set; }
        //public string Name { get; set; }
        public System.DateTime Started { get; set; }
        public System.DateTime Finished { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }

        public Nullable<byte> Status { get; set; }
        public bool Completed { get; set; }
        //      public bool IsDeleted { get; set; }
        //      public byte[] Timestamp { get; set; }
        public System.DateTime ModifiedLast { get; set; }
        public string ModifiedId { get; set; }

        // relations:
      //  public virtual User User { get; set; }
//public virtual Department Department { get; set; }
    }
}
