using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.Helpers
{
	public static class ViewHelper
	{
		public static string RenderPartialView(this Controller controller, string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

			controller.ViewData.Model = model;
			using (var sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
				var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}

		public static string GetPrefixedName( string collectionName, string name, int index)
		{
			return string.Format("{0}[{1}].{2}", collectionName, index, name);
		}

	}

}