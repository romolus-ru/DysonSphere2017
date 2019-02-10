﻿using System;
using System.Collections.Generic;

namespace Submarines.Utils
{
	/// <summary>
	/// Преобразование строки в нужные типы игры
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

		public static bool ToBool(this string value, bool defaultValue = false)
		{
			bool result;
			return bool.TryParse(value, out result)
				? result
				: defaultValue;
		}

		public static string GetString(this Dictionary<string, string> values, string keyName)
		{
			return values.ContainsKey(keyName)
				? values[keyName]
				: null;
		}
	}
}