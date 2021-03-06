using System.Diagnostics.Contracts;
using FoxSec.Common.Helpers;
using System;
using System.Diagnostics;
using System.Reflection;

namespace FoxSec.Common.EventAggregator
{
	public class DelegateReference : IDelegateReference
	{
		private readonly Delegate _delegate;
		private readonly WeakReference _weakReference;
		private readonly MethodInfo _method;
		private readonly Type _delegateType;

		public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
		{
			Contract.Requires(Check.Argument.IsNotNull(@delegate));

			if( keepReferenceAlive )
			{
				_delegate = @delegate;
			}
			else
			{
				_weakReference = new WeakReference(@delegate.Target);
				_method = @delegate.Method;
				_delegateType = @delegate.GetType();
			}
		}

		public Delegate Target
		{
			[DebuggerStepThrough]
			get
			{
				return _delegate ?? TryGetDelegate();
			}
		}

		private Delegate TryGetDelegate()
		{
			if( _method.IsStatic )
			{
				return Delegate.CreateDelegate(_delegateType, null, _method);
			}

			object target = _weakReference.Target;

			return (target != null) ? Delegate.CreateDelegate(_delegateType, target, _method) : null;
		}
	}
}