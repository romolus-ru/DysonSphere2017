using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization.Debug.Modules
{
	public class StateClientView : ViewWindow
	{
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			var y = 0;
			visualizationProvider.SetColor(Color.Coral);
			visualizationProvider.Print(X + 10, Y, "");
			visualizationProvider.Print("Registration : ");
			visualizationProvider.SetColor(Color.Azure);
			visualizationProvider.Print(StateClient.RegistrationState.ToString());
			if (!string.IsNullOrEmpty(StateClient.RegistrationMessage)) {
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Print(" " + StateClient.RegistrationMessage);
			}
		}
	}
}