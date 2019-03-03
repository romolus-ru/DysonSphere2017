﻿using System;
using System.Collections.Generic;
using Engine.Extensions;
using Submarines.Geometry;

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

		public float CurrentAngle { get; protected set; } = -90;

		/// <summary>
		/// Позиция корабля относительно текущего центра квадранта
		/// </summary>
		public Vector Position { get; protected set; }

		public Engine Engine { get; private set; }

		public ManeuverDevice ManeuverDevice { get; private set; }

		public GeometryBase Geometry { get; private set; }

		private int _currentRotatedAngle = 0;

		public List<LineInfo> GeometryRotatedLines { get; private set; }

		public SubmarineBase(GeometryBase geometry, Engine engine, ManeuverDevice maneuverDevice)
		{
			Geometry = geometry;
			Engine = engine;
			ManeuverDevice = maneuverDevice;
			VCurrentPrev = 0;

			GeometryRotatedLines = new List<LineInfo>(geometry.Lines);
			RecalculateGeometry();
		}

		/// <summary>
		/// Расчёт движения устройства
		/// </summary>
		public virtual void CalculateMovement(float timeCoefficient)
		{
			VCurrent = Engine.CalculateSpeed(this, timeCoefficient);
			var deltaSteeringAngle = ManeuverDevice.CalculateSteering(this, timeCoefficient);
			if (!deltaSteeringAngle.IsZero()) {
				SteeringAngle -= deltaSteeringAngle;
				CurrentAngle += deltaSteeringAngle;
				RecalculateGeometry();
			}

			// вычисляем вектор скорости
			if (!VCurrent.IsEqualTo(VCurrentPrev) || !deltaSteeringAngle.IsZero()) {
				var radians = CurrentAngle * (Math.PI / 180);
				float cosRad = (float) Math.Cos(radians);
				float sinRad = (float) Math.Sin(radians);
				float x = VCurrent * timeCoefficient * cosRad;
				float y = VCurrent * timeCoefficient * sinRad;
				SpeedVector = new Vector(x, y, 0);
				VCurrentPrev = VCurrent;
			}

			// меняем положение корабля
			if (!VCurrent.IsZero()) {
				Vector newPos = Position.MoveRelative(SpeedVector.X, SpeedVector.Y);
				SubmarineCollisionResult collision = OnCheckCollision?.Invoke(this, Position, newPos);
				MoveToNewPos(collision, newPos);
			}
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

		protected void RecalculateGeometry()
		{
			const int recalcStep = 5;
			var angle5 = (int) CurrentAngle / recalcStep;
			if (angle5 == _currentRotatedAngle)
				return;

			_currentRotatedAngle = angle5;
			var radians = (_currentRotatedAngle + 90) * recalcStep * (Math.PI / 180);
			float cosRad = (float) Math.Cos(radians);
			float sinRad = (float) Math.Sin(radians);
			for (int i = 0; i < Geometry.Lines.Count; i++) {
				var original = Geometry.Lines[i];
				var p1 = new Vector(
					original.From.X * cosRad - original.From.Y * sinRad,
					original.From.X * sinRad + original.From.Y * cosRad,
					0);
				var p2 = new Vector(
					original.To.X * cosRad - original.To.Y * sinRad,
					original.To.X * sinRad + original.To.Y * cosRad,
					0);
				var point = new LineInfo(p1, p2);
				GeometryRotatedLines[i] = point;
			}
		}
	}
}