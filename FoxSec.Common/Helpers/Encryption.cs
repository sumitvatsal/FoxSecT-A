using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FoxSec.Common.Helpers
{
	public static class Encryption
	{
		public const string ENCRYPTION_KEY = "A456E4DA104F960563A66DDC";

		public static string Encrypt(string input)
		{
			byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = UTF8Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = tripleDES.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
			tripleDES.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		public static string Decrypt(string input)
		{
			byte[] inputArray = Convert.FromBase64String(input);
			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = UTF8Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = tripleDES.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
			tripleDES.Clear();
			return UTF8Encoding.UTF8.GetString(resultArray);
		}
	}
}
