using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	public static class BuildingEnumExtension
	{
		/// <summary>
		/// Построено что-нибудь или нет
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsEmpty(this BuildingEnum value)
		{
			return value == BuildingEnum.Nope;
		}

		/// <summary>
		/// Ресурсное ли здание
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsSource(this BuildingEnum value)
		{
			return value == BuildingEnum.MineMaterial
				|| value == BuildingEnum.CraftTools
				|| value == BuildingEnum.PopulationCamp;
		}
	}
}