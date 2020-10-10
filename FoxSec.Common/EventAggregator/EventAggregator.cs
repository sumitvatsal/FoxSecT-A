using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FoxSec.Common.EventAggregator
{
	public class EventAggregator : IEventAggregator
	{
		private readonly List<BaseEvent> _events = new List<BaseEvent>();
		private readonly ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim();

		public TEventType GetEvent<TEventType>() where TEventType : BaseEvent
		{
			_rwl.EnterUpgradeableReadLock();

			try
			{
				var event_instance = _events.SingleOrDefault(evt => evt.GetType() == typeof(TEventType)) as TEventType;

				if( event_instance == null )
				{
					_rwl.EnterWriteLock();
					try
					{
						event_instance = _events.SingleOrDefault(evt => evt.GetType() == typeof(TEventType)) as TEventType;

						if( event_instance == null )
						{
							event_instance = Activator.CreateInstance<TEventType>();
							_events.Add(event_instance);
						}
					}
					finally
					{
						_rwl.ExitWriteLock();
					}
				}

				return event_instance;
			}
			finally
			{
				_rwl.ExitUpgradeableReadLock();
			}
		}
	}
}