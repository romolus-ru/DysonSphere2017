﻿using Engine.Models;
using Submarines.Submarines;
using System;

namespace Submarines
{
	internal class ModelGame : Model
	{
		private Submarine _submarine;
		private ShipController _shipController;
		private DateTime _currentTime;

		public ModelGame(Submarine submarine, ShipController shipController)
		{
			_submarine = submarine;
			_shipController = shipController;
			_currentTime=DateTime.Now;// но лучше передавать это извне через тик
		}

		public override void Tick()
		{
			var timeCoefficient = (DateTime.Now - _currentTime).Milliseconds / 100f;
			_submarine.CalculateMovement(timeCoefficient);
			_currentTime = DateTime.Now;
		}

	}
}