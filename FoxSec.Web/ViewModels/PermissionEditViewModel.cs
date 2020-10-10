using FoxSec.DomainModel.DomainObjects;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class PermissionEditViewModel : ViewModelBase
    {
        public PermissionEditViewModel()
        {
            Permission = new PermissionItem();
        }

        public PermissionItem Permission { get; set; }
        public SelectList Groups { get; set; }
        public SelectList UserList { get; set; }
        public int SelectedGroupId { get; set; }
   
    }
}