using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Helpers
{
	/// <summary>
	/// Обеспечивает глобальный доступ к вспомогательным функциям, таким как:
	/// - вывод подсказок
	/// - вывод сообщения в центре экрана
	/// - вывод отладочной информации
	/// </summary>
	public static class ViewHelper
	{
		private static ViewManager _viewManager;
		public static void SetViewManager(ViewManager viewManager) { if (viewManager != null) _viewManager = viewManager; }

		public static void ShowHint(ViewComponent component, string hintText, string hintKeys = null)
		{
			_viewManager?.ShowHint(component, hintText, hintKeys);
		}

		public static void ShowBigMessage(string message)
		{
			_viewManager.ShowBigMessage(message);
		}
	}
}