using System;
using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class UserDepartment : Entity
    {
        public virtual int UserId { get; set; }

        public virtual int DepartmentId { get; set; }
 
        public virtual DateTime ValidFrom { get; set; }

        public virtual DateTime ValidTo { get; set; }

        public virtual bool CurrentDep { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual bool IsDepartmentManager { get; set; }

        // realtions:

        public virtual Department Department { get; set; }

        public virtual User User { get; set; }
    }
}