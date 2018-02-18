using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	public static class Constants
	{
		public const int PauseStateNone = 0;
		public const int PauseStatePauseFirst = 1;
		public const int PauseStatePause = 2;

		public const int PauseDoubleClick = 200;

		public const int CursorFrequency = 20;

		/// <summary>
		/// Там хранятся архивы с JSON файлами в которых данные из БД хранятся
		/// </summary>
		public const string PlayerDBPath = "/data/";
		/// <summary>
		/// Соль для паролей. а название в честь фильма где ангелина джоли играла роль тома круза
		/// </summary>
		internal const string Solt = "vsbNM37SD8FBv46jf6jf";
	}
}
