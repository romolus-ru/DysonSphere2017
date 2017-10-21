using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// Основная модель - управляет остальными
	/// </summary>
	/// <remarks>Умеет подключать классы модели к обработке</remarks>
	public class ModelMain:Model
	{
		private List<Model> _models = new List<Model>();

		public void AddModel(Model model)
		{
			_models.Add(model);
		}

		/// <summary>
		/// Один такт работы или получение данных для view
		/// </summary>
		public void Tick()
		{
			foreach (var md in _models) {
				
			}
		}
	}
}
