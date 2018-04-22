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
				|| value == BuildingEnum.ConsumablesFactory
				|| value == BuildingEnum.ToolsFactory;
		}

		public static ResourcesEnum GetResourceEnum(this BuildingEnum value)
		{
			switch (value) {
				case BuildingEnum.MineMaterial:
					return ResourcesEnum.RawMaterials;
				case BuildingEnum.ConsumablesFactory:
					return ResourcesEnum.Consumables;
			}
			return ResourcesEnum.Tools;
		}

		public static BuildingEnum GetBuildingEnum(this ResourcesEnum value)
		{
			switch (value) {
				case ResourcesEnum.RawMaterials:
					return BuildingEnum.MineMaterial;
				case ResourcesEnum.Consumables:
					return BuildingEnum.ConsumablesFactory;
			}
			return BuildingEnum.ToolsFactory;
		}


	}
}