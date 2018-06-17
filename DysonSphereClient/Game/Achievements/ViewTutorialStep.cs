using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Visualization.Text;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Вывод ачивки в виде шага туториала
	/// </summary>
	public class ViewTutorialStep:ViewComponent
	{
		public Func<GameAchievementValue> OnGetActiveTutorValue;

		private GameAchievementValue _currentTutorialStep;
		private ViewText _headerMain;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			X = 10;
			Y = visualizationProvider.CanvasHeight - 200;
			Width = 300;
			Height = 160;

			_headerMain = new ViewText();
			AddComponent(_headerMain);
			_headerMain.SetParams(0, 0, Width, 30, "Header");
			_headerMain.CreateSplitedTextAuto(System.Drawing.Color.White, null, "HEADER");
			_headerMain.CalculateTextPositions();
			AchievementsChanged();
		}

		public void AchievementsChanged()
		{
			_currentTutorialStep = OnGetActiveTutorValue?.Invoke();
			if (_currentTutorialStep == null) {
				// TODO remove ViewTutorStep
				this.Hide();
				return;
			}
			_headerMain.ClearTexts();
			_headerMain.CreateSplitedTextAuto(System.Drawing.Color.White, null, _currentTutorialStep.Achieve.Description);
			_headerMain.CalculateTextPositions();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}
	}
}