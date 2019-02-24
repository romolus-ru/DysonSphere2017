using Submarines.Items;

namespace Submarines.Submarines
{
	/// <summary>
	/// Создать подлодку в соответствии с переданными параметрами
	/// </summary>
	internal static class SubmarinesBuilder
	{
		public static SubmarineBase CreateHull(ItemHull info, Engine engine, ManeuverDevice maneuverDevice)
		{
			return new Submarine(info.Geometry, engine, maneuverDevice);
		}

		/// <summary>
		/// Создать двигатель с заданными характеристиками
		/// </summary>
		/// <returns></returns>
		private static Engine CreateEngine(ItemEngine info)
		{
			Engine engine = null;
			switch (info.EngineType) {
				case "Submarine":
					engine = new SubmarineEngine(
						info.EnginePower,
						info.EnginePercentMin,
						info.EnginePercentMax
					);
					break;
			}

			return engine;
		}

		/// <summary>
		/// Создать устройство изменения курса
		/// </summary>
		/// <returns></returns>
		private static ManeuverDevice CreateManeuverDevice(ItemManeuverDevice info)
		{
			ManeuverDevice maneuverDevice = null;
			switch (info.DeviceType) {
				case "Submarine":
					maneuverDevice = new SubmarineManeuverDevice(
						info.MaxSteeringPerSecond,
						info.SteeringLimit
					);
					break;
			}

			return maneuverDevice;
		}

		internal static SubmarineBase Create(ItemSubmarine itemSubmarine)
		{
			var md = CreateManeuverDevice(itemSubmarine.ManeuverDevice);
			var en = CreateEngine(itemSubmarine.Engine);
			var sub = CreateHull(itemSubmarine.Hull, en, md);

			return sub;
		}
	}
}