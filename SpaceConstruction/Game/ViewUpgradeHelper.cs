using System.Drawing;
using SpaceConstruction.Game.Items;

namespace SpaceConstruction.Game
{
	internal static class ViewUpgradeHelper
	{
		internal static Color GetQualityColor(ItemUpgradeQualityEnum quality)
		{
			var color = Color.SandyBrown; // poor
			if (quality == ItemUpgradeQualityEnum.Normal)
				color = Color.Silver;
			if (quality == ItemUpgradeQualityEnum.Extra
			    || quality == ItemUpgradeQualityEnum.Autopilot)
				color = Color.Gold;
			if (quality == ItemUpgradeQualityEnum.Bad)
				color = Color.Red;
			return color;
		}
	}
}
