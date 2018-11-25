using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Одно улучшение корабля
	/// </summary>
	internal class ItemUpgradeValue
	{
		public string Name;
		/// <summary>
		/// Имя улучшаемого значения
		/// </summary>
		public string UpName;
		/// <summary>
		/// Величина улучшаемого значения
		/// </summary>
		public int UpValue;
	}
}