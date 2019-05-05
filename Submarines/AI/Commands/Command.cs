using System;

namespace Submarines.AI.Commands
{
	/// <summary>
	/// Команда для корабля/снаряда
	/// </summary>
	internal class Command
	{
		/// <summary>
		/// Выполнение команды завершено
		/// </summary>
		public Action OnEndCommand;

		public Command(Action onEndCommand = null)
		{
			OnEndCommand = onEndCommand;
		}

		/// <summary>
		/// Запускаем команду
		/// </summary>
		public virtual void Execute() => OnEndCommand?.Invoke();
	}
}