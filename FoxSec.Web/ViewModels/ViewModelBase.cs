using FoxSec.Authentication;

namespace FoxSec.Web.ViewModels
{
	public class ViewModelBase
	{
		public IFoxSecIdentity User { get; set; }
	}
}