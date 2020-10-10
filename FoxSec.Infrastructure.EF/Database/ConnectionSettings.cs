using System.Diagnostics;
using System.Diagnostics.Contracts;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.Configuration;

namespace FoxSec.Infrastructure.EF.Database
{
	internal class ConnectionSettings : IConnectionSettings
	{
		private readonly string _connectionString;
		private readonly string _defaultContainerName;

		public ConnectionSettings(IConfigurationManager configuration, string name)
		{
			Contract.Requires(Check.Argument.IsNotNull(configuration));
			Contract.Requires(Check.Argument.IsNotEmpty(name));

			_connectionString = configuration.AppSettings["connectionString"];
			_defaultContainerName = configuration.AppSettings["defaultContainerName"];
		}

		public string ConnectionString
		{
			[DebuggerStepThrough]
			get { return _connectionString; }
		}

		public string DefaultContainerName
		{
			[DebuggerStepThrough]
			get { return _defaultContainerName; }
		}
	}
}
