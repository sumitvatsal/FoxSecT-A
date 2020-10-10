using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
	public class CardsTabContentViewModel : ViewModelBase
	{
		public bool CanAddCard { get; set; }
        public bool? CannotAddUsersAndCard { get; set; }

    }
}