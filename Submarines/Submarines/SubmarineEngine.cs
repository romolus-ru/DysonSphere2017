namespace Submarines.Submarines
{
	internal class SubmarineEngine : Engine
	{
		public SubmarineEngine(float enginePower, int enginePercentMin, int enginePercentMax) 
			: base(enginePower, enginePercentMin, enginePercentMax)
		{
		}

		public override float CalculateSpeed(IEngineSupport parameters, float deltaTime)
		{
			return parameters.EnginePercent * EnginePower / 100;
		}
	}
}