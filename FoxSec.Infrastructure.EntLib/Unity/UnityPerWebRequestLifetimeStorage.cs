using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web;
using FoxSec.Common.Helpers;

namespace FoxSec.Infrastructure.EntLib.Unity
{
	static class UnityPerWebRequestLifetimeStorage
	{
		private static readonly object _key;

		static UnityPerWebRequestLifetimeStorage()
		{
			_key = new object();
		}
		
		public static IDictionary<UnityPerWebRequestLifetimeManager, object> GetInstances(HttpContextBase httpContext)
		{
			Contract.Requires(Check.Argument.IsNotNull(httpContext));

			IDictionary<UnityPerWebRequestLifetimeManager, object> instances;

			if( httpContext.Items.Contains(_key) )
			{
				instances = (IDictionary<UnityPerWebRequestLifetimeManager, object>)httpContext.Items[_key];
			}
			else
			{
				lock( httpContext.Items )
				{
					if( httpContext.Items.Contains(_key) )
					{
						instances = (IDictionary<UnityPerWebRequestLifetimeManager, object>)httpContext.Items[_key];
					}
					else
					{
						instances = new Dictionary<UnityPerWebRequestLifetimeManager, object>();
						httpContext.Items.Add(_key, instances);
					}
				}
			}

			return instances;
		}
	}
}
