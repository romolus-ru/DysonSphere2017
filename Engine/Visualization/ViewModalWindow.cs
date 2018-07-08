using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Visualization
{
	/// <summary>
	/// Модальное окно
	/// </summary>
	public class ViewModalWindow : ViewWindow
	{
		private ViewFadeScreen _fadeScreen;
		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.AddCursorAction(CursorHandler);
			_fadeScreen = new ViewFadeScreen();
			_fadeScreen.Init(visualizationProvider, input);
		}

		protected override void ClearObject()
		{
			Input.RemoveCursorAction(CursorHandler);
			base.ClearObject();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			_fadeScreen.DrawObject(visualizationProvider);
			base.DrawObject(visualizationProvider);
		}
	}
}
