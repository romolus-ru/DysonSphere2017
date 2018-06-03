using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Text
{
	/// <summary>
	/// Выводит текстуру
	/// </summary>
	public class TextTexture : TextPiece
	{
		private string _texture;
		private string _font;
		private VisualizationProvider _visualizationProvider;

		public void Init(VisualizationProvider visualizationProvider, string texture, string font = null)
		{
			_visualizationProvider = visualizationProvider;
			_font = font;
			_texture = texture;
		}

		public override void CalculateSize(VisualizationProvider visualizationProvider)
		{
			var size = visualizationProvider.GetTextureSize(_texture);
			var fontSize = visualizationProvider.GetFontSize(_font);
			Height = size.Height;
			Width = size.Width;

			float coeff = 0;
			if (size.Width < size.Height) {
				coeff = size.Width / size.Height;
				Width = fontSize;
				Height = (int)(Width * coeff + 0.5);
			} else {
				coeff = size.Height / size.Width;
				Height = fontSize;
				Width = (int)(Height * coeff + 0.5);
			}
		}

		public override void Draw(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.DrawTexturePart(X, Y, _texture, Width, Height);
		}
	}
}