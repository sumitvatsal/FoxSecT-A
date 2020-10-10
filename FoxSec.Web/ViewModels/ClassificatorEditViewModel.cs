using System.Collections.Generic;

namespace FoxSec.Web.ViewModels
{
	public class ClassificatorEditViewModel : ViewModelBase
	{
        public ClassificatorEditViewModel()
		{
            Classificator = new ClassificatorItem();
            ClassificatorValues = new List<ClassificatorValueItem>();
		}

        public ClassificatorItem Classificator { get; set; }
        public IEnumerable<ClassificatorValueItem> ClassificatorValues { get; set; }
	}
}