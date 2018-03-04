using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	public class ViewProgressBar : ViewComponent
	{
		public int Percent;
		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ProgressBar");
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.Red);
			visualizationProvider.Box(10, 10, Percent, 10);
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Box(10 + Percent, 10, 100 - Percent, 10);
		}
	}
}