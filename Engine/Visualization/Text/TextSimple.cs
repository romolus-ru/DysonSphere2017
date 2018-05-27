using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Text
{
	/// <summary>
	/// Текст со шрифтом и цветом
	/// </summary>
	public class TextSimple : TextPiece
	{
		private Color _color;
		private string _font;
		private string _text;
		public void Init(VisualizationProvider visualizationProvider, Color color, string font = null, string text = null)
		{
			_color = color;
			_font = font;
			SetText(text);
			CalculateSize(visualizationProvider);
		}
		public void SetText(string text)
		{
			_text = text;
		}

		public override void Draw(VisualizationProvider visualizationProvider)
		{
			if (!string.IsNullOrEmpty(_font))
				visualizationProvider.SetFont(_font);
			visualizationProvider.SetColor(_color);
			visualizationProvider.Print(X, Y, _text);
		}
	}
}