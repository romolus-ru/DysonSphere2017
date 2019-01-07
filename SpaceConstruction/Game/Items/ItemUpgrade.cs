using System.Collections.Generic;

namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Айтем, отвечает за апгрейд корабля
	/// </summary>
	internal class ItemUpgrade : Item
	{
		public ItemUpgradeQualityEnum Quality;
		/// <summary>
		/// Порядок установки улучшения, например автопилот применяется только после применения остальных улучшений
		/// </summary>
		public int InstallOrder;
		public List<ItemUpgradeValue> Upgrades = new List<ItemUpgradeValue>();

		public override string ToString()
		{
			return base.ToString() + " " + Quality;
		}
	}
}