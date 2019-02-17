using System.Collections.Generic;
using Submarines.Utils;

namespace Submarines.Items
{
	internal class ItemManeuverDevice : ItemBase
	{
		public float MaxSteeringPerSecond { get; private set; }

		public float SteeringLimit { get; private set; }

		/// <summary>
		/// Класс для устройства
		/// </summary>
		public string DeviceType { get; private set; }

		internal override void Init(Dictionary<string, string> values)
		{
			base.Init(values);
			MaxSteeringPerSecond = values.GetString("MaxSteeringPerSecond").ToFloat(0);
			SteeringLimit = values.GetString("SteeringLimit").ToFloat(0);
			DeviceType = values.GetString("DeviceType");
		}
	}
}