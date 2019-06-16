﻿using Submarines.Geometry;
using System;

namespace Submarines.Submarines
{
	internal class Submarine : SubmarineBase
	{
		public int EnginePercentMax { get; private set; }
		public float VMax;
		public float VMin;

		/// <summary>
		/// Допустимая обратная скорость (обычно меньше чем скорость вперед)
		/// </summary>
		public int EnginePercentMin { get; private set; }

		private DateTime _currentTime;

		public Submarine(GeometryBase geometry, Engine engine, ManeuverDevice maneuverDevice, Weapon weapon) 
			: base(geometry, engine, maneuverDevice, weapon)
		{
			EnginePercentMax = engine.EnginePercentMax;
			EnginePercentMin = engine.EnginePercentMin;
			_currentTime = DateTime.Now;
			Mass = 100000;
			AddSpeed(5);
		}

		public void SetSpeed(float value)
		{
			AddSpeed(value - EnginePercent);
		}

		public void AddSpeed(float delta)
		{
			EnginePercent += delta;
			if (EnginePercent > EnginePercentMax)
				EnginePercent = EnginePercentMax;
			if (EnginePercent < EnginePercentMin)
				EnginePercent = EnginePercentMin;
		}

		public void StopEngine() => AddSpeed(-EnginePercent);

		/// <summary>
		/// Направление в углах по часовой стрелке
		/// </summary>
		/// <param name="angle"></param>
		public void AddSteering(float angle) => SteeringAngle += ManeuverDevice.AddSteering(this, angle);

	}
}