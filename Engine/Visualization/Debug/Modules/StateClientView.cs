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
			BorderCutOffStart();
			base.DrawObject(visualizationProvider);
			DrawMsg(visualizationProvider, 0,
				"Registration : ",
				StateClient.RegistrationState.ToString(),
				StateClient.RegistrationMessage);
			DrawMsg(visualizationProvider, 15,
				"Connection : ",
				StateClient.ConnectionState.ToString(),
				StateClient.ConnectionMessage);
			DrawMsg(visualizationProvider, 30,
				"Login : ",
				StateClient.LoginState.ToString(),
				StateClient.LoginMessage);
			BorderCutOffEnd();
		}

		public void DrawMsg(VisualizationProvider visualizationProvider, int y, string caption, string value, string msg)
		{
			visualizationProvider.SetColor(Color.Coral);
			visualizationProvider.Print(X + 10, Y + y, "");
			visualizationProvider.Print(caption);
			visualizationProvider.SetColor(Color.Azure);
			visualizationProvider.Print(value);
			if (!string.IsNullOrEmpty(msg)) {
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Print(" " + msg);
			}
		}

	}
}