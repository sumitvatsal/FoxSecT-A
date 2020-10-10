using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.Core.Infrastructure.Localization;
using FoxSec.Web.Helpers;

namespace FoxSec.Web.Infrastructure.Localization
{
	internal class CurrentLanguage : ICurrentLanguage
	{
		public string Get()
		{
			HttpCookie lang_cookie = HttpContext.Current.Request.Cookies[Literals.CURRENT_LANGUAGE_COOKIE_NAME];

			if( lang_cookie == null ||
				string.IsNullOrWhiteSpace(lang_cookie.Value) ) //or someone tampered with the cookie's content on the client
			{
				lang_cookie = HttpContext.Current.Response.Cookies[Literals.CURRENT_LANGUAGE_COOKIE_NAME];
			}

			return lang_cookie != null ? lang_cookie.Value : null;
		}

		public void Set(string language)
		{
			HttpContext.Current.Response.Cookies.Set(new HttpCookie(Literals.CURRENT_LANGUAGE_COOKIE_NAME, language){ Expires = DateTime.Now.AddMonths(1)});
		}

		public void Reset()
		{
			HttpCookie lang_cookie = HttpContext.Current.Request.Cookies[Literals.CURRENT_LANGUAGE_COOKIE_NAME];

			if( lang_cookie != null )
			{
				lang_cookie.Value = null;

				HttpContext.Current.Request.Cookies.Remove(Literals.CURRENT_LANGUAGE_COOKIE_NAME);
			}
		}
	}
}