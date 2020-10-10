using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
	public class PaginatorViewModel
	{
		public PaginatorViewModel()
		{
			RowsPerPageItems = new List<SelectListItem>
			            	{
			            		new SelectListItem()
			            			{Value = "10", Text = string.Format("{0} {1}", 10, ViewResources.SharedStrings.CommonPerPage)},
			            		new SelectListItem()
			            			{Value = "50", Text = string.Format("{0} {1}", 50, ViewResources.SharedStrings.CommonPerPage)},
			            		new SelectListItem()
			            			{Value = "100", Text = string.Format("{0} {1}", 100, ViewResources.SharedStrings.CommonPerPage)},
			            		new SelectListItem() {Value = "99999", Text = ViewResources.SharedStrings.CommonAll}
			            	};
		}

		public bool? sortOrder { get; set; }
		public int? sortField { get; set; }
		public int CurrentPage { get; set; }
		public int RowsPerPage { get; set; }
		public int TotalRows { get; set; }
		public int RowsShown { get; set; }
		public int TotalPages { get; set; }
		public string DivToRefresh { get; set; }
        public string Prefix { get; set; }
		public IEnumerable<SelectListItem> RowsPerPageItems { get; set; }
	}
}