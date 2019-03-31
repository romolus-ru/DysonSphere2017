using System;
using System.Collections.Generic;
using Engine.Visualization;

namespace Submarines.AI.Commands.Move
{
	/// <summary>
	/// Команда перемещения из одной точки в другую
	/// </summary>
	internal class MoveCommand : Command
	{
		public List<ScreenPoint> BasePoints;
		public List<ScreenPoint> BezierPoints;
		public MoveCommand(Action onEndCommand = null) 
			: base(onEndCommand)
		{
		}
	}
}