using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Заголовок окна за который можно перемещать
	/// </summary>
	public class ViewWindowCaption : ViewDragable
	{
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			if (!string.IsNullOrEmpty(Name)) {
				visualizationProvider.SetColor(Color.Black);
				visualizationProvider.Print(X+10, Y - 8, Name);
			}
		}
	}
}
