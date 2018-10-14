using Engine.Helpers;
using System.Collections.Generic;

namespace Engine.Extensions
{
	/// <summary>
	/// Расширения для списков
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Перемешиваем список
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void Shuffle<T>(this List<T> list)
		{
			for (int i = list.Count - 1; i >= 1; i--) {
				int j = RandomHelper.Random(i + 1);

				T tmp = list[j];
				list[j] = list[i];
				list[i] = tmp;
			}
		}
	}
}
