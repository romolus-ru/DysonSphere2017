using System;
using System.Collections.Generic;
using Engine.Visualization;
using Submarines.Submarines;
using Submarines.Utils;
using Engine.Visualization.Maths;
using Engine;

namespace Submarines.AI.Commands.Move
{
	/// <summary>
	/// Создаёт команду перемещения корабля
	/// </summary>
	internal class MoveCommandCreator
	{
		/// <summary>
		/// Создаём команду движения из текущей точки к заданной
		/// </summary>
		/// <param name="onEndCommand">Уведомление о завершении выполнения команды</param>
		/// <param name="submarine"></param>
		/// <param name="targetAngle">Конечный угол на который надо повернуться</param>
		/// <param name="targetPos">Координаты которых надо достичь</param>
		/// <param name="cruisingSpeedPercent">Крейсерская скорость в процентах</param>
		/// <returns></returns>
		public static MoveCommand Create(Action<Command> onEndCommand, SubmarineBase submarine, float targetAngle, Vector targetPos, float cruisingSpeedPercent = 100)
		{
			MoveCommand result = new MoveCommand(submarine as Submarine, onEndCommand);
			var basePoints = new List<ScreenPoint>();

			// данная команда должна подписаться на реакцию на столкновение что бы завершить обработку
			// считаем что скорость изменяется мгновенно до нужной
			// начальная точка (- разгон) - случайная длина прямолинейного движения (случайная траектория)
			// - движение (- торможение) - конечная точка

			// подлодка может не достигнуть конечной точки.
			// вычисляем целевой угол поворота
			var startPoint = submarine.Position.ToScreenPoint();
			var endPoint = targetPos.ToScreenPoint();
			var calculatedTargetAngle = startPoint.AngleWith(endPoint) - 90;

			// формируем 4 точки - начальная конечная и вспомогательные для них
			// третью точку вычислить
			// текущую третью точку надо отразить по линии второй и 4й точек
			// пока остановимся на 3х точках
			basePoints.Add(startPoint);
			basePoints.Add(CreatePoint(submarine.Position, submarine.CurrentAngle, 250));
			//basePoints.Add(CreatePoint(targetPos, (float) calculatedTargetAngle, -250));
			basePoints.Add(endPoint);

			// по ним строим кривую безье
			var bc = new BezierCurve();
			var pathLength = (int) (submarine.ManeuverDevice.MaxSteeringPerSecond * 180);
			var bezierPoints = new List<Vector>();
			bc.Bezier2D(basePoints, pathLength, (x, y) => bezierPoints.Add(new Vector((float) x, (float) y, 0)));
			result.BasePoints = basePoints;
			result.BezierPoints = bezierPoints;

			result.Simplified = new List<Vector>();
			result.Segments = new List<MoveCommandSegment>();

			// по этим точкам определяем углы поворота, скорость и расстояние которые надо пройти
			CreateSimplified(
				result.Simplified,
				result.Segments,
				bezierPoints,
				submarine.CurrentAngle,
				submarine.Engine.Speed //0.5f //submarine.VCurrent
			);

			// сохраняем данные и возвращаем команду
			return result;
		}

		private static ScreenPoint CreatePoint(Vector point, float angle, float radius)
		{
			var ret = new ScreenPoint((int) point.X, (int) point.Y);
			ret.MovePolar(angle, radius);
			return ret;
		}

		private static void CreateSimplified(List<Vector> simplified, List<MoveCommandSegment> segments,
			List<Vector> bezierPoints, float startAngle, float startSpeed)
		{
			bool end;
			var currentPoint = new Vector(bezierPoints[0].X, bezierPoints[0].Y, 0);
			var nextPointNum = 0;
			var currentSpeed = startSpeed; // const
			var currentAngle = startAngle;

			do {
				float dist1 = 0;
				Vector nextPoint = Vector.Zero();
				end = false;

				var foundedNextPoint = false;
				do {
					if (nextPointNum >= bezierPoints.Count)
						break;
					var nextPointS = bezierPoints[nextPointNum];
					nextPoint = new Vector(nextPointS.X, nextPointS.Y, 0);
					dist1 = currentPoint.DistanceTo(nextPoint);
					if (dist1 / currentSpeed >= Constants.TimerInterval) {
						foundedNextPoint = true;
					}
					else
						nextPointNum++;
				} while (!foundedNextPoint);

				if (foundedNextPoint) {
					var angle1 = (float) currentPoint.AngleWith(nextPoint);
					if (dist1 > Constants.TimerInterval * currentSpeed)
						dist1 = Constants.TimerInterval * currentSpeed;
					currentPoint = currentPoint.MovePolar(angle1, dist1);
					simplified.Add(currentPoint);
					var segment = new MoveCommandSegment() {
						Angle = angle1,
						Speed = currentSpeed,
						Distance = dist1,
						Time = new TimeSpan(0, 0, 0, 0, (int) (dist1 / currentSpeed))
					};
					segments.Add(segment);
					currentAngle = angle1;
				}
				else
					end = true;

			} while (!end);
		}

	}
}