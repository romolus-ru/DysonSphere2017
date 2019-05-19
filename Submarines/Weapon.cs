using System;

namespace Submarines
{
	/// <summary>
	/// Корабельное оружие. отвечает за паузу перед выстрелом и возможно за поворот оружия и снаряжение выстрела
	/// </summary>
	internal class Weapon
	{
		public TimeSpan LoadWeaponTime { get; private set; }
		public int AmmunitionType { get; private set; }
		/// <summary>
		/// Готовность к выстрелу
		/// </summary>
		public bool ReadyToShoot { get; private set; }

		public TimeSpan WaitForShoot = TimeSpan.Zero;

		public Weapon(TimeSpan loadWeaponTime, int ammunitionType)
		{
			LoadWeaponTime = loadWeaponTime;
			WaitForShoot = LoadWeaponTime; // сразу запускаем зарядку оружия
			AmmunitionType = ammunitionType;
			ReadyToShoot = false;
		}

		/// <summary>
		/// Изменить блокировку выстрела
		/// </summary>
		/// <param name="elapsedTime"></param>
		public void ChangeShootLock(TimeSpan elapsedTime)
		{
			WaitForShoot -= elapsedTime;
			if (WaitForShoot > TimeSpan.Zero)
				return;

			ReadyToShoot = true;
		}

	}
}