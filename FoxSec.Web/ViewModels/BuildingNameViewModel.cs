using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace FoxSec.Web.ViewModels
{
    public class BuildingNameViewModel
    {
        public List<itembuilding> BuildingData { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Select Building")]
        public int BuildingId { get; set; }
        [Remote("isNameExists", "TAReport", ErrorMessage = "This Building Name Already Exits")]

        [Required(ErrorMessage = "Enter Building Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Valid From")]
        public DateTime ValidFrom { get; set; }
        [Required(ErrorMessage = "Enter Valid To")]
        public DateTime ValidTo { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Enter Building License")]
        public string BuildingLicense { get; set; }
        [Required(ErrorMessage = "Enter CadastralNr License")]
        public string CadastralNr { get; set; }
        public bool IsDeleted { get; set; }
        public string Customer { get; set; }
        public string Contractor { get; set; }
        public string Contract { get; set; }
        public string Sum { get; set; }
        public decimal? Lat { get; set; }

        public decimal? Lng { get; set; }

        public GridViewEditingDemosHelper gbm { get; set; }
        public IEnumerable<itembuilding> buildings { get; set; }

    }
   
    public class itembuilding
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Name { get; set; }
        //public string ValidFrom { get; set; }
        //public string ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Address { get; set; }      
        public string BuildingLicense { get; set; }
        public string CadastralNr { get; set; }
        public bool IsDeleted { get; set; }
        public string BuildingName { get; set; }
        public bool Isdeleted { get; set; }
        public string Customer { get; set; }
        public string Contractor { get; set; }
        public string Contract { get; set; }
        public string Sum { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

    }
    public class TaModel
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Address { get; set; }

        public string BuildingLicense { get; set; }
        public string CadastralNr { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BatchEditRepository
    {
        public static List<itembuilding> GridData
        {
            get
            {
                var key = "34FAA431-CF79-4869-9488-93F6AAE81263";
                var Session = HttpContext.Current.Session;
                if (Session[key] == null)
                    Session[key] = Enumerable.Range(0, 100).Select(i => new itembuilding
                    {
                        //id = i
                        
                    }).ToList();
                return (List<itembuilding>)Session[key];
            }
        }
        //public static void InsertNewItem(GridDataItem postedItem, MVCxGridViewBatchUpdateValues<GridDataItem, int> batchValues)
        //{
        //    try
        //    {
        //        var newItem = new GridDataItem() { ID = GridData.Count };
        //        LoadNewValues(newItem, postedItem);
        //        GridData.Add(newItem);

        //    }
        //    catch (Exception e)
        //    {
        //        batchValues.SetErrorText(postedItem, e.Message);
        //    }
        //}
        public static void UpdateItem(itembuilding postedItem, MVCxGridViewBatchUpdateValues<itembuilding, int> batchValues)
        {
            try
            {
                var editedItem = GridData.First(i => i.Id == postedItem.Id);
                LoadNewValues(editedItem, postedItem);
            }
            catch (Exception e)
            {
                batchValues.SetErrorText(postedItem, e.Message);
            }
        }
        //public static void DeleteItem(int itemKey, MVCxGridViewBatchUpdateValues<GridDataItem, int> batchValues)
        //{
        //    try
        //    {
        //        var item = GridData.First(i => i.ID == itemKey);
        //        GridData.Remove(item);
        //    }
        //    catch (Exception e)
        //    {
        //        batchValues.SetErrorText(itemKey, e.Message);
        //    }

        //}
        protected static void LoadNewValues(itembuilding newItem, itembuilding postedItem)
        {
            newItem.Id = postedItem.Id;
            newItem.Name = postedItem.Name;
            newItem.Address = postedItem.Address;
            newItem.BuildingLicense = postedItem.BuildingLicense;
            newItem.CadastralNr = postedItem.CadastralNr;
        }
    }

    public class GridViewEditingDemosHelper
    {

        const string EditingModeSessionKey = "AA054892-1B4C-4158-96F7-894E1545C05C";

        public static GridViewEditingMode EditMode
        {
            get
            {
                if (Session[EditingModeSessionKey] == null)
                    Session[EditingModeSessionKey] = GridViewEditingMode.EditFormAndDisplayRow;
                return (GridViewEditingMode)Session[EditingModeSessionKey];
            }
            set { HttpContext.Current.Session[EditingModeSessionKey] = value; }
        }

        static List<GridViewEditingMode> availableEditModesList;
        public static List<GridViewEditingMode> AvailableEditModesList
        {
            get
            {
                if (availableEditModesList == null)
                    availableEditModesList = new List<GridViewEditingMode> {
                        GridViewEditingMode.Inline,
                        GridViewEditingMode.EditForm,
                        GridViewEditingMode.EditFormAndDisplayRow,
                        GridViewEditingMode.PopupEditForm
                    };
                return availableEditModesList;
            }
        }

        protected static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    }
}