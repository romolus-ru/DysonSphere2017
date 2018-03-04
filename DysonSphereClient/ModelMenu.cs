using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Visualization;
using Engine.Visualization.Debug;
using Engine;
using Engine.Models;
using System.Diagnostics;
using System.Windows.Forms;

namespace DysonSphereClient
{
	/// <summary>
	/// Меню - создание вьюхи для меню и реакция на кнопки и т.п.
	/// </summary>
	public class ModelMenu : Model
	{
		private ModelMain _modelMain;
		private ViewManager _viewManager;
		public Action OnExitPressed;

		public ModelMenu(Stopwatch stopwatch, ModelMain modelMain, ViewManager viewManager)
		{
			_modelMain = modelMain;
			_viewManager = viewManager;

			var debugView = new DebugView();
			_viewManager.AddView(debugView, true);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");

			var clientView = new ClientView();
			clientView.SetTimerInfo(stopwatch);
			_viewManager.AddView(clientView);

			var btnClose = new ViewButton();
			_viewManager.AddView(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

		}

		private void Close()
		{
			OnExitPressed();
		}
	}
}
