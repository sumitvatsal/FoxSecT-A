using System.Diagnostics.Contracts;
using System.Web.Mvc;
using System.Web.Routing;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.Bootstrapper;
using FoxSec.Core.Infrastructure.Configuration;
using FoxSec.Web.Infrastructure.Configuration;

namespace FoxSec.Web.Infrastructure.BootstrapperTasks
{
	class RegisterRoutes : IBootstrapperTask
	{
		private readonly IConfigurationSettings _settings;
		private readonly RouteCollection _routes;

		public RegisterRoutes(IConfigurationSettings settings, RouteCollection routes)
		{
			Contract.Requires(Check.Argument.IsNotNull(settings));
			Contract.Requires(Check.Argument.IsNotNull(routes));

			_settings = settings;
			_routes = routes;
		}

		public RegisterRoutes(IConfigurationSettings settings) : this(settings, RouteTable.Routes)
		{
		}

		public void Execute()
		{
			_routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			_routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			_routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				new { id = @"\d+" }
			);
            
			_routes.MapRoute(
				"",
				"{controller}/{action}",
				new { controller = "Home", action = "Index" }
			);

			_routes.MapRoute(
				"Language",
				"Language/SwitchLanguage"
			);
            
		}
	}
}