using Engine;
using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Windows.Forms;

namespace EngineTools
{
	public class MiniGameSelectWindow : FilteredScrollViewWindow
	{
		private Action<long> _selectedGame;
		private Action _cancel;
		private DataSupportBase _dataSupport;

		public void InitWindow(ViewManager viewManager, DataSupportBase dataSupport, Action<long> selectedGame, Action cancel)
		{
			_selectedGame = selectedGame;
			_cancel = cancel;
			_dataSupport = dataSupport;

			InitWindow("Выбор миниигры", viewManager, showOkButton: true);
		}

		protected override void InitScrollItems()
		{
			var i = 1;
			foreach (var game in _dataSupport.GetMinigames()) {
				var scrollItem = new GameNameScrollView(game);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, game.Id + " " + game.Name);
				scrollItem.OnEdit += EditMiniGame;
				scrollItem.OnSelect += SelectSection;
				i++;
			}
		}

		protected override void InitButtonOk(ViewButton btnOk)
		{
			base.InitButtonOk(btnOk);
			btnOk.Caption = "ok";
			btnOk.Hint = "выбрать";
		}

		private void SelectSection(MiniGames miniGame)
		{
			new MiniGameInfoSelectWindow().InitWindow(ViewManager, _dataSupport, miniGame, null, null);
		}

		protected override void NewCommand()
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
			new DataEditor<MiniGames>().InitWindow(ViewManager, minigame, UpdateGame);
		}

		private void UpdateGame(MiniGames miniGame)
		{
			_dataSupport.AddMinigame(miniGame);
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
	}
}