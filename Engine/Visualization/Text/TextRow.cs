using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Text
{
	/// <summary>
	/// Строка текста - рассчитывает свои размеры и корректирует координаты элементов
	/// </summary>
	public class TextRow : TextPiece
	{
		public List<TextPiece> Pieces = new List<TextPiece>();

		public void AddPiece(TextPiece piece)
		{
			piece.Parent = this;
			Pieces.Add(piece);
		}

		public override void CalculateSize(VisualizationProvider visualizationProvider)
		{
			if (Pieces == null || Pieces.Count == 0) {
				Width = 0;
				Height = 0;
				return;
			}
			Width = 0;
			Height = 0;
			foreach (var piece in Pieces) {
				piece.CalculateSize(visualizationProvider);
				Width += piece.Width;
				// а высоту считаем по максимальной высоте
				Height = Math.Max(Height, piece.Height);
			}
		}

		public override void CalculatePlace(int offsetX, int offsetY)
		{
			var x = offsetX;
			var y = offsetY;
			foreach (var piece in Pieces) {
				piece.X = x;
				piece.Y = y;
				x += piece.Width;
			}
		}

		public override void Draw(VisualizationProvider visualizationProvider)
		{
			foreach (var piece in Pieces) {
				piece.Draw(visualizationProvider);
			}
		}
	}
}