namespace Submarines.Submarines
{
	internal class SubmarineEngine : Engine
	{
		public SubmarineEngine(float enginePower, int enginePercentMin, int enginePercentMax)
			: base(enginePower, enginePercentMin, enginePercentMax)
		{
		}

		public override float CalculateSpeed(float deltaTime)
		{
			//return Parameters.EnginePercent * EnginePower / 100;
			return base.CalculateSpeed(deltaTime);
		}
	}
}