using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine
{
	/// <summary>
	/// Для хранения состояния кнопки и времени её изменения
	/// </summary>
	internal class InputKeyStatePause
	{
		public DateTime StateLimit;
		public int PauseState;
		public List<Keys> KeyCombination;
	}
}
