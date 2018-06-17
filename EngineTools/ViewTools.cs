using Engine;
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

		public void InitTools(ViewManager viewManager)
		{
			_viewManager = viewManager;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewTools");
			var debugView = new DebugView();
			AddComponent(debugView, true);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");

			var btnSelectGame = new ViewButton();
			AddComponent(btnSelectGame);
			btnSelectGame.InitButton(SelectGame, "SelectGame", "SelectGame hint", Keys.Y);
			btnSelectGame.SetParams(20, 120, 140, 30, "btnSelectGame");
			btnSelectGame.InitTexture("textRB", "textRB");

			var btnSaveTablesToFiles = new ViewButton();
			AddComponent(btnSaveTablesToFiles);
			btnSaveTablesToFiles.InitButton(null, "SaveTablesToFiles", "SaveTablesToFiles hint", Keys.S);
			btnSaveTablesToFiles.SetParams(250, 70, 140, 30, "btnSaveTablesToFiles");
			btnSaveTablesToFiles.InitTexture("textRB", "textRB");

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");
		}

		private void SelectGame()
		{
			//new ShopWindow().InitWindow(_viewManager);
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