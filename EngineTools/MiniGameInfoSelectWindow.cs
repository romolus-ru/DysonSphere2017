using Engine;
using Engine.Data;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	public class MiniGameInfoSelectWindow : ViewModalWindow
	{
		private Action<long> _selectedGame;
		private Action _cancel;
		private ViewManager _viewManager;
		private ViewInput _filter;
		private ViewScroll _viewScroll;
		private DataSupportBase _datasupport;
		private MiniGames _miniGame;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
		}

		protected override void ClearObject()
		{
			base.ClearObject();
		}

		public void InitWindow(ViewManager viewManager, DataSupportBase datasupport, MiniGames miniGame, Action<long> selectedGame, Action cancel)
		{
			viewManager.AddViewModal(this);
			SetParams(150, 150, 1200, 700, "Выбор секции миниигры");
			InitTexture("textRB", 10);

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btnSelect.SetParams(50, 670, 80, 25, "btnCancel");
			btnSelect.InitTexture("textRB", "textRB");

			var btnNew = new ViewButton();
			AddComponent(btnNew);
			btnNew.InitButton(AddNewGameSection, "New", "New", Keys.N);
			btnNew.SetParams(20, 20, 80, 25, "btnNewGameSection");
			btnNew.InitTexture("textRB", "textRB");

			_filter = new ViewInput();
			AddComponent(_filter);
			_filter.SetParams(140, 30, 500, 40, "filter for games");
			_filter.InputAction("");

			_miniGame = miniGame;
			_selectedGame = selectedGame;
			_cancel = cancel;
			_viewManager = viewManager;
			_datasupport = datasupport;

			_viewScroll = new ViewScroll();
			AddComponent(_viewScroll);
			_viewScroll.SetParams(10, 80, 1000, 560, "Список игр");

			var i = 2;
			foreach (var game in _datasupport.GetMinigameInfos(_miniGame)) {
				var scrollItem = new GameSectionScrollView(game);
				_viewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, game.IdMiniGamesInfos + " " + game.Section);
				scrollItem.OnEdit += EditMiniGameInfo;
				scrollItem.OnSelect += SelectSection;
				i++;
			}
			_viewScroll.CalcScrollSize();
		}

		private void SelectSection(MiniGamesInfos miniGameInfo)
		{

		}

		private void AddNewGameSection()
		{
			var newMiniGame = new MiniGamesInfos();
			newMiniGame.IdMiniGamesInfos = 0;
			newMiniGame.CollectClassId = 0;
			newMiniGame.MiniGameId = _miniGame.Id;
			newMiniGame.Section = "";
			newMiniGame.Values = "";
			EditMiniGameInfo(newMiniGame);
		}

		private void EditMiniGameInfo(MiniGamesInfos minigameInfo)
		{
			new DataEditor<MiniGamesInfos>().InitWindow(_viewManager, minigameInfo, UpdateGameInfo, dataSupport: _datasupport);
		}

		private void UpdateGameInfo(MiniGamesInfos miniGameInfo)
		{
			_datasupport.AddMinigameInfo(miniGameInfo);
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