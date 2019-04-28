namespace Submarines.AI.Commands.Move
{
	/// <summary>
	/// Команда движения для одного сегмента с одним поворотом, постоянной скоростью и заданным расстоянием
	/// </summary>
	internal class MoveCommandSegment
	{
		public float Angle;
		public float Speed;
		public float Distance;
	}
}
