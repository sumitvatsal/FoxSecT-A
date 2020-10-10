using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace FoxSec.Core.Infrastructure.Configuration
{
	internal class ConfigurationManagerWrapper : IConfigurationManager
	{
		public NameValueCollection AppSettings
		{
			[DebuggerStepThrough]
			get
			{
				return ConfigurationManager.AppSettings;
			}
		}

		public string DependencyResolverTypeName
		{
			[DebuggerStepThrough]
			get
			{
				return AppSettings["dependencyResolverTypeName"];
			}
		}

		[DebuggerStepThrough]
		public string GetConnectionString(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString;
		}

		[DebuggerStepThrough]
		public string GetProviderName(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ProviderName;
		}

		[DebuggerStepThrough]
		public T GetSection<T>(string sectionName)
		{
			return (T)ConfigurationManager.GetSection(sectionName);
		}
	}
}
