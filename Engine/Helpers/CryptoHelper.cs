using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Helpers
{
	public static class CryptoHelper
	{
		private const string STR = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz`1234567890-=~!@#$%^&*()_+";
		private static byte _counter = 0;
		private const byte MINLENGTH = 16;
		public static string Generate(int totalLength = MINLENGTH)
		{
			if (totalLength < MINLENGTH) totalLength = MINLENGTH;
			byte[] bytes = new byte[16];
			_counter++;
			var dt = DateTime.Now;
			var dtBytes = BitConverter.GetBytes(dt.Ticks);
			for (int i = 0; i < 4; i++) {
				bytes[i] = dtBytes[i];
			}

			bytes[4] = _counter;// пригодится только или еслибудет сервис или несколько раз подряд будет генерироваться на одном компе
			bytes[5] = 1;// type of operation system
			bytes[6] = 1;// OS version 
			for (int i = 7; i < totalLength; i++) {
				bytes[i] = (byte)STR[RandomHelper.Random(STR.Length)];
			}
			var full = Convert.ToBase64String(bytes);
			var part = full.Replace("/", "").Replace("+", "");
			var res = part.Substring(0, 7) + /*"><" +*/ part.Substring(9, 12);

			return res;
		}

		/// <summary>
		/// Вычисляем хэш по строке
		/// </summary>
		/// <param name="pass"></param>
		/// <returns></returns>
		public static string CalculateHash(string pass)
		{
			using (SHA256 sha = SHA256.Create()) {
				// step 1, calculate SHA256 hash from input
				byte[] inputBytes = Encoding.ASCII.GetBytes(pass);
				byte[] hash = sha.ComputeHash(inputBytes);
				// step 2, convert byte array to hex string
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hash.Length; i++) {
					sb.Append(hash[i].ToString("x2"));
				}
				return sb.ToString();
			}
		}
	}
}
