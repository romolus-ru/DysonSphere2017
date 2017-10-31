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
		private ViewCursor _viewCursor;

		public ViewManager(VisualizationProvider provider, Input input)
		{
			_provider = provider;
			_viewSystem = new ViewSystem();
			_viewSystem.Init(_provider, input);
			_viewCursor = new ViewCursor();
			_viewCursor.Init(_provider, input);
		}

		public void AddView(ViewComponent view)
		{
			_viewSystem.AddComponent(view);
		}

		int a = 1;
		/// <summary>
		/// Визуализируем имеющиеся данные
		/// </summary>
		public void Draw()
		{
			_provider.BeginDraw();

			_viewSystem.Draw(_provider);
			// TODO сделать простую визуализацию чтоб показывать таймер сохраненный на сервере

			//_provider.SetColor(System.Drawing.Color.Teal);
			//_provider.Box(10, 10, 1660, 1030);
			//a++;if (a > 1600) a = 0;
			//_provider.SetColor(System.Drawing.Color.Indigo);
			//_provider.Box(a, 100, 200, 300);

			_viewCursor.Draw(_provider);
			_provider.FlushDrawing();
		}
	}
}
