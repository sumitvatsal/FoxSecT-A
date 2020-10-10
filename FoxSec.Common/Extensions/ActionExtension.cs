using System;

namespace FoxSec.Common.Extensions
{
	public static class ActionExtension
	{
		public static Action<T> ExceptionSafe<T>(this Action<T> action)
		{
			return obj =>
			{
				try
				{
					action(obj);
				}
				catch( Exception ) {}
			};
		}
	}
}
