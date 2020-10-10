namespace FoxSec.Web.ViewModels
{
	public class TimeZoneEditViewModel : ViewModelBase
	{
        public TimeZoneEditViewModel()
		{
            TimeZone = new TimeZoneItem();
		}

        public TimeZoneItem TimeZone { get; set; }
	}
}