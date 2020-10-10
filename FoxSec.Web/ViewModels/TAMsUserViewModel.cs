using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class TAMsUserViewModel : ViewModelBase
    {
        public TAMsUserViewModel()
        {
            TAMsUserItems = new List<TAMove>();//<TAMove>();
        }
        public IEnumerable<TAMove> TAMsUserItems { get; set; }
      
    }

    public class TAMsUserItem
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Label { get; set; }

        public string Name { get; set; }
        public System.DateTime Started { get; set; }
        public Nullable<System.DateTime> Finished { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public bool Job_Move { get; set; }
        public bool Completed { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Timestamp { get; set; }
        public System.DateTime ModifiedLast { get; set; }
        public string ModifiedBy { get; set; }
        public virtual User User { get; set; }
        public string UserName
        {
            get { return User.LastName + " " + User.FirstName; }
            set {  }
        }
      
    }
}