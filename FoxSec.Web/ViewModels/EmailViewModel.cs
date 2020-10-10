using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class EmailViewModel
    {
        public string ThisServerMailSubject { get; set; }
        public string ThisServerMailFrom { get; set; }
        public string ThisServerSmtpServer { get; set; }
        public int ThisServerSmtpPort { get; set; }
        public string ThisServerSmtpUser { get; set; }
        public string ThisServerSmtpPsw { get; set; }
        public bool ThisserveremailsparamOK { get; set; }
        public string VisitorMailText { get; set; }
    }
}