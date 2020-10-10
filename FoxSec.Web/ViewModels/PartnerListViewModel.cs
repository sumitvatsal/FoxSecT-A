using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class PartnerListViewModel : ViewModelBase
	{
        public PartnerListViewModel()
        {
            Partners = new List<PartnerItem>();
        }

        public IEnumerable<PartnerItem> Partners { get; set; }
	}

    public class PartnerItem
    {
        public int? Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public bool Active { get; set; }

        public int CompanyId { get; set; }

        public string ManagerName { get; set; }

        public int ManagerId { get; set; }
    }
}