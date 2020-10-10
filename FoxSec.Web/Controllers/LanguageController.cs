using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using FoxSec.Core.Infrastructure.Localization;
using FoxSec.Web.ViewModels;

namespace FoxSec.Web.Controllers
{
	public class LanguageController : Controller
	{
		private readonly ICurrentLanguage _currentLanguage;
		private readonly ILanguageSource _languageSource;

        public LanguageController(  ICurrentLanguage currentLanguage, ILanguageSource languageSource )
		{
			_currentLanguage = currentLanguage;
			_languageSource = languageSource;

			Contract.Ensures(_currentLanguage != null);
			Contract.Ensures(_languageSource != null);
		}
        private void SetLanguageValue(string language)
        {
           // Session["Language"] = language.ToString();
        }
        [HttpPost]
		public ActionResult SwitchLanguage(string language, string redirectUrl)
		{
            //SetLanguageValue( language);
            Contract.Requires(!string.IsNullOrWhiteSpace(language));
			Contract.Requires(!string.IsNullOrWhiteSpace(redirectUrl));
            Session["Language"] = language;

            _currentLanguage.Set(language);
			return Redirect(redirectUrl);
		}

		[ValidateInput(false)]
		[ChildActionOnly]
		public ActionResult LanguageBar()
		{
			var lvm = new List<LanguageBarViewModel>();
            var currentLanguage = Core.Infrastructure.IoC.IoC.Resolve<ICurrentLanguage>().Get();
			foreach( var language in _languageSource )
			{
				lvm.Add(new LanguageBarViewModel
                { 
                    Culture = language.Culture,
                    DisplayName = language.DisplayName,
                    IsCurrentLanguage = language.DisplayName.Trim() == currentLanguage.Trim()
                });
			}
			return View(lvm);
		}
	}
}