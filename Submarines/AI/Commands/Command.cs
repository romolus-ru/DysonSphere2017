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
		public Action<Command> OnEndCommand;

		public virtual bool IsActive => true;

		public Command(Action<Command> onEndCommand = null)
		{
			OnEndCommand = onEndCommand;
		}

		/// <summary>
		/// Запускаем команду
		/// </summary>
		public virtual void Execute(TimeSpan elapsedTime) => OnEndCommand?.Invoke(this);
	}
}