namespace Submarines
{
	/// <summary>
	/// Устройство для изменения курса и расчёта поворота
	/// </summary>
	internal class ManeuverDevice
	{
		public int Power;
		public const float g = 9.8f;

		/// <summary>
		/// Текущее значение поворота (на сколько ещё надо повернуться)
		/// </summary>
		public float SteeringAngle { get; private set; }
		
		/// <summary>
		/// Поворачиваемся
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="deltaTime"></param>
		/// <returns>На сколько градусов надо повернуться</returns>
		public virtual float CalculateSteering(IManeuverSupport parameters, float deltaTime)
		{
			// изменение поворота корабля если нужно
			return 0;
		}

		public void AddSteering(float angle)
		{
			SteeringAngle += angle;
			// проверяем границу чтоб постоянно не накручивалось
		}
	}
}
