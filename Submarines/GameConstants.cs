using System.Collections.Generic;

namespace Submarines
{
	public static class GameConstants
	{
		public const string DataDirectory = "Submarines";

		public static List<float> ZoomValue = new List<float>() {
			1f, 1 / 2f, 1 / 3f, 1 / 5f, 1 / 7f, 1 / 10f,
			1 / 15f, 1 / 20f, 1 / 25f, 1 / 30f, 1 / 40f, 1 / 50f,
		};

		public static float TimeCoefficient = 1 / 1f;

		public const float G = 9.8f;

	}
}