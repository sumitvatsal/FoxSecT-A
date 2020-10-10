namespace FoxSec.DomainModel.DomainObjects
{
	public class Entity
	{
		protected internal Entity() {}

	    public virtual int Id { get; set; }

		public virtual byte[] Timestamp { get; set; }
	}
}