using Engine;
using Engine.Data;
using Engine.EventSystem.Event;
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
		private DataSupportBase _datasupport;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
		}

		protected override void ClearObject()
		{
			base.ClearObject();
		}

		public void InitWindow(ViewManager viewManager, DataSupportBase datasupport, Action<long> selectedGame, Action cancel)
		{
			viewManager.AddViewModal(this);
			SetParams(150, 150, 1200, 700, "Выбор миниигры");
			InitTexture("textRB", 10);

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(Entered, "ok", "выбрать", Keys.Enter);
			btnSelect.SetParams(50, 670, 80, 25, "btnSelect");
			btnSelect.InitTexture("textRB", "textRB");

			var btnCancel = new ViewButton();
			AddComponent(btnCancel);
			btnCancel.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btnCancel.SetParams(150, 670, 80, 25, "btnCancel");
			btnCancel.InitTexture("textRB", "textRB");

			var btnNew = new ViewButton();
			AddComponent(btnNew);
			btnNew.InitButton(AddNewGame, "New", "New", Keys.N);
			btnNew.SetParams(20, 20, 80, 25, "btnNewGame");
			btnNew.InitTexture("textRB", "textRB");

			_filter = new ViewInput();
			AddComponent(_filter);
			_filter.SetParams(140, 30, 500, 40, "filter for games");
			_filter.InputAction("");

			_selectedGame = selectedGame;
			_cancel = cancel;
			_viewManager = viewManager;
			_datasupport = datasupport;

			_viewScroll = new ViewScroll();
			AddComponent(_viewScroll);
			_viewScroll.SetParams(10, 80, 1000, 560, "Список игр");

			foreach (var item in Enumerable.Range(1, 7)) {
				var scrollItem = new GameNameScrollView();
				_viewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (item - 1) * 90 + 10, 950, 50, "item" + item);
			}
			_viewScroll.CalcScrollSize();
		}

		private void AddNewGame()
		{
			var a = new MiniGames();
			a.Id = 0;
			a.Name = "";
			a.VersionCode = 1;
			a.CodeName = "";
			a.Description = "";
			new DataEditor<MiniGames>().InitWindow(_viewManager, a, UpdateGame);
		}

		private void UpdateGame(MiniGames miniGame)
		{
			var a = 1;
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