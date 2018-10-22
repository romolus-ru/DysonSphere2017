using System;
using System.Collections.Generic;

namespace Engine.EventSystem
{
	/// <summary>
	/// Позволяет добавлять ссылки на Action и вызывать их
	/// </summary>
	/// <remarks>Возможно не самая лучшая реализация</remarks>
	public class EventsHolder
	{
		private Dictionary<Type, Dictionary<string, EventHolderBase>> _actions = new Dictionary<Type, Dictionary<string, EventHolderBase>>();

		public void AddAction<T>(Action<T> action, string actionName="_default") where T : class
		{
			Type t = GetGenericType(action);
			EventHolderBase e = null;
			Dictionary<string, EventHolderBase> dict = null;
			if (!_actions.ContainsKey(t)) {
				dict = new Dictionary<string, EventHolderBase>();
				_actions.Add(t, dict);
			} else
				dict = _actions[t];

			if (!dict.ContainsKey(actionName)) {
				e = new EventHolderAction<T>();
				dict.Add(actionName, e);
			} else e = dict[actionName];

			e.AddAction(action);
		}

		public void RemoveAction<T>(Action<T> action, string actionName="_default") where T:class
		{
			Type t = GetGenericType(action);
			if (!_actions.ContainsKey(t))
				return;
			var dict = _actions[t];
			if (!dict.ContainsKey(actionName))
				return;

			var e = dict[actionName];
			e.RemoveAction<T>(action);
			if (e.IsEmpty)
				dict[actionName] = null;

			if (dict.Count == 0)
				_actions[t] = null;
		}

		private Type GetGenericType<T>(Action<T> action) where T : class
		{
			var t1 = action.GetType();
			var pr= t1.GetGenericArguments();
			return pr[0];
		}

		public void StartAction<T1>(T1 value, string actionName="_default") where T1 : class
		{
			Type t = value.GetType();
			if (!_actions.ContainsKey(t))
				return;

			var dict = _actions[t];
			if (!dict.ContainsKey(actionName))
				return;

			dict[actionName].StartAction(value as T1);
		}
	}
}