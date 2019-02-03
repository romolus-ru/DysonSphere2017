using System;

namespace Submarines
{
	/// <summary>
	/// Вектор
	/// </summary>
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
			=> new Vector(this.X + x, this.Y + y, this.Z + z);
	}
}