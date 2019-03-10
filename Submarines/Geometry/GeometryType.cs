namespace Submarines.Geometry
{
	internal enum GeometryType
	{
		Unknown,
		/// <summary>
		/// Для карты
		/// </summary>
		Map,
		/// <summary>
		/// Стена или препятствие. возможно, элементы карты
		/// </summary>
		Wall,
		/// <summary>
		/// Корпус подлодки или чего то такого
		/// </summary>
		Hull,
		/// <summary>
		/// Точка на карте - город, квестовое, портал, туториал
		/// </summary>
		Place,
		/// <summary>
		/// Символический знак
		/// </summary>
		Symbol,

	}
}