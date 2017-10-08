using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	/// <summary>
	/// Строка данных для логирования
	/// </summary>
	internal class LogData
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
