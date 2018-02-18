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
		private Input _input;
		private ViewSystem _viewSystem;
		private ViewCursor _viewCursor;

		public ViewManager(VisualizationProvider provider, Input input)
		{
			_provider = provider;
			_input = input;
			_viewSystem = new ViewSystem();
			_viewSystem.SetParams(0, 0, provider.CanvasWidth, provider.CanvasHeight, "ViewSystem");
			_viewSystem.Init(_provider, input);
			_input.AddCursorAction(_viewSystem.CursorHandler);
			_viewCursor = new ViewCursor();
			_viewCursor.Init(_provider, input);
		}

		public void AddView(ViewComponent view, bool toTop = false)
		{
			_viewSystem.AddComponent(view, toTop);
		}

		public void RemoveView(ViewComponent view)
		{
			_viewSystem.RemoveComponent(view);
		}

		public void AddViewModal(ViewComponent view)
		{
			_input.ModalStateStart();
			AddView(view, true);
		}

		public void RemoveViewModal(ViewComponent view)
		{
			_input.ModalStateStop();
			RemoveView(view);
		}		
		
		/// <summary>
		/// Визуализируем имеющиеся данные
		/// </summary>
		public void Draw()
		{
			_provider.BeginDraw();

			_viewSystem.Draw(_provider);

			_viewCursor.Draw(_provider);
			_provider.FlushDrawing();
		}

		public void WindowPosChanged(int windowPosX, int windowPosY)
		{
			_viewSystem.SetScreenPos(windowPosX, windowPosY);
		}
	}
}
