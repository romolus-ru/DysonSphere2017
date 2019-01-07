using System;
using Engine.Helpers;

namespace Engine.Utils
{
	public struct SafeInt:IComparable<SafeInt>
	{
		private int _offset;
		private int _value;

		public SafeInt(int value)
		{
			_offset = RandomHelper.Random(5000000);
			_value = value + _offset;
		}

		private int GetValue()
		{
			return _value - _offset;
		}

		public override string ToString()
		{
			return GetValue().ToString();
		}

		public int CompareTo(SafeInt other)
		{
			return GetValue().CompareTo(other.GetValue());
		}

		public static SafeInt operator +(SafeInt sf1, SafeInt sf2)
			=> new SafeInt(sf1.GetValue() + sf2.GetValue());

		public static SafeInt operator -(SafeInt sf1, SafeInt sf2)
			=> new SafeInt(sf1.GetValue() - sf2.GetValue());

		public static SafeInt operator +(SafeInt sf1, int value)
			=> new SafeInt(sf1.GetValue() + value);

		public static SafeInt operator --(SafeInt sf1)
		{
			sf1._value--;
			return sf1;
		}

		public static SafeInt operator ++(SafeInt sf1)
		{
			sf1._value++;
			return sf1;
		}

		public static SafeInt operator -(SafeInt sf1, int value)
			=> new SafeInt(sf1.GetValue() - value);

		public static bool operator <(SafeInt sf1, SafeInt sf2)
			=> sf1.GetValue() < sf2.GetValue();

		public static bool operator <=(SafeInt sf1, SafeInt sf2)
			=> sf1.GetValue() <= sf2.GetValue();

		public static bool operator >(SafeInt sf1, SafeInt sf2)
			=> sf1.GetValue() > sf2.GetValue();

		public static bool operator >=(SafeInt sf1, SafeInt sf2)
			=> sf1.GetValue() >= sf2.GetValue();

		public static bool operator <(SafeInt sf1, int value)
			=> sf1.GetValue() < value;

		public static bool operator <=(SafeInt sf1, int value)
			=> sf1.GetValue() <= value;

		public static bool operator >(SafeInt sf1, int value)
			=> sf1.GetValue() > value;

		public static bool operator >=(SafeInt sf1, int value)
			=> sf1.GetValue() >= value;

		public static implicit operator int(SafeInt sf1)
			=> sf1.GetValue();

		public static implicit operator SafeInt(int i1)
			=> new SafeInt(i1);
	}
}