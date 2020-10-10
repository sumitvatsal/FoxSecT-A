using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Web.ViewModels;
//using static FoxSec.DomainModel.DomainObjects.CameraPermissions;


namespace FoxSec.Web.ViewModels
{
    public class cameralistviewmodel : PaginatorViewModelBase
    {
        public cameralistviewmodel()
        {
            cameras = new List<value>();
        }

        public IEnumerable<value> cameras { get; set; }

        public int FilterCriteria { get; set; }

        public bool IsDispalyColumn1 { get; set; }

        public bool IsDispalyColumn2 { get; set; }

        public bool IsDispalyColumn3 { get; set; }

        public bool IsDispalyColumn4 { get; set; }

        public bool IsDispalyColumn5 { get; set; }

        public bool IsDispalyColumn6 { get; set; }

        public bool Comment { get; set; }
    }

    public class value
    {
        //public  virtual int Id { get; set; }
        public string Name { get; set; }
        // public int CompanyId { get; set; }
        public string status { get; set; }
        public int Id { get; set; }
        public virtual int ? CameraID { get; set; }
        public string CameraName { get; set; }
        public virtual int ? CompanyID { get; set; }
        public virtual bool? Access { get; set; }

    }
}


