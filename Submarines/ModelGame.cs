using Engine.Models;
using System;
using Submarines.Maps;

namespace Submarines
{
	internal class ModelGame : Model
	{
		private MapBase _map;
		private DateTime _currentTime;

		public ModelGame(MapBase map)
		{
			_map = map;
			_currentTime = DateTime.Now; // но лучше передавать это извне через тик
		}

		public override void Tick()
		{
			var timeCoefficient = (DateTime.Now - _currentTime).Milliseconds / 100f;
			_map.RunActivities(timeCoefficient);
			_currentTime = DateTime.Now;
		}

	}
}