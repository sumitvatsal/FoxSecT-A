using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using FoxSec.DomainModel.DomainObjects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FoxSec.Web.ViewModels
{
    public class Connection
    {
        public static string connstring = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
    }


    public class TaReport1
    {
        public int UserId { get; set; }
        public int BuildingId { get; set; }
    }
    public class Companies
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? ClassificatorValueId { get; set; }

        public string Name { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedLast { get; set; }

        public string Comment { get; set; }

        public bool Active { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsCanUseOwnCards { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
    public class Custormer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> CityId { get; set; }

        //public virtual City City { get; set; }
        //public virtual Country Country
        //{
        //    get; set;
        //}
    }
    public class Departments
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public int CompanyId { get; set; }
    }

    public class Buildings
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TimediffGMTMinutes { get; set; }
        public string AdressStreet { get; set; }
        public string AdressHouse { get; set; }
        public string AdressIndex { get; set; }
        public int LocationId { get; set; }
        public bool IsDeleted { get; set; }
        public int Floors { get; set; }
        public string TimezoneId { get; set; }
       
    }
    public class TABuildingNames
    {
        [Key]
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Address { get; set; }
        public string BuildingLicense { get; set; }
        public string CadastralNr { get; set; }
        public bool IsDeleted { get; set; }
        public string Customer { get; set; }
        public string Contractor { get; set; }
        public string Contract { get; set; }
        public string Sum { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
    }
    public class specificUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class Users
    {
        [Key]
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
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public int? CompanyId { get; set; }
        public int? TitleId { get; set; }
        public bool? IsShortTermVisitor { get; set; }

        public bool? ApproveTerminals { get; set; }
        public bool? ApproveVisitor { get; set; }

        public bool? DisableAddUsers { get; set; }

        //relations
        public virtual ICollection<TaUsersShifts> TaUsersShifts { get; set; }
    }

    [Table("CameraIP")]
    public class CameraIP
    {
        [Key]
        public int CameraId { get; set; }
        public int? ServerId { get; set; }
        public string IP { get; set; }
        public string UID { get; set; }
        public string PWD { get; set; }
    }
    public class FSINISettings
    {
        [Key]
        public int Id { get; set; }
        public int SoftType { get; set; }
        public string Value { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class FSBuildingObjectCameras
    {
        [Key]
        public int Id { get; set; }
        public int BuildingObjectId { get; set; }
        public int CameraId { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class FSVideoServers
    {
        [Key]
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public string UID { get; set; }
        public string PWD { get; set; }
        public bool Deleted { get; set; }
    }

    public class FSVideoServersDetails
    {
        [Key]
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public string UID { get; set; }
        public string PWD { get; set; }
        public bool Deleted { get; set; }
    }

    public class FSProjects
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool active { get; set; }
    }

    public class FSBOC
    {
        [Key]
        public int Id { get; set; }
        public int BuildingObjectId { get; set; }
        public int CameraId { get; set; }
        public bool IsDeleted { get; set; }

    }


    [Table("CamPermission")]
    public class CamPermission
    {

        [Key]
        public int? CameraID { get; set; }
        public string CameraName { get; set; }

        public int? CompanyID { get; set; }

    }
    public class FSCameras
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string status { get; set; }

        public int ServerNr { get; set; }
        public int CameraNr { get; set; }
        public int Port { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }
        public int Skip { get; set; }
        public int Delay { get; set; }
        public int QuickPreviewSeconds { get; set; }

        public bool Deleted { get; set; }
    }

    public class FSCamera_test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ServerNr { get; set; }
        public int? CameraNr { get; set; }
        public int? Port { get; set; }
        public int? ResX { get; set; }
        public int? ResY { get; set; }
        public int? Skip { get; set; }
        public int? Delay { get; set; }
        public int? QuickPreviewSeconds { get; set; }

    }

    public class UserDepartments
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsDepartmentManager { get; set; }

    }
    public class Hr_Clone
    {
        [Key]
        public int Id { get; set; }
        public string ois_id_isik { get; set; }
        public string Personal_code { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string username { get; set; }
        public string dateform { get; set; }
        public string dateto { get; set; }
        public string email { get; set; }
        public string Address { get; set; }
    }
    public class Titles
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    //Commented by Manoranjan
    //public class TAReports
    //{
    //    public int UserId { get; set; }
    //    public int DepartmentId { get; set; }
    //    public int BuildingId { get; set; }
    //}
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class BuildingObjects
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
    }
    public class TAMoves
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Label { get; set; }
        public string Remark { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Schedule { get; set; }
        public Nullable<byte> Status { get; set; }
        public bool JobNotMove { get; set; }
        public bool Completed { get; set; }

        public System.DateTime ModifiedLast { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.Int32> StartedBoId { get; set; }
        public Nullable<System.Int32> FinishedBoId { get; set; }

        public virtual User User { get; set; }
        public virtual Department Department { get; set; }
        public string UserName
        {
            get { return User.FirstName + " " + User.LastName; }
            set { }
        }
        public bool IsDeleted { get; set; }
        public int BuildingID { get; set; }

    }
    public class Box111
    {
        public int id { get; set; }
    }
    public class UserLog
    {
        [Key]
        public int id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserRole { get; set; }
        public string SearchBOx { get; set; }
        public DateTime Datefrom { get; set; }
        public DateTime dateto { get; set; }
        public string buildingfilter { get; set; }
        public string ShowDefaultOrfullLog { get; set; }
        public string Node { get; set; }
        public string CompanyFiter { get; set; }
        public string Name { get; set; }
        public string Filter_text { get; set; }
        public string user_text { get; set; }
        public string Activity { get; set; }
        public string button_clicked { get; set; }
        public int totalRecordView { get; set; }
        public DateTime VisitDate { get; set; }
    }

    public class FSHR
    {
        [Key]
        public int Id { get; set; }
        public string FoxSecFieldName { get; set; }
        public string HRFieldname { get; set; }
        public int FoxSecTableId { get; set; }
        public bool IsIndex { get; set; }
        public bool AutoUpdate { get; set; }
        public int FieldType { get; set; }
        public bool IsDeleted { get; set; }
        public string IndexFilename { get; set; }
        public string FoxsecTableName { get; set; }

        public List<FSHR> fsHrList { get; set; }
    }
    public class Buildingclass
    {
        public string Building { get; set; }
    }

    public class Countries
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ISONumber { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Locations
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Terminal
    {
        [Key]
        public int Id { get; set; }
        public bool ShowScreensaver { get; set; }
        public TimeSpan ScreenSaverShowAfter { get; set; }
        public int MaxUserId { get; set; }
        public int? CompanyId { get; set; }
        public string TerminalId { get; set; }
        public bool ApprovedDevice { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<bool> InfoKioskMode { get; set; }
        public string SoundAlarms { get; set; }
        public Nullable<int> ShowMaxAlarmsFistPage { get; set; }
        public Nullable<DateTime> LastLogin { get; set; }

        public int? TARegisterBoId { get; set; }
    }

    //visitor
    public class Visitors
    {
        [Key]
        public int Id { get; set; }
        public virtual string CarNr { get; set; }

        /* [Key, ForeignKey("User")] */
        public virtual Nullable<int> UserId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string CarType { get; set; }
        public virtual Nullable<System.DateTime> StartDateTime { get; set; }
        public virtual Nullable<System.DateTime> StopDateTime { get; set; }
        public virtual bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        //public virtual Nullable<System.DateTime> UpdateDatetime { get; set; }
        public virtual DateTime? UpdateDatetime { get; set; }
        // public virtual System.DateTime LastChange { get; set; }
        public virtual DateTime? LastChange { get; set; }
        public virtual Nullable<int> CompanyId { get; set; }
        public virtual byte[] Timestamp { get; set; }
        public virtual bool Accept { get; set; }
        public virtual Nullable<int> AcceptUserId { get; set; }
        // public virtual Nullable<System.DateTime> AcceptDateTime { get; set; }
        // public virtual DateTime? AcceptDateTime { get; set; }
        public virtual DateTime? AcceptDateTime { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool Active { get; set; }
        public virtual string Company { get; set; }
        public virtual Nullable<int> ParentVisitorsId { get; set; }
        public virtual string Comment { get; set; }
        // public virtual Nullable<System.DateTime> ReturnDate { get; set; }
        public virtual DateTime? ReturnDate { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool IsCarNrAccessUnit { get; set; }
        public virtual bool IsPhoneNrAccessUnit { get; set; }
        public virtual Nullable<int> ResponsibleUserId { get; set; }
        public virtual bool CardNeedReturn { get; set; }
    }

    //By manoranjan
    [Table("CompanieRoles")]
        public class CompanyRoleModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }

        //relations
        public virtual Companies Company { get; set; }
    }

        //

    //Added by Manoranjan Date:13July2020
    [Table("userlastMoves")]
        public class UserLastMoves
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? NotFinishedMoveTaReportLabelId { get; set; }
        public DateTime NotFinishedMoveStartTime { get; set; }
        public int? NotFinishedJobTaReportLabelId { get; set; }
        public DateTime NotFinishedJobStartTime { get; set; }
        public bool MoveAllowNewWork { get; set; }
        public DateTime FirstComeToWork { get; set; }
        public DateTime DepartureFromWork { get; set; }
        public DateTime LastRecordedTimeAtWork { get; set; }
        public DateTime NextMoveBlockedTo { get; set; }
        public string LocationAtWork { get; set; }
        public DateTime LastMoveTime { get; set; }
        public int? LastMoveBOId { get; set; }
        public bool LastEnteredExited { get; set; }
        public bool EnteredBuilding { get; set; }
        public int Savestatus { get; set; }
        public int? TerminalLastEntryBoId { get; set; }

        //relations
        public virtual Users User { get; set; }
        
    }
    //
    
    //By Manoranjan
       [Table("Log")]
        public class Log
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public int? UserId { get; set; }
        public DateTime EventTime { get; set; } 
        public int? CompanyId { get; set; }
        public int? BuildingObjectId { get; set; }
        public int LogTypeId { get; set; }
        public string Building { get; set; }
        public string Node { get; set; }
        public string EventKey { get; set; }
        public int? TAReportLabelId { get; set; }

        //relations
        [ForeignKey("Id")]
        public  Users Users { get; set; }
        [ForeignKey("Id")]
        public virtual Company Companies { get; set; }
        [ForeignKey("Id")]
        public virtual BuildingObjects BuildingObjects { get; set; }
        [ForeignKey("Id")]
        public virtual LogTypes LogTypes { get; set; }

    }

    //

        //Added by manoranjan
        [Table("CompanieSubCompanies")]
        public class CompanieSubCompanies
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ParentCompanieId { get; set; }
        public bool IsDeleted { get; set; }
    }

        //

        //Added by Manoranjan
        [Table("LogTypes")]
        public class LogTypes
    {
        public int Id { get; set; }
        public int NumberOfError { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public byte[] TimeStamp { get; set; }
        public bool IsDefault { get; set; }
    }

    [Table("TAReports")]
    public class TAReports
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string Name { get; set; }
        public DateTime ReportDate { get; set; }
        public Int16 Day { get; set; }
        public float Hours { get; set; }
        public string Hours_Min { get; set; }
        public int Shift { get; set; }
        public byte Status { get; set; }
        public bool Completed { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Timestamp { get; set; }
        public DateTime ModifiedLast { get; set; }
        public int ModifiedId { get; set; }
        public int? BuildingId { get; set; }

    }

    [Table("TaReportLabels")]
    public class TAReportLabels
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int? PIN { get; set; }
        public Nullable<System.Int16> RegistratorKey { get; set; }
        public Nullable<System.Int16> RegistratorMenuNr { get; set; }
        [ForeignKey("Companies")]
        public int? CompanyId { get; set; }
        public bool? JobNotMove { get; set; }
        public bool Fixed { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool EnteredEvent { get; set; }
        public bool At_work { get; set; }
        public bool InBuilding { get; set; }
        public bool Allow_Jobs { get; set; }
        public bool DaysNotHours { get; set; }
        public int AskStartStopCount { get; set; }
        public string DefaultStart { get; set; }
        public string DefaultStop { get; set; }
        public bool FinishPrevious { get; set; }
        public bool Admin_only { get; set; }
        public int? RegistratorId { get; set; }
        public int? ShiftId { get; set; }
        public int NotFixNextTASeconds { get; set; }
        public byte[] Timestamp { get; set; }
        public DateTime ModifiedLast { get; set; }
        public bool Report { get; set; }
        public Nullable<System.Int16> SaveStatus { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] MenuImageSelectedPng { get; set; }
        public byte[] MenuImageNotSelectedPng { get; set; }

        public virtual Companies Companies { get; set; }
       
    }
    
    [Table("TAShifts")]
    public class TAShifts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? FinishAt { get; set; }
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public bool Changed { get; set; }
        public int? DuratOfBreak { get; set; }
        public int? LateAllowed { get; set; }
        public int? BreakMinInterval { get; set; }
        public int? DuratOfBreakOvertime { get; set; }
        public int? BreakMinIntervalOvertime { get; set; }
        public int? Presence { get; set; }
        public bool? FirstEntryLastExit { get; set; }
        public int? OvertimeStartLater { get; set; }
        public int? OvertimeStartsEarlier { get; set; }
        //relations
        public virtual Companies Company { get; set; }
        public virtual ICollection<TaShiftTimeIntervals> TaShiftTimeIntervals { get; set; }
       

    }

    [Table("TaWeekShift")]
    public class TaWeekShift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CompanyId { get; set; }
        public int? TaUserGroupeShiftsId { get; set; }
        public int? InTaUserGroupeShiftsOrder { get; set; }
        [ForeignKey("TAShifts1")]
        public int? TaShiftIdMonday { get; set; }
        [ForeignKey("TAShifts2")]
        public int? TaShiftIdTuesday { get; set; }
        [ForeignKey("TAShifts3")]
        public int? TaShiftIdWednesday { get; set; }
        [ForeignKey("TAShifts4")]
        public int? TaShiftIdThursday { get; set; }
        [ForeignKey("TAShifts5")]
        public int? TaShiftIdFriday { get; set; }
        [ForeignKey("TAShifts6")]
        public int? TaShiftIdSaturday { get; set; }
        [ForeignKey("TAShifts7")]
        public int? TaShiftIdSunday { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Timestamp { get; set; }

        public virtual TAShifts TAShifts1 { get; set; }
        public virtual TAShifts TAShifts2 { get; set; }
        public virtual TAShifts TAShifts3 { get; set; }
        public virtual TAShifts TAShifts4 { get; set; }
        public virtual TAShifts TAShifts5 { get; set; }
        public virtual TAShifts TAShifts6 { get; set; }
        public virtual TAShifts TAShifts7 { get; set; }
        public virtual TaUserGroupeShifts TaUserGroupeShifts { get; set; }

    }

    [Table("TaUserGroupeShifts")]
    public class TaUserGroupeShifts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RepeatAfterWeeks { get; set; }
        public DateTime StartFromDate { get; set; }
        public int? CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<TaWeekShift> TaWeekShifts  { get; set; }
    }

    [Table("TaUsersShifts")]
    public class TaUsersShifts
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UeserId { get; set; }
        [ForeignKey("TaUserGroupShifts")]
        public int TaUserGroupShiftId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }

        //relations
        public virtual Users User { get; set; }
        public virtual TaUserGroupeShifts TaUserGroupShifts { get; set; }
    }

    [Table("TaShiftTimeIntervals")]
    public class TaShiftTimeIntervals
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [ForeignKey("TAShifts")]
        public int TaShiftId { get; set; }
        [ForeignKey("TAReportLabels")]
        public int TaReportLabelId { get; set; }
       

        //relations
        public virtual TAShifts TAShifts { get; set; }
        public virtual TAReportLabels TAReportLabels { get; set; }
    }
    //
    public class FoxSecDBContext : DbContext
    {
        public FoxSecDBContext() : base("FoxSecDBContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 3600;
            Database.CommandTimeout = 3600;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TABuildingNames>().Property(x => x.Lng).HasPrecision(11, 6);
            modelBuilder.Entity<TABuildingNames>().Property(x => x.Lat).HasPrecision(11, 6);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();// added by manoranjan
           // modelBuilder.Ignore<FoxSec.DomainModel.DomainObjects.Log>(); // added by manoranjan
          
        }

        public DbSet<Companies> Companies { get; set; }
        public DbSet<Departments> Departments { get; set; }

        public DbSet<Users> User { get; set; }

        public DbSet<Visitors> Visitor { get; set; }//Added
        

        public DbSet<UserPermissionGroup> UserPermissionGroups { get; set; } //Added
        public DbSet<UsersAccessUnit> UserAccessUnits { get; set; }
        public DbSet<FSINISettings> FSINISettings { get; set; }
        //public DbSet<CameraIP> CameraIPs { get; set; }
        public DbSet<FSBuildingObjectCameras> FSBuildingObjectCameras { get; set; }
        public DbSet<CamPermission> CamPermissions { get; set; }
        public DbSet<Buildings> Buildings { get; set; }
        public DbSet<TABuildingNames> TABuildingName { get; set; }
        public DbSet<FSCameras> FSCameras { get; set; }
        public DbSet<UserDepartments> UserDeparts { get; set; }
        public DbSet<Custormer> Custmers { get; set; }
        public DbSet<Hr_Clone> HrClones { get; set; }
        public DbSet<TAReport> TAreport { get; set; }
        //public DbSet<TAReports> TANewReoports { get; set; }
        public DbSet<Titles> Title { get; set; }

        // public DbSet<TAMove>
        public DbSet<TAMoves> NewTaMoves { get; set; }
        public DbSet<Roles> UserRoles { get; set; }

        public DbSet<BuildingObjects> BuildingObject { get; set; }

        public DbSet<UserLog> UserLogs { get; set; }
        //public DbSet<Log> NewLogs { get; set; }  //Commented by Manoranjan
        public DbSet<FSHR> FSHR { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<FSVideoServers> FSVideoServers { get; set; }
        public DbSet<FSProjects> FSProjects { get; set; }
        public DbSet<Terminal> _Terminal { get; set; }

        public DbSet<CompanyRoleModel> CompanieRoles { get; set; }  //Added by manoranjan  for new table "CompanieRoles"

        public DbSet<UserLastMoves> UserLastMoves { get; set; }     //Added by manoranjan Date:13July2020 Time: 17:01

        public DbSet<FoxSec.DomainModel.DomainObjects.Log> Log { get; set; }  //Added by manoranjan

        public DbSet<CompanieSubCompanies> CompanieSubCompanies { get; set; } //Added by Manoranjan Date:17July2020 Time: 16:39

        public DbSet<LogTypes> LogTypes { get; set; } //Added by Manoranjan Date:31July2020 Time: 14:48
       
        public DbSet<TAReports> TAReports { get; set; } //Added by Manoranjan Date:8August2020 Time: 20:31

        public DbSet<TAReportLabels> TAReportLabels { get; set; } //Added by Manoranjan Date:29September2020 Time: 13:57

        public DbSet<TAShifts> TAShifts { get; set; } //Added by Manoranjan Date:29September2020 Time: 15:38

        public DbSet<TaWeekShift> TaWeekShifts { get; set; } //Added by Manoranjan Date:06October2020 Time: 15:24

        public DbSet<TaUserGroupeShifts> TaUserGroupeShifts { get; set; } //Added by Manoranjan Date:10October2020 Time: 16:28

        public DbSet<TaUsersShifts> TaUsersShifts { get; set; } //Added by Manoranjan Date:20October2020 Time: 11:53

        public DbSet<TaShiftTimeIntervals> TaShiftTimeIntervals { get; set; } //Added by Manoranjan Date:26October2020 Time: 13:57

    }
}
