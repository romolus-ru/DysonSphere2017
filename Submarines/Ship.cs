
namespace Submarines
{
	internal class Ship
	{
		// TODO двигатель поместить в отдельный класс который будет контролировать текущую скорость двигателя
		// TODO продумать как будут сохраняться айтемы - айтемы можно будет апгрейдить и покупать с разными характеристиками
		// но будут и такие же как и в SpaceConstruction - изменяющиеся только количественно

		// https://nptel.ac.in/courses/108103009/3
		// dV = Fengine-Ftotalresistance/(sigma*Mass)*dt
		// F[Nm] sigma - koefficient of inertia

		// дельта т взять из констант движка. добавить механизм проверки чтоб период был точно таким же
		// иначе придётся поместить корабль в другой поток (как минимум двигатель)

		public int Mass { get; private set; }
		public int EngineSpeed { get; private set; }
		public int EngineSpeedMax { get; private set; }
		/// <summary>
		/// Допустимая обратная скорость (обычно меньше чем скорость вперед)
		/// </summary>
		public int EngineSpeedRev { get; private set; }
		public Vector CurrentVector { get; private set; }

		public Ship()
		{
			EngineSpeedMax = 150;
			EngineSpeedMax = -50;
		}

		public void AddSpeed(int delta)
		{
			EngineSpeed += delta;
			if (EngineSpeed > EngineSpeedMax)
				EngineSpeed = EngineSpeedMax;
			if (EngineSpeed < -EngineSpeedRev)
				EngineSpeed = -EngineSpeedRev;
		}

		public void StopEngine() => EngineSpeed = 0;
	}
}