using System.Web.Mvc;
using System.Web.SessionState;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.Web.ViewModels;

namespace FoxSec.Web.Controllers
{
    
    [SessionState(SessionStateBehavior.ReadOnly)]
	public abstract class ControllerBase : Controller
	{
		private const string MODEL_STATE_KEY = "__##MODEL_STATE##__";

		protected ICurrentUser CurrentUser { get; private set; }
		protected ILogger Logger { get; private set; }

		protected ControllerBase(ICurrentUser currentUser, ILogger logger)
		{
			CurrentUser = currentUser;
			Logger = logger;
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if( filterContext.Result is RedirectToRouteResult && !ModelState.IsValid )
			{
				TempData.Add(MODEL_STATE_KEY, ModelState);
			}
			else
			{
				if( filterContext.Result is ViewResult )
				{
					RestoreModelState();
				}
			}
			base.OnActionExecuted(filterContext);
		}

		private void RestoreModelState()
		{
			object model_state;
			TempData.TryGetValue(MODEL_STATE_KEY, out model_state);
			var msd = model_state as ModelStateDictionary;
			if( msd != null )
			{
				foreach( var item in msd )
				{
					if( !ModelState.ContainsKey(item.Key) )
					{
						ModelState.Add(item);
					}
				}
			}
		}

		protected virtual T CreateViewModel<T>() where T : ViewModelBase, new()
		{
			return new T { User = CurrentUser.Get() };
		}
	}
}