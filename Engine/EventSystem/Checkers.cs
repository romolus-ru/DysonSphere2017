using System;
using System.Collections.Generic;

namespace Engine.EventSystem
{
	/// <summary>
	/// Экспериментальная технология
	/// Добавляем обработчик который должен быть запущен в начале цикла обработки
	/// На данный момент используется в View что бы после инициализации перенастроить компоненты
	/// </summary>
	public static class Checkers
	{
		private static List<Action> _actionsOnce = new List<Action>();
		/// <summary>
		/// Добавленный метод выполнится один раз в начале цикла обработки
		/// </summary>
		/// <param name="action"></param>
		public static void AddToCheckOnce(Action action)
		{
			_actionsOnce.Add(action);
		}

		public static void CheckOnce()
		{
			if (_actionsOnce.Count == 0) return;
			var list = new List<Action>(_actionsOnce);
			_actionsOnce.Clear();
			foreach (var action in list) {
				action();
			}
		}
	}
}
