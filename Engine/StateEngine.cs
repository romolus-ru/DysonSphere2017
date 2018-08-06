using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// Некоторые значения текущего состояния движка - что инициализировано, логи, коллектор и т.п.
	/// </summary>
	public static class StateEngine
	{
		public static LogSystem Log;
		public static Collector Collector;
	}
}
