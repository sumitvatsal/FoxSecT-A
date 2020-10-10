using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.Web.ViewModels;


namespace FoxSec.Web.Controllers
{
    public class csvController : Controller
    {
        // GET: csv
        public ActionResult Index()
        {
            Session["DataTableModel"] = csv.OpenExcelFile();
            return View(Session["DataTableModel"]);
        }

        public ActionResult GridViewPartialView()
        {
            return PartialView(Session["DataTableModel"]);
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            //return PartialView("GridViewPartialView", NorthwindDataProvider.GetCustomers());
        }
    }
}