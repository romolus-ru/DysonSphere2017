using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// Инициализирует игру. вид и модель
	/// </summary>
	class GameInitializer
	{
		public void InitGame(ModelMain modelMain, ViewMain viewMain, VisualizationProvider provider)
		{
			var model = CreateModel();
			var view = CreateView(provider);
			InitDelegates(model, view);
			// TODO добавить модель и вид в нужные места
			// TODO посмотреть может быть можно сделать интерфейс для логирования. может быть и partial для этого использовать
		}

		/// <summary>
		/// Соединяем нужные делегаты модели и вида
		/// </summary>
		/// <param name="model"></param>
		/// <param name="view"></param>
		protected void InitDelegates(Model model, View view)
		{
			
		}

		/// <summary>
		/// Создаём нужную модель
		/// </summary>
		/// <returns></returns>
		protected virtual Model CreateModel()
		{
			return null;
		}

		/// <summary>
		/// Создаём нужный вид
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		protected virtual View CreateView(VisualizationProvider provider)
		{
			return null;
		}

		public 
	}
}
