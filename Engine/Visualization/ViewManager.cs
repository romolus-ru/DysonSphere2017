using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Основной класс - управляет всеми остальными View. Через ViewSystem
	/// </summary>
	public class ViewManager
	{
		private VisualizationProvider _provider;
		private ViewSystem _viewSystem;

		public ViewManager(VisualizationProvider provider)
		{
			_provider = provider;
			_viewSystem = new ViewSystem(_provider);
		}

		public void AddView(View view)
		{
			//_component.Add(view);
		}

		int a = 1;
		/// <summary>
		/// Визуализируем имеющиеся данные
		/// </summary>
		public void Draw()
		{
			_provider.BeginDraw();
			// TODO сделать простую визуализацию чтоб показывать таймер сохраненный на сервере

			_provider.SetColor(System.Drawing.Color.Teal);
			_provider.Box(10, 10, 1660, 1030);
			a++;
			if (a > 1600) a = 0;
			_provider.SetColor(System.Drawing.Color.Indigo);
			_provider.Box(a, 100, 200, 300);


			_provider.FlushDrawing();
		}
	}
}
