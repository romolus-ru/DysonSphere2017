using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Scroll2
{
	public class ScrollItem<T> : ViewComponent where T : EventBase
	{
		public T Value { get; protected set; }

		public virtual void DrawItem(VisualizationProvider visualizationProvider, int x, int y)
		{

		}

		public virtual void ScrollBy(int deltaX, int deltaY)
		{
			SetCoordinatesRelative(deltaX, deltaY, 0);
		}
	}
}