using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class TAReportMounthViewModel : ViewModelBase
    {
        public TAReportMounthViewModel()
        {
            TAReportMounthItems = new List<TAReportMounthItem>();
        }

        public IEnumerable<TAReportMounthItem>  TAReportMounthItems { get; set; }
        public IEnumerable<TAReportUserTotal> TAReportUserTotal { get; set; }

    }
    public class TAReportUserTotal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public TimeSpan ValidityPeriods { get; set; }
    }

    public class TAReportMounthItem 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public DateTime ReportDate { get; set; }
        public short Day { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Shift { get; set; }
        public Nullable<byte> Status { get; set; }
        public bool Completed { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Timestamp { get; set; }
        public System.DateTime ModifiedLast { get; set; }
        public int ModifiedId { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ValidityPeriods
        {
            get { return TimeSpan.FromSeconds(Hours); }
            set { Hours = value.Ticks; }
        }
        public virtual Department Department { get; set; }
        public virtual User User { get; set; }
        public string UserName
        {
            get { return User.LastName + " " + User.FirstName; }
            set { }
        }
        public string FirstName { get; set; }
        public string RprtDate { get; set; }
        public string LastName { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int SrNo { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string PersonalCode { get; set; }
        public string ConstructorName { get; set; }
        public string ConstructorTaxNo { get; set; }
        public string ContractNo { get; set; }
        public string ContractAmount { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNo { get; set; }
        public string ContractDate { get; set; }
        public string UserID { get; set; }
    }
    

}
