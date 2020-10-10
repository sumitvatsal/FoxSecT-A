using System;
using System.Security.Cryptography;
using System.Text;

namespace FoxSec.Common.Helpers
{
    public static class EncodePassword
	{
        public static string ToMD5(string password)
        {
            byte[] original_bytes = System.Text.Encoding.Unicode.GetBytes(password);
            byte[] encoded_bytes = new MD5CryptoServiceProvider().ComputeHash(original_bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < encoded_bytes.Length; i++)
            {
                result.Append(encoded_bytes[i].ToString("x2"));
            }
            return result.ToString().ToUpper();
        }
	}
}
