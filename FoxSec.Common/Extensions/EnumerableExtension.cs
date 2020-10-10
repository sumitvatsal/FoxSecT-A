using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FoxSec.Common.Extensions
{
	public static class EnumerableExtension
	{
		[DebuggerStepThrough]
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach( T item in enumerable )
			{
				action(item);
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<T> NullSafe<T>(this IEnumerable<T> enumerable)
		{
			return enumerable ?? Enumerable.Empty<T>();
		}
	}
}
