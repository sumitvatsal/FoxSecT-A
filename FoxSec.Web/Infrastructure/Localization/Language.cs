using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.Core.Infrastructure.Localization;

namespace FoxSec.Web.Infrastructure.Localization
{
	internal class Language : ILanguage
	{
		public Language(string culture, string displayName)
		{
			Culture = culture;
			DisplayName = displayName;
		}

		public string Culture { get; private set; }

		public string DisplayName { get; private set; }
	}
}