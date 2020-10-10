using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class UserAccessUnitListViewModel : PaginatorViewModelBase
    {
        public UserAccessUnitListViewModel()
        {
            Cards = new List<UserAccessUnitListItem>();
        }

        public int FilterCriteria { get; set; }

        public Boolean CardExists { get; set; }

        public int IsInList { get; set; }

        public IEnumerable<UserAccessUnitListItem> Cards { get; set; }

        public bool IsDispalyColumn1 { get; set; }

        public bool IsDispalyColumn2 { get; set; }

        public bool IsDispalyColumn3 { get; set; }

        public bool IsDispalyColumn4 { get; set; }

        public bool IsDispalyColumn5 { get; set; }

        public bool IsDispalyColumn6 { get; set; }
        public bool IsDispalyColumn7 { get; set; }
        public bool IsDispalyColumn8 { get; set; }
        public bool IsDispalyColumn9 { get; set; }
    }

    public class UserAccessUnitListItem
    {
        public int? Id { get; set; }

        public int? UserId { get; set; }

        public string Code { get; set; }

        public string Serial { get; set; }

        public string Dk { get; set; }

        public int? CompanyId { get; set; }

        public bool Active { get; set; }

        public bool Free { get; set; }

        // custom:

        public String DeactivationReason { get; set; }

        public String DeactivationDateTime { get; set; }

        public string CompanyName { get; set; }

        public string Building { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public string ValidFromStr { get; set; }

        public string ValidToStr { get; set; }

        public DateTime? ValidTo { get; set; }

        public string FullCardCode
        {
            get
            {
                if (!string.IsNullOrEmpty(Code)) return Code;
                if (!string.IsNullOrEmpty(Serial) && (!string.IsNullOrEmpty(Dk))) return Serial + "+" + Dk;
                return "-";
            }
        }

        public string CardStatus
        {
            get
            {
                if (Free) return ViewResources.SharedStrings.FilterFreeShort;
                return Active ? ViewResources.SharedStrings.FilterActiveShort : ViewResources.SharedStrings.FilterDeactivatedShort;
            }
        }

        public int CardStatusNumber
        {
            get
            {
                if (Free)
                {
                    return 2;
                }

                return Active ? 1 : 0;
            }
        }
    }
}