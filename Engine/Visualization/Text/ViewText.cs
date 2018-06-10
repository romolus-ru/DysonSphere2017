using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Text
{
	/// <summary>
	/// Вывод текста
	/// </summary>
	public class ViewText : ViewComponent
	{
		private List<TextPiece> _texts = new List<TextPiece>();

		public TextRow CreateTextRow()
		{
			var tr = new TextRow();
			_texts.Add(tr);
			return tr;
		}

		public TextPiece AddText(TextRow textRow, Color color, string font, string text)
		{
			var ts = new TextSimple();
			ts.Init(VisualizationProvider, color, font, text);
			textRow.AddPiece(ts);
			return ts;
		}

		public void AddTexture(TextRow textRow, string texture, string font = null)
		{
			var tt = new TextTexture();
			tt.Init(VisualizationProvider, texture, font);
			textRow.AddPiece(tt);
		}

		public void CalculateTextPositions()
		{
			var h = 0;
			foreach (var txt in _texts) {
				txt.CalculateSize(VisualizationProvider);
				h += txt.Height;
			}
			var height = (Height - h) / 2;
			foreach (var txt in _texts) {
				var xpos = (Width - txt.Width) / 2;
				txt.CalculatePlace(xpos, height);
				height += txt.Height;
			}
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.FloralWhite);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.OffsetAdd(X, Y);
			foreach (var txt in _texts) {
				txt.Draw(visualizationProvider);
			}
			visualizationProvider.OffsetRemove();
		}
	}
}