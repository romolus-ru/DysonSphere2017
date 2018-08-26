using System.Collections.Generic;

namespace Engine.Extensions
{
	/// <summary>
	/// Расширения для Dictionary
	/// </summary>
	public static class DictionaryExtensions
	{
		public static void Save(this Dictionary<string, string> values, string key, string value)
		{
			if (values.ContainsKey(key))
				values[key] = value;
			else
				values.Add(key, value);
		}
	}
}