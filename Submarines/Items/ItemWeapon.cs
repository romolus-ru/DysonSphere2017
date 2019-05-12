using System;
using System.Collections.Generic;
using Submarines.Utils;

namespace Submarines.Items
{
	internal class ItemWeapon : ItemBase
	{
		/// <summary>
		/// Время перезарядки оружия (время сборки снаряда пока не учитываем)
		/// </summary>
		public TimeSpan LoadWeaponTime { get; private set; }
		/// <summary>
		/// Тип боеприпасов. определяет некоторые дополнительные характеристики,
		/// зависящие в том числе от самого корабля (например доступные боеприпасы, включенные опции для создания боеприпасов и т.п.)
		/// (эти опции в основном для корабля игрока, так как у остальных кораблей снаряды будут прописаны
		/// </summary>
		public int AmmunitionType { get; private set; }
		/// <summary>
		/// Описание выстрела (детали снаряда по умолчанию)
		/// </summary>
		public string ShootName { get; private set; }


		internal override void Init(Dictionary<string, string> values)
		{
			base.Init(values);
			LoadWeaponTime = values.GetString("LoadWeaponTime").ToTimeSpan(2000);
			AmmunitionType = values.GetString("AmmunitionType").ToInt(1);
			ShootName = values.GetString("ShootName");
		}
	}
}