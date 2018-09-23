using Engine.Helpers;
using System;

namespace Engine.Visualization
{
	/// <summary>
	/// Вывод подсказки
	/// </summary>
	public class ViewHint : ViewComponent
	{
		private string _hintText;
		private string _hintKeys;
		private DateTime _hintHideTime;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.OnModalStateChanged += ModalStateChanging;
		}

		protected override void ClearObject()
		{
			Input.OnModalStateChanged -= ModalStateChanging;
			base.ClearObject();
		}

		public void ShowHint(ViewComponent component, TimeSpan hintHideDelay, string hintText, string hintKeys)
		{
			if (string.IsNullOrEmpty(hintText))
				return;
			_hintText = hintText;
			_hintKeys = hintKeys != "None" ? hintKeys : null;
			X = component._xScreen;
			Y = component._yScreen;
			Height = component.Height;
			Width = component.Width;

			var f = VisualizationProvider.FontHeight / 2;
			var l = VisualizationProvider.TextLength(_hintText + " " + _hintKeys);

			if (X < 0) X = 0;
			if (Y < 0) Y = 0;
			if (X + l > VisualizationProvider.CanvasWidth) X = VisualizationProvider.CanvasWidth - l;
			if (Y + f > VisualizationProvider.CanvasHeight) Y = VisualizationProvider.CanvasHeight - f;
			_hintHideTime = DateTime.Now + hintHideDelay;
			Show();
		}

		public void HideHint()
		{
			_hintText = null;
			_hintKeys = null;
			Hide();
		}

		public void ModalStateChanging()
		{
			HideHint();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			GUIHelper.DrawHint(visualizationProvider, this, _hintText, _hintKeys);
			if (_hintHideTime <= DateTime.Now) HideHint();
		}
	}
}