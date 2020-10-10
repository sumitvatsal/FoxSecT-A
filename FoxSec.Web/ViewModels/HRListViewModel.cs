using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;
using System.Xml.Serialization;
using System.Data;

namespace FoxSec.Web.ViewModels
{
    public class HRListViewModel : ViewModelBase
    {
        public HRListViewModel()
        {
            HRItems = new List<HRItem>();
        }
        public IEnumerable<HRItem> HRItems { get; set; }
    }
    public class HRItem
    {
        public long id { get; set; }
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Department { get; set; }
        public string LastDateOfWork { get; set; }

        public string PersonalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public string Name { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string Serial { get; set; }
        public string Dk { get; set; }       
    }


    public class datatableListViewModel : ViewModelBase
    {
        public datatableListViewModel()
        {
            datatables = new List<datatable>();
        }
        public IEnumerable<datatable> datatables { get; set; }
    }


    public class datatable
    {
        public int Id { get; set; }
        public string ois_id_isik { get; set; }
        public string isikukood { get; set; }
        public string e_nimi { get; set; }
        public string p_nimi { get; set; }
        public string kasutajatunnus { get; set; }
        public string toosuhte_algus { get; set; }
        public string toosuhte_lopp { get; set; }
        public string ylikooli_e_post { get; set; }
        public string tookoha_aadress { get; set; }
    }

    public class XmlListViewModel : ViewModelBase
    {
        //public XmlListViewModel()
        //{
        //    xmlTable = new DataTable();
        //}
        public DataTable xmlTable { get; set; }
        //public IEnumerable<xmlHR> xmlTable { get; set; }
    }

}