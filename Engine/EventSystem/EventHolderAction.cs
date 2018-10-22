using System;

namespace Engine.EventSystem
{
	/// <summary>
	/// Хранилище для Action(T)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EventHolderAction<T> : EventHolderBase where T : class
	{
		private Action<T> _action = null;

		private void AddActionM(Action<T> action)
		{
			_action += action;
		}

		private void RemoveActionM(Action<T> action)
		{
			_action -= action;
		}

		public override void AddAction<T1>(Action<T1> action)
		{
			AddActionM(action as Action<T>);
		}

		public override void RemoveAction<T1>(Action<T1> action)
		{
			RemoveActionM(action as Action<T>);
		}

		public override void StartAction<T1>(T1 value)
		{
			_action?.Invoke(value as T);
		}

		public override bool IsEmpty {
			get {
				if (_action == null)
					return false;
				return _action.GetInvocationList().Length == 0;
			}
		}
	}
}