using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Classificator : Entity
    {
        public virtual string Description { get; set; }

        public virtual string Comments { get; set; }

        public virtual ICollection<ClassificatorValue> ClassificatorValues { get; set; }
    }
}