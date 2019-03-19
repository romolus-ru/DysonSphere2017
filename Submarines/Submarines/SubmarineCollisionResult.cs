namespace Submarines.Submarines
{
	/// <summary>
	/// Результат столкновений корабля.
	/// лучше структура. или у каждого корабля хранить класс результатов столкновений
	/// </summary>
	internal struct SubmarineCollisionResult
	{
		/// <summary>
		/// Изменение вектора движения вследствии столкновений
		/// </summary>
		public float DeltaSteeringResult;
	}
}