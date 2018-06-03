using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Text
{
	/// <summary>
	/// Абзац текста. получает одну строку текста и разделяет её на строки
	/// </summary>
	class TextParagraph
	{
		/// <summary>
		/// Оригинал строк которые потом преобразуются в текст выводимый на экран с учётом размеров
		/// </summary>
		public List<TextPiece> RowsOriginals;
		private List<TextPiece> _text;
	}
}