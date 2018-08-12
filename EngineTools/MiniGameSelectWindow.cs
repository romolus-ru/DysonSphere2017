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
		private DataSupportBase _dataSupport;

		public void InitWindow(ViewManager viewManager, DataSupportBase dataSupport, Action<long> selectedGame, Action cancel)
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
			_dataSupport = dataSupport;

			_viewScroll = new ViewScroll();
			AddComponent(_viewScroll);
			_viewScroll.SetParams(10, 80, 1000, 560, "Список игр");

			var i = 2;
			foreach (var game in _dataSupport.GetMinigames()) {
				var scrollItem = new GameNameScrollView(game);
				_viewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, game.Id + " " + game.Name);
				scrollItem.OnEdit += EditMiniGame;
				scrollItem.OnSelect += SelectSection;
				i++;
			}
			_viewScroll.CalcScrollSize();
		}

		private void SelectSection(MiniGames miniGame)
		{
			new MiniGameInfoSelectWindow().InitWindow(_viewManager, _dataSupport, miniGame, null, null);
		}

		private void AddNewGame()
		{
			var newMiniGame = new MiniGames();
			newMiniGame.Id = 0;
			newMiniGame.Name = "";
			newMiniGame.VersionCode = 1;
			newMiniGame.CodeName = "";
			newMiniGame.Description = "";
			EditMiniGame(newMiniGame);
		}

		private void EditMiniGame(MiniGames minigame)
		{
			new DataEditor<MiniGames>().InitWindow(_viewManager, minigame, UpdateGame);
		}

		private void UpdateGame(MiniGames miniGame)
		{
			_dataSupport.AddMinigame(miniGame);
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