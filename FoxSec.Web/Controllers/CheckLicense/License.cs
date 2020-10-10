using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FoxSec.Web.Controllers;
using System.Management;
using Microsoft.Win32;
using FoxSecLicense;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FoxSec.Infrastructure.EF.Repositories.Interfaces;
using System.Security.Cryptography;

namespace FoxSec.Web.Controllers
{
    public class License : BusinessCaseController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        FoxSecDBContext db = new FoxSecDBContext();
        private readonly IClassificatorRepository _classificatorRepository;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IClassificatorService _classificatorService;
        private readonly IUserRepository _userRepository;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IVisitorService _visitorService;
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IVisitorRepository _visitorRepository;

        public License(
         IClassificatorRepository classificatorRepository,
         IClassificatorValueRepository classificatorValueRepository,
         IClassificatorService classificatorService,
         IVisitorService visitorService,
         IUserRepository userRepository,
         IUserTimeZoneRepository userTimeZoneRepository,
         ICurrentUser currentUser,
         IBuildingRepository buildingRepository,
        IBuildingObjectRepository buildingObjectRepository,
        IVisitorRepository visitorRepository,
        ILogger logger)
         : base(currentUser, logger)
        {
            _classificatorRepository = classificatorRepository;
            _classificatorValueRepository = classificatorValueRepository;
            _classificatorService = classificatorService;
            _userTimeZoneRepository = userTimeZoneRepository;
            _userRepository = userRepository;
            _visitorService = visitorService;
            _buildingObjectRepository = buildingObjectRepository;
            _buildingRepository = buildingRepository;
            _visitorRepository = visitorRepository;
        }

        public static int Users;
        public static int Companies;
        public static int Doors;
        public static int Zones;
        public static int TimeAndAttendense;
        public static int Visitors;
        public static int Video;
        public static int Terminals;
        public static string ValidTo;

        public static int LessFlag = 0;
        public static string datacomnr;
        public static string rTime = string.Empty;
        public readlicenseclass TAtest = new readlicenseclass();
        public static bool appIsLocked = true;
        public static string deviceID = string.Empty;
        public static string deviceIDflash = string.Empty;
        private const RegexOptions regexOptions =
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture;
        private const string dateRegexp = "(19|20)\\d\\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])";
        private static readonly Regex dateRegex =
        new Regex(dateRegexp, regexOptions);
        private const string digitsRegexPattern = @"^[0-9]+$";
        private static readonly Regex digitsRegex = new Regex(digitsRegexPattern, regexOptions);
        private static readonly string CR = Environment.NewLine;
        public static string serialnember = string.Empty;
        public static string licenseFilePath = "";
        public string diskName = string.Empty;
        public string confirmPassword = "hard12";
        private static string appPath = string.Empty;
        private static string error = string.Empty;
        public const string ENCRYPTION_KEY = "A456E4DA104F960563A66DDC";
        public static string Licensenumber;
        internal const string LICENSE_SERNUM =
       "[CountUsers]\r\nCompUniqNr={0}";
        private static object l = new object();
        internal const string SERVICE_NAME =
           "ServiceName={0}";
        internal const string COUNT_CountUsers =
           "Users={0}";
        internal const string COUNT_CountDoors =
           "Doors={0}";
        internal const string ZONES =
           "Zones={0}";
        internal const string COMPANIES =
           "Companies={0}";
        internal const string WORKTIME =
           "TimeAndAttendense={0}";
        internal const string VALIDTO =
           "ValidTo={0}";
        internal const string PORTAL =
           "Terminals={0}";
        internal const string VIDEO =
           "Video={0}";
        internal const string VISITORS =
          "Visitors={0}";

