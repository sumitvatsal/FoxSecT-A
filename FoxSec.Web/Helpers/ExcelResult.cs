using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Winnovative.WnvHtmlConvert;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.UI;
using System.IO;

namespace System.Web.Mvc
{
	public class ExcelResult : ViewResult
	{
		public string Content { get; set; }
		public string OutputFileName { get; set; }
		public bool ReturnAsAttachment { get; set; }



		public ExcelResult(string html, string outputFileName, bool returnAsAttachment)
		{

			this.Content = html;
			this.OutputFileName = outputFileName;
			this.ReturnAsAttachment = returnAsAttachment;
		}

		public override void ExecuteResult(ControllerContext context)
		{
            context.HttpContext.Response.ClearHeaders();
            string attachment = "attachment; filename=" + OutputFileName + ".xls";
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.AddHeader("content-disposition", attachment);
            //context.HttpContext.Response.ContentType = "application/ms-excel";
            context.HttpContext.Response.ContentType = "application/ms-excel";
            context.HttpContext.Response.ContentEncoding = System.Text.Encoding.Unicode;
            context.HttpContext.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            HtmlString str = new HtmlString(Content);
            context.HttpContext.Response.Write(str);
            context.HttpContext.Response.End();
        }
	}
}