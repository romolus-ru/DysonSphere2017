using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Основной класс, отвечает за связь с внешним миром и отображение информации
	/// </summary>
	public class View
	{
		private VisualizationProvider _provider;
		public View(VisualizationProvider provider)
		{
			_provider = provider;
		}
	}
}