        string NEW_LINE = System.Environment.NewLine;
        //string comnr = "CompUniqNr";
        string user = "Users";
        string door = "Doors";
        string zones = "Zones";
        string companies = "Companies";
        string worktime = "TimeAndAttendense";
        string portal = "Terminals";
        string video = "Video";
        string visitor = "Visitors";
        string validTo = "ValidTo";
        public void chklicence(int id, string HostName, string filepath, string licencepath)
        {
            user = "Users";
            door = "Doors";
            zones = "Zones";
            companies = "Companies";
            worktime = "TimeAndAttendense";
            portal = "Terminals";
            video = "Video";
            visitor = "Visitors";
            validTo = "ValidTo";

            FileInfo sFile = new FileInfo(filepath);
            bool fileExist = sFile.Exists;

            if (fileExist == true)
            {
                int tc = 0;
                string compuniqnr = TAtest.ReadCompNr(filepath);
                string uniqkey = ENCRYPTION_KEY + compuniqnr;
                ValidTo = TAtest.ReadlicenseValidTo(validTo, filepath, uniqkey);
                string Usersrec = TAtest.Readlicense(user, filepath, uniqkey);
                if (Usersrec != "-1")
                {
                    int remaining = gettotalcount(user, Convert.ToInt32(Usersrec.Split('_')[0]));
                    _classificatorService.InsertNewLicense(user, Convert.ToInt32(Usersrec.Split('_')[0]), Usersrec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                string Doorsrec = TAtest.Readlicense(door, filepath, uniqkey);
                if (Doorsrec != "-1")
                {
                    int remaining = gettotalcount(door, Convert.ToInt32(Doorsrec.Split('_')[0]));
                    _classificatorService.InsertNewLicense(door, Convert.ToInt32(Doorsrec.Split('_')[0]), Doorsrec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                string Zonesrec = TAtest.Readlicense(zones, filepath, uniqkey);
                if (Zonesrec != "-1")
                {
                    int remaining = gettotalcount(zones, Convert.ToInt32(Zonesrec.Split('_')[0]));
                    _classificatorService.InsertNewLicense(zones, Convert.ToInt32(Zonesrec.Split('_')[0]), Zonesrec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                string Companiesrec = TAtest.Readlicense(companies, filepath, uniqkey);
                if (Companiesrec != "-1")
                {
                    int remaining = gettotalcount(companies, Convert.ToInt32(Companiesrec.Split('_')[0]));
                    _classificatorService.InsertNewLicense(companies, Convert.ToInt32(Companiesrec.Split('_')[0]), Companiesrec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                string TimeAndAttendenserec = TAtest.Readlicense(worktime, filepath, uniqkey);
                if (TimeAndAttendenserec != "-1")
                {
                    int remaining = gettotalcount(worktime, Convert.ToInt32(TimeAndAttendenserec.Split('_')[0]));
                    _classificatorService.InsertNewLicense("Time&attendense", Convert.ToInt32(TimeAndAttendenserec.Split('_')[0]), TimeAndAttendenserec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                string Terminalsrec = TAtest.Readlicense(portal, filepath, uniqkey);
                if (Terminalsrec != "-1")
                {
                    int remaining = gettotalcount(portal, Convert.ToInt32(Terminalsrec.Split('_')[0]));
                    _classificatorService.InsertNewLicense(portal, Convert.ToInt32(Terminalsrec.Split('_')[0]), Terminalsrec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                string Videorec = TAtest.Readlicense(video, filepath, uniqkey);
                if (Videorec != "-1")
                {
                    int remaining = gettotalcount(video, Convert.ToInt32(Videorec.Split('_')[0]));
                    _classificatorService.InsertNewLicense(video, Convert.ToInt32(Videorec.Split('_')[0]), Videorec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                string Visitorsrec = TAtest.Readlicense(visitor, filepath, uniqkey);
                if (Visitorsrec != "-1")
                {
                    int remaining = gettotalcount(visitor, Convert.ToInt32(Visitorsrec.Split('_')[0]));
                    _classificatorService.InsertNewLicense(visitor, Convert.ToInt32(Visitorsrec.Split('_')[0]), Visitorsrec.Split('_')[1], id, HostName, ValidTo, remaining, uniqkey);
                    tc = tc + 1;
                }
                if (!String.IsNullOrEmpty(filepath))
                {
                    _classificatorService.InsertNewLicense("Licence Path", 0, licencepath, id, "", "", 0, uniqkey);
                }
                if (tc == 0 && !String.IsNullOrEmpty(ValidTo))
                {
                    _classificatorService.UpdateLicenseValidTo(id, HostName, ValidTo);
                }
                try
                {
                    System.IO.File.Delete(licenseFilePath);
                }
                catch
                {
                }
                try
                {
                    System.IO.File.Delete(filepath);
                }
                catch
                {
                }
            }
        }

        public int CheckLicenseLessValidation(int id, string HostName, string filepath, string licencepath)
        {
            LessFlag = 0;
            user = "Users";
            door = "Doors";
            zones = "Zones";
            companies = "Companies";
            worktime = "TimeAndAttendense";
            portal = "Terminals";
            video = "Video";
            visitor = "Visitors";
            validTo = "ValidTo";

            string compuniqnr = TAtest.ReadCompNr(filepath);
            string uniqkey = ENCRYPTION_KEY + compuniqnr;

            string Usersrec = TAtest.Readlicense(user, filepath, uniqkey);
            if (Usersrec != "-1")
            {
                int remaining = gettotalcount(user, Convert.ToInt32(Usersrec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation(user, Convert.ToInt32(Usersrec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            string Doorsrec = TAtest.Readlicense(door, filepath, uniqkey);
            if (Doorsrec != "-1")
            {
                int remaining = gettotalcount(door, Convert.ToInt32(Doorsrec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation(door, Convert.ToInt32(Doorsrec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            string Zonesrec = TAtest.Readlicense(zones, filepath, uniqkey);
            if (Zonesrec != "-1")
            {
                int remaining = gettotalcount(zones, Convert.ToInt32(Zonesrec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation(zones, Convert.ToInt32(Zonesrec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            string Companiesrec = TAtest.Readlicense(companies, filepath, uniqkey);
            if (Companiesrec != "-1")
            {
                int remaining = gettotalcount(companies, Convert.ToInt32(Companiesrec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation(companies, Convert.ToInt32(Companiesrec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            string TimeAndAttendenserec = TAtest.Readlicense(worktime, filepath, uniqkey);
            if (TimeAndAttendenserec != "-1")
            {
                int remaining = gettotalcount(worktime, Convert.ToInt32(TimeAndAttendenserec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation("Time&attendense", Convert.ToInt32(TimeAndAttendenserec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            string Terminalsrec = TAtest.Readlicense(portal, filepath, uniqkey);
            if (Terminalsrec != "-1")
            {
                int remaining = gettotalcount(portal, Convert.ToInt32(Terminalsrec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation(portal, Convert.ToInt32(Terminalsrec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            string Videorec = TAtest.Readlicense(video, filepath, uniqkey);
            if (Videorec != "-1")
            {
                int remaining = gettotalcount(video, Convert.ToInt32(Videorec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation(video, Convert.ToInt32(Videorec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            string Visitorsrec = TAtest.Readlicense(visitor, filepath, uniqkey);
            if (Visitorsrec != "-1")
            {
                int remaining = gettotalcount(visitor, Convert.ToInt32(Visitorsrec.Split('_')[0]));
                int vc = _classificatorService.CheckLicenseLessValidation(visitor, Convert.ToInt32(Visitorsrec.Split('_')[0]));
                if (vc == 1)
                {
                    LessFlag = 1;
                }
            }
            return LessFlag;
        }

        public int gettotalcount(string type, int legal)
        {
            int tc = 0;
            int tc1 = 0;
            if (type == "Users")
            {
                tc1 = _userRepository.FindAll(x => !x.IsDeleted && x.Active == true).ToList().Count();
            }
            if (type == "Doors")
            {
                tc1 = _buildingObjectRepository.FindAll(x => !x.IsDeleted && (x.TypeId == 8 || x.TypeId == 9)).ToList().Count();
            }
            if (type == "Visitors")
            {
                //tc1 = _userRepository.FindAll(x => !x.IsDeleted && x.Active == true && (x.IsShortTermVisitor == true || x.IsVisitor == true)).ToList().Count();
                tc1 = _visitorRepository.FindAll(x => !x.IsDeleted && x.StopDateTime > DateTime.Now).ToList().Count();
            }
            if (type == "Video")
            {
                tc1 = db.FSCameras.Where(x => x.Deleted == false && x.Port > 0).Select(x => x.Id).ToList().Count();
            }
            if (type == "Terminals")
            {
                //tc1 = db._Terminal.Where(x => x.IsDeleted == false && x.ApprovedDevice == true).Select(x => x.Id).ToList().Count();
                tc1 = db.Database.SqlQuery<Terminal>("Select * from Terminal where IsDeleted=0 and ApprovedDevice=1").ToList().Count();
            }
            if (type == "Zones")
            {
                tc1 = _buildingObjectRepository.FindAll(x => !x.IsDeleted && x.TypeId == 10).ToList().Count();
            }
            if (type == "TimeAndAttendense")
            {
                tc1 = db.TABuildingName.Where(x => !x.IsDeleted).ToList().Count();
            }
            if (type == "Companies")
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(distinct CompanyId) from users where IsDeleted=0 and CompanyId is not null and id in (select UserId from uSERROLES where IsDeleted=0 and RoleId in (select id from Roles where IsDeleted=0 and RoleTypeId=3))", con);
                tc1 = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
            tc = legal - tc1;
            tc = tc < 0 ? 0 : tc;
            return tc;
        }
    }
}