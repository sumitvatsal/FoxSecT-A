using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Audit.Extensions
{
	public static class AuditEventUserExtension
	{
		public static string ToXmlString(this AuditEventUser user)
		{
			var root = new XElement(Literals.USER);

			root.Add(new XElement(Literals.LOGIN_NAME, user.LoginName));
			root.Add(new XElement(Literals.FIRST_NAME, user.FirstName));
			root.Add(new XElement(Literals.LAST_NAME, user.LastName));
			root.Add(new XElement(Literals.PASSWORD, user.Password));
			root.Add(new XElement(Literals.EMAIL, user.Email));

			var roles = new XElement(Literals.ROLES);
			foreach( string role in user.UserRoles )
			{
				roles.Add(new XElement(Literals.ROLE, role));
			}

			root.Add(roles);

			return root.ToString();
		}
	}
}
