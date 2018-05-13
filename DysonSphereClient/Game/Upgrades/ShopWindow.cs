using Engine;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DysonSphereClient.Game.Upgrades
{
	class ShopWindow : ViewModalWindow
	{
		private ViewManager _viewManager;
		private ViewScroll _scroll;

		public void InitWindow(ViewManager viewManager)
		{
			viewManager.AddViewModal(this);
			SetParams(250, 250, 700, 500, "Магазин");
			InitTexture("textRB", 10);

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "Close", "Закрыть", Keys.Escape);
			btnClose.SetParams(150, 450, 280, 25, "btnClose");
			btnClose.InitTexture("textRB", "textRB");

			_scroll = new ViewScroll();
			AddComponent(_scroll);
			_scroll.SetParams(10, 10, 500, 400, "Апгрейды");

			foreach (var item in Enumerable.Range(1, 30)) {
				var scrollItem = new UpgradeScrollItem();
				_scroll.AddComponent(scrollItem);
				scrollItem.SetParams((item - 1) * 90 + 10, 10, 80, 150, "item" + item);
			}

			_viewManager = viewManager;
		}

		private void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			_viewManager = null;
		}

		private void Close()
		{
			CloseWindow();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}
	}
}