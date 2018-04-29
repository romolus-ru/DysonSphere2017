using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Scroll
{
	/// <summary>
	/// Интерфейс элемента скрола
	/// </summary>
	interface IScrollItem
	{
		void DrawItem(VisualizationProvider visualizationProvider, int x, int y);
	}
}
