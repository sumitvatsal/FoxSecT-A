using System.Collections.Generic;
using System;

namespace FoxSec.DomainModel.DomainObjects
{
    public class ClassificatorValue : Entity
    {
        public virtual int ClassificatorId { get; set; }

        public virtual string Value { get; set; }

        public virtual string Comments { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual string DisplayValue { get; set; }

        public virtual int? Legal { get; set; }

        public virtual string LegalHash { get; set; }
        public virtual int? Remaining { get; set; }

        public virtual string RemainingHash { get; set; }
        public virtual string ValidToHash { get; set; }

        public virtual DateTime? ValidTo { get; set; }
        //relations:
        public virtual Classificator Classificator { get; set; }

		public virtual ICollection<Company> Companies { get; set; }

		public virtual ICollection<UsersAccessUnit> UsersAccessUnits { get; set; }
		
		public virtual ICollection<User> Users { get; set; }
    }
}