using System;

namespace FoxSec.Common.EventAggregator
{
	public interface IEventSubscription
	{
		SubscriptionToken SubscriptionToken{ get; set; }
		Action<object[]> GetExecutionStrategy();
	}
}