namespace Submarines
{
	/// <summary>
	/// Устройство для изменения курса и расчёта поворота
	/// </summary>
	internal class ManeuverDevice
	{
		/// <summary>
		/// Максимальный угол поворота в секунду
		/// </summary>
		public float MaxSteeringPerSecond { get; }
		
		/// <summary>
		/// Предельное значение задаваемого угла поворота (инерция поворота)
		/// </summary>
		protected float SteeringLimit { get; }

		public ManeuverDevice(float maxSteeringPerSecond, float steeringLimit)
		{
			MaxSteeringPerSecond = maxSteeringPerSecond;
			SteeringLimit = steeringLimit;
		}

		/// <summary>
		/// Поворачиваемся
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="timeCoefficient"></param>
		/// <returns>На сколько градусов надо повернуться</returns>
		public virtual float CalculateSteering(IManeuverSupport parameters, float timeCoefficient)
		{
			// изменение поворота корабля если нужно
			return 0;
		}

		public virtual float AddSteering(IManeuverSupport parameters, float angle)
		{
			return angle;
		}
	}
}
