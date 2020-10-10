using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxSec.ServiceLayer.ServiceResults;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IVisitorService
    {

        VisitorCreateResult CreateVisitor(string carNr, int? userId, string firstName, string carType, DateTime? startDateTime, DateTime? stopDateTime, int? companyId, string lastName, string company, string email, string host, string phoneNumber, bool isphonenraccessunit, bool iscarnraccessunit, DateTime? returnDate, bool cardneedreturn,string PersonalCode, string Comment);

        /*  bool CardIsBack(int id);
          bool EditCard(int Id, int? userId, int? typeId, int? companyId, int? buildingId, string serial, string dk, string code, bool isFree, DateTime? from, DateTime? to, string Comment, bool? isActive = null);
          void Deactivate(int cardId, int? classificatorValueId);
          void Activate(int cardId, int? classificatorValueId);
          void Delete(int cardId);
          void SetFreeState(int cardId, int? classificatorValueId);
          void SetValidFrom(int cardId, DateTime date);
          void SetValidTo(int cardId, DateTime date); */


        /*
         
        Visitor Entity
        public new int Id { get; set; }
        public string CarNr { get; set; }

        [Key, ForeignKey("User")]
        public Nullable<int> UserId { get; set; }
        public string FirstName { get; set; }
        public string CarType { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> StopDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public System.DateTime LastChange { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public new byte[] Timestamp { get; set; }
        public bool Accept { get; set; }
        public Nullable<int> AcceptUserId { get; set; }
        public Nullable<System.DateTime> AcceptDateTime { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public string Company { get; set; }
        public Nullable<int> ParentVisitorsId { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCarNrAccessUnit { get; set; }
        public bool IsPhoneNrAccessUnit { get; set; }
        public Nullable<int> ResponsibleUserId { get; set; }
        public bool CardNeedReturn { get; set; }

         
         
         
         
         
         */

        void EditUserPersonalData(int id,
                                 string firstName,
                                 string lastName,
                                 int? companyid,
                                 string phonenr,
                                 bool isphonenraccessunit,
                                 string email,
                                 string carnr,
                                 bool iscarnraccessunit,
                                 string cartype,
                                 DateTime? startdatetime,
                                 DateTime? stopdatetime,
                                 int? userid,
                                 bool cardneedreturn,
                                 DateTime? returndate,
                                 string PersonalCode,
                                 string Comment
            );

        void DeleteVisitor(int id,string host);

        int CreateLog(int userId, string building, string flag, string node, int? companyId, string action, int? logTypeId = null);

        bool GiveCardBack(int id, DateTime returndate, bool cardneedreturn,int userID);
    }
}
