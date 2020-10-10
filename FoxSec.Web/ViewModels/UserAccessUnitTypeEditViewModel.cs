using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class UserAccessUnitTypeEditViewModel : ViewModelBase
    {
        public UserAccessUnitTypeEditViewModel()
        {
            CardType = new CardTypeItem();
        }

        public CardTypeItem CardType { get; set; }
    }
}