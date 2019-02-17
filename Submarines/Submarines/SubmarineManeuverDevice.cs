using System;
using Engine.Extensions;

namespace Submarines.Submarines
{
	internal class SubmarineManeuverDevice : ManeuverDevice
	{
		public SubmarineManeuverDevice(float maxSteeringPerSecond, float steeringLimit) : base(maxSteeringPerSecond, steeringLimit)
		{
		}

		public override float CalculateSteering(IManeuverSupport parameters, float timeCoefficient)
		{
			var steeringAngle = parameters.SteeringAngle;
			if (steeringAngle.IsZero())
				return 0;

			float ret;
			if (steeringAngle > 0) {
				ret = MaxSteeringPerSecond * timeCoefficient;
				if (steeringAngle < ret)
					ret = steeringAngle;
			}
			else {
				ret = -MaxSteeringPerSecond * timeCoefficient;
				if (steeringAngle > ret)
					ret = steeringAngle;
			}

			return ret;
		}

		public override float AddSteering(IManeuverSupport parameters, float angle)
		{
			float ret;
			if (angle > 0) {
				ret = parameters.SteeringAngle + angle;
				if (SteeringLimit < ret)
					ret = SteeringLimit - parameters.SteeringAngle;
			}
			else {
				ret = parameters.SteeringAngle + angle;
				if (-SteeringLimit > ret)
					ret = -SteeringLimit - parameters.SteeringAngle;
			}

			return ret;
		}
	}
}