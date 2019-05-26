using System;
using System.Collections.Generic;
using Engine;
using Engine.Visualization;
using Submarines.Submarines;

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
		private Submarine _submarine;

		public MoveCommand(Submarine submarine, Action<Command> onEndCommand = null) 
			: base(onEndCommand)
		{
			_submarine = submarine;
		}

		public MoveCommandSegment GetCommand()
			=>
				_currentNum < 0 || _currentNum >= Segments.Count
					? null
					: Segments[_currentNum];

		public override void Execute(TimeSpan elapsedTime)
		{
			переделать
				брать текущий сегмент в котором должно быть время движения и скорость
				выставлять параметры в зависимости от сегмента и пришедшего времени
				
			_currentNum++;
			if (_currentNum >= Segments.Count) {
				base.Execute(elapsedTime);
				return;
			}
			
			var segment = Segments[_currentNum];

			
			_submarine.SetSpeed(speedPercent);
			_submarine.AddSteering(angle);
			//_submarine.SetAutoPosition(pos, Constants.TimerInterval * segment.Speed * 100, -segment.Angle);

		}
	}
}