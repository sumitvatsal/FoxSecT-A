using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class TAReportListViewModel : ViewModelBase  // PaginatorViewModelBase
    {
        public TAReportListViewModel()
        {
            TAReportItems = new List<TAReportItem>();
        }

        public IEnumerable<TAReportItem> TAReportItems { get; set; }

    }

    public class TAReportItem 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public DateTime ReportDate { get; set; }
        public Int16 Day { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Shift { get; set; }
        public Nullable<byte> Status { get; set; }

        public bool Completed { get; set; }
        public bool Job { get; set; }

//      public int ScheduleNo { get; set; }
        public System.DateTime ModifiedLast { get; set; }
//      public string ModifiedBy { get; set; }
        public string ModifiedId { get; set; }

        public bool IsDeleted { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}