using System;
using System.Collections.Generic;
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
		private TimeSpan _currentSpan = new TimeSpan();

		public MoveCommand(Submarine submarine, Action<Command> onEndCommand = null) 
			: base(onEndCommand)
		{
			_submarine = submarine;
		}

		public override void Execute(TimeSpan elapsedTime)
		{
			переделать
			if (_currentNum == -1) {
				_currentNum = 0;
			}
				
			вычисляем время необходимое для движения
				двигаемся на это время
				запоминаем сколько уже прошли для этого сегмента


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

		private void ProcessMove(MoveCommandSegment segment, TimeSpan processingTime)
		{
			_submarine.SetSpeed(segment.Speed);
			_submarine.AddSteering(segment.Angle);
			_submarine.CalculateMovement(processingTime);
		}

	}
}