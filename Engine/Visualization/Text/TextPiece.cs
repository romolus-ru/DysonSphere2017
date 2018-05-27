using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Text
{
	/// <summary>
	/// Кусочек текста
	/// </summary>
	public class TextPiece
	{
		public TextPiece Parent = null;
		public int X;
		public int Y;
		public int Height;
		public int Width;
		public bool IsCentered;// change to Enum

		/// <summary>
		/// Вывести на экран объект
		/// </summary>
		/// <param name="visualizationProvider"></param>
		public virtual void Draw(VisualizationProvider visualizationProvider) { }
		/// <summary>
		/// Пересчитать размеры
		/// </summary>
		/// <param name="visualizationProvider"></param>
		public virtual void CalculateSize(VisualizationProvider visualizationProvider) { }
	}
}