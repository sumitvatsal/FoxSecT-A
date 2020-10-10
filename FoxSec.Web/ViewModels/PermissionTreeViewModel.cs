using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class PermissionTreeViewModel : ViewModelBase
    {
		public PermissionTreeViewModel()
		{
			Countries = new List<Node>();
			Towns = new List<Node>();
			Buildings = new List<Node>();
			Floors = new List<Node>();
			Objects = new List<Node>();
			ActiveObjectIds = new List<int>();
            IsOriginal = true;
			IsGroupExist = true;
		    IsCurrentUserAssignedGroup = false;
		}
        public IEnumerable<Node> Countries;
        public IEnumerable<Node> Towns;
        public IEnumerable<Node> Buildings;
        public IEnumerable<Node> Floors;
        public IEnumerable<Node> Objects;
        public IEnumerable<int> ActiveObjectIds;
        public bool IsOriginal;
		public bool IsGroupExist { get; set; }
		public bool IsCurrentUserAssignedGroup { get; set; }
    }
}