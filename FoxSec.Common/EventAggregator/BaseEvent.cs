using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace FoxSec.Common.EventAggregator
{
	public abstract class BaseEvent
	{
		private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

		protected ICollection<IEventSubscription> Subscriptions
		{

            [DebuggerStepThrough]
			get
			{
				return _subscriptions;
			}
		}

		protected virtual SubscriptionToken Subscribe(IEventSubscription eventSubscription)
		{
			eventSubscription.SubscriptionToken = new SubscriptionToken();

			lock( _subscriptions )
			{
				_subscriptions.Add(eventSubscription);
			}
			return eventSubscription.SubscriptionToken;
		}

		protected virtual void Publish(params object[] arguments)
		{
			IEnumerable<Action<object[]>> execution_strategies = PruneAndReturnStrategies();

			foreach( var execution_strategy in execution_strategies )
			{
				execution_strategy(arguments);
			}
		}

		public virtual void Unsubscribe(SubscriptionToken token)
		{
			lock( _subscriptions )
			{
				IEventSubscription subscription = _subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);

				if( subscription != null )
				{
					_subscriptions.Remove(subscription);
				}
			}
		}

		public virtual bool Contains(SubscriptionToken token)
		{
			lock( _subscriptions )
			{
				IEventSubscription subscription = _subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);

				return (subscription != null);
			}
		}

		private IEnumerable<Action<object[]>> PruneAndReturnStrategies()
		{
			var return_list = new List<Action<object[]>>();

			lock( _subscriptions )
			{
				for( int i = _subscriptions.Count - 1; i >= 0; i-- )
				{
					Action<object[]> subscription_action = _subscriptions[i].GetExecutionStrategy();

					if( subscription_action == null )
					{
						_subscriptions.RemoveAt(i);
					}
					else
					{
						return_list.Add(subscription_action);
					}
				}
			}
			return return_list;
		}
	}
}