using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	public static class Constants
	{
		public const long UnknownValue = -1;
		public const int PauseStateNone = 0;
		public const int PauseStatePauseFirst = 1;
		public const int PauseStatePause = 2;

		public const int PauseDoubleClick = 200;

		public const int CursorFrequency = 20;

		public const int HintHidePause = 3;

		/// <summary>
		/// Дистанция для начала перемещения. может меняться в зависимости от разрешения экрана
		/// </summary>
		public const int DragDistance = 5;
		/// <summary>
		/// Граница перемещения, пределы которой нельзя перейти и в пределах которой элементы возвращаются в границы контрола
		/// </summary>
		public const int ScrollMoveBorder = 50;

		/// <summary>
		/// Основной разделитель строк в строке ("value1;value2")
		/// </summary>
		public const char BaseStringSeparator = ';';

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
