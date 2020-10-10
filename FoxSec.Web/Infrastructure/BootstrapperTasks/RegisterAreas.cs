using System.Web.Mvc;
using FoxSec.Core.Infrastructure.Bootstrapper;

namespace FoxSec.Web.Infrastructure.BootstrapperTasks
{
	class RegisterAreas : IBootstrapperTask
	{
		public void Execute()
		{
			AreaRegistration.RegisterAllAreas();
		}
	}
}