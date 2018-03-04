using Engine;
using Engine.Models;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient
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