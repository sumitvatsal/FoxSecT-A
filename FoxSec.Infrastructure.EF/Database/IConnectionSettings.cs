namespace FoxSec.Infrastructure.EF.Database
{
	public interface IConnectionSettings
	{
		string ConnectionString { get; }

		string DefaultContainerName { get; }
	}
}
