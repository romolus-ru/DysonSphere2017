using System;

namespace Engine.Extensions
{
	/// <summary>
	/// Но пока пришлось использовать напрямую подобный код
	/// </summary>
	public static class EnumExtensions
	{
		public static T Next<T>(this T src) where T : struct
		{
			if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

			T[] arr = (T[]) Enum.GetValues(src.GetType());
			int j = Array.IndexOf<T>(arr, src) + 1;
			return (arr.Length == j) ? arr[0] : arr[j];
		}

		public static T Prev<T>(this T src) where T : struct
		{
			if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

			T[] arr = (T[]) Enum.GetValues(src.GetType());
			int j = Array.IndexOf<T>(arr, src) - 1;
			return (j == 0) ? arr[arr.Length - 1] : arr[j];
		}
	}
}