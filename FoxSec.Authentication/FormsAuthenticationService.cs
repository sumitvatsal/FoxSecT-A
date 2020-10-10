using System;
using System.Web.Security;

namespace FoxSec.Authentication
{
	internal class FormsAuthenticationService : IFormsAuthenticationService
	{
		public void SignIn(string userName, bool createPersistentCookie)
		{
			if( string.IsNullOrWhiteSpace(userName) ) throw new ArgumentException("Value cannot be null or empty.", "userName");

			 FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}
}