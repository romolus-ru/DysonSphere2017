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

			result.Simplified = CreateSimplified(bezierPoints,
				submarine.CurrentAngle, 
				0.5f//submarine.VCurrent
				);
			// по этим точкам определяем углы поворота, скорость и расстояние которые надо пройти
			result.Segments = CreateSegments(bezierPoints, 
				submarine.CurrentAngle, submarine.VCurrent, 
				90 /*submarine.ManeuverDevice.MaxSteeringPerSecond*/);
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

		private static List<Vector> CreateSimplified(List<ScreenPoint> bezierPoints, float startAngle, float startSpeed)
		{
			List<Vector> ret = new List<Vector>();

			bool end;

			var currentPoint = new Vector(bezierPoints[0].X, bezierPoints[0].Y, 0);
			var nextPointNum = 0;
			var currentSpeed = startSpeed;// const
			var currentAngle = startAngle;// not limited

			do {
				float dist1 = 0;
				Vector nextPoint = Vector.Zero();
				end = false;
				if (ret.Count > 200) {
					var a = 1;
				}

				var foundedNextPoint = false;
				большое расхождение
					сохранять точки в файл, посмотреть что с ними. возможно расхождение из-за точности
				do {
					nextPointNum++;
					if (nextPointNum >= bezierPoints.Count)
						break;
					var nextPointS = bezierPoints[nextPointNum];
					nextPoint = new Vector(nextPointS.X, nextPointS.Y, 0);
					dist1 = currentPoint.DistanceTo(nextPoint);
					if (dist1 / currentSpeed >= Constants.TimerInterval) {
						foundedNextPoint = true;
					}

				} while (!foundedNextPoint);

				if (foundedNextPoint) {
					var angle1 = (float) currentPoint.AngleWith(nextPoint);
					if (angle1 > 2) {
						var a = 1;
					}

					currentAngle += angle1;
					if (dist1 > Constants.TimerInterval * currentSpeed)
						dist1 = Constants.TimerInterval * currentSpeed;
					currentPoint = currentPoint.MovePolar(currentAngle, dist1);
					ret.Add(currentPoint);
				}
				else
					end = true;

			} while (!end);

			return ret;

		}

		private static List<MoveCommandSegment> CreateSegments(List<ScreenPoint> bezierPoints, float startAngle, 
			float startSpeed, float angleLimit)
		{
			List<MoveCommandSegment> ret = new List<MoveCommandSegment>();

			//bool end;

			//var currentPoint = bezierPoints[0].Clone();
			//var nextPointNum = 1;
			//var currentSpeed = startSpeed;
			//var currentAngle = startAngle;
			//float angle1;

			//do {

			//	float dist1;
			//	ScreenPoint nextPoint;
			//	end = false;

			//	var foundedNextPoint = false;
			//	do {
			//		nextPoint = bezierPoints[nextPointNum];
			//		dist1 = currentPoint.distanceTo(nextPoint);
			//		if (dist1 / currentSpeed > Constants.TimerInterval) {
			//			foundedNextPoint = true;
			//		} else {
			//			nextPointNum++;
			//			if (nextPointNum + 1 >= bezierPoints.Count)
			//				break;
			//		}
			//	} while (!foundedNextPoint);

			//	angle1 = (float) currentPoint.AngleWith(nextPoint);
			//	if (Math.Abs(angle1) > angleLimit) {
			//		angle1 = angleLimit * Math.Sign(angle1);
			//	}

			//	currentAngle += angle1;
			//	var segment = new MoveCommandSegment() {Distance = dist1/currentSpeed, Angle = currentAngle, Speed = currentSpeed};
			//	ret.Add(segment);
			//	currentPoint.MovePolar(currentAngle, dist1/currentSpeed);

			//	if (!foundedNextPoint)
			//		end = true;

			//} while (!end);

			// останавливаем
			//ret.Add(new MoveCommandSegment() {Angle = currentAngle, Distance = 0, Speed = 0});

			//List<MoveCommandSegment> tmp = new List<MoveCommandSegment>();

			//var spd = startSpeed;
			//var cur = 0;
			//var prev = -1;
			//do {
			//	cur++;
			//	prev = cur - 1;
			//	var segment=new MoveCommandSegment();
			//	var p1 = bezierPoints[prev]; возможно надо поменять точки местами. первая точка должна быть текущая
			//		или написать цикл который будет перебиать точки вперед пока не найдётся нужная длина
			//	var p2 = bezierPoints[cur];
			//	float dist = p1.distanceTo(p2);
			//	float angle = (float)p1.AngleWith(p2);
			//	if (dist / spd > Constants.TimerInterval) {
			//		вычисляем угол и расстояние между текущей и следующей следующей точкой
			//	}


			//} while (cur < bezierPoints.Count);


			//// формируем основной список
			//for (int i = 0; i < bezierPoints.Count - 1; i++) {
			//	var p1 = bezierPoints[i];
			//	var p2 = bezierPoints[i + 1];
			//	float dist = p1.distanceTo(p2);
			//	float angle = (float) p1.AngleWith(p2);
			//	tmp.Add(new MoveCommandSegment() {Distance = dist, Angle = angle, Speed = 0});
			//}
			//// корректируем список, объединяя или растягивая расстояния что бы действие происходило в 1 такт
			////смотрим какое расстояние от текущей точки до точки с которой работает
			////	усмтанавливаем растояние которое надо пройти в зависимости от растояния между точек и текущей скоростью
			////	если угол поворота слишком резкий - уменьшаем скоростью и ищем ближайшую точку из впередистоящих

			//var currentSpeed = startSpeed;


			return ret;

		}
	}
}