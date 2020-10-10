using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class TerminallViewModel: PaginatorViewModelBase
    {
        public List<TerminalModel> terminals { get; set; }
        public int FilterCriteria { get; set; }
    }

    public class TerminalModel
    {
        public Terminal term { get; set; }
        public string MaxUserFName { get; set; }
        public string MaxUserLName { get; set; }
        public string CompanyName { get; set; }
        public string lastLoginDt { get; set; }
        public string status { get; set; }
        public SelectList companies { get; set; }

        public SelectList TABuildingList { get; set; }

    }
}