namespace FoxSec.Infrastructure.EntLib.Logging
{
	internal class Logger : ILogger
	{
		public void Write(string message, string category, System.Diagnostics.TraceEventType severity)
		{
			Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, 0, 0, severity);
		}
	}
}
