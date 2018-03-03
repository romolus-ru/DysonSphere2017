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
		public string FontName;

		public static ViewLabelSimple Create(Color color, string text, string fontName = null)
		{
			return new ViewLabelSimple { Color = color, Text = text, FontName = fontName };
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (FontName != null) { visualizationProvider.SetFont(FontName); }
			visualizationProvider.SetColor(Color);
			visualizationProvider.Print(Text);
			if (FontName != null) { visualizationProvider.SetFont("default"); }
		}
	}
}