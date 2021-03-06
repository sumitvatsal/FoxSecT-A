﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using FoxSec.Authentication;
using FoxSec.Common.SendMail;
using FoxSec.Core.Infrastructure.Bootstrapper;
using FoxSec.Core.Infrastructure.IoC;
using FoxSec.Core.Infrastructure.Localization;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;
using FoxSec.Web.ViewModels;
using System.Management;
using System.IO;
using System.Collections.Generic;
using FoxSec.Web.Controllers;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using FoxSec_Web;
using System.Web.Optimization;
using FoxSec.Web.App_Start;

namespace FoxSec.Web
{
    public class MvcApplication : HttpApplication
    {
        public static string licenseFilePath = string.Empty;
        public static string flashdeviceID = string.Empty;
        public static string harddeviceID = string.Empty;
        public static string hardlicstatus = string.Empty;
        public static string flashlicstatus = string.Empty;
        public static string flashexist = string.Empty;
        public static string flashdrive = string.Empty;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        protected static void OnStart()
        {
            Bootstrapper.Run();
        }

        protected static void OnEnd()
        {
            IoC.Reset();
        }

        protected static void SetUILanguage()
        {
             ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //added by manoranjan
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder(); //added by manoranjan
            var curr_lang = IoC.Resolve<ICurrentLanguage>();
            string cl = curr_lang.Get();

            if (string.IsNullOrWhiteSpace(cl))
            {
                cl = Literals.CULTURE_EN;
                curr_lang.Set(cl);
            }

            CultureInfo ci;

            try
            {
                ci = new CultureInfo(cl);
            }
            catch (ArgumentException)
            {
                curr_lang.Reset();
                curr_lang.Set(Literals.CULTURE_EN);
                ci = new CultureInfo(Literals.CULTURE_EN);
            }

            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }

        protected void OnError()
        {
            const string FORMAT = "{0}\nStack Trace:\n{1}";
            Exception exception = Server.GetLastError();
            var message = new StringBuilder();

            message.AppendFormat(FORMAT, exception.Message, exception.StackTrace);

            for (; exception.InnerException != null;)
            {
                message.Append("\nInner Exception:\n");
                message.AppendFormat(FORMAT, exception.InnerException.Message, exception.InnerException.StackTrace);
                exception = exception.InnerException;
            }

            IoC.Resolve<ILogger>().Write(message.ToString(), FoxSec.Infrastructure.EntLib.Literals.UNHANDLED_EXCEPTION_CATEGORY, TraceEventType.Critical);
        }

        protected void OnPostAuthenticateRequest()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;

            if (!identity.IsAuthenticated)
            {
                return;
            }

            User user = IoC.Resolve<IUserRepository>().FindByLoginName(identity.Name);
            if (user == null)
            {
                return;
            }
            var activeUserRoles = user.UserRoles.Where(x => x.IsDeleted == false && x.ValidFrom < DateTime.Now && x.ValidTo.AddDays(1) > DateTime.Now).ToList();

            int currentRoleId = 0;
            int? currentRoleTypeId = null;
            string currentRoleName = String.Empty;

            if (activeUserRoles.Count != 0)
            {
                currentRoleId = activeUserRoles.First().RoleId;
                currentRoleTypeId = activeUserRoles.First().Role.RoleTypeId;
                currentRoleName = activeUserRoles.First().Role.Name;//IoC.Resolve<IRoleRepository>().FindById(currentRoleId).Name;
            }

            IFoxSecIdentity fox_identity = new FoxSecIdentity(user.Id,
                                                                user.LoginName,
                                                                user.FirstName,
                                                                user.LastName,
                                                                user.Email,
                                                                identity.AuthenticationType,
                                                                user.GetPermissions() == null ? null : user.GetPermissions().AsReadOnly(),
                                                                user.GetMenues() == null ? null : user.GetMenues().AsReadOnly(),
                                                                currentRoleId,
                                                                currentRoleTypeId,
                                                                currentRoleName,
                                                                user.CompanyId,
                                                                Request.UserHostAddress
                                                                );

