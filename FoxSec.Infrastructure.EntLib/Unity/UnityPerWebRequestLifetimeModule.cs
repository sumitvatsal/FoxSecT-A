using System;
using System.Collections.Generic;
using System.Web;
using FoxSec.Common.Extensions;
using FoxSec.Common.Helpers;


namespace FoxSec.Infrastructure.EntLib.Unity
{
	class UnityPerWebRequestLifetimeModule : DisposableResource, IHttpModule
	{
		private HttpContextBase _httpContext;

		public UnityPerWebRequestLifetimeModule(HttpContextBase httpContext)
		{
			_httpContext = httpContext;
		}

		public UnityPerWebRequestLifetimeModule()
		{
		}

		internal IDictionary<UnityPerWebRequestLifetimeManager, object> Instances
		{
			get
			{
				if( HttpContext.Current != null )
				{
					_httpContext = new HttpContextWrapper(HttpContext.Current);
				}

				return (_httpContext == null) ? null : UnityPerWebRequestLifetimeStorage.GetInstances(_httpContext);
			}
		}

		public void Init(HttpApplication context)
		{
			context.EndRequest += (sender, e) => RemoveAllInstances();
		}
		
		internal void RemoveAllInstances()
		{
			IDictionary<UnityPerWebRequestLifetimeManager, object> instances = Instances;

			if( !instances.IsNullOrEmpty() )
			{
				foreach( KeyValuePair<UnityPerWebRequestLifetimeManager, object> entry in instances )
				{
					var disposable = entry.Value as IDisposable;

					if( disposable != null )
					{
						disposable.Dispose();
					}
				}

				instances.Clear();
			}
		}
	}
}
