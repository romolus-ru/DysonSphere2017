using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Visualization
{
	/// <summary>
	/// Окно ожидания с возможностью прервать операцию
	/// </summary>
	public class WaitWindow : ViewModalWindow
	{
		private Action _cancelOperation;
		private ViewManager _viewManager;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.AddKeyActionPaused(Cancel, Keys.Enter);
		}

		protected override void ClearObject()
		{
			Input.RemoveKeyActionPaused(Cancel, Keys.Enter);
			base.ClearObject();
		}

		public void InitWindow(ViewManager viewManager, string message, Action calcelOperation, string fontName = null)
		{
			_viewManager = viewManager;
			_viewManager.AddViewModal(this);
			_cancelOperation = calcelOperation;

			var wh = 50;
			var ww = 250;
			var h = VisualizationProvider.CanvasHeight / 4;
			var w = VisualizationProvider.CanvasWidth / 2;
			SetParams(w - ww / 2, h - wh / 2, ww, wh, "waiting");
			InitTexture("textRB", 10);

			var label = ViewLabel.Create(20, 05, Color.RosyBrown, message, fontName);
			AddComponent(label);

			var btn2 = new ViewButton();
			AddComponent(btn2);
			btn2.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btn2.SetParams(20, wh - 25, 220, 20, "btnCancel");
			btn2.InitTexture("textRB", "textRB");
		}

		public void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			_cancelOperation = null;
			_viewManager = null;
		}

		/// <summary>
		/// Для закрытия окна по кнопке или извне
		/// </summary>
		public void Cancel()
		{
			_cancelOperation?.Invoke();
			CloseWindow();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			//visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			//visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}

	}
}