using Submarines.Items;

namespace Submarines.Submarines
{
	/// <summary>
	/// Результат столкновений корабля.
	/// лучше структура. или у каждого корабля хранить объект результатов столкновений
	/// </summary>
	internal struct SubmarineCollisionResult
	{
		public bool CollisionDetected;
		/// <summary>
		/// Изменение вектора движения вследствии столкновений
		/// </summary>
		public float DeltaSteeringResult;
        /// <summary>
        /// Тип 
        /// </summary>
        public CollisionType CollisionType;
	}
}