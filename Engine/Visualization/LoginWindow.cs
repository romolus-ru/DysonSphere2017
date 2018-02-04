using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Visualization
{
	public class LoginWindow : ViewModalWindow
	{
		private Action<string, string> _toLogin;
		private Action _cancelLogin;
		private ViewManager _viewManager;
		private ViewInput _field1;
		private ViewInput _field2;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.AddKeyActionPaused(TabInputs, Keys.Tab);
			Input.AddKeyActionPaused(PressEnter, Keys.Enter);
		}

		protected override void ClearObject()
		{
			Input.RemoveKeyActionPaused(PressEnter, Keys.Enter);
			Input.RemoveKeyActionPaused(TabInputs);
			base.ClearObject();
		}

		public void InitWindow(ViewManager viewManager, Action<string, string> toLogin, Action cancelLogin, string loginName=null)
		{
			viewManager.AddViewModal(this);
			SetParams(150, 150, 500, 200, "Залогирование");
			InitTexture("textRB", 10);

			var btn1 = new ViewButton();
			AddComponent(btn1);
			btn1.InitButton(Entered, "ok", "Логин", Keys.Enter);
			btn1.SetParams(50, 140, 80, 25, "btnLogin");
			btn1.InitTexture("textRB", "textRB");

			var btn2 = new ViewButton();
			AddComponent(btn2);
			btn2.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btn2.SetParams(150, 140, 80, 25, "btnCancel");
			btn2.InitTexture("textRB", "textRB");

			_field1 = new ViewInput();
			AddComponent(_field1);
			_field1.SetParams(140, 30, 200, 40, "inputField1");
			_field1.InputAction(loginName ?? "");// enterLogin

			_field2 = new ViewInput();
			AddComponent(_field2);
			_field2.SetParams(140, 80, 200, 40, "inputField2");

			TabInputs();
			_toLogin = toLogin;
			_cancelLogin = cancelLogin;
			_viewManager = viewManager;
		}

		private void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			_toLogin = null;
			_cancelLogin = null;
			_viewManager = null;
		}

		private void Cancel()
		{
			_cancelLogin?.Invoke();
			CloseWindow();
		}

		private void Entered()
		{
			if (string.IsNullOrEmpty(_field1.Txt) || string.IsNullOrEmpty(_field2.Txt))
				return;
			_toLogin?.Invoke(_field1.Txt, _field2.Txt);
			CloseWindow();
		}

		private int _tabNum = 0;
		private void TabInputs()
		{
			_tabNum++;
			if (_tabNum > 2) _tabNum = 1;

			_field1.IsFocused = _tabNum == 1;
			_field2.IsFocused = _tabNum == 2;
		}

		private void PressEnter()
		{
			TabInputs();
			Entered();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}
	}
}
