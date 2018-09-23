using Engine.Utils;
using System;

namespace Engine
{
	/// <summary>
	/// Некоторые значения текущего состояния движка - что инициализировано, логи, коллектор и т.п.
	/// </summary>
	public static class StateEngine
	{
		public static LogSystem Log;
		public static Collector Collector;
		public static JintController Jint;
		public static string AppPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }
	}
}
