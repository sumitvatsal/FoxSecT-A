using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class RoleFormsAccess
    {
        protected internal RoleFormsAccess(){}

        public virtual int Id { get; set; }
        public virtual int RoleId { get; set; }
        public virtual int FieldId { get; set; }
        public virtual int ForRoleId { get; set; }
        public virtual int AccessTypeId { get; set; }
    }
}