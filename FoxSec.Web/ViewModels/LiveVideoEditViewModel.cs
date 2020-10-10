using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class LiveVideoEditViewModel : ViewModelBase
    {
        public LiveVideoEditViewModel()
        {
            LiveVideoItem = new LiveVideoItem();
            FSVServers = new List<FSVideoServers>();
        }

        public LiveVideoItem LiveVideoItem { get; set; }
        public List<FSVideoServers> FSVServers { get; set; }
    }
}