using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class TaReportViewNewCustomoseUser
    {

        public List<TaNewUserDetails> TaUserDetails { get; set; }
    }
    public class TaNewUserDetails
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalCode { get; set; }
        public string Name { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string checkin { get; set; }
        public double Hours { get; set; }
        public string checkout { get; set; }
        public string totaltime { get; set; }
        public int id { get; set; }
        public string FullName { get; set; }
        public int CompnyId { get; set; }
        public int buildngId { get; set; }
        public string companydeatils { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegistration_Num { get; set; }
        public string colorflag { get; set; }
        public string companycolrflag { get; set; }
        public string Department { get; set; }

        public DateTime? Birthday { get; set; }

        public string Birthdate { get; set; }
        public string StartedInLat { get; set; }  //Added

        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public string ReportDate { get; set; }

        public DateTime TodayDate { get; set; }
        public int SrNo { get; set; }

        public string Remark { get; set; }
        public string RegistrationNo { get; set; }
        public string ContractNo { get; set; }
        public string Hours_Min { get; set; }
        public string RprtDate { get; set; }

        public int DepartmentId { get; set; }

        public string Status { get; set; }
        public string Completed { get; set; }
        public string ModifiedLast { get; set; }
        public int? TotalWorkingDays { get; set; }

        public string Comment { get; set; }

        public string ExtPersonalCode { get; set; }
    }
}