using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	public static class Settings
	{
		private static bool _inited = false;
		public static void Init()
		{
			if (_inited) return;
			_inited = true;
			_keyBoardRepeatPauseFirst = 500;
			_keyBoardRepeatPause = 200;
		}

		private static int _keyBoardRepeatPauseFirst = 0;
		public static int KeyBoardRepeatPauseFirst { get { return _keyBoardRepeatPauseFirst; } }

		private static int _keyBoardRepeatPause = 0;
		public static int KeyBoardRepeatPause { get { return _keyBoardRepeatPause; } }
	}
}
