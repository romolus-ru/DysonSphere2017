using Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Праобраз игрового модуля
	/// </summary>
	public class GameView : ViewComponent
	{
		private List<ViewLabel> _labels;
		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			_labels = new List<ViewLabel>();
			for (int i = 0; i < 10; i++) {
				var label = ViewLabel.Create(
					RandomHelper.Random(visualizationProvider.CanvasWidth),
					RandomHelper.Random(visualizationProvider.CanvasHeight),
					Color.Red, "label");
				AddComponent(label);
				_labels.Add(label);
			}
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var w = visualizationProvider.CanvasWidth + 50;
			foreach (var label in _labels) {
				label.SetCoordinatesRelative(1, 0, 0);
				if (label.X > w) label.SetCoordinatesRelative(-w, 0, 0);
			}
		}
	}
}