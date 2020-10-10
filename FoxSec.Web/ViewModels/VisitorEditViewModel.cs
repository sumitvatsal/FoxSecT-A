using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class VisitorEditViewModel : ViewModelBase
    {
        public VisitorEditViewModel()
        {
            FoxSecVisitor = new VisitorItem();
        }

        public UserItem FoxSecUser { get; set; }
        public VisitorItem FoxSecVisitor { get; private set; }
        public System.Web.Mvc.SelectList Companies { get; set; }
        public IEnumerable<SelectListItem> LanguageItems { get; set; }
        public enum UserLanguageEnum
        {
            UserLanguageEstonian = 1,

            UserLanguageEnglish = 2,

            UserLanguageLatvian = 3,

            UserLanguageLithuanian = 4,

            UserLanguageRussian = 5,

            UserLanguageSwedish = 6,

            UserLanguageFinnish = 7
        }
    }
}