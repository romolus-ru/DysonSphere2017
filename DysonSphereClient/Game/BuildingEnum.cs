using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Здания
	/// </summary>
	public enum BuildingEnum
	{
		/// <summary>
		/// Ничего не производит
		/// </summary>
		Nope,
		/// <summary>
		/// Добыча материалов
		/// </summary>
		MineMaterial,
		/// <summary>
		/// Фабрика расходных материалов
		/// </summary>
		ConsumablesFactory,
		/// <summary>
		/// Инструментальный завод
		/// </summary>
		ToolsFactory,
		/// <summary>
		/// Док для кораблей, после заданий они возвращаются туда
		/// </summary>
		ShipDepot,
		/// <summary>
		/// Стройка, требуется подвозить материалы
		/// </summary>
		QuestBuilding,

	}
}