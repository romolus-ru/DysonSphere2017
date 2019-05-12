using System;
using System.Collections.Generic;

namespace Submarines.Utils
{
	/// <summary>
	/// Преобразование строки в нужные типы
	/// </summary>
	internal static class StringExtensions
	{
		public static T ToEnum<T>(this string value, T defaultValue) where T : struct
		{
			T result;
			return Enum.TryParse(value, true, out result)
				? result
				: defaultValue;
		}

		public static float ToFloat(this string value, float defaultValue)
		{
			float result;
			return float.TryParse(value, out result)
				? result
				: defaultValue;
		}

		public static int ToInt(this string value, int defaultValue)
		{
			int result;
			return int.TryParse(value, out result)
				? result
				: defaultValue;
		}

		public static bool ToBool(this string value, bool defaultValue = false)
		{
			bool result;
			return bool.TryParse(value, out result)
				? result
				: defaultValue;
		}

		public static TimeSpan ToTimeSpan(this string value, int milliseconds=5000)
		{
			var ms = ToInt(value, milliseconds);
			return new TimeSpan(0, 0, 0, 0, ms);
		}

		public static string GetString(this Dictionary<string, string> values, string keyName)
		{
			return values.ContainsKey(keyName)
				? values[keyName]
				: null;
		}
	}
}