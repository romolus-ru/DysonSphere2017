using System;

namespace Engine.Utils
{
	/// <summary>
	/// Строка данных для логирования
	/// </summary>
	public class LogData
	{
		public DateTime Time;
		public int Id;
		public int Level;
		public string Tag;
		public string Message;

		public override string ToString()
		{
			return "" + Time.ToString() + " id=" + Id + " lvl=" + Level +
				" tag=" + Tag + " msg=" + Message;
		}
	}
}
