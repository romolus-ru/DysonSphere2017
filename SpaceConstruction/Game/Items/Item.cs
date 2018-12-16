using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Общее определение вещи из магазина
	/// </summary>
	internal class Item
	{
		internal string Texture;
		internal ItemTypeEnum Type;
		internal string Code;
		internal string Name;
		internal string Description;
		/// <summary>
		/// Стоимость
		/// </summary>
		internal ItemManager Cost;

		public override string ToString()
		{
			return Name + " " + Type;
		}
	}
}