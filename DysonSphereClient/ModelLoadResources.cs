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
	/// Загружаем основные ресурсы
	/// </summary>
	public class ModelLoadResources : Model
	{
		public Action<Model> OnComplete;
		public Action<int> OnProgress;

		private int _counter = 0;

		public override void Tick()
		{
			_counter++;
			if (_counter > 1) {
				OnComplete(this);
				return;
			}
			OnProgress(_counter);
		}
	}
}