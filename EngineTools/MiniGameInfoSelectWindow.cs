using Engine;
using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EngineTools
{
	public class MiniGameInfoSelectWindow : FilteredScrollViewWindow
	{
		private Action<long> _selectedGame;
		private Action _cancel;
		private DataSupportBase _datasupport;
		private MiniGames _miniGame;

		public void InitWindow(ViewManager viewManager, DataSupportBase datasupport, MiniGames miniGame, Action<long> selectedGame, Action cancel)
		{
			_miniGame = miniGame;
			_selectedGame = selectedGame;
			_cancel = cancel;
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
			var type1 = ToolsCollectorHelper.GetTypeFromFile(miniGameInfo.ClassFile, miniGameInfo.ClassName);
			if (type1 == null) {
				StateEngine.Log?.AddLog("class not found " + miniGameInfo.ClassName + " in " + miniGameInfo.ClassFile);
				return;
			}

			var type2 = typeof(ViewJsonWindow<>);
			var windowType = type2.MakeGenericType(new Type[] { type1 });
			var window = Activator.CreateInstance(windowType);
			IEnumerable<MethodInfo> ms = windowType.GetMethods().Where(mv => mv.Name == "InitWindow");
			MethodInfo m = null;
			foreach (var method in ms) {
				var parameters = method.GetParameters();
				if (parameters.Length < 5) continue;
				var p2 = parameters[2];
				if (p2.Name == "miniGameInfo")
					m = method;
			}
			if (m != null) {
				//InitWindow(ViewManager viewManager, MiniGames miniGame, MiniGamesInfos miniGameInfo, Action<string> saveData, Action cancel)
				object[] parametersArray = { ViewManager, _miniGame, miniGameInfo, (Action<MiniGamesInfos, string>)SaveSection, null };
				m.Invoke(window, parametersArray);
			} else
				StateEngine.Log.AddLog("InitWindow in ViewJsonWindow not found");
		}

		private void SaveSection(MiniGamesInfos miniGameInfos, string newValue)
		{
			// сохранить обновленные данные в секции
			miniGameInfos.Values = newValue;
			_datasupport.SaveChanges();
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
			new DataEditor<MiniGamesInfos>().InitWindow(ViewManager, minigameInfo, UpdateGameInfo, dataSupport: _datasupport);
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