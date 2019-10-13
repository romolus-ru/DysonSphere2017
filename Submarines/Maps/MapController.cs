using System;
using System.Collections.Generic;
using Submarines.Geometry;
using Submarines.Maps.Spawns;
using Submarines.Submarines;

namespace Submarines.Maps
{
	/// <summary>
	/// Обрабатывает создание снарядов, столкновения, перемещение
	/// </summary>
	internal class MapController
	{
		/// <summary>
		/// Границы карты
		/// </summary>
		private GeometryBase _mapGeometry;

        private List<MapSpawn> _mapSpawns;

        public Action<MapSpawn> OnSpawnActivated;

		/// <summary>
		/// Подлодки
		/// </summary>
		private List<Submarine> _submarines = new List<Submarine>();

		/// <summary>
		/// Снаряды, ракеты и т.п.
		/// </summary>
		private List<SubmarineBase> _rockets = new List<SubmarineBase>();

		/// <summary>
		/// Выстрелы защитного вооружения
		/// </summary>
		private List<SubmarineBase> _defance = new List<SubmarineBase>();

		public MapController(GeometryBase mapGeometry, List<MapSpawn> mapSpawns)
		{
			_mapGeometry = mapGeometry;
            _mapSpawns = mapSpawns;
		}

		public void AddSubmarine(Submarine submarine)
		{
			submarine.OnCheckCollision = SubmarineCheckCollision;
			_submarines.Add(submarine);
		}

		public void AddShoot(SubmarineBase shoot)
		{
			shoot.OnCheckCollision = ShootCheckCollision;
			_rockets.Add(shoot);
		}


		/// <summary>
		/// Определяем столкновение и возвращаем результат
		/// </summary>
		/// <param name="submarine"></param>
		/// <param name="currentPosition"></param>
		/// <param name="newPosition"></param>
		/// <returns></returns>
		private SubmarineCollisionResult SubmarineCheckCollision(SubmarineBase submarine, Vector currentPosition, Vector newPosition)
		{
            // добавить проверку столкновения с объектами карты
            foreach (var spawn in _mapSpawns) {
                var collisionSpawn = CollisionHelper.GetCollision(submarine, newPosition, spawn.Geometry.Lines);
                if (collisionSpawn.CollisionDetected && spawn.ActiveCollision) {
                    collisionSpawn.CollisionType = spawn.SpawnType.GetCollisionType();
                    OnSpawnActivated?.Invoke(spawn);
                    return collisionSpawn;
                }
                if (!collisionSpawn.CollisionDetected && !spawn.ActiveCollision) {
                    spawn.ActiveCollision = true;
                }
            }
            var collision = CollisionHelper.GetCollision(submarine, newPosition, _mapGeometry.Lines);
            if (collision.CollisionDetected)
                collision.CollisionType = CollisionType.Map;
            return collision;
		}

		/// <summary>
		/// Определяем столкновение и возвращаем результат
		/// </summary>
		/// <param name="submarine"></param>
		/// <param name="currentPosition"></param>
		/// <param name="newPosition"></param>
		/// <returns></returns>
		private SubmarineCollisionResult ShootCheckCollision(SubmarineBase submarine, Vector currentPosition, Vector newPosition)
		{
			return new SubmarineCollisionResult();
		}

	}
}