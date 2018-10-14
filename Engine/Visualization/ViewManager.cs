using System;
using System.Diagnostics;
using Engine.Utils;

namespace Engine.Visualization
{
	/// <summary>
	/// Основной класс - управляет всеми остальными View. Через ViewSystem
	/// </summary>
	public class ViewManager
	{
		public VisualizationProvider Provider { get; private set; }
		private Input _input;
		/// <summary>
		/// Подсказки, важная информация и т.п. - всё что должно быть всегда наверху
		/// </summary>
		/// <remarks>Если нужно будет расширить эту систему то можно будет ввести словарь с уровнями</remarks>
		private ViewSystem _viewSystemTop;
		private ViewSystem _viewSystem;
		private ViewCursor _viewCursor;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ViewHint _viewHint;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ViewBigMessages _viewBigMessages;

		public ViewManager(VisualizationProvider provider, Input input)
		{
			Provider = provider;
			_input = input;
			_viewSystemTop = new ViewSystem();
			_viewSystemTop.SetParams(0, 0, provider.CanvasWidth, provider.CanvasHeight, "ViewSystemTop");
			_viewSystemTop.Init(Provider, input);
			_viewSystem = new ViewSystem();
			_viewSystem.SetParams(0, 0, provider.CanvasWidth, provider.CanvasHeight, "ViewSystem");
			_viewSystem.Init(Provider, input);
			_input.AddCursorAction(_viewSystem.CursorHandler);
			_input.OnModalStateChanged += CursorOverHide;

			_viewCursor = new ViewCursor();
			_viewCursor.Init(Provider, input);

			_viewHint = new ViewHint();
			//_viewHint.Init(Provider, input);
			_viewSystemTop.AddComponent(_viewHint);
			_viewHint.Hide();

			_viewBigMessages = new ViewBigMessages();
			//_viewBigMessages.Init(Provider, input);
			_viewBigMessages.SetParams(0, 0, provider.CanvasWidth, provider.CanvasHeight, "BigMessages");
			_viewSystemTop.AddComponent(_viewBigMessages);
			if (StateEngine.Log != null)
				StateEngine.Log.OnNewLogRecieved += NewLogRecieved;
		}

		private void CursorOverHide()
		{
			_viewSystem.CursorOverClear();
		}

		/// <summary>
		/// Вывод информации обычных компонентов, в том числе и модальных окон
		/// </summary>
		/// <param name="view"></param>
		/// <param name="toTop"></param>
		public void AddView(ViewComponent view, bool toTop = false)
		{
			_viewSystem.AddComponent(view, toTop);
		}

		/// <summary>
		/// Вывод информации которая должна быть всегда наверху - ачивки, сообщения, подсказки и т.п.
		/// </summary>
		public void AddViewSystem(ViewComponent view, bool toTop = false)
		{
			_viewSystemTop.AddComponent(view, toTop);
		}
		public void RemoveView(ViewComponent view)
		{
			_viewSystem.RemoveComponent(view);
			_viewSystemTop.RemoveComponent(view);
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
			Provider.BeginDraw();

			_viewSystem.Draw(Provider);
			_viewSystemTop.Draw(Provider);

			_viewCursor.Draw(Provider);
			Provider.FlushDrawing();
		}

		public void WindowPosChanged(int windowPosX, int windowPosY)
		{
			_viewSystemTop.SetScreenPos(windowPosX, windowPosY);
			_viewSystem.SetScreenPos(windowPosX, windowPosY);
		}

		public void ShowHint(ViewComponent component, string hintText, string hintKeys = null)
		{
			_viewHint.ShowHint(component, TimeSpan.FromSeconds(Constants.HintHidePause), hintText, hintKeys);
		}

		private void NewLogRecieved(LogData logData)
		{
			ShowBigMessage(logData.ToString());
		}

		public void ShowBigMessage(string message)
		{
			_viewBigMessages.ShowMessage(TimeSpan.FromSeconds(7), "BigFont", message);
		}
	}
}