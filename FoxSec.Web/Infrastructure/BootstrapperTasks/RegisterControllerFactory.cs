using System.Diagnostics.Contracts;
using System.Web.Mvc;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.Bootstrapper;

namespace FoxSec.Web.Infrastructure.BootstrapperTasks
{
	public class RegisterControllerFactory : IBootstrapperTask
	{
		private readonly IControllerFactory _controllerFactory;

		public RegisterControllerFactory(IControllerFactory controllerFactory)
		{
			Contract.Requires(Check.Argument.IsNotNull(controllerFactory));

			_controllerFactory = controllerFactory;
		}

		public void Execute()
		{
			ControllerBuilder.Current.SetControllerFactory(_controllerFactory);
		}
	}
}