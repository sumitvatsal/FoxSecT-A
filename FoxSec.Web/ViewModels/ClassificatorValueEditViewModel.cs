using System.Collections.Generic;

namespace FoxSec.Web.ViewModels
{
	public class ClassificatorValueEditViewModel : ViewModelBase
	{
        public ClassificatorValueEditViewModel()
		{
            ClassificatorValue = new ClassificatorValueItem();
		}

        public ClassificatorValueItem ClassificatorValue { get; set; }
	}
}