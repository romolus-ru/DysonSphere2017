namespace Engine.Visualization.Scroll
{
	/// <summary>
	/// Интерфейс элемента скрола
	/// </summary>
	public interface IScrollItem
	{
		int Height { get; }
		int Width { get; }
		bool Selected { get; }
		void DrawItem(VisualizationProvider visualizationProvider, int x, int y);
		void ScrollBy(int deltaX, int deltaY);
		void SetSelected(int cursorX, int cursorY);
		bool Filtrate(string filter = null);
		bool Visible { get; set; }
		void SetCoordinates(int x, int y, int z = 0);
	}
}