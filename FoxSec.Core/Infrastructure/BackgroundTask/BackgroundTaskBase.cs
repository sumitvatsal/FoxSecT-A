using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using FoxSec.Common.EventAggregator;
using FoxSec.Common.Helpers;

namespace FoxSec.Core.Infrastructure.BackgroundTask
{
	public abstract class BackgroundTaskBase : IBackgroundTask
	{
		private readonly IEventAggregator _eventAggregator;

		protected BackgroundTaskBase(IEventAggregator eventAggregator)
		{
			Contract.Requires(Check.Argument.IsNotNull(eventAggregator));

			_eventAggregator = eventAggregator;
		}

		public bool IsRunning { get; private set; }

		protected internal IEventAggregator EventAggregator
		{
			[DebuggerStepThrough]
			get{ return _eventAggregator; }
		}

		public void Start()
		{
			OnStart();
			
			IsRunning = true;
		}

		public void Stop()
		{
			OnStop();			
			IsRunning = false;
		}

		protected abstract void OnStart();

		protected abstract void OnStop();

		protected internal SubscriptionToken Subscribe<TEvent, TEventArgs>(Action<TEventArgs> action)
			where TEvent : BaseEvent<TEventArgs>
			where TEventArgs : class
		{
			return _eventAggregator.GetEvent<TEvent>().Subscribe(action, true);
		}

		protected internal void Unsubscribe<TEvent>(SubscriptionToken token) where TEvent : BaseEvent
		{
			_eventAggregator.GetEvent<TEvent>().Unsubscribe(token);
		}
	}
}
