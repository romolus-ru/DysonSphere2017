using Engine;
using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;

namespace EngineTools
{
	public class MiniGameInfoSelectWindow : FilteredScrollViewWindow
	{
		private Action<long> _selectedGame;
		private Action _cancel;
		private ViewManager _viewManager;
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
			_miniGame = miniGame;
			_selectedGame = selectedGame;
			_cancel = cancel;
			_viewManager = viewManager;
			_datasupport = datasupport;

			InitWindow("Выбор секции миниигры", viewManager, false);
		}

		protected override void InitScrollItems()
		{
			var i = 2;
			foreach (var game in _datasupport.GetMinigameInfos(_miniGame)) {
				var scrollItem = new GameSectionScrollView(game);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, game.IdMiniGamesInfos + " " + game.Section);
				scrollItem.OnEdit += EditMiniGameInfo;
				scrollItem.OnSelect += SelectSection;
				i++;
			}
		}

		private void SelectSection(MiniGamesInfos miniGameInfo)
		{

		}

		protected override void NewCommand()
		{
			var newMiniGame = new MiniGamesInfos();
			newMiniGame.IdMiniGamesInfos = 0;
			newMiniGame.ClassFile = null;
			newMiniGame.ClassName = null;
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

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_selectedGame = null;
			_cancel = null;
		}

		protected override void CancelCommand()
		{
			_cancel?.Invoke();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}
	}
}