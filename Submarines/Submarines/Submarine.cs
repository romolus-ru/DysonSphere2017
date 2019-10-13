using Submarines.Geometry;
using System;

namespace Submarines.Submarines
{
	internal class Submarine : SubmarineBase
	{
		public int EnginePercentMax { get; private set; }

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

		public void SetSpeed(float speed)
		{
			Engine.SetSpeed(speed);
		}

		public void AddSpeed(float delta)
		{
			EnginePercent += delta;
			if (EnginePercent > EnginePercentMax)
				EnginePercent = EnginePercentMax;
			if (EnginePercent < EnginePercentMin)
				EnginePercent = EnginePercentMin;
			Engine.SetSpeedPercent(EnginePercent);
		}

		public void StopEngine() => AddSpeed(-EnginePercent);

		/// <summary>
		/// Направление в углах по часовой стрелке
		/// </summary>
		/// <param name="angle"></param>
		public void AddSteering(float angle) => SteeringAngle += ManeuverDevice.AddSteering(this, angle);

		public void SetAngle(float angle)
		{
			CurrentAngle = angle - 0.1f;
			SteeringAngle = 0.1f;
			//SteeringAngle = CurrentAngle - angle;
			//CurrentAngle = angle;
			//SteeringAngle = 0;
		}

		protected override void MoveToNewPos(SubmarineCollisionResult collisionResult, Vector newPos)
		{
			if (collisionResult.CollisionDetected) {
				AddSteering(collisionResult.DeltaSteeringResult);
				return;
			}
			base.MoveToNewPos(collisionResult, newPos);
		}

        internal void SetStartValues(Vector startPoint, float startAngle) {
            Position = startPoint;
            CurrentAngle = startAngle;
        }
    }
}