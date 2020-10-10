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
using System.Web.Hosting;
using System.Configuration;
using System.Security.Cryptography;

namespace FoxSec.Web.Controllers
{
    public static class Searcher
    {
        public static ManagementObject First(this ManagementObjectSearcher searcher)
        {
            ManagementObject result = null;
            foreach (ManagementObject item in searcher.Get())
            {
                result = item;
                break;
            }
            return result;
        }
        public static string TrimEnd(this string input, string suffixToRemove)
        {
            while (input.StartsWith(suffixToRemove))
            {
                input = input.Substring(suffixToRemove.Length);
            }
            return input;
        }
    }
    public class CheckLicense
    {
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
        private static string appPath = string.Empty;
        private static string error = string.Empty;
        public const string ENCRYPTION_KEY = "A456E4DA104F960563A66DDC";
        public static int flgFlash = 0;
        public static string flgFlashDrive = "";
        public static int flgHard = 0;

        public string checkusb(string path)
        {
            string chk = "0";
            string disknr = deviceIDflash;

            licenseFilePath = path;

            FileInfo sFile = new FileInfo(licenseFilePath);
            bool fileExist = sFile.Exists;

            if (fileExist == true)
            {
                string licencedevid = TAtest.Readlicensenr(licenseFilePath);

                GetSerFlashDisk(licencedevid);
                if (flgFlash == 0)
                {
                    GetSerHardDisk(licencedevid);
                }

                if (String.IsNullOrEmpty(licencedevid))
                {
                    chk = "0";//invalid licence
                }
                else if (flgFlash == 0 && flgHard == 0)
                {
                    chk = "0";//invalid licence
                }
                else if (flgFlash == 1)
                {
                    chk = flgFlashDrive;//valid licence
                }
                else if (flgHard == 1)
                {
                    chk = "2";//valid licence                      
                }
                else
                {
                    chk = "0";//invalid licence
                }
            }
            return Convert.ToString(chk);
        }

        //Return a hardware identifier
        public static string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        public void GetSerFlashDisk(string licencedevid)
        {
            flgFlash = 0;
            string diskName = string.Empty;
            string testser = string.Empty;
            var numint = string.Empty;
            StringBuilder volumename = new StringBuilder(256);
            deviceIDflash = string.Empty;
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

                                    UsbDisk diskn = new UsbDisk(logical["Name"].ToString());

                                    string pnpdeviceid = parseSerialFromDeviceID(drive["PNPDeviceID"].ToString().Trim());
                                    var conpnp = pnpdeviceid.Substring(0, 5);

                                    var conpnpn = converttoascii(conpnp);
                                    var pnpdevidint = Convert.ToUInt64(conpnpn, 16).ToString();

                                    list.Add(pnpdevidint.Substring(0, 4));

                                    diskn.Size = (ulong)volume["Size"];
                                    string size = diskn.ToString();
                                    size = volume["Size"].ToString();
                                    list.Add(size.Substring(0, 4));

                                    list.Add(numint.Substring(0, 7));

                                    string str = "f";
                                    string flashser = Encrypt(str, true);
                                    list.Add(flashser);

                                    StringBuilder builder = new StringBuilder();
                                    foreach (string cat in list)
                                    {
                                        builder.Append(cat).Append("");
                                    }
                                    string result = builder.ToString();
                                    deviceIDflash = result;
                                    if (licencedevid == deviceIDflash)
                                    {
                                        flgFlash = 1;
                                        flgFlashDrive = diskName;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            //   string key = (string)settingsReader.GetValue(ENCRYPTION_KEY, typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ENCRYPTION_KEY));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(ENCRYPTION_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string converttoascii(string text)
        {
            string asciitxt = "";
            for (var i = 0; i < text.Length; i++)
            {
                char current = text[i];
                if (!(Char.IsDigit(current)))
                {
                    asciitxt = asciitxt + Convert.ToString((int)(current));
                }
                else
                {
                    asciitxt = asciitxt + current;
                }
            }
            return asciitxt;
        }
        public void GetSerHardDisk(string licencedevid)
        {
            try
            {
                flgHard = 0;
                deviceID = string.Empty;
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
                            serial = identifier("Win32_NetworkAdapterConfiguration", "MacAddress");
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

                string name = identifier("Win32_LogicalDisk", "Name");
                string Size = Convert.ToString(totalSize);
                list.Add(Size.Substring(0, 5)); //Jelena Ver67          

                String numint = serial.Substring(0, 6); //Jelena Ver67

                byte[] baser = System.Text.Encoding.ASCII.GetBytes(serial);
                string baser0 = baser[0].ToString();
                string baser1 = baser[1].ToString();
                string baser2 = baser[2].ToString();
                string baser3 = baser[3].ToString();
                string baser4 = baser[4].ToString();
                string baser5 = baser[5].ToString();
                string baser6 = baser[6].ToString();

                int intbaser0 = Convert.ToInt32(baser0) % 10;
                int intbaser1 = Convert.ToInt32(baser1) % 10;
                int intibaser2 = Convert.ToInt32(baser2) % 10;
                int intbaser3 = Convert.ToInt32(baser3) % 10;
                int intbaser4 = Convert.ToInt32(baser4) % 10;
                int intibaser5 = Convert.ToInt32(baser5) % 10;
                int intbaser6 = Convert.ToInt32(baser6) % 10;

                string intser0 = intbaser0.ToString();
                string intser1 = intbaser1.ToString();
                string intser2 = intibaser2.ToString();
                string intser3 = intbaser3.ToString();
                string intser4 = intbaser4.ToString();
                string intser5 = intibaser5.ToString();
                string intser6 = intbaser6.ToString();

                string str = "h";
                string hardser = Encrypt(str, true);

                list.Add(intser0);
                list.Add(intser1);
                list.Add(intser2);
                list.Add(intser3);
                list.Add(intser4);
                list.Add(intser5);
                list.Add(intser6);
                list.Add(hardser);

                StringBuilder builder = new StringBuilder();
                foreach (string cat in list) // Loop through all strings
                {
                    builder.Append(cat).Append(""); // Append string to StringBuilder
                }
                string result = builder.ToString();

                deviceID = result;
                if (deviceID == licencedevid)
                {
                    flgHard = 1;
                }
            }
            catch (Exception ex) { error = ex.ToString(); }
        }

        public static string parseSerialFromDeviceID(string deviceId)
        {
            string[] splitDeviceId = deviceId.Split('\\');
            string[] serialArray; string serial;
            int arrayLen = splitDeviceId.Length - 1;
            serialArray = splitDeviceId[arrayLen].Split('&');
            serial = serialArray[0]; return serial;
        }

        public static string ReadCompNr(string licenseFilePath)
        {
            Int32 i = default(Int32);
            string strUniqNr = string.Empty;
            string strEncryptLicNr = string.Empty;
            string decryptedLicense = string.Empty;
            string line = string.Empty;
            // string sername;
            string datavalidto = string.Empty;
            string cmpnr = string.Empty;
            string[] lines = FileReader.ReadLines(licenseFilePath);

            for (i = 0; i < lines.Length; i++)
            {
                line = lines[i];
                if (line.Contains("[CountRequest]"))
                {
                    break;
                }
                else
                {
                    if (line.Length > 1)
                    {
                        if (line.Contains("CompUniqNr"))
                        {
                            string returndata = Searcher.TrimEnd(line, "CompUniqNr=");
                            cmpnr = returndata;
                        }
                    }
                }
            }
            return cmpnr;
        }
    }
}