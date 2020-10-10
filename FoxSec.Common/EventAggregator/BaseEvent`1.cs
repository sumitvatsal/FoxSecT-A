using System;
using System.Linq;

namespace FoxSec.Common.EventAggregator
{
	public abstract class BaseEvent<TPayload> : BaseEvent
	{
		public virtual SubscriptionToken Subscribe(Action<TPayload> action)
		{
			return Subscribe(action, false);
		}

		public virtual SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
		{
			return Subscribe(action, keepSubscriberReferenceAlive, delegate { return true; });
		}

		public virtual SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
		{
			IDelegateReference action_reference = new DelegateReference(action, keepSubscriberReferenceAlive);
			IDelegateReference filter_reference = new DelegateReference(filter, keepSubscriberReferenceAlive);
			var subscription = new EventSubscription<TPayload>(action_reference, filter_reference);
			return base.Subscribe(subscription);
		}

		public virtual void Publish(TPayload payload)
		{
			base.Publish(payload);
		}

		public virtual void Unsubscribe(Action<TPayload> subscriber)
		{
			lock( Subscriptions )
			{
				IEventSubscription event_subscription = Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);

				if( event_subscription != null )
				{
					Subscriptions.Remove(event_subscription);
				}
			}
		}

		public virtual bool Contains(Action<TPayload> subscriber)
		{
			IEventSubscription event_subscription;

			lock( Subscriptions )
			{
				event_subscription = Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
			}
			return (event_subscription != null);
		}
	}
}