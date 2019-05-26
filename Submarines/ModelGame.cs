using Engine.Models;
using System;
using Engine;
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
			var elapsedTime = DateTime.Now - _currentTime;
			_currentTime = DateTime.Now;
			if (elapsedTime.TotalMilliseconds > Constants.TimerInterval)
				elapsedTime = new TimeSpan(0, 0, 0, 0, Constants.TimerInterval);
			var timeCoefficient = (elapsedTime).Milliseconds / 100f;
			_map.RunActivities(timeCoefficient, elapsedTime);
		}

	}
}