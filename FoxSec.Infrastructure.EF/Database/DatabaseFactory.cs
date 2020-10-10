using System.Diagnostics.Contracts;
using FoxSec.Common.Helpers;

namespace FoxSec.Infrastructure.EF.Database
{
	internal class DatabaseFactory : DisposableResource, IDatabaseFactory
	{
		private readonly string _connectionString;
		private readonly string _defaultContainerName;

		private IDatabase _database;

		public DatabaseFactory(IConnectionSettings connectionSettings)
		{
			Contract.Requires(Check.Argument.IsNotNull(connectionSettings));

			_connectionString = connectionSettings.ConnectionString;
			_defaultContainerName = connectionSettings.DefaultContainerName;
		}

		public virtual IDatabase Get()
		{
			if( _database == null )
			{
				_database = new Database(_connectionString, _defaultContainerName);
			}

			return _database;
		}

		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if( _database != null )
				{
					_database.Dispose();
				}
			}

			base.Dispose(disposing);
		}
	}
}
