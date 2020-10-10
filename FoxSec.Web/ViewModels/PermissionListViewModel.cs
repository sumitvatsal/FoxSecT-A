using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class PermissionListViewModel : ViewModelBase
    {
        public PermissionListViewModel()
        {
            Permissions = new List<PermissionItem>();
        }

        public List<PermissionItem> Permissions { get; set; }
    }

    public class PermissionItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int UserID { get; set; }
    }
}