using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Microsoft.Practices.Unity;

namespace FoxSec.Infrastructure.EntLib.Unity
{
	class UnityPerWebRequestLifetimeManager : LifetimeManager
	{
		private HttpContextBase _httpContext;

		public UnityPerWebRequestLifetimeManager() : this(new HttpContextWrapper(HttpContext.Current))
		{
		}

		public UnityPerWebRequestLifetimeManager(HttpContextBase httpContext)
		{
			_httpContext = httpContext;
		}

		private IDictionary<UnityPerWebRequestLifetimeManager, object> BackingStore
		{
			get
			{
				if( HttpContext.Current != null )
				{
					_httpContext = new HttpContextWrapper(HttpContext.Current);
				}

				return UnityPerWebRequestLifetimeStorage.GetInstances(_httpContext);
			}
		}

		private object Value
		{
			[DebuggerStepThrough]
			get
			{
				IDictionary<UnityPerWebRequestLifetimeManager, object> backing_store = BackingStore;

				return backing_store.ContainsKey(this) ? backing_store[this] : null;
			}

			[DebuggerStepThrough]
			set
			{
				IDictionary<UnityPerWebRequestLifetimeManager, object> backing_store = BackingStore;

				object old_value;

				backing_store.TryGetValue(this, out old_value);

				if( old_value != null )
				{
					if( !ReferenceEquals(value, old_value) )
					{
						var disposable = old_value as IDisposable;

						if( disposable != null )
						{
							disposable.Dispose();
						}

						if( value == null )
						{
							backing_store.Remove(this);
						}
						else
						{
							backing_store[this] = value;
						}
					}
				}
				else
				{
					if( value != null )
					{
						backing_store.Add(this, value);
					}
				}
			}
		}

		[DebuggerStepThrough]
		public override object GetValue()
		{
			return Value;
		}

		[DebuggerStepThrough]
		public override void SetValue(object newValue)
		{
			Value = newValue;
		}

		[DebuggerStepThrough]
		public override void RemoveValue()
		{
			Value = null;
		}
	}
}
