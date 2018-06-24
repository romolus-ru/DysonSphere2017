﻿using Engine;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	public class MiniGameSelectWindow : ViewModalWindow
	{
		private Action<long> _selectedGame;
		private Action _cancel;
		private ViewManager _viewManager;
		private ViewInput _filter;
		private ViewScroll _viewScroll;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
		}

		protected override void ClearObject()
		{
			base.ClearObject();
		}

		public void InitWindow(ViewManager viewManager, Action<long> selectedGame, Action cancel)
		{
			viewManager.AddViewModal(this);
			SetParams(150, 150, 1200, 700, "Выбор миниигры");
			InitTexture("textRB", 10);

			var btn1 = new ViewButton();
			AddComponent(btn1);
			btn1.InitButton(Entered, "ok", "выбрать", Keys.Enter);
			btn1.SetParams(50, 670, 80, 25, "btnSelect");
			btn1.InitTexture("textRB", "textRB");

			var btn2 = new ViewButton();
			AddComponent(btn2);
			btn2.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btn2.SetParams(150, 670, 80, 25, "btnCancel");
			btn2.InitTexture("textRB", "textRB");

			_filter = new ViewInput();
			AddComponent(_filter);
			_filter.SetParams(140, 30, 500, 40, "filter for games");
			_filter.InputAction("");

			_selectedGame = selectedGame;
			_cancel = cancel;
			_viewManager = viewManager;

			_viewScroll = new ViewScroll();
			AddComponent(_viewScroll);
			_viewScroll.SetParams(10, 80, 1000, 560, "Список игр");

			foreach (var item in Enumerable.Range(1, 30)) {
				var scrollItem = new GameNameScrollView();
				_viewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (item - 1) * 90 + 10, 950, 20, "item" + item);
			}
		}

		private void Entered()
		{
			var i = 1;
			_selectedGame?.Invoke(i);
			CloseWindow();
		}

		private void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			_selectedGame = null;
			_cancel = null;
			_viewManager = null;
		}

		private void Cancel()
		{
			_cancel?.Invoke();
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