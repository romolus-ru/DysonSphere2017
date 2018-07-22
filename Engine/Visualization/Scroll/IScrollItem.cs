namespace Engine.Visualization.Scroll
{
	/// <summary>
	/// Интерфейс элемента скрола
	/// </summary>
	public interface IScrollItem
	{
		bool Selected { get; }
		void DrawItem(VisualizationProvider visualizationProvider, int x, int y);
		void ScrollBy(int deltaX, int deltaY);
		void SetSelected(int cursorX, int cursorY);
	}
}