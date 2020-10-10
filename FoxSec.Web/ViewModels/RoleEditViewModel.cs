namespace FoxSec.Web.ViewModels
{
	public class RoleEditViewModel : ViewModelBase
	{
		public RoleEditViewModel()
		{
			Role = new RoleItem();
		}

		public RoleItem Role { get; set; }

		public bool IsViewOnlyMode { get; set; }
	}
}