using Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public void ShowHint(TimeSpan hintHideDelay, string hintText, string hintKeys)
		{
			_hintText = hintText;
			_hintKeys = hintKeys;
			_hintHideTime = DateTime.Now + hintHideDelay;
			Show();
		}

		public void HideHint()
		{
			_hintText = null;
			_hintKeys = null;
			Hide();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			GUIHelper.ShowHint(visualizationProvider, this, _hintText, _hintKeys);
			if (_hintHideTime <= DateTime.Now) HideHint();
		}
	}
}