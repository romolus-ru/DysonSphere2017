using Engine;
using Engine.DataPlus;
using Engine.Enums.Client;
using Engine.Visualization;
using Engine.Visualization.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DysonSphereClient
{
	/// <summary>
	/// Меню после заставки и загрузки. начальный вариант
	/// </summary>
	/// <remarks>Логин/регистрация/запуск/выбор - всё тут</remarks>
	public class ViewMenu : ViewComponent
	{
		private ViewManager _viewManager;// TODO избавиться от ViewManager в этом классе - всё должен делать ModelViewManager
		private Stopwatch _stopwatch;

		public Action OnConnect;
		public Action OnSendLogin;
		public Action OnRegistration;
		public Action OnExitPressed;
		public Action OnStartGame;

		public ViewMenu(ViewManager viewManager, Stopwatch stopwatch)
		{
			_viewManager = viewManager;
			_stopwatch = stopwatch;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewMenu");

			var clientView = new ClientView();
			clientView.SetTimerInfo(_stopwatch);
			AddComponent(clientView);

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			var debugView = new DebugView();
			AddComponent(debugView);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugViewMenu");
			
			var pnl = new ViewPanel();
			AddComponent(pnl);
			pnl.SetParams(100, 100, 600, 400, "Pnl");

			var btn1 = new ViewButton();
			pnl.AddComponent(btn1);
			btn1.InitButton(MsgToModel, "c", "hint", Keys.Y);
			btn1.SetParams(20, 20, 40, 40, "btn1");
			btn1.InitTexture("textRB", "textRB");

			var btn2 = new ViewButton();
			pnl.AddComponent(btn2);
			btn2.InitButton(RunModalWindow, "Тестовое модальное окно", "hint", Keys.U);
			btn2.SetParams(70, 20, 240, 23, "btn2");
			btn2.InitTexture("textRB", "textRB");

			var btn3 = new ViewButton();
			pnl.AddComponent(btn3);
			btn3.InitButton(() => OnConnect?.Invoke(), "Соединиться с сервером", "hint", Keys.U);
			btn3.SetParams(70, 45, 240, 23, "btn3");
			btn3.InitTexture("textRB", "textRB");

			var btn4 = new ViewButton();
			pnl.AddComponent(btn4);
			btn4.InitButton(() => OnRegistration?.Invoke(), "Регистрация", "hint", Keys.I);
			btn4.SetParams(70, 70, 240, 23, "btn4");
			btn4.InitTexture("textRB", "textRB");

			var btn5 = new ViewButton();
			pnl.AddComponent(btn5);
			btn5.InitButton(() => OnSendLogin?.Invoke(), "Залогиниться", "login", Keys.L);
			btn5.SetParams(70, 95, 240, 23, "btn5");
			btn5.InitTexture("textRB", "textRB");

			var btn6 = new ViewButton();
			pnl.AddComponent(btn6);
			btn6.InitButton(() => OnStartGame?.Invoke(), "Запустить игру", "StartGame", Keys.R);
			btn6.SetParams(70, 120, 240, 23, "btn6");
			btn6.InitTexture("textRB", "textRB");
		}

		protected override void ClearObject()
		{
			base.ClearObject();
		}

		private void MsgToModel()
		{
			new LoginWindow().InitWindow(_viewManager, null, null);
		}

		private ViewModalWindow win;
		private void RunModalWindow()
		{
			win = new ViewModalWindow();
			_viewManager.AddViewModal(win);
			win.SetParams(150, 150, 500, 150, "Окно");
			win.InitTexture("WindowSample", 10);

			var btn1 = new ViewButton();
			win.AddComponent(btn1);
			btn1.InitButton(Entered, "ok", "Согласен", Keys.Enter);
			btn1.SetParams(20, 110, 40, 25, "btn1");
			btn1.InitTexture("WindowSample", "WindowSample");

			var btn2 = new ViewButton();
			win.AddComponent(btn2);
			btn2.InitButton(CloseModalWindow, "Cancel", "Отмена", Keys.Escape);
			btn2.SetParams(70, 110, 40, 25, "btn2");
			btn2.InitTexture("WindowSample", "WindowSample");

			var field = new ViewInput();
			win.AddComponent(field);
			field.SetParams(30, 30, 200, 40, "inputField");
			field.IsFocused = true;
		}

		private void Entered()
		{
			CloseModalWindow();
		}

		private void CloseModalWindow()
		{
			if (win == null) return;
			_viewManager.RemoveViewModal(win);
			win = null;
		}

		private void Close()
		{
			OnExitPressed();
		}








	}
}
