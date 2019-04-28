using System;
using System.Diagnostics;

namespace Submarines
{
	/// <summary>
	/// Вектор
	/// </summary>
	[DebuggerDisplay("Vector {X},{Y},{Z}")]
	struct Vector
	{
		public float X;
		public float Y;
		public float Z;

		public Vector(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static Vector Zero()
			=> new Vector() { X = 0, Y = 0, Z = 0 };

		public static Vector One()
			=> new Vector() { X = 1, Y = 1, Z = 1 };

		public static Vector Up()
			=> new Vector() { X = 0, Y = 1, Z = 0 };

		public static Vector Down()
			=> new Vector() { X = 0, Y = -1, Z = 0 };

		public static Vector Left()
			=> new Vector() { X = -1, Y = 0, Z = 0 };

		public static Vector Right()
			=> new Vector() { X = 1, Y = 0, Z = 0 };

		public static Vector Back()
			=> new Vector() { X = 0, Y = 0, Z = -1 };

		public static Vector Forward()
			=> new Vector() { X = 0, Y = 0, Z = 1 };

		public void AddForward(float f)
		{
			Z += f;
		}

		public static Vector operator /(Vector v1, int value)
			=> new Vector(v1.X / value, v1.Y / value, v1.Z / value);

		public static Vector operator /(Vector v1, float value)
			=> new Vector(v1.X / value, v1.Y / value, v1.Z / value);

		public static Vector operator *(Vector v1, int value)
			=> new Vector(v1.X * value, v1.Y * value, v1.Z * value);

		public static Vector operator *(Vector v1, float value)
			=> new Vector(v1.X * value, v1.Y * value, v1.Z * value);

		public static Vector operator *(float value, Vector v1)
			=> new Vector(v1.X * value, v1.Y * value, v1.Z * value);

		public static Vector operator -(Vector v1, Vector v2)
			=> new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

		public static Vector operator +(Vector v1, Vector v2)
			=> new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

		public Vector MoveRelative(float x, float y, float z = 0)
			=> new Vector(X + x, Y + y, Z + z);


		private float Distance2To(Vector other)
		{
			float dx = X - other.X;
			float dy = Y - other.Y;
			return dx * dx + dy * dy;
		}

		public float DistanceTo(Vector other)
		{
			return (float)Math.Sqrt(Distance2To(other));
		}

		public Vector MovePolar(float angle, float radius)
		{
			var radians = angle * (Math.PI / 180);
			float x = (int)(radius * Math.Cos(radians));
			float y = (int)(radius * Math.Sin(radians));

			X += x;
			Y += y;
			return new Vector(X, Y, 0);
		}

		/// <summary>
		/// Угол по отношению к другому вектору в градусах
		/// </summary>
		/// <param name="vector2"></param>
		/// <returns></returns>
		public double AngleWith(Vector vector2)
		{
			double sin = this.X * vector2.Y - vector2.X * this.Y;
			double cos = this.X * vector2.X + this.Y * vector2.Y;

			return Math.Atan2(sin, cos) * (180 / Math.PI);
		}

	}
}