using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	public class ViewPanel : ViewComponent
	{
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Green);
			visualizationProvider.Rectangle(X, Y, Width, Height);// если текстуры будут то они замаскируют этот прямоугольник
		}

		protected override void DrawComponents(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetStencilArea(_xScreen, _yScreen, _xScreen + Width, _yScreen + Height);
			base.DrawComponents(visualizationProvider);
			visualizationProvider.StensilAreaOff();
		}
	}
}