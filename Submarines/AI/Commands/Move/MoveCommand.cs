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
			while (elapsedTime.TotalMilliseconds > 0) {

				if (_currentSpan.TotalMilliseconds <= 0) {
					_currentNum++;
					if (_currentNum >= Segments.Count) { // выходим
						base.Execute(elapsedTime);
						return;
					}

					_currentSpan = Segments[_currentNum].Time;
				}

				var segment = Segments[_currentNum];
				if (_currentSpan <= elapsedTime) { // время сегмента меньше чем время для обработки
					// двигаемся оставшееся время и смотрим что дальше происходит
					ProcessMove(segment, _currentSpan);
					elapsedTime -= _currentSpan;
					_currentSpan = TimeSpan.Zero;
					continue;
				}

				// время сегмента больше чем время на обработку
				ProcessMove(segment, elapsedTime);
				_currentSpan -= elapsedTime;
				elapsedTime = TimeSpan.Zero;
			}

		}

		private void ProcessMove(MoveCommandSegment segment, TimeSpan processingTime)
		{
			_submarine.SetSpeed(segment.Speed);
			_submarine.AddSteering(segment.Angle);
			_submarine.CalculateMovement(processingTime);
		}

	}
}