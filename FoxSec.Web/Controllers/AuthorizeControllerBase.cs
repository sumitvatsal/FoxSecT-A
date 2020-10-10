using System.Web.Mvc;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EntLib.Logging;

namespace FoxSec.Web.Controllers
{
	[Authorize]
	public abstract class AuthorizeControllerBase : ControllerBase
	{
		protected AuthorizeControllerBase(ICurrentUser currentUser, ILogger logger) : base(currentUser, logger)
		{
		}

		[HttpGet]
		public ActionResult AccessDenied()
		{
			return Redirect("~/Content/AccessDenied.htm");
		}
	}
}
