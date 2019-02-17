using System;
using Submarines.Items;

namespace Submarines.Submarines
{
	/// <summary>
	/// Создать подлодку в зависимости от переданных параметров
	/// </summary>
	internal class SubmarinesBuilder
	{
		/// <summary>
		/// Создать подлодку или боеприпас и оснастить его начинкой
		/// </summary>
		/// <returns></returns>
		public SubmarineBase Build()
		{
			return null;
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
			var sub = new Submarine(en, md);

			return sub;
		}
	}
}
