namespace Submarines
{
	/// <summary>
	/// Позволяет объекту изменения курса учитывать параметры объекта
	/// </summary>
	internal interface IManeuverSupport
	{
		float VCurrent { get; }
		float SteeringAngle { get; }
		Vector SpeedVector { get; }
		Vector Position { get; }
	}
}