using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class TAReportEditViewModel : ViewModelBase
    {
        public TAReportEditViewModel()
        {
            TAReport = new TAReportItem();    //  TAPivotRowItem
//          TAReportItems = new List<TAReport>();
        }
        public TAReportItem TAReport { get; set; }    //  TAPivotRowItem
        public SelectList Users { get; set; }
        public SelectList Departments { get; set; }
    }
}