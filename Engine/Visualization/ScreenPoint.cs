using System;

namespace Engine.Visualization
{
	public class ScreenPoint
	{
		// TODO попробовать заменить Vetex из triangulation на ScreenPoint. пока проблема в том что там плавающие переменные, погрешности большие могут быть
		public int X;
		public int Y;

		public ScreenPoint() { }

		public ScreenPoint(int x, int y)
		{
			X = x; Y = y;
		}

		public static ScreenPoint operator +(ScreenPoint a, ScreenPoint b)
			=> new ScreenPoint { X = a.X + b.X, Y = a.Y + b.Y };

		public static ScreenPoint operator -(ScreenPoint a, ScreenPoint b)
			=> new ScreenPoint { X = a.X - b.X, Y = a.Y - b.Y };

		public bool IsEqual(ScreenPoint otherPoint)
		{
			if (this == otherPoint) return true;
			if (this.X == otherPoint.X
				&& this.Y == otherPoint.Y)
				return true;
			return false;
		}

		public int distance2To(ScreenPoint other)
		{
			int dx = X - other.X;
			int dy = Y - other.Y;
			return dx * dx + dy * dy;
		}

		public float distanceTo(ScreenPoint other)
		{
			return (float)Math.Sqrt(distance2To(other));
		}

		public override string ToString()
		{
			return $"({X},{Y})";
		}

		public void MovePolar(float angle, float radius)
		{
			var radians = angle * (Math.PI / 180);
			int x = (int) (radius * Math.Cos(radians));
			int y = (int) (radius * Math.Sin(radians));

			X += x;
			Y += y;
		}

	}
}