using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Helpers
{
	/// <summary>
	/// Прорисовка вспомогательных компонентов, для упрощения VisualizationProvider
	/// </summary>
	public static class GUIHelper
	{
		public static Color DefaultMissingTextureColor = Color.Red;
		public static Color CursorLightColor = Color.Aqua;
		public static Color CursorDarkColor = Color.Beige;
		public static Color ButtonHintColor = Color.ForestGreen;
		public static Color ButtonHintKeysColor = Color.LawnGreen;

		public static void ViewGUIRectangle(VisualizationProvider visualizationProvider, ViewComponent component, string textureName)
		{
			var f = visualizationProvider.FontHeight / 2;

			if (textureName != null) {
				var size = visualizationProvider.GetTextureSize(textureName + ".t1");
				if (size.Width * 2 > component.Width) size.Width = component.Width / 2 - 1;
				if (size.Height * 2 > component.Height) size.Height = component.Height / 2 - 1;
				// углы
				visualizationProvider.DrawTexturePart(component.X, component.Y,
					textureName + ".t1", size.Width, size.Height);
				visualizationProvider.DrawTexturePart(component.X + component.Width - size.Width, component.Y,
					textureName + ".t3", size.Width, size.Height);
				visualizationProvider.DrawTexturePart(component.X, component.Y + component.Height - size.Height,
					textureName + ".t7", size.Width, size.Height);
				visualizationProvider.DrawTexturePart(component.X + component.Width - size.Width, component.Y + component.Height - size.Height,
					textureName + ".t9", size.Width, size.Height);
				// стороны
				visualizationProvider.DrawTexturePart(component.X + size.Width, component.Y,
					textureName + ".t2", component.Width - size.Width * 2, size.Height);
				visualizationProvider.DrawTexturePart(component.X, component.Y + size.Height,
					textureName + ".t4", size.Width, component.Height - size.Height * 2);
				visualizationProvider.DrawTexturePart(component.X + component.Width - size.Width, component.Y + size.Height,
					textureName + ".t6", size.Width, component.Height - size.Height * 2);
				visualizationProvider.DrawTexturePart(component.X + size.Width, component.Y + component.Height - size.Height,
					textureName + ".t8", component.Width - size.Width * 2, size.Height);
				// центр
				visualizationProvider.DrawTexturePart(component.X + size.Width, component.Y + size.Height,
					textureName + ".t5", component.Width - size.Width * 2, component.Height - size.Height * 2);
			} else {
				visualizationProvider.SetColor(DefaultMissingTextureColor);
				visualizationProvider.Rectangle(component.X, component.Y, component.Width, component.Height);
			}
		}
	}
}
