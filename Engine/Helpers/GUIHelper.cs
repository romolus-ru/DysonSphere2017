using Engine.Visualization;
using System.Drawing;

namespace Engine.Helpers
{
	/// <summary>
	/// Прорисовка вспомогательных компонентов, для упрощения VisualizationProvider
	/// </summary>
	public static class GUIHelper
	{
		public static Color BackgroundFadeColor = Color.FromArgb(170, Color.Black);
		public static Color DefaultMissingTextureColor = Color.Red;
		public static Color CursorLightColor = Color.Aqua;
		public static Color CursorDarkColor = Color.Beige;
		public static Color ButtonHintColor = Color.ForestGreen;
		public static Color ButtonHintKeysColor = Color.LawnGreen;
		public static Color HintBackgroundColor = Color.FromArgb(210, Color.Black);
		public static Color DraggableDefaultColor = Color.Beige;
		public static Color DraggableCursorOverColor = Color.Aqua;
		public static Color DraggableDragModeColor = Color.BurlyWood;
		public static string DraggableTexture = "WindowSample";

		public static void ViewGUIRectangle(VisualizationProvider visualizationProvider, ViewComponent component, string textureName)
		{
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

		public static void ShowHint(VisualizationProvider visualizationProvider, ViewComponent component, string hintText, string hintKeys)
		{
			if (!string.IsNullOrEmpty(hintText) && component.CursorOver) {
				DrawHint(visualizationProvider, component, hintText, hintKeys);
			}
		}

		public static void DrawHint(VisualizationProvider visualizationProvider, ViewComponent component, string hintText, string hintKeys)
		{
			var f = visualizationProvider.FontHeight / 2;
			var l = visualizationProvider.TextLength(hintText + " " + hintKeys);

			//visualizationProvider.SetColor(Color.White);
			//visualizationProvider.Rectangle(component.X+5 , component.Y + component.Height + 5 - f, l + 15, (int)(f * 2.6f));
			visualizationProvider.SetColor(HintBackgroundColor);
			visualizationProvider.Box(component.X + 5, component.Y + component.Height + 5 - f, l + 15, (int)(f * 2.6f));

			visualizationProvider.SetColor(ButtonHintColor);
			visualizationProvider.Print(component.X + 10, component.Y + component.Height + 5 - f, hintText);
			if (!string.IsNullOrEmpty(hintKeys)) {
				visualizationProvider.SetColor(ButtonHintKeysColor);
				visualizationProvider.Print(" " + hintKeys);
			}
		}
	}
}
