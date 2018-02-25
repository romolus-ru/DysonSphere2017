using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// выводит текст без изменения своих координат
	/// </summary>
	public class ViewLabelSimple : ViewComponent
	{
		public Color Color;
		public string Text;

		public static ViewLabelSimple Create(Color color, string text)
		{
			return new ViewLabelSimple { Color = color, Text = text };
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color);
			visualizationProvider.Print(Text);
		}
	}
}