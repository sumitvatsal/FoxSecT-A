using System;
using FoxSec.Web.ViewModels;
using System.Data;
using System.Web;
using System.Web.Security;
using System.IO;

namespace FoxSec.Web.Views.Shared
{
	public partial class Site : System.Web.Mvc.ViewMasterPage<ViewModelBase>
	{
		protected void Page_Load(object sender, EventArgs e) {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //string url = "https://localhost:64594/Home/Index";
            if (url.ToLower().Contains("https"))
            {
                string projectpath = Path.Combine(Server.MapPath("~/"));
                string[] certfiles = Directory.GetFiles(projectpath, "*.pfx");
                if (certfiles.Length == 0)
                {
                    downloadfile.Visible = false;
                }
                else
                {
                    string certpath = certfiles[0];
                    txtlink.Text = certpath;
                    downloadfile.Visible = true;
                }               
            }
            else
            {
                downloadfile.Visible = false;
            }           
        }

    }
}