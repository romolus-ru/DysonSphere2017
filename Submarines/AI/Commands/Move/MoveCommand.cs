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
		public List<Vector> Simplified;
		public List<MoveCommandSegment> Segments;
		private int _currentNum = -1;
		public MoveCommand(Action onEndCommand = null) 
			: base(onEndCommand)
		{
		}

		public MoveCommandSegment GetCommand()
			=>
				_currentNum < 0 || _currentNum >= Segments.Count
					? null
					: Segments[_currentNum];

		public override void Execute()
		{
			_currentNum++;
			if (_currentNum >= Segments.Count) {
				base.Execute();
				return;
			}
		}
	}
}