using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
	public class UserEditViewModel : ViewModelBase
	{
		public UserEditViewModel()
		{
			FoxSecUser = new UserItem();
		}

		public UserItem FoxSecUser { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Titles { get; set; }

		public IEnumerable<SelectListItem> LanguageItems { get; set; }

        
    }

	public enum UserLanguageEnum
	{
		UserLanguageEstonian = 1,

		UserLanguageEnglish = 2,

		UserLanguageLatvian = 3,

		UserLanguageLithuanian = 4,

        UserLanguageRussian = 5,

        UserLanguageSwedish  = 6,

        UserLanguageFinnish = 7
    }
}