using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class AssignShiftUserModel
    {
        public AssignShiftUserModel()
        {
            
            AssignShiftUserModelsList = new List<AssignShiftUserModel>();
        }
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PersonalId { get; set; }

        public bool Active { get; set; }

        public string Comment { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedLast { get; set; }

        public string OccupationName { get; set; }

        public string PhoneNumber { get; set; }

        public Int16? WorkHours { get; set; }

        public int? GroupId { get; set; }

        public byte[] Image { get; set; }

        public DateTime? Birthday { get; set; }

        public string Birthplace { get; set; }

        public string FamilyState { get; set; }

        public string Citizenship { get; set; }

        public string Residence { get; set; }

        public string Nation { get; set; }

        public string ContractNum { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public DateTime? PermitOfWork { get; set; }

        public bool? PermissionCallGuests { get; set; }

        public bool? MillitaryAssignment { get; set; }

        public String PersonalCode { get; set; }

        public String ExternalPersonalCode { get; set; }

        public int? LanguageId { get; set; }

        public DateTime RegistredStartDate { get; set; }

        public DateTime RegistredEndDate { get; set; }

        public int? TableNumber { get; set; }

        public virtual bool? WorkTime { get; set; }

        public bool? EServiceAllowed { get; set; }

        public bool? IsVisitor { get; set; }

        public bool? CardAlarm { get; set; }

        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }
      
        public int? CompanyId { get; set; }
        public int? TitleId { get; set; }
        public bool? IsShortTermVisitor { get; set; }

        public bool? ApproveTerminals { get; set; }
        public bool? ApproveVisitor { get; set; }

        public bool? DisableAddUsers { get; set; }

        public int TaUsersGroupShiftId { get; set; }

        public TaUsersShifts TaUsersShifts { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsSelected { get; set; }

        public List<AssignShiftUserModel> AssignShiftUserModelsList { get; set; }
    }
}