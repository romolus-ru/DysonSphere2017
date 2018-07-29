using Engine;
using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	public class GameSectionScrollView : ScrollItem
	{
		private MiniGamesInfos _minigameInfo;
		public Action<MiniGamesInfos> OnEdit;
		public Action<MiniGamesInfos> OnSelect;

		public GameSectionScrollView(MiniGamesInfos miniGameInfos)
		{
			_minigameInfo = miniGameInfos;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var btnEdit = new ViewButton();
			AddComponent(btnEdit);
			btnEdit.InitButton(EditMiniGameSection, "Edit", "Редактировать", Keys.None);
			btnEdit.SetParams(20, 10, 60, 30, "EditMiniGame");
			btnEdit.InitTexture("textRB", "textRB");

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(SelectGameSection, "Select", "Выбрать", Keys.None);
			btnSelect.SetParams(90, 10, 120, 30, "Выбрать");
			btnSelect.InitTexture("textRB", "textRB");
		}

		private void EditMiniGameSection()
		{
			OnEdit?.Invoke(_minigameInfo);
		}

		private void SelectGameSection()
		{
			OnSelect?.Invoke(_minigameInfo);
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
