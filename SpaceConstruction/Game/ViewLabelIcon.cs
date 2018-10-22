using Engine.Visualization;
using System.Drawing;

namespace SpaceConstruction.Game
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