using Engine;
using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EngineTools
{
	public class GameNameScrollView : ScrollItem
	{
		private MiniGames _minigame;
		public Action<MiniGames> OnEdit;
		public Action<MiniGames> OnSelect;

		public GameNameScrollView(MiniGames miniGame)
		{
			_minigame = miniGame;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var btnEdit = new ViewButton();
			AddComponent(btnEdit);
			btnEdit.InitButton(EditMiniGame, "Edit", "Редактировать", Keys.None);
			btnEdit.SetParams(20, 10, 60, 30, "EditMiniGame");
			btnEdit.InitTexture("textRB", "textRB");

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(SelectGameInfo, "Select", "Выбрать", Keys.None);
			btnSelect.SetParams(90, 10, 120, 30, "Выбрать");
			btnSelect.InitTexture("textRB", "textRB");
		}

		private void EditMiniGame()
		{
			OnEdit?.Invoke(_minigame);
		}

		private void SelectGameInfo()
		{
			OnSelect?.Invoke(_minigame);
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(
				CursorOver
				? Color.YellowGreen
				: Color.Red);
			vp.Rectangle(X, Y, Width, Height);

			vp.SetColor(Color.White);
			vp.Print(X + 250, Y + 15, Name);
		}
	}
}
