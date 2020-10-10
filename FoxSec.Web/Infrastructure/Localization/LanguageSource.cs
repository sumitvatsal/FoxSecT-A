using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.Core.Infrastructure.Configuration;
using FoxSec.Core.Infrastructure.Localization;

namespace FoxSec.Web.Infrastructure.Localization
{
	internal class LanguageSource : ILanguageSource
	{
		private readonly IConfigurationManager _configManager;

		public LanguageSource(IConfigurationManager configManager)
		{
			_configManager = configManager;
		}

		public IEnumerator<ILanguage> GetEnumerator()
		{
			foreach( LanguageConfigSection.LanguageElement language in _configManager.GetSection<LanguageConfigSection>(Literals.CONFIG_SECTION_NAME).Languages )
			{
				yield return new Language(language.Culture, language.DisplayName);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}