using System;
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
		private MapController _mapController;

		/// <summary>
		/// Все подлодки, включая игрока
		/// </summary>
		public List<SubmarineBase> Submarines { get; }

		/// <summary>
		/// Корабль, управляемый игроком в данный момент
		/// </summary>
		public SubmarineBase PlayerShip { get; protected set; }

		public MapBase(GeometryBase mapGeometry, List<SubmarineBase> submarines)
		{
			Geometry = mapGeometry;
			Submarines = submarines;
			_mapController = new MapController(mapGeometry);
			foreach (var submarineBase in submarines) {
				var submarine = (Submarine) submarineBase;
				if (submarine != null)
					_mapController.AddSubmarine(submarine);
			}
		}
		
		/// <summary>
		/// Установить корабль, управляемый игроком
		/// </summary>
		/// <param name="playerShip"></param>
		public virtual void SetPlayerShip(SubmarineBase playerShip)
		{
			PlayerShip = playerShip;
			if (!Submarines.Contains(playerShip))
				Submarines.Add(playerShip);
		}

		public void RunActivities(float timeCoefficient, TimeSpan elapsedTime)
		{
			foreach (var submarine in Submarines) {
				submarine.Shoot(elapsedTime);
				submarine.CalculateMovement(timeCoefficient);
			}
		}

	}
}