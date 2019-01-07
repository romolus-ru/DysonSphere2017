namespace Engine
{
	public static class Settings
	{
		private static bool _isInited = false;
		public static void Init()
		{
			if (_isInited) return;
			_isInited = true;
			KeyBoardRepeatPauseFirst = 500;
			KeyBoardRepeatPause = 200;
		}

		public static int KeyBoardRepeatPauseFirst { get; private set; }

		public static int KeyBoardRepeatPause { get; private set; }
	}
}
