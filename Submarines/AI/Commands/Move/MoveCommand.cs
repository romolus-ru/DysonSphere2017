using System;

namespace Submarines.AI.Commands.Move
{
	/// <summary>
	/// Команда перемещения из одной точки в другую
	/// </summary>
	internal class MoveCommand : Command
	{
		public MoveCommand(Action onEndCommand = null) 
			: base(onEndCommand)
		{
		}
	}
}