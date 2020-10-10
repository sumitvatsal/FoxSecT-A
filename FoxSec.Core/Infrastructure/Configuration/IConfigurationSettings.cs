namespace FoxSec.Core.Infrastructure.Configuration
{
	public interface IConfigurationSettings
	{
		bool AuditUsers { get; }
		bool AuditRoles { get; }
		string DateTimeFormatString { get; }
		int ControllerUpdateRecordLife { get; }
	}
}