using System.Collections.Generic;
using Submarines.Geometry;
using Submarines.Submarines;

namespace Submarines.Maps
{
	/// <summary>
	/// Карта. содержит геометрию, создаёт все начальные объекты и из этого объекта берётся вся информация для вывода на экран
	/// </summary>
	/// <remarks>По идее из этого объекта можно будет сделать карту-туториал</remarks>
	internal class MapBase
	{
		public string Name { get; }
		public string Description { get; }
		public GeometryBase Geometry { get; }

		/// <summary>
		/// Все подлодки, включая игрока
		/// </summary>
		public List<SubmarineBase> Submarines { get; }



	}
}