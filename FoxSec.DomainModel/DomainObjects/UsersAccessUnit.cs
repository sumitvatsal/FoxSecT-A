using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoxSec.DomainModel.DomainObjects
{
  
    public class UsersAccessUnit : Entity
    {
        public virtual int? UserId { get; set; }

        public virtual int? TypeId { get; set; }

        public virtual int? CompanyId { get; set; }

        public virtual string Serial { get; set; }

        public virtual string Code { get; set; }

        public virtual bool Active { get; set; }

        public virtual bool Free { get; set; }

        public virtual DateTime? Opened { get; set; }

        public virtual DateTime? Closed { get; set; }

        public virtual DateTime? ValidFrom { get; set; }

        public virtual DateTime? ValidTo { get; set; }
        [Range(0,99999)]
        [StringLength(5,MinimumLength=5)]
        public virtual string Dk { get; set; }

        public virtual int CreatedBy { get; set; }

        public virtual bool IsDeleted { get; set; }

		public virtual int? ClassificatorValueId { get; set; }

		public int BuildingId { get; set; }

        public virtual DateTime? Classificator_dt { get; set; }

        public virtual string Comment { get; set; }

        public virtual bool? IsMainUnit { get; set; }
        // relations:

        public virtual User User { get; set; }

        public virtual UserAccessUnitType UserAccessUnitType { get; set; } 

        public virtual Company Company { get; set; }

		public virtual ClassificatorValue ClassificatorValue { get; set; }

		public virtual Building Building { get; set; }

        // custom:

        public string CardFullCode
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Code)) return this.Code;
                if (!string.IsNullOrEmpty(this.Serial) && (!string.IsNullOrEmpty(this.Dk))) return this.Serial + "+" + this.Dk;
                return "-";
            }
        }

        public int CardFullStatus
        {
            get
            {
                if (this.Free) return 2;
                return this.Active ? 1 : 0;
            }
        }

        public string CardFullBuildings { get; set; }
    }
}