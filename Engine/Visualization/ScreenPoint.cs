using System;

namespace Engine.Visualization
{
	public class ScreenPoint
	{
		// TODO попробовать заменить Vertex из triangulation на ScreenPoint. пока проблема в том что там плавающие переменные, погрешности большие могут быть
		// или возможно заменить ScreenPoint и vertex на Vector (от microsoft)
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

		/// <summary>
		/// Угол по отношению к другому вектору в градусах
		/// </summary>
		/// <param name="vector2"></param>
		/// <returns></returns>
		public double AngleWith(ScreenPoint vector2)
		{
			double sin = vector2.Y - this.Y;
			double cos = vector2.X - this.Y;

			return Math.Atan2(sin, cos) * (180 / Math.PI);
		}

		public ScreenPoint Clone()
		{
			return new ScreenPoint(this.X, this.Y);
		}
	}
}