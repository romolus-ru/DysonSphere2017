using System;

namespace Engine.Helpers
{
	public static class RandomHelper
	{
		private static Random _rnd = new Random();

		public static int Random(int maxValue)
		{
			return _rnd.Next(maxValue);
		}
	}
}