using System.Collections.Specialized;

namespace FoxSec.Core.Infrastructure.Configuration
{
	public interface IConfigurationManager
	{
		NameValueCollection AppSettings
		{
			get;
		}

		string GetConnectionString(string name);
		string GetProviderName(string name);
		T GetSection<T>(string sectionName);
	}
}
