using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FoxSec.Common.Extensions
{
	public static class EnumExtension
	{
		public static string GetDisplayName(this Enum @enum)
		{
			string default_name = @enum.ToString();

			var attr = Attribute.GetCustomAttribute(@enum.GetType().GetField(default_name), typeof(DisplayAttribute)) as DisplayAttribute;

			return attr == null ? default_name : attr.Name;
		}

		public static IEnumerable<T> GetAllValues<T>() where T : struct
		{
			Type type = typeof(T);

			return type.IsEnum ? type.GetEnumValues().Cast<T>() : Enumerable.Empty<T>();
		}
	}
}
