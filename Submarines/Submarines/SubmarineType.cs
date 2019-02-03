namespace Submarines.Submarines
{
	/// <summary>
	/// Тип подлодки
	/// </summary>
	internal enum SubmarineType
	{
		Unknown,
		Player,
		/// <summary>
		/// Враг
		/// </summary>
		AiEnemy,
		/// <summary>
		/// Квестовая подлодка, не враждебная
		/// </summary>
		AiNpc,
		/// <summary>
		/// Союзная подлодка
		/// </summary>
		AiFoe,
		/// <summary>
		/// Торпеда
		/// </summary>
		Torpedo,
		/// <summary>
		/// Ракета
		/// </summary>
		Rocket,
		/// <summary>
		/// Пуля
		/// </summary>
		Bullet
	}
}