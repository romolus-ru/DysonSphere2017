using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Utils;
using Engine.Enums;
using Engine.TCPNet;

namespace Engine
{
	/// <summary>
	/// Основная модель - управляет остальными
	/// </summary>
	/// <remarks>Умеет подключать классы модели к обработке</remarks>
	public class ModelMain:Model
	{
		/// <summary>
		/// Модели для добавления
		/// </summary>
		private List<Model> _addModel = new List<Model>();
		/// <summary>
		/// Модели для удаления
		/// </summary>
		private List<Model> _delModel = new List<Model>();
		private bool IsNeedRefresh = false;
		private List<Model> _models = new List<Model>();
		private List<EventModel> _eventModels = new List<EventModel>();
		public ModelMain()
		{
		}

		public void AddModel(Model model)
		{
			_addModel.Add(model);
			IsNeedRefresh = true;
		}

		public void DelModel(Model model)
		{
			_delModel.Add(model);
			IsNeedRefresh = true;
		}

		public void AddEventModel(EventModel eventModel)
		{
			_eventModels.Add(eventModel);
		}

		public void DelEventModel(EventModel eventModel)
		{
			_eventModels.Add(eventModel);
		}

		/// <summary>
		/// Один такт работы или получение данных для view
		/// </summary>
		public override void Tick()
		{
			if (IsNeedRefresh) {
				IsNeedRefresh = false;
				_models.AddRange(_addModel);
				_addModel.Clear();
				foreach (var model in _delModel) {
					_models.Remove(model);
				}
				_delModel.Clear();
			}
			foreach (var md in _models) {
				md.Tick();
			}
		}

		public override void Stop()
		{
			foreach (var md in _models) {
				md.Stop();
			}
		}
	}
}
