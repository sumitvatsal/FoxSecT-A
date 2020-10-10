namespace FoxSec.DomainModel.DomainObjects
{
    public class FSINISetting : EntityName
    {
        public virtual int SoftType { get; set; }
        public virtual int? SoftId { get; set; }
        public virtual string Value { get; set; }
    }
}
