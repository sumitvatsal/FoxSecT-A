using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FoxSec.Core.Infrastructure.Configuration;

namespace FoxSec.Infrastructure.EntLib.Logging
{
	public static class ILoggerExtension
	{
		public static void Write(this ILogger logger, string message)
		{
			logger.Write(message, Literals.UNHANDLED_EXCEPTION_CATEGORY, TraceEventType.Critical);
		}

		public static void Write(this ILogger logger, string message, Exception e)
		{
			Write(logger, string.Format("{0}\n{1}\nStack Trace:\n{2}", message, e.Message, e.StackTrace));
		}
	}
}
