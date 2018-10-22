using System;

namespace Engine.EventSystem
{
	/// <summary>
	/// Хранит событие заданного типа
	/// </summary>
	public abstract class EventHolderBase
	{
		public abstract void AddAction<T>(Action<T> action) where T : class;
		public abstract void RemoveAction<T>(Action<T> action) where T : class;
		public abstract void StartAction<T>(T value) where T: class;
		public abstract bool IsEmpty { get; }
	}
}