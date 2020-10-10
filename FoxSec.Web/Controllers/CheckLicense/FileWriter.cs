using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSecLicense
{
   public class FileWriter
    {
        private static object l = new object();
        public static string projectsPath ="";

        public static void UpdateEventsLogFile(string arg)
        {
            lock (l)
            {
                if (arg == "00:00:00  0")
                {
                    // Beep()
                }
                string path = VerifyEventsLogFilePath();
                bool append = true;
                WriteToFile(path, arg, append);
            }
        }

        static internal string VerifyEventsLogFilePath()
        {
            string logsFileName = string.Format("Events_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
            string path = VerifyPath(projectsPath, logsFileName);
            return path;
        }

        static internal string VerifyPath(string path1, string file1)
        {
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }

            path1 = Path.Combine(path1, file1);

            if (file1 != string.Empty)
            {
                if (!File.Exists(path1))
                {
                    CreateFile(path1);
                }
            }
            return path1;
        }
        static internal void CreateFile(string path)
        {
            StreamWriter fs = File.CreateText(path);
            fs.Close();
        }
        static internal void WriteToFile(string path, string arg, bool append)
        {
            lock (l)
            {
                StreamWriter sw = new StreamWriter(path, append);
                sw.WriteLine(arg);
                sw.Close();
            }
        }
        //public void INIWrite(string INIPath, string SectionName, string KeyName, string TheValue)
        //{
        //    WritePrivateProfileString(SectionName, KeyName, TheValue, INIPath);
        //}
    }
}
