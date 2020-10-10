using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class MoveToDepartmentViewModel : ViewModelBase
	{
        public SelectList Departments { get; set; }
	}
}