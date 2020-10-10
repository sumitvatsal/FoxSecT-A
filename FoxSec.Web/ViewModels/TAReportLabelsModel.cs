using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class TAReportLabelsModel
    {
        public List<TAReportLabels> TAReportLabels { get; set; }
        public IEnumerable<Companies> CompaniesList { get; set; }
      
    }
}