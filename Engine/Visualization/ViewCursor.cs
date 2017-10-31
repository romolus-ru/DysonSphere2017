using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Вывод курсора
	/// </summary>
	class ViewCursor:ViewComponent
	{
		private int cx;
		private int cy;
		protected override void Cursor(int cursorX, int cursorY)
		{
			cx = cursorX;
			cy = cursorY;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			input.AddCursorAction(Cursor);
		}

		public override void DrawObject(VisualizationProvider provider)
		{
			provider.SetColor(System.Drawing.Color.White);
			//provider.Print(cx-8, cy-8, "X");
			provider.Line(cx - 10, cy - 10, cx + 10, cy + 10);
			provider.Line(cx + 10, cy - 10, cx - 10, cy + 10);
		}
	}
}
