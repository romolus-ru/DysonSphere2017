using System.Windows.Forms;
using Submarines.Items;

namespace Submarines.Submarines
{
	/// <summary>
	/// Создать подлодку в соответствии с переданными параметрами
	/// </summary>
	internal static class SubmarinesBuilder
	{
		private static SubmarineBase CreateHull(ItemHull info, Engine engine, ManeuverDevice maneuverDevice, Weapon weapon)
		{
			return new Submarine(info.Geometry, engine, maneuverDevice, weapon);
		}

		/// <summary>
		/// Создать оружие с заданными характеристиками
		/// </summary>
		/// <returns></returns>
		private static Weapon CreateWeapon(ItemWeapon info)
		{
			Weapon weapon = new Weapon(
						info.LoadWeaponTime,
						info.AmmunitionType
					);

			return weapon;
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
			var wp = CreateWeapon(itemSubmarine.Weapon);
			var sub = CreateHull(itemSubmarine.Hull, en, md, wp);

			return sub;
		}
	}
}