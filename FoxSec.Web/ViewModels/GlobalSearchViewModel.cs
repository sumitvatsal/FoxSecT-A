using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class GlobalSearchViewModel : PaginatorViewModelBase
    {
        public GlobalSearchViewModel()
        {
            Items = new List<GlobalSearchItem>();
        }
        
        public string SearchCriteria { get; set; }

        public IEnumerable<GlobalSearchItem> Items { get; set; }
    }

    public class GlobalSearchItem
    {
        public int Id{ get; set;}

        public string Name{ get; set;}

        public Menu ItemType{ get; set;}

        public string TypeDescription{ get; set;}

        public string ToolTipName { get; set; }

        public string Function { get; set; }

    }

    
}