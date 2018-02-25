using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Вывод строки текста заданным цветом
	/// </summary>
	public class ViewLabel : ViewLabelSimple
	{
		public static ViewLabel Create(int x, int y, Color color, string text)
		{
			return new ViewLabel { Color = color, Text = text, X = x, Y = y };
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.Print(X, Y, null);
			base.DrawObject(visualizationProvider);
		}
	}
}