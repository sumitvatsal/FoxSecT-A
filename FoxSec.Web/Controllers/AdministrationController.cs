using System.Web.Mvc;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.Web.ViewModels;

namespace FoxSec.Web.Controllers
{
    public class AdministrationController : BusinessCaseController
    {
        public AdministrationController(ILogger logger, ICurrentUser currentUser) : base(currentUser, logger) { }

        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }
    }
}