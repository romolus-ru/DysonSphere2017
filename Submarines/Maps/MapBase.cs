using System;
using System.Collections.Generic;
using Submarines.Geometry;
using Submarines.Items;
using Submarines.Maps.Spawns;
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
		public MapAiController _mapAIController;
        public List<MapSpawn> Spawns;
        
        /// <summary>
        /// Событие телепорта
        /// </summary>
        public Action<MapSpawnTeleport> OnTeleport;

		/// <summary>
		/// Все подлодки, включая игрока
		/// </summary>
		public List<SubmarineBase> Submarines { get; }

		/// <summary>
		/// Корабль, управляемый игроком в данный момент
		/// </summary>
		public SubmarineBase PlayerShip { get; protected set; }

		public MapBase(GeometryBase mapGeometry, List<SubmarineBase> submarines, List<MapSpawn> spawns)
		{
			Geometry = mapGeometry;
			Submarines = submarines;
            Spawns = spawns;
			_mapAIController = new MapAiController();
			_mapAIController.Map = this;
            _mapController = new MapController(mapGeometry, Spawns);
            _mapController.OnSpawnActivated += SpawnActivated;
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
			foreach (var submarine in Submarines) {
				submarine.ManualControl = false;
			}
			PlayerShip = playerShip;
			playerShip.ManualControl = true;
			if (!Submarines.Contains(playerShip))
				Submarines.Add(playerShip);
		}

		public void RunActivities(TimeSpan elapsedTime)
		{
			_mapAIController.ProcessCommands(elapsedTime);

			foreach (var submarine in Submarines) {
				submarine.ChangeShootLock(elapsedTime);
				if (submarine.ManualControl)
					submarine.CalculateMovement(elapsedTime);
			}
		}

		public void PlayerShoot(SubmarineBase submarine, Weapon weapon, float x, float y)
		{
			_mapAIController.ShootToCoordinatesCommand(submarine, weapon, x, y);
		}

		public void AddShoot(SubmarineBase submarine)
		{
			Submarines.Add(submarine);
			_mapController.AddShoot(submarine);
		}

        private void SpawnActivated(MapSpawn spawn) {
            if (spawn.SpawnType == SpawnType.Portal)
                OnTeleport?.Invoke(spawn as MapSpawnTeleport);
        }
    }
}