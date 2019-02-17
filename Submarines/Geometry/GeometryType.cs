namespace Submarines.Geometry
{
	internal enum GeometryType
	{
		Unknown,
		/// <summary>
		/// Стена или препятствие
		/// </summary>
		Wall,
		/// <summary>
		/// Корпус подлодки или чего то такого
		/// </summary>
		Hull,
		/// <summary>
		/// Точка на карте - город, квестовое, портал, туториал
		/// </summary>
		Place
	}
}