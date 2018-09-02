using Engine.Enums;
using System.Collections.Generic;

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
		public TextAlign Align;
		/// <summary>
		/// Можно ли элемент разделить на части
		/// </summary>
		public virtual bool CanSplit { get { return false; } }

		/// <summary>
		/// Возвращается 2 объекта - новый укороченный до нужного размера и второй - оставшаяся часть
		/// </summary>
		/// <returns></returns>
		public List<TextPiece> Split(int width)
		{
			return null;
		}

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

		/// <summary>
		/// Вычислить координаты расположения текста с учётом смещения
		/// </summary>
		public virtual void CalculatePlace(int offsetX, int offsetY) { }
	}
}