            Thread.CurrentPrincipal = HttpContext.Current.User = new FoxSecPrincipal(fox_identity);
        }

        protected void Application_Start()
        {
            DevExtremeBundleConfig.RegisterBundles(BundleTable.Bundles);
            FontAwesomeBundleConfig.RegisterBundles();
            OnStart();
            //illi 25.12.1012   Logger4SendingEMail.InitLogger();
            //IoC.Resolve<ICurrentLanguage>().Set("EN");
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            //Database.SetInitializer<FoxSecDBContext>(new DropCreateDatabaseIfModelChanges<FoxSecDBContext>());
            Database.SetInitializer<FoxSecDBContext>(null);
            GetSerFlashDisk();
            if (string.IsNullOrEmpty(flashlicstatus))
            {
                GetSerHardDisk();
                if (string.IsNullOrEmpty(hardlicstatus))
                {
                    CreateLicFile();
                }
            }
            else
            {
                string strpath = Path.Combine(flashdrive + @"\FoxSecLicense.ini");
                con.Open();
                SqlCommand cmd = new SqlCommand("select id from classificatorvalues where Value='Licence Path'", con);
                int? tc = Convert.ToInt32(cmd.ExecuteScalar());
                if (tc != null && tc > 0)
                {
                    SqlCommand cmdi = new SqlCommand("update ClassificatorValues set Comments='" + strpath + "' where id='" + tc + "'", con);
                    cmdi.ExecuteNonQuery();
                }
                con.Close();
            }
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(*) from Classificators where Description='T&A report main company name'", con);
                int tc = Convert.ToInt32(cmd.ExecuteScalar());
                if (tc == 0)
                {
                    SqlCommand cmd1 = new SqlCommand("insert into Classificators(Description)values('T&A report main company name')", con);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("select id from Classificators where Description = 'T&A report main company name'", con);
                    string id = Convert.ToString(cmd2.ExecuteScalar());

                    SqlCommand cmd3 = new SqlCommand("insert into ClassificatorValues(ClassificatorId,Value,SortOrder)values('" + id + "','FoxSec',0)", con);
                    cmd3.ExecuteNonQuery();
                }
                con.Close();
            }
            catch
            {
            }
        }

        protected void Application_End()
        {
            OnEnd();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            SetUILanguage();
        }

        protected void Application_Error()
        {
            OnError();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            OnPostAuthenticateRequest();
        }
        protected void Session_Start(Object sender, EventArgs e)
        { //illi natuke kahtlane security ,võimalus muukida
            Session["init"] = 0;
        }

        public void GetSerFlashDisk()
        {
            string diskName = string.Empty;
            string testser = string.Empty;
            var numint = string.Empty;
            StringBuilder volumename = new StringBuilder(256);
            try
            {
                foreach (ManagementObject drive in new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get())
                {
                    foreach (System.Management.ManagementObject partition in new System.Management.ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                    {
                        foreach (System.Management.ManagementObject disk in new System.Management.ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
                        {
                            diskName = disk["Name"].ToString().Trim();
                            testser = disk["VolumeSerialNumber"].ToString().Trim();
                            numint = Convert.ToInt64(testser, 16).ToString();
                        }
                        flashexist = "1";
                        flashdrive = diskName;
                        licenseFilePath = Path.Combine(diskName + "//FoxSecLicense.ini");
                        try
                        {
                            File.Copy("FoxSecLicense.ini", licenseFilePath);
                        }
                        catch
                        {
                        }
                        if (partition != null)
                        {
                            // associate partitions with logical disks (drive letter volumes)
                            ManagementObject logical = new ManagementObjectSearcher(String.Format(
                                "associators of {{Win32_DiskPartition.DeviceID='{0}'}} where AssocClass = Win32_LogicalDiskToPartition",
                                partition["DeviceID"])).First();

                            if (logical != null)
                            {
                                List<string> list = new List<string>();

                                ManagementObject volume = new ManagementObjectSearcher(String.Format(
                                    "select FreeSpace, Size from Win32_LogicalDisk where Name='{0}'",
                                    logical["Name"])).First();

                                FoxSecLicense.UsbDisk disk = new FoxSecLicense.UsbDisk(logical["Name"].ToString());

                                string pnpdeviceid = CheckLicense.parseSerialFromDeviceID(drive["PNPDeviceID"].ToString().Trim());
                                var conpnp = pnpdeviceid.Substring(0, 5);

                                var conpnpn = CheckLicense.converttoascii(conpnp);
                                var pnpdevidint = Convert.ToUInt64(conpnpn, 16).ToString();

                                list.Add(pnpdevidint.Substring(0, 4));

                                disk.Size = (ulong)volume["Size"];
                                string size = disk.Size.ToString();
                                size = volume["Size"].ToString();
                                list.Add(size.Substring(0, 4));

                                list.Add(numint.Substring(0, 7));

                                string str = "f";
                                string flashser = CheckLicense.Encrypt(str, true);
                                list.Add(flashser);

                                StringBuilder builder = new StringBuilder();
                                foreach (string cat in list)
                                {
                                    builder.Append(cat).Append("");
                                }
                                string result = builder.ToString();
                                flashdeviceID = result;
                            }
                        }
                        if (File.Exists(licenseFilePath))
                        {
                            string compuniqnumber = CheckLicense.ReadCompNr(licenseFilePath);
                            if (flashdeviceID == compuniqnumber)
                            {
                                flashlicstatus = "1";
                                return;
                            }
                            else
                            {
                                flashdeviceID = string.Empty;
                                licenseFilePath = string.Empty;
                                flashlicstatus = string.Empty;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public void GetSerHardDisk()
        {
            try
            {
                string strpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(""), "FoxSecLicense.ini");

                string serial = "";
                List<string> list = new List<string>();
                string model = "";
                ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                long totalSize = 0;
                foreach (ManagementObject wmi_HD in moSearcher.Get())
                {
                    if (wmi_HD.Properties["InterfaceType"].Value.ToString() != "USB")
                    {
                        model = wmi_HD["Model"].ToString();  //Model Number
                        try
                        {
                            serial = wmi_HD.GetPropertyValue("SerialNumber").ToString();
                        }
                        catch
                        {
                            serial = CheckLicense.identifier("Win32_NetworkAdapterConfiguration", "MacAddress");
                        }
                        totalSize += Convert.ToInt64(wmi_HD.Properties["Size"].Value.ToString());
                    }
                }

                byte[] ba = System.Text.Encoding.ASCII.GetBytes(model);
                string ba0 = ba[0].ToString();
                string ba1 = ba[1].ToString();
                string ba2 = ba[2].ToString();

                long intba0 = Convert.ToInt64(ba0) % 10;
                long intba1 = Convert.ToInt64(ba1) % 10;
                long intba2 = Convert.ToInt64(ba2) % 10;
                string intstrba0 = intba0.ToString();
                string intstrba1 = intba1.ToString();
                string intstrba2 = intba2.ToString();

                list.Add(intstrba0);
                list.Add(intstrba1);
                list.Add(intstrba2);

                string name = CheckLicense.identifier("Win32_LogicalDisk", "Name");

                //string Size = identifier("Win32_DiskDrive", "Size");
                string Size = Convert.ToString(totalSize);
                list.Add(Size.Substring(0, 5)); //Jelena Ver67          

                // string serial = identifier("Win32_DiskDrive", "SerialNumber");
                String numint = serial.Substring(0, 6); //Jelena Ver67

                byte[] baser = System.Text.Encoding.ASCII.GetBytes(serial);
                string baser0 = baser[0].ToString();
                string baser1 = baser[1].ToString();
                string baser2 = baser[2].ToString();
                string baser3 = baser[3].ToString();
                string baser4 = baser[4].ToString();
                string baser5 = baser[5].ToString();
                string baser6 = baser[6].ToString();
                // string baser7 = baser[7].ToString();     //Jelena Ver67

                int intbaser0 = Convert.ToInt32(baser0) % 10;
                int intbaser1 = Convert.ToInt32(baser1) % 10;
                int intibaser2 = Convert.ToInt32(baser2) % 10;
                int intbaser3 = Convert.ToInt32(baser3) % 10;
                int intbaser4 = Convert.ToInt32(baser4) % 10;
                int intibaser5 = Convert.ToInt32(baser5) % 10;
                int intbaser6 = Convert.ToInt32(baser6) % 10;
                //int intbaser7 = Convert.ToInt32(baser7) % 10;  //Jelena Ver67

                string intser0 = intbaser0.ToString();
                string intser1 = intbaser1.ToString();
                string intser2 = intibaser2.ToString();
                string intser3 = intbaser3.ToString();
                string intser4 = intbaser4.ToString();
                string intser5 = intibaser5.ToString();
                string intser6 = intbaser6.ToString();
                // string intser7 = intbaser7.ToString();    //Jelena Ver67

                string str = "h";
                string hardser = CheckLicense.Encrypt(str, true);

                list.Add(intser0);
                list.Add(intser1);
                list.Add(intser2);
                list.Add(intser3);
                list.Add(intser4);
                list.Add(intser5);
                list.Add(intser6);

                list.Add(hardser);
                // list.Add(intser7);   //Jelena Ver67

                StringBuilder builder = new StringBuilder();
                foreach (string cat in list) // Loop through all strings
                {
                    builder.Append(cat).Append(""); // Append string to StringBuilder
                }
                string result = builder.ToString();
                harddeviceID = result;
                if (File.Exists(strpath))
                {
                    string compuniqnumber = CheckLicense.ReadCompNr(strpath);
                    if (harddeviceID == compuniqnumber)
                    {
                        licenseFilePath = strpath;
                        hardlicstatus = "1";
                    }
                }
            }
            catch { }
        }

        public void CreateLicFile()
        {
            try
            {
                string strpath = "";
                string uniqnr = "";
                //if (string.IsNullOrEmpty(flashexist))
                //{
                //    uniqnr = harddeviceID;
                //    strpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(""), "FoxSecLicense.ini");
                //}
                //else
                //{
                //    uniqnr = flashdeviceID;
                //    strpath = Path.Combine(flashdrive + "//FoxSecLicense.ini");
                //}

                uniqnr = harddeviceID;
                strpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(""), "FoxSecLicense.ini");

                if (File.Exists(strpath))
                {
                    try
                    {
                        File.Delete(strpath);
                    }
                    catch
                    {
                    }
                }
                using (StreamWriter stream = new StreamWriter(strpath, false))
                {
                    stream.WriteLine("[LicenseCounts]");
                    stream.WriteLine("CompUniqNr=" + uniqnr);
                    stream.WriteLine("CompanieName=");
                    stream.WriteLine("Users=");
                    stream.WriteLine("Doors=");
                    stream.WriteLine("Zones=");
                    stream.WriteLine("Companies=");
                    stream.WriteLine("TimeAndAttendense=");
                    stream.WriteLine("Terminals=");
                    stream.WriteLine("SmartPhoneRegistrators=");
                    stream.WriteLine("Video=");
                    stream.WriteLine("Visitors=");
                    stream.WriteLine("License Number=");
                    stream.WriteLine("ValidTo=");
                    stream.Close();
                }

                con.Open();
                SqlCommand cmd = new SqlCommand("select id from classificatorvalues where Value='Licence Path'", con);
                int? tc = Convert.ToInt32(cmd.ExecuteScalar());
                if (tc != null && tc > 0)
                {
                    SqlCommand cmdi = new SqlCommand("update ClassificatorValues set Comments='" + strpath + "' where id='" + tc + "'", con);
                    cmdi.ExecuteNonQuery();

                    SqlCommand cmd1 = new SqlCommand("update ClassificatorValues set Legal=null,LegalHash=null,Remaining=null,RemainingHash=null,ValidTo=null,ValidToHash=null where ClassificatorId=(select Id from Classificators where Description='License')", con);
                    cmd1.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }
    }
}