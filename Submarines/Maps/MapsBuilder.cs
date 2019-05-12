﻿using System.Collections.Generic;
using Submarines.Items;
using Submarines.Submarines;

namespace Submarines.Maps
{
	/// <summary>
	/// Создаёт карту и заполняет её нужными объектами
	/// </summary>
	internal static class MapsBuilder
	{
		/// <summary>
		/// Создаём карту и заполняем ее нужными параметрами. 
		/// </summary>
		/// <param name="mapInfo"></param>
		/// <param name="playerSubmarine">Текущая подлодка игрока</param>
		/// <returns></returns>
		public static MapBase CreateMap(ItemMap mapInfo, SubmarineBase playerSubmarine)
		{
			var submarines = new List<SubmarineBase>();
			submarines.Add(playerSubmarine);

			var geometry = ItemsManager.GetGeometry(mapInfo.MapGeometryName);
			var map = new MapBase(geometry, submarines);
			map.SetPlayerShip(playerSubmarine);
			return map;
		}
	}
}