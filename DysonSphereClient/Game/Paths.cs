using Engine.Helpers;
using Engine.Visualization;
using Engine.Visualization.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Централизация работы с путями - что бы лишний раз не вычислять и хранить и вычислять пути тут
	/// </summary>
	public class Paths
	{
		//private Dictionary<ScreenPoint, Dictionary<ScreenPoint, List<ScreenPoint>>> paths;

		/// <summary>
		/// Получаем путь корабля от одной точки до другой
		/// </summary>
		/// <returns></returns>
		public List<ScreenPoint> GetShipRoad(List<ScreenEdge> roadMST, ScreenPoint A, ScreenPoint B)
		{
			var shortRoad = GetShortRoad(roadMST, A, B);
			return GetPath(shortRoad, A);
		}

		/// <summary>
		/// Почти по дейкстре. 
		/// распространяем сигнал по всем направлениям. как только достигли целевой точки - 
		/// двигаемся назад по граням с меньшим весом
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		private List<ScreenEdge> GetShortRoad(List<ScreenEdge> roadMST, ScreenPoint A, ScreenPoint B)
		{
			var edges = new List<ScreenEdge>();
			var pointsSearched = new List<ScreenPoint>();

			foreach (var edgeRoad in roadMST) {
				var ne = new ScreenEdge(edgeRoad.A, edgeRoad.B);
				edges.Add(ne);
			}

			var path = new List<ScreenEdge>();
			var beginEdges = edges.Where(e => (e.A == A || e.B == A)).DefaultIfEmpty().ToList();
			if (/*beginEdges == null || */beginEdges.Count == 0) return null;
			pointsSearched.Add(A);
			path.AddRange(beginEdges);
			beginEdges.ForEach(e =>
			{
				edges.Remove(e);// удаляем 
				if (e == null) throw new Exception("e = null");
				e.Weight = e.A.distanceTo(e.B);// расчитываем начальный вес
			}
			);

			var founded = false;
			while (!founded) {
				ScreenPoint search = null;
				ScreenEdge searchEdge = null;
				// ищем точку для которой будем искать следующие ребра
				foreach (var pathEdge in path) {
					if (!pointsSearched.Contains(pathEdge.A)) {
						search = pathEdge.A;
						searchEdge = pathEdge;
						break;
					}
					if (!pointsSearched.Contains(pathEdge.B)) {
						search = pathEdge.B;
						searchEdge = pathEdge;
						break;
					}
				}
				if (search == null) { founded = true; continue; }

				// ищем ребра которые связаны с новой вершиной, за исключением самого найденного ребра
				var edgesSearch = edges.Where(
					e => (/*e != searchEdge &&*/ (e.A == search || e.B == search))
				).ToList();
				foreach (var e in edgesSearch) {
					e.Weight = searchEdge.Weight + e.A.distanceTo(e.B);
					if (e.A == B || e.B == B) { founded = true; }
				}

				pointsSearched.Add(search);// запоминаем
				path.AddRange(edgesSearch);// добавляем
				edgesSearch.ForEach(e => edges.Remove(e));// удаляем
			}

			// выбираем только нужные вершины
			var ret = new List<ScreenEdge>();
			ScreenPoint searchPoint = B;
			var edge = path.Where(e => e.A == searchPoint || e.B == searchPoint).OrderBy(e => e.Weight).First();
			ret.Add(edge);
			searchPoint = edge.A == searchPoint ? edge.B : edge.A;
			while (searchPoint != A) {// ищем вершины по точке и берём из них только с меньшим весом. из нее берём следующую точку
				edge = path.Where(e => e.A == searchPoint || e.B == searchPoint)
					.OrderBy(e => e.Weight).First();
				ret.Add(edge);
				searchPoint = edge.A == searchPoint ? edge.B : edge.A;// выбираем другую точку
			}
			ret.Reverse();

			return ret;
		}

		/// <summary>
		/// По присланным граням 
		/// </summary>
		/// <param name="basePath"></param>
		/// <param name="firstPoint"></param>
		/// <returns></returns>
		private List<ScreenPoint> GetPath(List<ScreenEdge> basePath, ScreenPoint firstPoint)
		{
			var bc = new BezierCurve();
			var pathlength = (int)(basePath/*.Count * 20);//*/ .Sum(e => e.Weight) / 5 / basePath.Count);
			var basePoints = GetPathFromEdges(basePath, firstPoint);
			var ret = new List<ScreenPoint>();
			bc.Bezier2D(basePoints, pathlength, ret);
			return ret;
		}

		private List<ScreenPoint> GetPathFromEdges(List<ScreenEdge> basePath, ScreenPoint firstPoint)
		{
			var ret = new List<ScreenPoint>();
			var p = firstPoint;
			ret.Add(p);// сохраняем первую точку
			foreach (var edge in basePath) {
				p = edge.A == p ? edge.B : edge.A;
				ret.Add(p);// сохраняем другую точку 
			}
			return ret;
		}

		/// <summary>
		/// Ищем минимальное остовное дерево
		/// </summary>
		public List<ScreenEdge> AlgorithmByPrim(List<ScreenEdge> E, IEnumerable<ScreenPoint> points)
		{
			List<ScreenEdge> MST = new List<ScreenEdge>();
			//неиспользованные ребра
			List<ScreenEdge> notUsedE = new List<ScreenEdge>(E);
			//использованные вершины
			List<ScreenPoint> usedV = new List<ScreenPoint>();
			//неиспользованные вершины
			List<ScreenPoint> notUsedV = new List<ScreenPoint>(points);
			//выбираем случайную начальную вершину
			var num = RandomHelper.Random(notUsedV.Count);
			usedV.Add(notUsedV[num]);
			notUsedV.RemoveAt(num);
			while (notUsedV.Count > 0) {
				int minE = -1; //номер наименьшего ребра
							   //поиск наименьшего ребра
				for (int i = 0; i < notUsedE.Count; i++) {
					if ((usedV.IndexOf(notUsedE[i].A) != -1) && (notUsedV.IndexOf(notUsedE[i].B) != -1) ||
						(usedV.IndexOf(notUsedE[i].B) != -1) && (notUsedV.IndexOf(notUsedE[i].A) != -1)) {
						if (minE != -1) {
							if (notUsedE[i].Weight < notUsedE[minE].Weight)
								minE = i;
						} else
							minE = i;
					}
				}
				//заносим новую вершину в список использованных и удаляем ее из списка неиспользованных
				if (usedV.IndexOf(notUsedE[minE].A) != -1) {
					usedV.Add(notUsedE[minE].B);
					notUsedV.Remove(notUsedE[minE].B);
				} else {
					usedV.Add(notUsedE[minE].A);
					notUsedV.Remove(notUsedE[minE].A);
				}
				//заносим новое ребро в дерево и удаляем его из списка неиспользованных
				MST.Add(notUsedE[minE]);
				notUsedE.RemoveAt(minE);
			}
			return MST;
		}

		/// <summary>
		/// Создать галактику
		/// </summary>
		/// <param name="count"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="countAttempts">Количесто попыток поставить точку перед уменьшением минимальной дистанции</param>
		/// <returns></returns>
		public List<Planet> CreateGalaxy(int count, int width, int height, int countAttempts)
		{
			List<Planet> resultPoints = new List<Planet>();
			var minDist = width / 30;
			var counter = 0;

			var first = new Planet()
			{
				X = RandomHelper.Random(width),
				Y = RandomHelper.Random(height),
			};
			resultPoints.Add(first);

			while (resultPoints.Count != count) {
				counter++;
				if (counter > countAttempts) {
					counter = 0;
					minDist = minDist / 2;
					if (minDist == 0) break;
				}

				var p = new Planet()
				{
					X = RandomHelper.Random(width),
					Y = RandomHelper.Random(height)
				};
				var foundedMin = false;
				foreach (var pt in resultPoints) {
					var dist = p.distanceTo(pt);
					if (dist < minDist) {
						foundedMin = true;
						break;
					}
				}
				if (foundedMin) continue;// начинаем цикл по новой - нашлась точка которая близко расположена к новой										 
				resultPoints.Add(p);
				counter = 0;
			}

			return resultPoints;
		}
	}
}