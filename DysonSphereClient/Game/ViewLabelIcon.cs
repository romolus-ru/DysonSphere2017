using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Текст с иконкой
	/// </summary>
	class ViewLabelIcon : ViewLabelSimple
	{
		public string Icon;
		public static ViewLabelIcon Create(int x, int y, Color color, string text, string fontName = null, string icon = null)
		{
			return new ViewLabelIcon { Color = color, Text = text, FontName = fontName, X = x, Y = y, Icon = icon };
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.Print(X, Y, null);
			base.DrawObject(visualizationProvider);
			if (!string.IsNullOrEmpty(Icon)) {
				visualizationProvider.PrintTexture(Icon, FontName);
			}
		}
	}
}