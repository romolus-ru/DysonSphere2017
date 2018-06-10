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
		private TextSimple _header;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			X = 10;
			Y = visualizationProvider.CanvasHeight - 200;
			Width = 300;
			Height = 160;

			var header = new ViewText();
			AddComponent(header);
			var row = header.CreateTextRow();
			_header = header.AddText(row, System.Drawing.Color.White, null, "HEADER") as TextSimple;
			AchievementsChanged();
		}

		public void AchievementsChanged()
		{
			_currentTutorialStep = OnGetActiveTutorValue?.Invoke();
			if (_currentTutorialStep == null) {
				// TODO remove ViewTutorStep
				return;
			}
			_header.Text = _currentTutorialStep.Achieve.Description;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}
	}
}