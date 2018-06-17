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

		/// <summary>
		/// Добавить строку текста с автоматической разбивкой на строки по ширине
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public void CreateSplitedTextAuto(Color color, string font, string text)
		{
			while (!string.IsNullOrEmpty(text)) {
				var len = GetWidthLength(font, text);
				if (len == -1) break;
				var str = text.Substring(0, len);
				text = len < text.Length ? text.Substring(len + 1) : null;
				var tr = new TextRow();
				_texts.Add(tr);
				AddText(tr, color, font, str);
			}
		}

		/// <summary>
		/// Получить длину строки которая влезает в ширину компонента
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private int GetWidthLength(string font,string text)
		{
			if (string.IsNullOrEmpty(text))
				return -1;
			int ret = 0;
			int counter = 0;
			int prevCounter = -1;
			do {
				counter = text.IndexOf(' ', counter + 1);
				if (counter == -1) return text.Length;
				var len = VisualizationProvider.TextLength(font, text.Substring(0, counter));
				if (len > Width) {
					return prevCounter;
				}
				prevCounter = counter;
			}
			while (counter != -1);
			return -1;
		}

		public void ClearTexts() => _texts.Clear();

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