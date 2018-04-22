using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Для вывода на экран информации о ресурсах
	/// </summary>
	public static class ResourcesHelper
	{
		public static void PrintResources(VisualizationProvider visualizationProvider, int x, int y, Resources res, string fontName=null)
		{
			visualizationProvider.Print(x, y, null);
			var enumerator = res.GetEnumerator();
			while (enumerator.MoveNext()) {
				var cur = enumerator.Current;
				if (cur.Value <= 0) continue;
				visualizationProvider.PrintTexture(GetTexture(cur.Key),fontName);
				visualizationProvider.Print(cur.Value + " ");
			}
		}

		public static string GetTexture(ResourcesEnum key)
		{
			switch (key) {
				case ResourcesEnum.RawMaterials:
					return "Resources.RawMaterial";
				case ResourcesEnum.Consumables:
					return "Resources.Consumables";
				case ResourcesEnum.Tools:
					return "Resources.Tools";
				default:
					return null;
			}
		}
	}
}
