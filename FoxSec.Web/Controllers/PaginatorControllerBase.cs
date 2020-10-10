using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.Web.ViewModels;

namespace FoxSec.Web.Controllers
{
	public abstract class PaginatorControllerBase<T> : BusinessCaseController
	{
		protected PaginatorControllerBase(ICurrentUser currentUser, ILogger logger) : base(currentUser, logger)
		{}

		protected virtual PaginatorViewModel SetupPaginator(ref IEnumerable<T> collection, int? current_page, int? rows_per_page)
		{
            //FoxSecDBContext db = new FoxSecDBContext();        

            //var totalRows = db.FSCameras.Count(x => x.Id != null);
            //int rcount = Convert.ToInt32(totalRows);

            PaginatorViewModel paginator = new PaginatorViewModel();
            paginator.TotalRows = collection.Count();
            //paginator.TotalRows = rcount;
			paginator.DivToRefresh = typeof(T).Name + "List";  

            if (rows_per_page.HasValue)
            {
                paginator.RowsPerPage = rows_per_page.Value;
            }
            else
            {
                paginator.RowsPerPage = 10;
            }

            paginator.TotalPages = (int)Math.Ceiling(paginator.TotalRows / (float)paginator.RowsPerPage);
			if (!current_page.HasValue)
			{
				paginator.CurrentPage = 0;
				collection = collection.Skip(0).Take(paginator.RowsPerPage);
			}
			else if (current_page == -1)
			{
				paginator.CurrentPage = 0;
			}
			else
			{
				if( current_page >= paginator.TotalPages ) current_page = paginator.TotalPages - 1;
				paginator.CurrentPage = (int)current_page;
				collection = collection.Skip(paginator.CurrentPage * paginator.RowsPerPage).Take(paginator.RowsPerPage);
			}
			paginator.RowsShown = collection.Count();
            return paginator;
		}

          protected virtual PaginatorViewModel SetupPaginator(int? current_page, int? rows_per_page, int totalRecCount, int recCount)
          {
              PaginatorViewModel paginator = new PaginatorViewModel();
              paginator.TotalRows = totalRecCount;
              paginator.DivToRefresh = typeof(T).Name + "List";
              if (rows_per_page.HasValue)
              {
                  paginator.RowsPerPage = rows_per_page.Value;
              }
              else
              {
                  paginator.RowsPerPage = 10;
              }
              paginator.TotalPages = (int)Math.Ceiling(paginator.TotalRows / (float)paginator.RowsPerPage);

              if (!current_page.HasValue)
              {
                  paginator.CurrentPage = 0;
              }
              else if (current_page == -1)
              {
                  paginator.CurrentPage = 0;
              }
              else
              {
                  if (current_page >= paginator.TotalPages) current_page = paginator.TotalPages - 1;
                  paginator.CurrentPage = (int)current_page;
              }
              paginator.RowsShown = recCount;
              return paginator;
          }

		public static void AddModelError(ModelStateDictionary modelState, string key, string error)
		{
			modelState[key].Errors.Add(error);
		}
	}
}