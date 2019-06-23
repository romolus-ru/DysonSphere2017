using System;
using System.Diagnostics;

namespace Submarines.AI.Commands.Move
{
	/// <summary>
	/// Команда движения для одного сегмента с одним поворотом, постоянной скоростью и заданным расстоянием
	/// </summary>
	[DebuggerDisplay("segment A={Angle} S={Speed} D={Distance} T={Time}")]
	internal class MoveCommandSegment
	{
		public float Angle;
		public float Speed;
		public float Distance;
		public TimeSpan Time;
	}
}
