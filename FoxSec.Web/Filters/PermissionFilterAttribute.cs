using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using FoxSec.Authentication;
using FoxSec.Core.Infrastructure.IoC;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.Filters
{
	internal class PermissionFilterAttribute : FilterAttribute, IActionFilter
	{
		private readonly ICurrentUser _currentUser;

		private readonly Permission[] _permissions;

		public PermissionFilterAttribute(Permission permission) : this(new [] { permission })
		{
		}

		public PermissionFilterAttribute(Permission[] permissions) : this()
		{
			_permissions = permissions;
		}

		private PermissionFilterAttribute()
		{
			_currentUser = IoC.Resolve<ICurrentUser>();
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			IFoxSecIdentity identity = _currentUser.Get();

			if( !_permissions.All(p => identity.Permissions[p]) )
			{
				var rvd =
					new RouteValueDictionary(
						new { controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, action = "AccessDenied" });

				filterContext.Result = new RedirectToRouteResult(rvd);
			}
		}
	}
}