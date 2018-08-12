using Engine;
using Engine.EventSystem.Event;
using Engine.Visualization;
using Engine.Visualization.Debug;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	public class ViewTools : ViewComponent
	{
		public Action OnExitPressed;
		private ViewManager _viewManager;
		private DataSupportBase _dataSupport;

		public void InitTools(ViewManager viewManager, DataSupportBase dataSupport)
		{
			_viewManager = viewManager;
			_dataSupport = dataSupport;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewTools");
			var debugView = new DebugView();
			AddComponent(debugView, true);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");
			
			var btnGetEventBaseClass = new ViewButton();
			AddComponent(btnGetEventBaseClass);
			btnGetEventBaseClass.InitButton(ShowGetEventBaseWindow, "GetEventBase", "GetEventBase hint", Keys.E);
			btnGetEventBaseClass.SetParams(320, 060, 140, 30, "btnGetEventBase");
			btnGetEventBaseClass.InitTexture("textRB", "textRB");

			var btnSaveTablesToFiles = new ViewButton();
			AddComponent(btnSaveTablesToFiles);
			btnSaveTablesToFiles.InitButton(null, "SaveTablesToFiles", "SaveTablesToFiles hint", Keys.S);
			btnSaveTablesToFiles.SetParams(150, 100, 140, 30, "btnSaveTablesToFiles");
			btnSaveTablesToFiles.InitTexture("textRB", "textRB");

			var btnSelectGame = new ViewButton();
			AddComponent(btnSelectGame);
			btnSelectGame.InitButton(SelectGame, "SelectGame", "SelectGame hint", Keys.Y);
			btnSelectGame.SetParams(150, 140, 140, 30, "btnSelectGame");
			btnSelectGame.InitTexture("textRB", "textRB");

			var btnLoginFormView = new ViewButton();
			AddComponent(btnLoginFormView);
			btnLoginFormView.InitButton(ShowLoginWindow, "LoginWindow", "LoginWindow hint", Keys.L);
			btnLoginFormView.SetParams(150, 060, 140, 30, "btnLoginFormView");
			btnLoginFormView.InitTexture("textRB", "textRB");

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");
		}

		private void ShowGetEventBaseWindow()
		{
			new CollectorClassSelectWindow<EventBase>()
				.InitWindow(_viewManager, _dataSupport, null, null);
		}

		private void ShowLoginWindow()
		{
			new LoginWindow().InitWindow(_viewManager, null, null);
		}
		private void SelectGame()
		{
			new MiniGameSelectWindow()
				.InitWindow(_viewManager, _dataSupport, null, null);
		}

		private void Close()
		{
			OnExitPressed();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.DeepSkyBlue);
			visualizationProvider.Print(100, 100, "Текст");
		}
	}
}