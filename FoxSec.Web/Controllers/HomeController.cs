using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.Web.ViewModels;
using ViewResources;
using System.Data;
using System;


namespace FoxSec.Web.Controllers
{
    public class HomeController : AuthorizeControllerBase
    {
        public HomeController(ICurrentUser currentUser, ILogger logger) : base(currentUser, logger) { }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        public ActionResult Index()
        {
            ViewData["Message"] = SharedStrings.WelcomeScreenMessage;

            var hmv = CreateViewModel<HomeViewModel>();
            //    if (Session["Visibletab"] == null)
            //    {
            //        return RedirectToAction("LogOn", "Account");
            //    }
            //else  if (Session["Visibletab"].ToString()!="True")
            //    {
            //         return RedirectToAction("LogOn", "Account");
            //    }
            //    //return RedirectToAction("UserLogOn", "Account");
            //    //return RedirectToAction("TabContent","User");
            hmv.TALicenseCount = 0;
            con.Open();
            SqlCommand cmd = new SqlCommand("select Remaining from ClassificatorValues where value='Time&attendense'", con);
            string tc = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            if(!string.IsNullOrEmpty(tc))
            {
                hmv.TALicenseCount = Convert.ToInt32(tc);
            }
            if (Session["Role_ID"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            return View(hmv);
        }

        public ActionResult About()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return View(hmv);
        }

        [HttpPost]
        public ActionResult SetOpenedTab(string tabIndex)
        {
            HttpContext.Session["tabIndex"] = tabIndex;
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SetOpenedSubTab(string tabIndex)
        {
            HttpContext.Session["subTabIndex"] = tabIndex;
            return new EmptyResult();
        }
    }
}