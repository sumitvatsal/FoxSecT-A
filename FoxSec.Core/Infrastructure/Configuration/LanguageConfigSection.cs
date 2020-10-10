using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FoxSec.Core.Infrastructure.Configuration
{
	public class LanguageConfigSection : ConfigurationSection
	{
		public class LanguageElement : ConfigurationElement
		{
			[ConfigurationProperty(Literals.CONFIG_SECTION_CULTURE, IsRequired = true)]
			public string Culture
			{
				get { return (string)this[Literals.CONFIG_SECTION_CULTURE]; }
				set { this[Literals.CONFIG_SECTION_CULTURE] = value; }
			}

			[ConfigurationProperty(Literals.CONFIG_SECTION_DISPLAYNAME, IsRequired = true)]
			public string DisplayName
			{
				get { return (string)this[Literals.CONFIG_SECTION_DISPLAYNAME]; }
				set { this[Literals.CONFIG_SECTION_DISPLAYNAME] = value; }
			}
		}

		public class LanguagesCollection : ConfigurationElementCollection
		{
			protected override ConfigurationElement CreateNewElement()
			{
				return new LanguageElement();
			}
			protected override object GetElementKey(ConfigurationElement element)
			{
				return ((LanguageElement)element).Culture;
			}
			public LanguageElement GetElement(string key)
			{
				return (LanguageElement)BaseGet(key);
			}
			public void Add(LanguageElement element)
			{
				BaseAdd(element);
			}
		}

		[ConfigurationProperty(Literals.CONFIG_SECTION_LANGUAGES)]
		public LanguagesCollection Languages
		{
			get
			{
				var collection = (LanguagesCollection)base[Literals.CONFIG_SECTION_LANGUAGES];
				return collection;
			}
		}
	}
}
