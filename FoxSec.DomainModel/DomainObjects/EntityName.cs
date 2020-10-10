namespace FoxSec.DomainModel.DomainObjects
{
    public class EntityName: Entity
    {
        public virtual string Name { get; set; }

        public virtual bool IsDeleted { get; set; }
    }
}