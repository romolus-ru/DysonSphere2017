using System;

namespace Submarines
{
	/// <summary>
	/// Корабельное оружие. отвечает за паузу перед выстрелом и возможно за поворот оружия и снаряжение выстрела
	/// </summary>
	internal class Weapon
	{
		public delegate void OnShootToCoordinatesDelegate(Weapon weapon, float shootX, float shootY);

		public TimeSpan LoadWeaponTime { get; private set; }
		public int AmmunitionType { get; private set; }
		public OnShootToCoordinatesDelegate OnShootToCoordinates;

		public TimeSpan WaitForShoot = TimeSpan.Zero;

		public float _shootX;
		public float _shootY;
		public bool ShootToCoordinates = false;
		
		public Weapon(TimeSpan loadWeaponTime, int ammunitionType)
		{
			LoadWeaponTime = loadWeaponTime;
			AmmunitionType = ammunitionType;
		}

		public void StartShootToCoordinates(float x, float y)
		{
			_shootX = x;
			_shootY = y;
			ShootToCoordinates = true;
		}

		public void Shoot(TimeSpan elapsedTime)
		{
			WaitForShoot -= elapsedTime;
			if (WaitForShoot>TimeSpan.Zero)
				return;
			if (!ShootToCoordinates)
				return;
			ShootToCoordinates = false;
			WaitForShoot = LoadWeaponTime;
			OnShootToCoordinates?.Invoke(this, _shootX, _shootY);
		}

	}
}
