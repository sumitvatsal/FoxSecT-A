using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FoxSec.Core.SystemEvents.DTOs;

namespace FoxSec.Audit.Extensions
{
	public static class AuditEventRoleExtension
	{
		public static string ToXmlString(this AuditEventRole role)
		{
			var root = new XElement(Literals.ROLE);

			var permissions = new XElement(Literals.PERMISSIONS);

			foreach( string permission in role.Permissions )
			{
				permissions.Add(new XElement(Literals.PERMISSION, permission));
			}

			root.Add(permissions);

			return root.ToString();
		}
	}
}
