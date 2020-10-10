using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FoxSec.Web.ViewModels
{
    public class TitleEditViewModel : ViewModelBase
	{
        public TitleEditViewModel()
		{
            Title = new TitleItem();
		}
        public TitleItem Title { get; set; }
        public SelectList Companies { get; set; }
	}
}