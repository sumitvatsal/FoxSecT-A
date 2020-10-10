using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FoxSecLicense
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
}
