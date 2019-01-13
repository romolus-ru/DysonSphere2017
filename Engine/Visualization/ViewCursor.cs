﻿namespace Engine.Visualization
{
	/// <summary>
	/// Вывод курсора
	/// </summary>
	class ViewCursor:ViewComponent
	{
		/*private int cx;
		private int cy;
		protected override void Cursor(int cursorX, int cursorY)
		{
			cx = cursorX;
			cy = cursorY;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			//input.AddCursorAction(Cursor, true);
		}
		*/

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var cx = Input.CursorX;
			var cy = Input.CursorY;
			visualizationProvider.SetColor(System.Drawing.Color.White);
			//provider.Print(cx-8, cy-8, "X");
			visualizationProvider.Line(cx - 10, cy - 10, cx + 10, cy + 10);
			visualizationProvider.Line(cx + 10, cy - 10, cx - 10, cy + 10);
			visualizationProvider.Print(20, 20, "x " + cx + " y" + cy);
		}
	}
}
