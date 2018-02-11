using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
