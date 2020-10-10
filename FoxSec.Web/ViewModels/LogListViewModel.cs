using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class LogListViewModel : PaginatorViewModelBase
    {
        public LogListViewModel()
        {
            Items = new List<LogItem>();
            userLastMovesItems = new List<UserLastMovesItem>();
        }
        public string Report { get; set; }
        public string Company { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int ReportType { get; set; }
        public int TotalUsers { get; set; }
        public IEnumerable<LogItem> Items { get; set; }
        public IEnumerable<UserLastMovesItem> userLastMovesItems { get; set; }
    }
    public class UserLastMovesItem
    {
        public int UserId { get; set; }
        public DateTime LastMoveTime { get; set; }
        public int BuildingId { get; set; }
        public int CompanyId { get; set; }
        public string User { get; set; }
    }

    public class LogItem
    {
        public int? UserId { get; set; }

        public string EventTimeStr { get; set; }

        public DateTime EventTime { get; set; }

        public string Action { get; set; }

        public string ShortAction { get; set; }

        public string Building { get; set; }

        public string Node { get; set; }

        public int? DefaultLogId { get; set; }

        public int? LogTypeId { get; set; }

        public string CompanyName { get; set; }

        public string UserName { get; set; }

        public bool IsUserDeleted { get; set; }

        public bool IsCompanyDeleted { get; set; }

        public string LogRecordColor { get; set; }



    }

    public class LogFilterItem
    {
        public int? LogFilterId { get; set; }

        public int? CompanyId { get; set; }

        public string UserName { get; set; }

        public string Activity { get; set; }

        public string Name { get; set; }

        public int? UserId { get; set; }

        public string Building { get; set; }

        public string Node { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string CommonSearch { get; set; }

        public bool IsShowDefaultLog { get; set; }

        public bool ischeck { get; set; }

        public bool NotMoved { get; set; }

        public int? CompId { get; set; }
    }

    //class GeoLocation
    //{
    //    public double Latitude { get; set; }
    //    public double Longitude { get; set; }
    //}
    //public class GoogleTimeZone
    //{
    //    private string apiKey;
    //    private GeoLocation location;
    //    private string previousAddress = string.Empty;

    //    public GoogleTimeZone(string apiKey)
    //    {
    //        this.apiKey = apiKey;
    //    }
    //}

    //public class GoogleTimeZoneResult
    //{
    //    public DateTime DateTime { get; set; }
    //    public string TimeZoneId { get; set; }
    //    public string TimeZoneName { get; set; }
    //}

}