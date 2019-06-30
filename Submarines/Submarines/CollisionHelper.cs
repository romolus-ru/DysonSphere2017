using System.Collections.Generic;
using Submarines.Geometry;

namespace Submarines.Submarines
{
	/// <summary>
	/// Для вычисления 
	/// </summary>
	internal static class CollisionHelper
	{
		public static SubmarineCollisionResult GetSubmarineMapCollision(
			SubmarineBase submarine,
			Vector newPosition,
			List<LineInfo> mapGeometryLines)
		{
			var res = new SubmarineCollisionResult();

			// возможно это надо будет сохранять где-нибудь для вычисления столкновений с ракетами
			Rect rect = GetRect(submarine.GeometryRotatedLines, newPosition);

			// вычисляем для каждой линии карты пересекает ли она прямоугольник (т.е. прямоугольник линии корабля входит в прямоугольник линии карты)
			foreach (var line in mapGeometryLines) {
				if ((line.From.X < rect.X1 && line.To.X < rect.X1) ||
				    (line.From.X > rect.X2 && line.To.X > rect.X2) ||
				    (line.From.Y < rect.Y1 && line.To.Y < rect.Y1) ||
				    (line.From.Y > rect.Y2 && line.To.Y > rect.Y2))
					continue;
				// проверяем есть ли пересечение самой линии карты с прямоугольником корабля
				if (!IntersectWithRect(line, rect))
					continue;
				// проверяем пересекаются ли линии корабля с линией карты
				foreach (var submarineLine in submarine.GeometryRotatedLines) {
					if (!Intersection(line.From.X, line.From.Y, line.To.X, line.To.Y,
						submarineLine.From.X + newPosition.X, submarineLine.From.Y + newPosition.Y,
						submarineLine.To.X + newPosition.X, submarineLine.To.Y + newPosition.Y
					))
						continue;

					res.CollisionDetected = true;
					// вычисляем направление поворота (угол будем определять по маневровому двигателю)
					тут
					goto Finish;// прерываем оба цикла
				}

			}

			Finish:
			return res;
		}

		private static bool IntersectWithRect(LineInfo line, Rect rect)
		{
			var x1 = line.From.X;
			var y1 = line.From.Y;
			var x2 = line.To.X;
			var y2 = line.To.Y;
			return
				Intersection(x1, y1, x2, y2, rect.X1, rect.Y1, rect.X1, rect.Y2) ||
				Intersection(x1, y1, x2, y2, rect.X1, rect.Y1, rect.X2, rect.Y1) ||
				Intersection(x1, y1, x2, y2, rect.X2, rect.Y2, rect.X1, rect.Y2) ||
				Intersection(x1, y1, x2, y2, rect.X2, rect.Y2, rect.X2, rect.Y1);
		}


		private static bool Intersection(Vector start1, Vector end1, Vector start2, Vector end2/*, out Vector out_intersection*/)
		{
			return Intersection(
				start1.X, start1.Y, end1.X, end1.Y,
				start2.X, start2.Y, end2.X, end2.Y);
		}

		private static bool Intersection(
			float start1X, float start1Y, 
			float end1X, float end1Y,
			float start2X,  float start2Y,
			float end2X, float end2Y/*, out Vector out_intersection*/)
		{
			// https://users.livejournal.com/-winnie/152327.html

			//считаем уравнения прямых проходящих через отрезки
			float a1 = -end1Y + start1Y;
			float b1 = +end1X - start1X;
			float d1 = -(a1 * start1X + b1 * start1Y);

			float a2 = -end2Y + start2Y;
			float b2 = +end2X - start2X;
			float d2 = -(a2 * start2X + b2 * start2Y);

			//подставляем концы отрезков, для выяснения в каких полуплоскоcтях они
			float seg1Line2Start = a2 * start1X + b2 * start1Y + d2;
			float seg1Line2End = a2 * end1X + b2 * end1Y + d2;

			float seg2Line1Start = a1 * start2X + b1 * start2Y + d1;
			float seg2Line1End = a1 * end2X + b1 * end2Y + d1;

			//если концы одного отрезка имеют один знак, значит он в одной полуплоскости и пересечения нет.
			if (seg1Line2Start * seg1Line2End >= 0 || seg2Line1Start * seg2Line1End >= 0)
				return false;

			//float u = seg1_line2_start / (seg1_line2_start - seg1_line2_end);
			//*out_intersection = start1 + u * dir1;

			return true;
		}

		private static Rect GetRect(List<LineInfo> geometry, Vector position)
		{
			var p1 = geometry[0].From;

			float x1 = p1.X;
			float x2 = x1;
			float y1 = p1.Y;
			float y2 = y1;

			foreach (var lineInfo in geometry) {
				if (lineInfo.From.X < x1)
					x1 = lineInfo.From.X;
				if (lineInfo.From.X > x2)
					x2 = lineInfo.From.X;
				if (lineInfo.From.Y < y1)
					y1 = lineInfo.From.Y;
				if (lineInfo.From.Y > y2)
					y2 = lineInfo.From.Y;
			}

			return new Rect() {
				X1 = x1 + position.X,
				Y1 = y1 + position.Y,
				X2 = x2 + position.X,
				Y2 = y2 + position.Y,
			};
		}
	}
}