using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;
using static FoxSec.DomainModel.DomainObjects.VideoAccess;


namespace FoxSec.Web.ViewModels
{
    public class LiveVideoListViewModel : PaginatorViewModelBase
    {
        public LiveVideoListViewModel()
        {
            Users = new List<item>();
            Users1 = new List<value>();
        }

        public IEnumerable<item> Users { get; set; }
        public IEnumerable<value> Users1 { get; set; }

        public int FilterCriteria { get; set; }

        public bool IsDispalyColumn1 { get; set; }

        public bool IsDispalyColumn2 { get; set; }

        public bool IsDispalyColumn3 { get; set; }

        public bool IsDispalyColumn4 { get; set; }

        public bool IsDispalyColumn5 { get; set; }

        public bool IsDispalyColumn6 { get; set; }

        public bool Comment { get; set; }
    }

    public class LiveVideoItem
    {
        public LiveVideoItem()
        {
            FSVideoServers = new List<FSVideoServers>();
        }

        public int Id { get; set; }

        public int? ServerNr { get; set; }
        public int? CameraNr { get; set; }

        public string Name { get; set; }

        public int? Port { get; set; }

        public int? ResX { get; set; }

        public int? ResY { get; set; }

        public int? Skip { get; set; }

        public int? Delay { get; set; }

        public bool EnableLiveControls { get; set; }

        public int? QuickPreviewSeconds { get; set; }

        public string URL { get; set; }

        public string Playback { get; set; }

        public IEnumerable<FSVideoServers> FSVideoServers { get; set; }

    }

    public class item
    {
       

        public string Name { get; set; }
        // public int CompanyId { get; set; }
        public string status { get; set; }
        public int Id { get; set; }
    }
}


