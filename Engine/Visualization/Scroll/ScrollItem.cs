using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Scroll
{
	public class ScrollItem : ViewComponent, IScrollItem
	{
		public bool Selected { get; protected set; }

		public virtual void DrawItem(VisualizationProvider visualizationProvider, int x, int y)
		{
		}

		public virtual void ScrollBy(int deltaX, int deltaY)
		{
			SetCoordinatesRelative(deltaX, deltaY, 0);
		}

		public void SetSelected(int cursorX, int cursorY)
			=> Selected = InRange(cursorX, cursorY);
	}
}