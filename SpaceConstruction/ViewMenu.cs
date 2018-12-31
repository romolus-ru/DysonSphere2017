using Engine;
using Engine.Visualization;
using Engine.Visualization.Debug;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpaceConstruction
{
	/// <summary>
	/// Меню после заставки и загрузки
	/// </summary>
	public class ViewMenu : ViewComponent
	{
		private Stopwatch _stopwatch;

		public Action OnConnect;
		public Action OnSendLogin;
		public Action OnRegistration;
		public Action OnExitPressed;
		public Action OnStartGame;

		public ViewMenu(Stopwatch stopwatch)
		{
			_stopwatch = stopwatch;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewMenu");

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			/*var debugView = new DebugView();
			AddComponent(debugView);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugViewMenu");*/

			var pnl = new ViewPanel();
			AddComponent(pnl);
			pnl.SetParams(100, 100, 600, 400, "Pnl");

			var btn6 = new ViewButton();
			pnl.AddComponent(btn6);
			btn6.InitButton(() => OnStartGame?.Invoke(), "Запустить игру", "StartGame", Keys.R);
			btn6.SetParams(70, 120, 240, 23, "btn6");
			btn6.InitTexture("textRB", "textRB");
		}

		private void Close()
		{
			OnExitPressed();
		}

	}
}