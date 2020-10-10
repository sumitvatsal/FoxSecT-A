using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSecLicense
{
    public class FileReader
    {
        private const string NEW_LINE = "\r\n";

        //internal const string LICENSE_INI = "FoxSecLicense.ini";
        //internal const string SERVICE_NAME =
        //    "\r\nServiceName={0}\r\n";
        //internal const string DEMO_LICENSE_TEMPLATE =
        //    "[CountUsers]\r\nCompUniqNr={0}\r\n";
        //internal const string COUNT_CountUsers =
        //    "\r\nCountCountUsers={0}\r\n";
        //internal const string COUNT_CountDoors =
        //    "\r\nCountDoors={0}\r\n";
        //internal const string DEMO =
        //    "\r\nDeviceId={0}\r\n";
        //List<string> list = new List<string>();
        internal static String[] ReadLines(string path)
        {
            string[] lines = new string[0];
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, Encoding.UTF8, true);
                string result = sr.ReadToEnd();
                sr.Close();
                lines = result.Replace(NEW_LINE, "\n").Split('\n');

            }
            return lines;

        }
    }
}
