using Engine;
using Engine.Visualization;
using System;
using System.Drawing;
using System.Windows.Forms;
using Submarines.Submarines;

namespace Submarines
{
	public class ViewGame : ViewComponent
	{
		public Action OnExitPressed;

		private ViewManager _viewManager;
		private int _mapX = 50;
		private int _mapY = 300;
		private int _curX;
		private int _curY;
		private Submarine _submarine;
		private ShipController _shipController;

		internal void InitGame(ViewManager viewManager, Submarine submarine, ShipController shipController)
		{
			_viewManager = viewManager;
			_submarine = submarine;
			_shipController = shipController;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewGame");

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "Выход", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			var viewShipController = new ViewShipController(_shipController);
			AddComponent(viewShipController);

			var viewShip = new ViewShip();
			AddComponent(viewShip);
			viewShip.SetShip(_submarine);

		}

		protected override void Cursor(int cursorX, int cursorY)
		{
			if (cursorX == _curX && cursorY == _curY) return;
			_curX = cursorX;
			_curY = cursorY;
		}

		private void Close()
		{
			OnExitPressed();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.OffsetAdd(_mapX, _mapY);

			visualizationProvider.OffsetRemove();
		}

	}
}