using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class DeactivateCardViewModel : ViewModelBase
	{
        public SelectList Reasons { get; set; }
        public bool IsDeactivateDialog { get; set; }
        public bool IsMoveToFree { get; set; }
	}
}