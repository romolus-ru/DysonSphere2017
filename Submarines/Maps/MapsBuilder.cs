using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Submarines.Geometry;
using Submarines.Items;
using Submarines.Maps.Spawns;
using Submarines.Submarines;

namespace Submarines.Maps
{
	/// <summary>
	/// Создаёт карту и заполняет её нужными объектами
	/// </summary>
	internal static class MapsBuilder
	{
        private static MapSpawn CreateSpawn(ItemMap map, ItemMap.ItemMapSpawnPoint mapSpawn, ItemGlobalMap globalMap) {
            MapSpawn spawn = null;
            switch (mapSpawn.SpawnType) {
                //case SpawnType.Unknown:
                //    break;
                //case SpawnType.Chest:
                //    break;
                //case SpawnType.Cargo:
                //    break;
                //case SpawnType.RawMaterials:
                //    break;
                //case SpawnType.Enemy:
                //    break;
                //case SpawnType.Npc:
                //    break;
                //case SpawnType.Town:
                //    break;
                case SpawnType.Portal:
                    var spawn1 = new MapSpawnTeleport();
                    var mapPointId = globalMap.MapPoints.FirstOrDefault(m => m.MapCode == map.MapCode).PointId;

                    // это всё надо будет переделать - у объекта глобальной карты будут другие возможности по нахождению нужных данных
                    foreach (var relation in globalMap.MapRelations) {
                        if (relation.MapPointId1 == mapPointId && relation.MapSpawnId1 == mapSpawn.Id) {
                            spawn1.TargetMapCode = globalMap.GetPointById(relation.MapPointId2).MapCode;
                            spawn1.TargetMapSpawnId = relation.MapSpawnId2;
                            break;
                        }
                        if (relation.MapPointId2 == mapPointId && relation.MapSpawnId2 == mapSpawn.Id) {
                            spawn1.TargetMapCode = globalMap.GetPointById(relation.MapPointId1).MapCode;
                            spawn1.TargetMapSpawnId = relation.MapSpawnId1;
                            break;
                        }
                    }
                    spawn = spawn1;
                    break;
                default:
                    break;
            }

            if (spawn != null) {
                spawn.Id = mapSpawn.Id;
                spawn.SpawnType = mapSpawn.SpawnType;
                spawn.Geometry = CreateSpawnGeometry(mapSpawn);
            }

            return spawn;
        }

        /// <summary>
        /// Создаём смещённую копию оригинального спавна
        /// </summary>
        /// <param name="mapSpawn"></param>
        /// <returns></returns>
        private static GeometryBase CreateSpawnGeometry(ItemMap.ItemMapSpawnPoint mapSpawn) {
            var geometryOriginal = ItemsManager.GetGeometry(mapSpawn.AreaGeometryName);
            var geometry = new GeometryBase();
            geometry.Color = geometryOriginal.Color;
            geometry.GeometryType = geometryOriginal.GeometryType;
            geometry.Name = geometryOriginal.Name;
            foreach (var line in geometryOriginal.Lines) {
                var spawnLine = new LineInfo();
                spawnLine.From = line.From + mapSpawn.Point;
                spawnLine.To = line.To + mapSpawn.Point;
                geometry.Lines.Add(spawnLine);
            }
            return geometry;
        }



        /// <summary>
        /// Создаём карту и заполняем ее нужными параметрами. 
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="playerSubmarine">Текущая подлодка игрока</param>
        /// <returns></returns>
        public static MapBase CreateMap(string mapCode, SubmarineBase playerSubmarine, ItemGlobalMap globalMap)
		{
            var mapInfo = ItemsManager.GetMap(mapCode);
            var submarines = new List<SubmarineBase>();
			submarines.Add(playerSubmarine);

            var spawns = new List<MapSpawn>();
            foreach (var mapSpawn in mapInfo.MapSpawns) {
                var spawn = CreateSpawn(mapInfo, mapSpawn, globalMap);
                if (spawn != null)
                    spawns.Add(spawn);
            }

			var geometry = ItemsManager.GetGeometry(mapInfo.MapGeometryName);
            var map = new MapBase(geometry, submarines, spawns);
			map.SetPlayerShip(playerSubmarine);
			return map;
		}


    }
}