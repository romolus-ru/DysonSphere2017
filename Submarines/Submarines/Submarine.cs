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

		public Submarine(Engine engine, ManeuverDevice maneuverDevice) : base(engine, maneuverDevice)
		{
			EnginePercentMax = 150;
			EnginePercentMin = -50;
			_currentTime = DateTime.Now;
			Mass = 100000;
			AddSpeed(5);
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

	}
}