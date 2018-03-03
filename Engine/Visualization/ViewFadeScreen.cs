using Engine.Helpers;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Заполняет весь экран прямоугольником с небольшой прозрачностью - для акцентрирования модальных диалоговых окон
	/// </summary>
	public class ViewFadeScreen : ViewComponent
	{
		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			visualizationProvider.OnResizeWindow += OnResizeWindow;
			OnResizeWindow(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		private void OnResizeWindow(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(GUIHelper.BackgroundFadeColor);
			visualizationProvider.Box(0, 0, Width, Height);
		}
	}
}
