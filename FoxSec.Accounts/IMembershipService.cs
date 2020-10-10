using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Accounts
{
	public interface IMembershipService
	{
		int MinPasswordLength { get; }
		bool ValidateUser(string loginName, string password, out User user);
		MembershipCreateStatus CreateUser(string userName, string password, string email);
		bool ChangePassword(string userName, string oldPassword, string newPassword);
	}
}
