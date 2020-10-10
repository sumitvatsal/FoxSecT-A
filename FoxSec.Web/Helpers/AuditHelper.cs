using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FoxSec.Common.Extensions;

namespace FoxSec.Web.Helpers
{
	public static class AuditHelper
	{
		public static string GetLoginName(XElement x)
		{
			return x.Elements(Audit.Extensions.Literals.LOGIN_NAME).Single().Value;
		}

		public static string GetFirstName(XElement x)
		{
			return x.Elements(Audit.Extensions.Literals.FIRST_NAME).Single().Value;
		}

		public static string GetLastName(XElement x)
		{
			return x.Elements(Audit.Extensions.Literals.LAST_NAME).Single().Value;
		}

		public static string GetEmail(XElement x)
		{
			return x.Elements(Audit.Extensions.Literals.EMAIL).Single().Value;
		}

		public static string GetPassword(XElement x)
		{
			return x.Elements(Audit.Extensions.Literals.PASSWORD).Single().Value;
		}

		public static string GetRoles(XElement x)
		{
			return (from XElement role in x.Elements(Audit.Extensions.Literals.ROLES).Nodes() select role.Value).AsString();
		}

		public static string GetPermissions(XElement x)
		{
			return (from XElement permission in x.Elements(Audit.Extensions.Literals.PERMISSIONS).Nodes() select permission.Value).AsString();
		}

		private static string AsString(this IEnumerable<string> list)
		{
			var sb = new StringBuilder();

			list.ForEach(item => sb.AppendFormat("<{0}>, ", item));

			return sb.Length != 0 ? sb.ToString(0, sb.Length - 2) : string.Empty;
		}
	}
}