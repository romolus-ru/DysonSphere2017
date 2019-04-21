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
		public static MoveCommand Create(Action onEndCommand, SubmarineBase submarine, float targetAngle, Vector targetPos, float cruisingSpeedPercent = 100)
		{
			MoveCommand result = new MoveCommand(onEndCommand);
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
			var bezierPoints = new List<ScreenPoint>();
			bc.Bezier2D(basePoints, pathLength, bezierPoints);
			result.BasePoints = basePoints;
			result.BezierPoints = bezierPoints;

			// по этим точкам определяем углы поворота, скорость и расстояние которые надо пройти
			result.Segments = CreateSegments(bezierPoints, submarine.CurrentAngle, submarine.VCurrent);
			// отдельный метод, в котором проходим по точкам и вычисляем угол поворота и расстояние
			// создаём массив и вычисляем углы поворота (не превышающие максимального угла для текущего устройства)
			// если углы поворота не позволят переместиться в нужную точку то маркируем команду как возможно сбойную

			// сохраняем данные и возвращаем команду
			return result;
		}

		private static ScreenPoint CreatePoint(Vector point, float angle, float radius)
		{
			var ret = new ScreenPoint((int) point.X, (int) point.Y);
			ret.MovePolar(angle, radius);
			return ret;
		}

		private static List<MoveCommandSegment> CreateSegments(List<ScreenPoint> bezierPoints, float startAngle, float startSpeed)
		{
			List<MoveCommandSegment> ret = new List<MoveCommandSegment>();
			List<MoveCommandSegment> tmp = new List<MoveCommandSegment>();

			var spd = startSpeed;
			var cur = 0;
			var prev = -1;
			do {
				cur++;
				prev = cur - 1;
				var segment=new MoveCommandSegment();
				var p1 = bezierPoints[prev]; возможно надо поменять точки местами. первая точка должна быть текущая
					или написать цикл который будет перебиать точки вперед пока не найдётся нужная длина
				var p2 = bezierPoints[cur];
				float dist = p1.distanceTo(p2);
				float angle = (float)p1.AngleWith(p2);
				if (dist / spd > Constants.TimerInterval) {
					вычисляем угол и расстояние между текущей и следующей следующей точкой
				}


			} while (cur < bezierPoints.Count);


			// формируем основной список
			for (int i = 0; i < bezierPoints.Count - 1; i++) {
				var p1 = bezierPoints[i];
				var p2 = bezierPoints[i + 1];
				float dist = p1.distanceTo(p2);
				float angle = (float) p1.AngleWith(p2);
				tmp.Add(new MoveCommandSegment() {Distance = dist, Angle = angle, Speed = 0});
			}
			// корректируем список, объединяя или растягивая расстояния что бы действие происходило в 1 такт
			//смотрим какое расстояние от текущей точки до точки с которой работает
			//	усмтанавливаем растояние которое надо пройти в зависимости от растояния между точек и текущей скоростью
			//	если угол поворота слишком резкий - уменьшаем скоростью и ищем ближайшую точку из впередистоящих

			var currentSpeed = startSpeed;


			return ret;

		}
	}
}