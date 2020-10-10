using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Winnovative.WnvHtmlConvert;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace System.Web.Mvc
{
	public class PdfResult : ViewResult
	{
        private MemoryStream output;
        private string v1;
        private bool v2;
        private PDFPageOrientation portrait;

        public string ContentType { get; set; }
		public string Content { get; set; }
		public string OutputFileName { get; set; }
		public PDFPageOrientation Orientation { get; set; }
		public bool ReturnAsAttachment { get; set; }
		public bool ShowHeader { get; set; }
		public string HeaderHtml { get; set; }


		public PdfResult(string html, string outputFileName, bool returnAsAttachment)
		{
			// ViewResult vr = ar as ViewResult;
			this.ContentType = "application/pdf";
			this.Content = html;
			this.OutputFileName = outputFileName;
			this.ReturnAsAttachment = returnAsAttachment;
			this.Orientation = PDFPageOrientation.Portrait;
		}

        public PdfResult(string html, bool returnAsAttachment)
        {
            // ViewResult vr = ar as ViewResult;
            this.ContentType = "application/pdf";
            this.Content = html;
            this.OutputFileName = "VisitorCard";
            this.ReturnAsAttachment = returnAsAttachment;  
        }

        public PdfResult(string html, string outputFileName, bool returnAsAttachment, PDFPageOrientation orientation)
		{
			// ViewResult vr = ar as ViewResult;
			this.ContentType = "application/pdf";
			this.Content = html;
			this.OutputFileName = outputFileName;
			this.ReturnAsAttachment = returnAsAttachment;
			this.Orientation = orientation;
		}

		public PdfResult(string html, string outputFileName, bool returnAsAttachment, PDFPageOrientation orientation, string headerHtml)
		{
			// ViewResult vr = ar as ViewResult;
			this.ContentType = "application/pdf";
			this.Content = html;
			this.OutputFileName = outputFileName;
			this.ReturnAsAttachment = returnAsAttachment;
			this.Orientation = orientation;
			this.ShowHeader = true;
			this.HeaderHtml = headerHtml;
		}

        public PdfResult(MemoryStream output, string v1, bool v2, PDFPageOrientation portrait)
        {
            this.output = output;
            this.v1 = v1;
            this.v2 = v2;
            this.portrait = portrait;
        }

        private void AddHeader(PdfConverter pdfConverter)
		{
			pdfConverter.PdfHeaderOptions.HeaderHeight = 50;

			//write the page number
			pdfConverter.PdfHeaderOptions.TextArea = new TextArea(0, 30, "Page &p; / &P; ", new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));
			pdfConverter.PdfHeaderOptions.TextArea.EmbedTextFont = true;
			pdfConverter.PdfHeaderOptions.TextArea.TextAlign = HorizontalTextAlign.Right;

			// set the footer HTML area
			pdfConverter.PdfHeaderOptions.HtmlToPdfArea = new HtmlToPdfArea(HeaderHtml, null);
			pdfConverter.PdfHeaderOptions.HtmlToPdfArea.EmbedFonts = true;
			pdfConverter.PdfHeaderOptions.HtmlToPdfArea.FitWidth = true;

			pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;
			pdfConverter.PdfHeaderOptions.ShowOnFirstPage = true;

		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = ContentType;

			string baseURL = "";
			string htmlString = this.Content;

			bool selectablePDF = true;

			// Create the PDF converter. Optionally you can specify the virtual browser width as parameter.
			//1024 pixels is default, 0 means autodetect
			PdfConverter pdfConverter = new PdfConverter();

			// set the license key
			pdfConverter.LicenseKey = "IgkQAhMCExcREwIXDBICERMMExAMGxsbGw==";

			// set the converter options
			pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
			pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
			pdfConverter.PdfDocumentOptions.PdfPageOrientation = this.Orientation;
			pdfConverter.PdfDocumentOptions.ShowHeader = ShowHeader;
			pdfConverter.PdfDocumentOptions.ShowFooter = false;

			pdfConverter.PdfDocumentOptions.FitWidth = true;
			
			if(ShowHeader)
				AddHeader(pdfConverter);

			pdfConverter.PdfDocumentOptions.TopMargin = 10;
			pdfConverter.PdfDocumentOptions.LeftMargin = 10;
            pdfConverter.PdfDocumentOptions.RightMargin = 5;
            pdfConverter.PdfDocumentOptions.BottomMargin = 5;

			// set to generate selectable pdf or a pdf with embedded image
			pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = selectablePDF;

			// Performs the conversion and get the pdf document bytes that you can further
			// save to a file or send as a browser response
			// The baseURL parameterhelps the converter to get the CSS files and images
			// referenced by a relative URL in the HTML string. This option has efect only
			// if the HTML string contains a valid HEAD tag.
			// The converter will automatically inserts a <BASE HREF="baseURL"> tag.
			byte[] pdfBytes = null;

			if( baseURL.Length > 0 )
			{
				pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(htmlString, baseURL);
			}
			else
			{
				pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(htmlString);
			}

			// send the PDF document as a response to the browser for download
			// System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
			context.HttpContext.Response.Clear();

			// this.OutputFileName = Regex.Replace(this.OutputFileName, " ", "_", RegexOptions.IgnoreCase);
			// this.OutputFileName = StringHelper.StripNonAlphaNumeric(this.OutputFileName);

			context.HttpContext.Response.AddHeader("Content-Type", "application/pdf");

			if( this.ReturnAsAttachment )
			{
				context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + this.OutputFileName + ".pdf; size=" + pdfBytes.Length.ToString());
			}

			context.HttpContext.Response.Flush();
			context.HttpContext.Response.BinaryWrite(pdfBytes);
			context.HttpContext.Response.Flush();
			context.HttpContext.Response.End();

		}
	}

}