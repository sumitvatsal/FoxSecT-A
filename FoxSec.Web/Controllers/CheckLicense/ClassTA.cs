using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoxSecLicense
{
    public class readlicenseclass
    {
        public static string licenseFilePath = "";
        public string diskName = string.Empty;
        public static string deviceID = string.Empty;
        public static string deviceIDflash = string.Empty;
        public const string ENCRYPTION_KEY = "A456E4DA104F960563A66DDC";
        public string Readlicense(string parameter, string filepath,string encrpytkey)
        {
            licenseFilePath = filepath;
            Int32 i = default(Int32);
            Int32 intUniqNrIdx = default(Int32);
            Int32 intEncryptLicNrIdx = default(Int32);
            string strUniqNr = string.Empty;
            string strEncryptLicNr = string.Empty;
            string decryptedLicense = string.Empty;
            string line = string.Empty;
            // string sername;
            string[] lines;
            string data = string.Empty;
            string datacomnr = string.Empty;
            string datauser = string.Empty;
            string datadoor = string.Empty;
            string datazones = string.Empty;
            string datacompanies = string.Empty;
            string dataworktime = string.Empty;
            string dataportal = string.Empty;
            string datavideo = string.Empty;
            string datavisitor = string.Empty;

            string datauserhash = string.Empty;
            string datadoorhash = string.Empty;
            string datazoneshash = string.Empty;
            string datacompanieshash = string.Empty;
            string dataworktimehash = string.Empty;
            string dataportalhash = string.Empty;
            string datavideohash = string.Empty;
            string datavisitorhash = string.Empty;

            string datacompnnr = string.Empty;
            string datacompnrhash = string.Empty;

            string remaininghashcode = "";
            lines = FileReader.ReadLines(licenseFilePath);
            for (i = 0; i < lines.Length; i++)
            {
                line = lines[i];
                if (line.Contains("[CountRequest]"))
                {
                    break;
                }
                else
                {
                    //   TAtest.Readlicense();
                    if (line.Length > 15)
                    {
                        if (line.Contains("CompUniqNr"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string datacomnr1 = Searcher.TrimEnd(line, "CompUniqNr=");

                            if (deviceIDflash == string.Empty)
                            {
                                string linedata = Searcher.TrimEnd(line, "CompUniqNr=");
                                datacompnnr = "";
                                datacompnrhash = linedata;
                            }
                        }
                        else
                        if (line.Contains("Users"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "Users=");
                            datauser = Encryption.Decrypt(linedata, true, encrpytkey);
                            datauserhash = linedata;
                        }
                        else
                        if (line.Contains("Doors"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "Doors=");

                            datadoor = Encryption.Decrypt(linedata, true, encrpytkey);
                            datadoorhash = linedata;
                        }
                        else
                        if (line.Contains("Zones"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "Zones=");

                            datazones = Encryption.Decrypt(linedata, true, encrpytkey);
                            datazoneshash = linedata;
                        }
                        else
                        if (line.Contains("Companies"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "Companies=");

                            datacompanies = Encryption.Decrypt(linedata, true, encrpytkey);
                            datacompanieshash = linedata;
                        }
                        else
                        if (line.Contains("TimeAndAttendense"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "TimeAndAttendense=");

                            dataworktime = Encryption.Decrypt(linedata, true, encrpytkey);
                            dataworktimehash = linedata;
                        }
                        else
                        if (line.Contains("Terminals"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "Terminals=");

                            dataportal = Encryption.Decrypt(linedata, true, encrpytkey);
                            dataportalhash = linedata;
                        }
                        else
                        if (line.Contains("Video"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "Video=");

                            datavideo = Encryption.Decrypt(linedata, true, encrpytkey);
                            datavideohash = linedata;
                        }
                        else
                        if (line.Contains("Visitors"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string linedata = Searcher.TrimEnd(line, "Visitors=");

                            datavisitor = Encryption.Decrypt(linedata, true, encrpytkey);
                            datavisitorhash = linedata;
                        }
                        else
                        if (line.Contains("ValidTo"))
                        {
                            string returndata = Searcher.TrimEnd(line, "ValidTo=");
                            //string data1 = Encryption.Decrypt(data, true);
                        }
                    }
                    if (intEncryptLicNrIdx > 0 & intUniqNrIdx > 0)
                        break;
                }
            }

            int Readlicensevale = -1;

            string counter = "0";

            switch (parameter)
            {
                case ("CompUniqNr"):
                    if (datacomnr == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(datacomnr.Substring(0, 5), "CompUniqNr", true) == 0)
                        {
                            counter = "1";
                            remaininghashcode = datacompnrhash;
                        }
                    }
                    break;
                case "Users":
                    if (datauser == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(datauser.Substring(0, 5), "users", true) == 0)
                        {
                            counter = Searcher.TrimEnd(datauser, "users");
                            remaininghashcode = datauserhash;
                        }
                        else
                        {
                            counter = datauser;
                        }
                    }
                    break;
                case "Doors":
                    if (datadoor == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(datadoor.Substring(0, 5), "doors", true) == 0)
                        {
                            //Encryption.Decrypt(datauser, true);
                            counter = Searcher.TrimEnd(datadoor, "doors");
                            remaininghashcode = datadoorhash;
                        }
                    }
                    //  Console.Write(datadoor);
                    break;
                case "Zones":
                    if (datazones == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(datazones.Substring(0, 5), "zones", true) == 0)
                        {
                            //Encryption.Decrypt(datauser, true);
                            counter = Searcher.TrimEnd(datazones, "zones");
                            remaininghashcode = datazoneshash;
                        }
                    }
                    //    Console.Write(datazones);
                    break;
                case "Companies":
                    if (datacompanies == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        counter = datacompanies;
                        remaininghashcode = datacompanieshash;
                    }
                    break;
                case "TimeAndAttendense":
                    if (dataworktime == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(dataworktime.Substring(0, 17), "timeandattendense", true) == 0)
                        {
                            counter = Searcher.TrimEnd(dataworktime, "timeandattendense");
                            remaininghashcode = dataworktimehash;
                        }
                    }

                    break;
                case "Terminals":
                    if (dataportal == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(dataportal.Substring(0, 9), "terminals", true) == 0)
                        {
                            counter = Searcher.TrimEnd(dataportal, "terminals");
                            remaininghashcode = dataportalhash;
                        }
                    }

                    break;
                case "Video":
                    if (datavideo == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(datavideo.Substring(0, 5), "video", true) == 0)
                        {
                            counter = Searcher.TrimEnd(datavideo, "video");
                            remaininghashcode = datavideohash;
                        }
                    }
                    break;

                case "Visitors":
                    if (datavisitor == "")
                    {
                        counter = "";
                        remaininghashcode = "";
                    }
                    else
                    {
                        if (string.Compare(datavisitor.Substring(0, 8), "visitors", true) == 0)
                        {
                            counter = Searcher.TrimEnd(datavisitor, "visitors");
                            remaininghashcode = datavisitorhash;
                        }
                    }

                    break;
            }

            if (counter != "0")
            {
                if (Regex.IsMatch(counter, @"^\d+$"))
                {
                    int result = Convert.ToInt32(counter);
                    return result + "_" + remaininghashcode;
                }
                else
                    return Convert.ToString(Readlicensevale);
            }
            else
                return Convert.ToString(Readlicensevale);
        }

        public string Readlicensenr(string licenseFilePath)
        {
            string datacompnrhash = string.Empty;
            string line = string.Empty;
            string[] lines = FileReader.ReadLines(licenseFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i];
                if (line.Contains("[CountRequest]"))
                {
                    break;
                }
                else
                {
                    //   TAtest.Readlicense();
                    if (line.Length > 15)
                    {

                        if (line.Contains("CompUniqNr"))
                        {
                            // intEncryptLicNrIdx = i;
                            //strEncryptLicNr = line.Substring(17, line.Length - 17);
                            string datacomnr1 = Searcher.TrimEnd(line, "CompUniqNr=");

                            if (deviceIDflash == string.Empty)
                            {
                                string linedata = Searcher.TrimEnd(line, "CompUniqNr=");
                                datacompnrhash = linedata;
                            }
                            break;
                        }
                    }
                }
            }
            return datacompnrhash;
        }

        public string ReadlicenseValidTo(string parameter, string filepath,string encryptkey)
        {
            Int32 i = default(Int32);
            //Int32 intUniqNrIdx = default(Int32);
            //Int32 intEncryptLicNrIdx = default(Int32);
            licenseFilePath = filepath;
            string line = string.Empty;
            // string sername;
            string[] lines;
            string datavalidto = null;
            string linedata = "";
            lines = FileReader.ReadLines(licenseFilePath);
            int flg = 0;
            for (i = 0; i < lines.Length; i++)
            {
                line = lines[i];
                if (line.Contains("[CountRequest]"))
                {
                    break;
                }
                else
                {
                    //   TAtest.Readlicense();
                    if (line.Length > 15)
                    {
                        if (line.Contains("ValidTo"))
                        {
                            linedata = Searcher.TrimEnd(line, "ValidTo=");
                            datavalidto = Encryption.Decrypt(linedata, true, encryptkey);
                            flg = flg + 1;
                        }
                    }
                }
            }
            if (flg == 0)
            {
                return "";
            }
            else
            {
                return datavalidto + "_" + linedata;
            }
        }

        public string ReadCompNr(string filepath)
        {
            Int32 i = default(Int32);
            //Int32 intUniqNrIdx = default(Int32);
            //Int32 intEncryptLicNrIdx = default(Int32);
            licenseFilePath = filepath;
            string line = string.Empty;
            // string sername;
            string[] lines;
            string linedata = "";
            lines = FileReader.ReadLines(licenseFilePath);
            for (i = 0; i < lines.Length; i++)
            {
                line = lines[i];
                if (line.Contains("[CountRequest]"))
                {
                    break;
                }
                else
                {
                    if (line.Length > 15)
                    {
                        if (line.Contains("CompUniqNr"))
                        {
                            linedata = Searcher.TrimEnd(line, "CompUniqNr=");
                        }
                    }
                }
            }
            
            return linedata;
        }
    }
}
