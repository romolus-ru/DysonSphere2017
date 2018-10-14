using Engine.Models;
using System;

namespace SpaceConstruction
{
	/// <summary>
	/// Вступительный ролик
	/// </summary>
	public class ModelIntro : Model
	{
		private int _counter = 0;
		public Action<Model> OnComplete;

		public override void Tick()
		{
			_counter++;
			if (_counter > 1) {
				OnComplete?.Invoke(this);
			}
		}
	}
}