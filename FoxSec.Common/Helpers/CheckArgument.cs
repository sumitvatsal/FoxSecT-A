using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using FoxSec.Common.Extensions;

namespace FoxSec.Common.Helpers
{
	public static class Check
	{
		[Pure]
		public static class Argument
		{
			[DebuggerStepThrough]
			public static bool IsNotEmpty(Guid argument)
			{
				return argument != Guid.Empty;
			}

			[DebuggerStepThrough]
			public static bool IsNotEmpty(string argument)
			{
				return !string.IsNullOrEmpty((argument ?? string.Empty).Trim());
			}

			[DebuggerStepThrough]
			public static bool IsNotOutOfLength(string argument, int length)
			{
				return argument.Trim().Length <= length;
			}

			[DebuggerStepThrough]
			public static bool IsNotNull(object argument)
			{
				return argument != null;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegative(int argument)
			{
				return argument >= 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegativeOrZero(int argument)
			{
				return argument > 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegative(long argument)
			{
				return argument >= 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegativeOrZero(long argument)
			{
				return argument > 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegative(float argument)
			{
				return argument >= 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegativeOrZero(float argument)
			{
				return argument > 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegative(decimal argument)
			{
				return argument >= 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegativeOrZero(decimal argument)
			{
				return argument > 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotInvalidDate(DateTime argument)
			{
				return argument.IsValid();
			}

			[DebuggerStepThrough]
			public static bool IsNotInPast(DateTime argument)
			{
				return argument >= SystemTime.Now();
			}

			[DebuggerStepThrough]
			public static bool IsNotInFuture(DateTime argument)
			{
				return argument <= SystemTime.Now();
			}

			[DebuggerStepThrough]
			public static bool IsNotNegative(TimeSpan argument)
			{
				return argument >= TimeSpan.Zero;
			}

			[DebuggerStepThrough]
			public static bool IsNotNegativeOrZero(TimeSpan argument)
			{
				return argument > TimeSpan.Zero;
			}

			[DebuggerStepThrough]
			public static bool IsNotEmpty<T>(ICollection<T> argument)
			{
				return IsNotNull(argument) && argument.Count != 0;
			}

			[DebuggerStepThrough]
			public static bool IsNotOutOfRange(int argument, int min, int max)
			{
				return (argument >= min) && (argument < max);
			}

			[DebuggerStepThrough]
			public static bool IsNotInvalidEmail(string argument)
			{
				return IsNotEmpty(argument) && argument.IsEmail();
			}

			[DebuggerStepThrough]
			public static bool IsNotInvalidWebUrl(string argument)
			{
				return IsNotEmpty(argument) && argument.IsWebUrl();
			}
		}
	}
}
