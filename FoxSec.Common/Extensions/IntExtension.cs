using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Common.Extensions
{
	public static class IntExtension
	{
		public static T AsEnum<T>(this int @int) where T : struct 
		{
			return (T)Enum.ToObject(typeof(T), @int);
		}
	}
}
