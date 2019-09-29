using Engine.Visualization;

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
		public static ViewManager ViewManager { get; private set; }
		public static void SetViewManager(ViewManager viewManager) { if (viewManager != null) ViewManager = viewManager; }

		public static void ShowHint(ViewComponent component, string hintText, string hintKeys = null)
		{
			ViewManager?.ShowHint(component, hintText, hintKeys);
		}

		public static void ShowBigMessage(string message)
		{
			ViewManager.ShowBigMessage(message);
		}
	}
}