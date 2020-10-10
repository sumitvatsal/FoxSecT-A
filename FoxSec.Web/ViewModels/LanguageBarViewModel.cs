using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
	public class LanguageBarViewModel
	{
		public string Culture { get; internal set; }
		public string DisplayName { get; internal set; }
        public bool IsCurrentLanguage { get;  set; }
	}
}