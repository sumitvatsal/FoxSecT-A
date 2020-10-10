using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class CompanyListViewModel : PaginatorViewModelBase
    {
        public CompanyListViewModel()
        {
            Companies = new List<CompanyItem>();
        }

        public IEnumerable<CompanyItem> Companies { get; set; }

        public int FilterCriteria { get; set; }
    }

    public class CompanyItem
    {
        public CompanyItem()
        {
            CompanyBuildingItems = new List<CompanyBuildingItem>();
        }
        public int? Id { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldsRequiredValidationMessage")]
        [StringLength(50, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError50")]
        public string Name { get; set; }

        public string ModifiedBy { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(ViewResources.SharedStrings), ErrorMessageResourceName = "FieldLengthError250")]
        public string Comment { get; set; }

        public bool Active { get; set; }

        public string Floors { get; set; }

        public string BuidingNames { get; set; }

        public bool IsCanUseOwnCards { get; set; }

        public IEnumerable<CompanyBuildingObject> CompanyBuildingObjects { get; set; }

        public IEnumerable<Building> CompanyBuildings { get; set; }

        public List<CompanyBuildingItem> CompanyBuildingItems { get; set; }
    }


    public class CompanyBuildingItem
    {
        public CompanyBuildingItem()
        {
            CompanyFloors = new List<CompanyFloorItem>();
            BuildingItems = new List<SelectListItem>();
            Index = 0;
        }

        public IEnumerable<SelectListItem> BuildingItems { get; set; }

        public int? BuildingId { get; set; }

        public List<CompanyFloorItem> CompanyFloors { get; set; }

        public bool IsAvailable { get; set; }

        public string BuildingName { get; set; }

        public int Index { get; set; }
    }

    public class CompanyFloorItem
    {
        public int? Id { get; set; }

        public int BuildingObjectId { get; set; }

        public bool IsSelected { get; set; }

        public bool IsAvailable { get; set; }

        public int? CompanyId { get; set; }

    }
}










    