using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Айтем, отвечает за апгрейд корабля
	/// </summary>
	internal class ItemUpgrade : Item
	{
		public ItemUpgradeQualityEnum Quality;
		public List<ItemUpgradeValue> Upgrades = new List<ItemUpgradeValue>();

		public override string ToString()
		{
			return base.ToString() + " " + Quality;
		}
	}
}