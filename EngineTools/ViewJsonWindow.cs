using Engine.Data;
using Engine.EventSystem.Event;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EngineTools
{
	/// <summary>
	/// Просмотр джисон строк содержащей записи класса типа EventBase
	/// </summary>
	public class ViewJsonWindow : ViewModalWindow
	{
		private Action<MiniGamesInfos> _saveData;
		private Action _cancel;
		private ViewManager _viewManager;
		private ViewInput _filter;
		private ViewScroll _viewScroll;
		private MiniGamesInfos _miniGameInfo;
		private List<EventBase> _data;

		public void InitWindow(ViewManager viewManager, MiniGames miniGame, MiniGamesInfos miniGameInfo , Action<MiniGamesInfos> saveData, Action cancel)
		{
			viewManager.AddViewModal(this);
			SetParams(150, 150, 1200, 700, "Редактирование " + miniGame.Name + " - " + miniGameInfo.Section);
			InitTexture("textRB", 10);

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(Entered, "ok", "сохранить", Keys.Enter);
			btnSelect.SetParams(50, 670, 80, 25, "btnSelect");
			btnSelect.InitTexture("textRB", "textRB");

			var btnCancel = new ViewButton();
			AddComponent(btnCancel);
			btnCancel.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btnCancel.SetParams(150, 670, 80, 25, "btnCancel");
			btnCancel.InitTexture("textRB", "textRB");

			var btnNew = new ViewButton();
			AddComponent(btnNew);
			btnNew.InitButton(AddNewDataRow, "New", "New", Keys.N);
			btnNew.SetParams(20, 20, 80, 25, "btnNewGame");
			btnNew.InitTexture("textRB", "textRB");

			_filter = new ViewInput();
			AddComponent(_filter);
			_filter.SetParams(140, 30, 500, 40, "filter");
			_filter.InputAction("");

			_saveData = saveData;
			_cancel = cancel;
			_viewManager = viewManager;
			_miniGameInfo = miniGameInfo;

			_viewScroll = new ViewScroll();
			AddComponent(_viewScroll);
			_viewScroll.SetParams(10, 80, 1000, 560, "Список данных");
			FillScrollView(miniGameInfo);
		}

		private void FillScrollView(MiniGamesInfos miniGameInfo)
		{
			/*var i = 2;
			foreach (var game in _dataSupport.GetMinigames()) {
				var scrollItem = new GameNameScrollView(game);
				_viewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, game.Id + " " + game.Name);
				scrollItem.OnEdit += EditMiniGame;
				scrollItem.OnSelect += SelectSection;
				i++;
			}
			_viewScroll.CalcScrollSize();
			*/
		}

		private void AddNewDataRow()
		{
			/*var newMiniGame = new MiniGames();
			newMiniGame.Id = 0;
			newMiniGame.Name = "";
			newMiniGame.VersionCode = 1;
			newMiniGame.CodeName = "";
			newMiniGame.Description = "";
			EditMiniGame(newMiniGame);
			*/
		}

		private void Entered()
		{
			// сохранить в джисон, сохранить в объекте и отправить объект на сохранение в базу
			_saveData?.Invoke(_miniGameInfo);
			CloseWindow();
		}

		private void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			_saveData = null;
			_cancel = null;
			_viewManager = null;
			_miniGameInfo = null;
		}

		private void Cancel()
		{
			_cancel?.Invoke();
			CloseWindow();
		}
	}
}