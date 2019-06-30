using System.Collections.Generic;
using Submarines.Geometry;
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

		public MapController(GeometryBase mapGeometry)
		{
			_mapGeometry = mapGeometry;
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
			return CollisionHelper.GetSubmarineMapCollision(submarine, newPosition, _mapGeometry.Lines);
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