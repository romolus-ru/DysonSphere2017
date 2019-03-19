﻿using Engine;
using Engine.Visualization;
using System;
using System.Drawing;
using System.Windows.Forms;
using Submarines.Maps;
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
		private MapBase _map;
		private ShipController _shipController;

		internal void InitGame(ViewManager viewManager, MapBase map, ShipController shipController)
		{
			_viewManager = viewManager;
			_map = map;
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

			var viewMap = new ViewMap();
			AddComponent(viewMap);
			viewMap.SetMap(_map);
			visualizationProvider.LoadAtlas("Submarines_background");
			visualizationProvider.LoadAtlas("Submarines01");
			visualizationProvider.LoadAtlas("Submarines01map");
			visualizationProvider.LoadAtlas("Result");
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
			//visualizationProvider.DrawTexturePart(0,0, "Submarines_background.main", 1650,1050);
			//visualizationProvider.DrawTexture(300, 300, "Submarines01.pl01");
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.OffsetAdd(_mapX, _mapY);


			visualizationProvider.OffsetRemove();
		}

	}
}