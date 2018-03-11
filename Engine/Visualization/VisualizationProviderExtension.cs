using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Visualization
{
	public partial class VisualizationProvider
	{
		/// <summary>
		/// Установить цвет c прозрачностью
		/// </summary>
		/// <param name="color"></param>
		/// <param name="alphaPercent">0..100</param>
		public void SetColor(Color color, byte alphaPercent)
		{
			if (alphaPercent > 100) { alphaPercent = 100; }// обрезаем до 100
			Color = Color.FromArgb(255 * alphaPercent / 100, color);// растягиваем до 255
			SetColor(Color.R, Color.G, Color.B, Color.A);
		}

		const int numSegments = 360;
		public static Dictionary<int, PointF> RoundPoints = InitPoints();

		public static Dictionary<int, PointF> InitPoints()
		{
			var ret = new Dictionary<int, PointF>();
			int radius = 1;

			double theta = 2 * Math.PI / numSegments;
			double tangetialFactor = Math.Tan(theta);
			double radialFactor = Math.Cos(theta);
			double xa = 0;
			double ya = -radius;
			ret.Add(0, new PointF((float)xa, (float)ya));

			for (int ii = 0; ii < numSegments; ii++) {
				double tx = -ya;
				double ty = xa;
				xa += tx * tangetialFactor;
				ya += ty * tangetialFactor;
				xa *= radialFactor;
				ya *= radialFactor;
				ret.Add(ii + 1, new PointF((float)xa, (float)ya));
			}
			return ret;
		}

		/// <summary>
		/// Нарисовать круговой прогресс бар
		/// </summary>
		/// <param name="cx"></param>
		/// <param name="cy"></param>
		/// <param name="radius"></param>
		/// <param name="cur"></param>
		/// <param name="max"></param>
		public void DrawRound(int cx, int cy, int radius, int cur, int max)
		{
			var curValue = (int)(1f * cur / max * numSegments);
			var color = Color; // потом этот цвет будет меняться
			OffsetAdd(cx, cy);
			for (int i = 360; i > 0; i--) {
				var c1 = (curValue - i) * 1;
				if (c1 < 0) c1 += 360;
				var c2 = c1 - 1;
				if (c2 < 0) c2 += 360;

				var p1 = RoundPoints[c1];
				var p2 = RoundPoints[c2];
				var mx1 = (int)(radius * p1.X);
				var my1 = (int)(radius * p1.Y);
				var nx1 = (int)((radius + 20) * p1.X);
				var ny1 = (int)((radius + 20) * p1.Y);
				var mx2 = (int)(radius * p2.X);
				var my2 = (int)(radius * p2.Y);
				var nx2 = (int)((radius + 20) * p2.X);
				var ny2 = (int)((radius + 20) * p2.Y);

				SetColor(color, (byte)(100 - i * 10 / 36f));
				Quad(mx1, my1, mx2, my2, nx2, ny2, nx1, ny1);
			}
			OffsetRemove();

			//{const int numSegments = 36;radius += 35;var mx1 = (int) (radius*RoundPoints[0].X + cx);var my1 = (int) (radius*RoundPoints[0].Y + cy);
			//	var nx1 = (int) ((radius + 20)*RoundPoints[0].X + cx);var ny1 = (int) ((radius + 20)*RoundPoints[0].Y + cy);
			//	var numSegmentsDraw = (int) (1f*cur/max*numSegments);
			//	SetColor(Color.Peru);for (var ii = 0; ii < numSegmentsDraw + 1; ii++){
			//		var i1 = ii*10;if (i1 > 360) i1 -= 360;

			//		var mx2 = (int) (radius*RoundPoints[i1].X + cx);var my2 = (int) (radius*RoundPoints[i1].Y + cy);
			//		var nx2 = (int) ((radius + 20)*RoundPoints[i1].X + cx);var ny2 = (int) ((radius + 20)*RoundPoints[i1].Y + cy);

			//		Quad(mx1, my1, mx2, my2, nx2, ny2, nx1, ny1);mx1 = mx2;my1 = my2;nx1 = nx2;ny1 = ny2;}}
		}

	}
}