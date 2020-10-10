using System;

namespace FoxSec.Common.Helpers
{
	public static class SystemTime
	{
		public static Func<DateTime> Now = () => DateTime.UtcNow;
	}
}
