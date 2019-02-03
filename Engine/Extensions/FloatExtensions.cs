using System;

namespace Engine.Extensions
{
	public static class FloatExtensions
	{
		private static float Epsilon = 0.0001f;

		public static bool IsZero(this float value)
			=> Math.Abs(value) < Epsilon;

		public static bool IsEqualTo(this float value, float otherValue)
			=> IsZero(value - otherValue);
	}
}