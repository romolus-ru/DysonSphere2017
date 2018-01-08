using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Ввод строкового значения
	/// </summary>
	public class ViewInput : ViewComponent
	{
		public string Txt { get; protected set; } = "";

		public void InputAction(string str)
		{
			Txt += str;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var color = CursorOver ? Color.DarkOliveGreen : Color.Green;
			visualizationProvider.SetColor(color, 75);
			visualizationProvider.Box(X, Y, Width, Height);
			visualizationProvider.SetColor(Color.Red);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			color = Color.White;
			visualizationProvider.Print(X + 10, Y + 10, Txt);
		}
	}
}
