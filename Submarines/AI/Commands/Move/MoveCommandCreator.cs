using System;
using System.Collections.Generic;
using Engine.Visualization;
using Submarines.Submarines;
using Submarines.Utils;
using Engine.Visualization.Maths;

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
		public static MoveCommand Create(Action onEndCommand, SubmarineBase submarine, float targetAngle, Vector targetPos, float cruisingSpeedPercent=100)
		{
			MoveCommand result = new MoveCommand(onEndCommand);
			var basePoints = new List<ScreenPoint>();

			// данная команда должна подписаться на реакцию на столкновение что бы завершить обработку
			// считаем что скорость изменяется мгновенно до нужной
			// начальная точка (- разгон) - случайная длина прямолинейного движения (случайная траектория)
			// - движение (- торможение) - конечная точка

			// подлодка может не достигнуть конечной точки.

			// формируем 4 точки - начальная конечная и вспомогательные для них
			basePoints.Add(submarine.Position.ToScreenPoint());
			basePoints.Add(CreatePoint(submarine.Position, submarine.CurrentAngle, 50));
			basePoints.Add(CreatePoint(targetPos, targetAngle, -50));
			basePoints.Add(submarine.Position.ToScreenPoint());

			// по ним строим кривую безье
			var bc = new BezierCurve();
			var pathLength = 180*2;угол поворота разделенный на минимальный угол поворота
			var bezierPoints = new List<ScreenPoint>();
			bc.Bezier2D(basePoints, pathLength, bezierPoints);

			// по этим точкам определяем углы поворота и расстояние которое надо пройти
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

	}
}