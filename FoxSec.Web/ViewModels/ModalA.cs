using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class ModalA
    {
        public class BatchEditRepository
        {
            public static List<GridDataItem> GridData
            {
                get
                {
                    var key = "34FAA431-CF79-4869-9488-93F6AAE81263";
                    var Session = HttpContext.Current.Session;
                    if (Session[key] == null)
                        Session[key] = Enumerable.Range(0, 100).Select(i => new GridDataItem
                        {
                            ID = i,
                            C1 = i % 2,
                            C2 = i * 0.5 % 3,
                            C3 = "C3 " + i,
                            C4 = i % 2 == 0,
                            C5 = new DateTime(2013 + i, 12, 16)
                        }).ToList();
                    return (List<GridDataItem>)Session[key];
                }
            }
            public static void InsertNewItem(GridDataItem postedItem, MVCxGridViewBatchUpdateValues<GridDataItem, int> batchValues)
            {
                try
                {
                    var newItem = new GridDataItem() { ID = GridData.Count };
                    LoadNewValues(newItem, postedItem);
                    GridData.Add(newItem);

                }
                catch (Exception e)
                {
                    batchValues.SetErrorText(postedItem, e.Message);
                }
            }
            public static void UpdateItem(GridDataItem postedItem, MVCxGridViewBatchUpdateValues<GridDataItem, int> batchValues)
            {
                try
                {
                    var editedItem = GridData.First(i => i.ID == postedItem.ID);
                    LoadNewValues(editedItem, postedItem);
                }
                catch (Exception e)
                {
                    batchValues.SetErrorText(postedItem, e.Message);
                }
            }
            public static void DeleteItem(int itemKey, MVCxGridViewBatchUpdateValues<GridDataItem, int> batchValues)
            {
                try
                {
                    var item = GridData.First(i => i.ID == itemKey);
                    GridData.Remove(item);
                }
                catch (Exception e)
                {
                    batchValues.SetErrorText(itemKey, e.Message);
                }

            }
            protected static void LoadNewValues(GridDataItem newItem, GridDataItem postedItem)
            {
                newItem.C1 = postedItem.C1;
                newItem.C2 = postedItem.C2;
                newItem.C3 = postedItem.C3;
                newItem.C4 = postedItem.C4;
                newItem.C5 = postedItem.C5;
            }
        }

        public class GridDataItem
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Field is required")]
            public int C1 { get; set; }
            [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
            public double C2 { get; set; }
            [Required(ErrorMessage = "Field is required")]
            [StringLength(10, ErrorMessage = "Must be under 10 characters")]
            public string C3 { get; set; }
            [Display(Name = "Current State")]
            [Required(ErrorMessage = "State is required")]
            public bool C4 { get; set; }
            [Display(Name = "Current Date")]
            [Required(ErrorMessage = "Date is required")]
            public DateTime C5 { get; set; }
        }
    }
}