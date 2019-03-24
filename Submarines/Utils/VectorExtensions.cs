using Engine.Visualization;

namespace Submarines.Utils
{
	internal static class VectorExtensions
	{
		public static ScreenPoint ToScreenPoint(this Vector value)
		{
			return new ScreenPoint((int) value.X, (int) value.Y);
		}
	}
}
