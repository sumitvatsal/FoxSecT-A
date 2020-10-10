using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class FSINISettingsListViewModel : ViewModelBase
    {
        public FSINISettingsListViewModel()
        {
            FSINISettingsItems = new List<FSINISettingsItem>();
        }
        public IEnumerable<FSINISettingsItem> FSINISettingsItems { get; set; }
    }
    public class FSINISettingsItem
    {
        public int Id { get; set; }
        public int SoftType { get; set; }
        public int? SoftId { get; set; }
        public string Value { get; set; }
    }
}