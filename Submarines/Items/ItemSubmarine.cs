using System.Collections.Generic;
using Submarines.Utils;

namespace Submarines.Items
{
	/// <summary>
	/// Набор предметов для подлодки (состав подлодки)
	/// </summary>
	internal class ItemSubmarine : ItemBase
	{
		public bool IsCanBuy { get; private set; }

		public ItemHull Hull { get; private set; }
		public ItemEngine Engine { get; private set; }
		public ItemManeuverDevice ManeuverDevice { get; private set; }

		internal override void Init(Dictionary<string, string> values)
		{
			base.Init(values);
			var hullName = values.GetString("Hull");
			Hull = (ItemHull) ItemsManager.GetItemBase(hullName);
			var engineName = values.GetString("Engine");
			Engine = (ItemEngine) ItemsManager.GetItemBase(engineName);
			var maneuverDeviceName = values.GetString("ManeuverDevice");
			ManeuverDevice = (ItemManeuverDevice) ItemsManager.GetItemBase(maneuverDeviceName);
		}
	}
}