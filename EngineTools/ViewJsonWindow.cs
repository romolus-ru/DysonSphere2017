using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;

namespace EngineTools
{
	/// <summary>
	/// Просмотр джисон строк содержащей записи класса типа EventBase
	/// </summary>
	public class ViewJsonWindow<T> : FilteredScrollViewWindow where T : class
	{
		private Action<string> _saveData;
		private Action _cancel;
		private ViewManager _viewManager;
		private MiniGamesInfos _miniGameInfo;
		private T _type;
		private List<T> _values;

		public void InitWindow(ViewManager viewManager, MiniGames miniGame, MiniGamesInfos miniGameInfo, Action<string> saveData, Action cancel)
		{
			_saveData = saveData;
			_cancel = cancel;
			_miniGameInfo = miniGameInfo;

			InitData(_miniGameInfo);
			InitWindow("Редактирование " + miniGame.Name + " - " + miniGameInfo.Section, viewManager, false);

			//btnSelect.InitButton(Entered, "ok", "сохранить", Keys.Enter);
			//btnCancel.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			//btnNew.InitButton(AddNewDataRow, "New", "New", Keys.N);
		}

		private void InitData(MiniGamesInfos miniGameInfo)
		{
			// получить список классов из строки
			тут
		}

		protected override void InitScrollItems()
		{
			// получаем список элементов
			// заполняем скролл
			//var type1 = typeof(EventBase);
			//var type2 = typeof(EventBaseRowScrollView<>);
			//var type3 = type2.MakeGenericType(new Type[] { type1 });
			//var scrollItem = Activator.CreateInstance(type3);
		}

		private void UpdateScroll()
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
			_saveData?.Invoke(null);
			CloseWindow();
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_saveData = null;
			_cancel = null;
			_viewManager = null;
			_miniGameInfo = null;
		}

		protected override void CancelCommand()
		{
			_cancel?.Invoke();
		}
	}
}