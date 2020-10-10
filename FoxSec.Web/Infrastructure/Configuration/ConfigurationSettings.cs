using FoxSec.Core.Infrastructure.Configuration;

namespace FoxSec.Web.Infrastructure.Configuration
{
	class ConfigurationSettings : IConfigurationSettings
	{
		public bool AuditUsers { get; set; }

		public bool AuditRoles { get; set; }

		public string DateTimeFormatString { get; set; }

		public int ControllerUpdateRecordLife { get; set; }
	}
}