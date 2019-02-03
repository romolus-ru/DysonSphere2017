using System;

namespace Engine.DataPlus
{
	/// <summary>
	/// Структура для хранения данных со временем
	/// </summary>
	public struct TimePoint<T>
	{
		public DateTime Time;
		public T Value;
	}
}