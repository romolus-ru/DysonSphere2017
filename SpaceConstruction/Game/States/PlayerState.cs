using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game.States
{
	/// <summary>
	/// Состояние игрока - уровень, опыт, инвентарь
	/// </summary>
	internal static class PlayerState
	{
		public static int Level { get; private set; }
		public static int XP { get; private set; }
		private static List<KeyValuePair<int, int>> _levels = new List<KeyValuePair<int, int>>()
		{
			new KeyValuePair<int, int>(100,2),
			new KeyValuePair<int, int>(300,3),
		};

		public static void AddXP(int xp)
		{
			XP += xp;
			LevelUp(XP);
		}

		private static void LevelUp(int xp)
		{
			var level = 1;
			var v = _levels.Where(l => l.Key < xp).Max();
			if (v.Value > 1) level = v.Value;
			Level = level;
		}
	}
}