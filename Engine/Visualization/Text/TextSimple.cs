using Engine.Enums;
using System.Diagnostics;
using System.Drawing;

namespace Engine.Visualization.Text
{
	/// <summary>
	/// Текст со шрифтом и цветом
	/// </summary>
	public class TextSimple : TextPiece
	{
		private Color _color;
		private string _font;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string _text;
		public string Text {
			get { return _text; }
			set{
				_text = value;
				_textSizeCalculated = false;
				CalculateSize(_visualizationProvider);
			}
		}
		private VisualizationProvider _visualizationProvider;
		private bool _textSizeCalculated = false;

		public override bool CanSplit {
			get {
				return true;
			}
		}
		public void Init(VisualizationProvider visualizationProvider, Color color, string font = null, string text = null, TextAlign textAlign = TextAlign.Left)
		{
			_visualizationProvider = visualizationProvider;
			_font = font;
			SetColor(color);
			Text = text;
			Align = textAlign;
		}

		public override void CalculateSize(VisualizationProvider visualizationProvider)
		{
			if (_textSizeCalculated) return;
			_textSizeCalculated = true;
			Width = visualizationProvider.TextLength(_font, Text);
			Height = visualizationProvider.GetFontSize(_font);
		}

		public void SetColor(Color color)
		{
			_color = color;
		}

		public override void CalculatePlace(int offsetX, int offsetY)
		{
			X = offsetX;
			Y = offsetY;
		}
		public override void Draw(VisualizationProvider visualizationProvider)
		{
			if (!string.IsNullOrEmpty(_font))
				visualizationProvider.SetFont(_font);
			visualizationProvider.SetColor(_color);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.Print(X, Y, Text);
			visualizationProvider.Line(X + 5, Y + Height + 5, X + Width - 5, Y + Height + 5);
		}
	}
}