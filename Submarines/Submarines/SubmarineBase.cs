using System;
using Engine.Extensions;

namespace Submarines.Submarines
{
	/// <summary>
	/// Основа для всех механизмов - от пуль до подлодок
	/// </summary>
	internal class SubmarineBase : IEngineSupport, IManeuverSupport
	{

		public SubmarineType SubmarineType = SubmarineType.Unknown;

		/// <summary>
		/// Проверяем есть ли столкновение
		/// </summary>
		public Func<SubmarineBase, Vector, Vector, SubmarineCollisionResult> OnCheckCollision;

		public float EnginePercent { get; protected set; }

		public float Mass { get; protected set; }

		public float OpposingCoefficient { get; protected set; }

		public float VCurrent { get; protected set; }
		protected float VCurrentPrev { get; set; }

		/// <summary>
		/// Направление вектора скорости (что бы лодку могло заносить немного)
		/// </summary>
		public Vector SpeedVector { get; protected set; }

		/// <summary>
		/// На сколько градусов надо повернуть корабль
		/// </summary>
		public float SteeringAngle { get; protected set; }

		/// <summary>
		/// Позиция корабля относительно текущего центра квадранта
		/// </summary>
		public Vector Position { get; protected set; }

		public Engine Engine { get; private set; }

		public ManeuverDevice ManeuverDevice { get; private set; }
		
		public DateTime CurrentTime;

		public SubmarineBase(Engine engine, ManeuverDevice maneuverDevice)
		{
			Engine = engine;
			ManeuverDevice = maneuverDevice;
			CurrentTime = DateTime.Now;
			VCurrentPrev = 0;
		}

		/// <summary>
		/// Расчёт движения устройства
		/// </summary>
		public virtual void CalculateMovement()
		{
			float dt = (DateTime.Now - CurrentTime).Milliseconds / 1000f;
			VCurrent = Engine.CalculateSpeed(this, dt);
			var deltaSteeringAngle = ManeuverDevice.CalculateSteering(this, dt);
			SteeringAngle -= deltaSteeringAngle;

			// TODO !!!! пока логика такая что не используется занос корабля - не сохраняются
			// предыдущие векторы движения, лодка будет поворачиваться мгновенно (но скорее всего это будет незначительно из-за малых скоростей поворота)

			// вычисляем поворот вектора скорости
			if (!deltaSteeringAngle.IsZero()) {
				var rad = deltaSteeringAngle * Math.PI / 180;
				float cosRad = (float)Math.Cos(rad);
				float sinRad = (float)Math.Sin(rad);
				float rx = SpeedVector.X * cosRad - SpeedVector.Y - sinRad;
				float ry = SpeedVector.X * sinRad - SpeedVector.Y - cosRad;
				SpeedVector = new Vector(rx, ry, 0);
			}

			// меняем длину вектора скорости
			if (!VCurrent.IsEqualTo(VCurrentPrev)) {
				var angle = Math.Atan2(SpeedVector.Y, SpeedVector.X);
				var radians = angle * (Math.PI / 180);
				float cosRad = (float)Math.Cos(radians);
				float sinRad = (float)Math.Sin(radians);
				float x = VCurrent * dt * cosRad;
				float y = VCurrent * dt * sinRad;
				SpeedVector = new Vector(x, y, 0);
				VCurrentPrev = VCurrent;
			}

			// меняем положение корабля
			if (!VCurrent.IsZero()) {
				var newPos = Position.MoveRelative(SpeedVector.X, SpeedVector.Y);
				var collision = OnCheckCollision(this, Position, newPos);
				MoveToNewPos(collision, newPos);
			}

			CurrentTime = DateTime.Now;
		}

		/// <summary>
		/// Движение корабля в зависимости от реакции на столкновение
		/// </summary>
		/// <remarks>возможно останавливать корабль, рассчитывать точку столкновения и т.п.</remarks>
		protected virtual void MoveToNewPos(SubmarineCollisionResult collisionResult, Vector newPos)
		{
			Position = newPos;
		}

		public virtual void SetEnginePercent(float coefficient)
		{
		}

		public virtual void TurnLeft()
		{
		}

		public virtual void TurnRight()
		{
		}

		/// <summary>
		/// Реакция на столкновение, вызывается извне (может быть лишнее)
		/// </summary>
		public virtual void CollisionReaction()
		{
		}
	}
}