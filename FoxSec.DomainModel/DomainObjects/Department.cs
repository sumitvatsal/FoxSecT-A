using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Department : EntityName
    {
        public virtual string Manager { get; set; }
        public virtual string Number { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime ModifiedLast { get; set; }
        public virtual int CompanyId { get; set; }

        // relations:
        public virtual ICollection<UserDepartment> UserDepartments { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<TAMove> TAMoves { get; set; }
        public virtual ICollection<TAReport> TAReports { get; set; }
    }
}