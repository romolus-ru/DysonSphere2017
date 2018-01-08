using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Окно
	/// </summary>
	public class ViewWindow : ViewPanel
	{
		private string _btnTexture = null;
		private int _btnTextureBorder = 0;

		/// <summary>
		/// Заголовок окна
		/// </summary>
		protected string Caption;

		public ViewWindow() { }

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var texture = _btnTexture;
			if (!string.IsNullOrEmpty(texture)) {
				// углы
				visualizationProvider.DrawTexturePart(X, Y, texture + ".t1", _btnTextureBorder, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y, texture + ".t3", _btnTextureBorder, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X, Y + Height - 10, texture + ".t7", _btnTextureBorder, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y + Height - 10, texture + ".t9", _btnTextureBorder, _btnTextureBorder);
				// стороны
				visualizationProvider.DrawTexturePart(X + 10, Y, texture + ".t2", Width - _btnTextureBorder * 2, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X, Y + 10, texture + ".t4", _btnTextureBorder, Height - _btnTextureBorder * 2);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y + 10, texture + ".t6", _btnTextureBorder, Height - _btnTextureBorder * 2);
				visualizationProvider.DrawTexturePart(X + 10, Y + Height - 10, texture + ".t8", Width - _btnTextureBorder * 2, _btnTextureBorder);
				// центр
				visualizationProvider.DrawTexturePart(X + 10, Y + 10, texture + ".t5", Width - _btnTextureBorder * 2, Height - _btnTextureBorder * 2);
			} else {
				var color = CursorOver ? Color.DarkOliveGreen : Color.Green;
				visualizationProvider.SetColor(color, 75);
				visualizationProvider.Box(X, Y, Width, Height);
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Rectangle(X, Y, Width, Height);
			}
		}

		public void InitTexture(string textureName, int textureBorder)
		{
			_btnTexture = textureName;
			_btnTextureBorder = textureBorder;
			VisualizationProvider.LoadAtlas(_btnTexture);
		}
	}
}
