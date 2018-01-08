using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Engine;

namespace VisualizationOpenGL
{
	/// <summary>
	/// Создаётся исключительно для данной визуализации
	/// </summary>
	class OpenGLInput : Input
	{
		/// <summary>
		/// Стандартная обработка мышки и клавиатуры для Windows
		/// </summary>
		/// <remarks>Тут в основном ничего не делаем - конструктор используется от Input</remarks>
		public OpenGLInput() { }

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetKeyboardState(byte[] lpKeyState);

		[DllImport("user32.dll")]
		static extern IntPtr GetKeyboardLayout(uint idThread);

		[DllImport("user32.dll")]
		static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

		[DllImport("user32.dll")]
		static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[]
		   lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
		   int cchBuff, uint wFlags, IntPtr dwhkl);

		/// <summary>
		/// массив с нажатыми кнопками. никому не доступен
		/// </summary>
		private byte[] _keys = new byte[256];

		/// <summary>
		/// Установить какие кнопки нажаты
		/// </summary>
		/// <returns>Нажата ли хоть 1 клавиша</returns>
		protected override bool UpdateKeyboardState()
		{
			bool ret = false;
			GetKeyboardState(_keys);
			// проходим по первой половине массива
			for (int i = 0; i < 256; i++){
				// если старший бит установлен, то 
				if ((_keys[i] & 0x80) == 0) continue;
				ret = true;
				break;
			}
			return ret;


			/*byte[] keyboardState = new byte[256];
			GetKeyboardState(keyboardState);
			IntPtr handle = GetKeyboardLayout(0);
			uint scanCode = MapVirtualKeyEx(VirtualKeyCode, 0, handle);
			StringBuilder stringBuilder = new StringBuilder(2);

			int nResultLower = ToUnicodeEx(VirtualKeyCode, scanCode, keyboardState, stringBuilder,
												   stringBuilder.Capacity, 0, handle);

			string output = string.Empty;
			if (nResultLower != 0)
			{
				output = stringBuilder.ToString();
			}
			*/








		}

		/// <summary>
		/// Проверить, нажата ли клавиша
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public override bool IsKeyPressed(Keys key)
		{
			// проверяем установлен ли старший бит. если установлен возвращаем true
			return (_keys[(int)key] & 0x80) != 0;
		}

		/// <summary>
		/// Установить новые координаты курсора
		/// </summary>
		/// <returns></returns>
		protected override bool UpdateCursorState()
		{
			bool ret = false;
			var newCur = Cursor.Position;// экранные координаты курсора
			// преобразуем координаты в клиентские
			// (в полноэкранном режиме это всё ненужно)
			//PointEventArgs newCurClient = PointEventArgs.Set(newCur);// создаём аргументы для передачи координат
			//Controller.StartEvent("VisualizationCursorGet", this, newCurClient);// получаем обновлённые координаты

			if (newCur.X != CursorX)
			{
				CursorX = newCur.X;
				ret = true;
			}
			if (newCur.Y != CursorY)
			{
				CursorY = newCur.Y;
				ret = true;
			}
			return ret;
		}

		public override string KeysToUnicode()
		{
			var s = "";
			//foreach (Keys key in Enum.GetValues(typeof(Keys)))// getvalues не возвращает уникальные значения, они могут повторяться
			for (uint keyUInt = 32; keyUInt < 256; keyUInt++)
			{
				Keys key = (Keys) keyUInt;
				if (!IsKeyPressed(key)) continue;
				if (key == Keys.Back)
					continue;
				var s1 = KeysToUnicode(keyUInt);
				s += s1;
			}
			return s;
		}

		private string KeysToUnicode(uint key)
		{
			// http://www.pinvoke.net/default.aspx/user32.ToUnicodeEx
			// http://stackoverflow.com/questions/1164172/intercept-keyboard-input-using-current-keyboard-layout
			IntPtr handle = GetKeyboardLayout(0);
			uint scanCode = MapVirtualKeyEx(key, 0, handle);
			StringBuilder stringBuilder = new StringBuilder(2);

			int nResultLower = ToUnicodeEx(key, scanCode, _keys, stringBuilder,
												   stringBuilder.Capacity, 0, handle);

			string output = string.Empty;
			if (nResultLower > 0)
			{
				output = stringBuilder.ToString();
				output = output.Substring(0, nResultLower);
			}
			return output;
		}
	}
}
