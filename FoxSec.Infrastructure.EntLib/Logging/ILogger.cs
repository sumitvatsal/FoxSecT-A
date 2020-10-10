using System.Diagnostics;

namespace FoxSec.Infrastructure.EntLib.Logging
{
	public interface ILogger
	{
		void Write(string message, string category, TraceEventType severity);
	}
}
