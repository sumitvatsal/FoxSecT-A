namespace FoxSec.Common.EventAggregator
{
	public interface IEventAggregator
	{
		TEventType GetEvent<TEventType>() where TEventType : BaseEvent;
	}
}