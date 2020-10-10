using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using FoxSec.Common.Helpers;

namespace FoxSec.Common.Extensions
{
	public static class StringExtension
	{
		private static readonly Regex WebUrlExpression;
		private static readonly Regex EmailExpression;
		private static readonly Regex StripHTMLExpression;

		static StringExtension()
		{
			WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);
			
			EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
			
			StripHTMLExpression = new Regex("<\\S[^><]*>",
			                                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline |
			                                RegexOptions.CultureInvariant | RegexOptions.Compiled);
		}


       [DebuggerStepThrough]
		public static bool IsWebUrl(this string target)
		{
			return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
		}

		[DebuggerStepThrough]
		public static bool IsEmail(this string target)
		{
			return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
		}

		[DebuggerStepThrough]
		public static string NullSafe(this string target)
		{
			return (target ?? string.Empty).Trim();
		}

		[DebuggerStepThrough]
		public static string Hash(this string target)
		{
			Contract.Requires(Check.Argument.IsNotEmpty(target));

			using( MD5 md5 = MD5.Create() )
			{
				byte[] data = Encoding.Unicode.GetBytes(target);
				byte[] hash = md5.ComputeHash(data);

				return Convert.ToBase64String(hash);
			}
		}

		[DebuggerStepThrough]
		public static string WrapAt(this string target, int index)
		{
			const int DotCount = 3;

			Contract.Requires(Check.Argument.IsNotEmpty(target));
			Contract.Requires(Check.Argument.IsNotNegativeOrZero(index));

			return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - DotCount), new string('.', DotCount));
		}

		[DebuggerStepThrough]
		public static string StripHtml(this string target)
		{
			return StripHTMLExpression.Replace(target, string.Empty);
		}

		[DebuggerStepThrough]
		public static Guid ToGuid(this string target)
		{
			Guid result = Guid.Empty;

			if( (!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22) )
			{
				string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

				try
				{
					byte[] base64 = Convert.FromBase64String(encoded);

					result = new Guid(base64);
				}
				catch( FormatException )
				{
				}
			}

			return result;
		}

		[DebuggerStepThrough]
		public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
		{
			T convertedValue = defaultValue;

			if( !string.IsNullOrEmpty(target) )
			{
				try
				{
					convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
				}
				catch( ArgumentException )
				{
				}
			}

			return convertedValue;
		}

		[DebuggerStepThrough]
		public static string UrlEncode(this string target)
		{
			return HttpUtility.UrlEncode(target);
		}

		[DebuggerStepThrough]
		public static string UrlDecode(this string target)
		{
			return HttpUtility.UrlDecode(target);
		}

		[DebuggerStepThrough]
		public static string AttributeEncode(this string target)
		{
			return HttpUtility.HtmlAttributeEncode(target);
		}

		[DebuggerStepThrough]
		public static string HtmlEncode(this string target)
		{
			return HttpUtility.HtmlEncode(target);
		}

		[DebuggerStepThrough]
		public static string HtmlDecode(this string target)
		{
			return HttpUtility.HtmlDecode(target);
		}

		public static string Replace(this string target, ICollection<string> oldValues, string newValue)
		{
			oldValues.ForEach(oldValue => target = target.Replace(oldValue, newValue));
			return target;
		}
	}
}
