using Engine.Data;
using Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Visualization
{
	public class RegistrationWindow : ViewModalWindow
	{
		private Action<string, string> _toLogin;
		private Action _cancelLogin;
		public Action OnClose;
		public Action<UserRegistration> OnRegistration;
		private ViewManager _viewManager;
		private ViewInput _fieldNick;
		private ViewInput _fieldName;
		private ViewInput _fieldMail;
		private ViewInput _fieldPass;
		private UserRegistration _userRegistration;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.AddKeyActionPaused(TabInputs, Keys.Tab);
			Input.AddKeyActionPaused(PressEnter, Keys.Enter);
		}

		protected override void ClearObject()
		{
			Input.RemoveKeyActionPaused(PressEnter, Keys.Enter);
			Input.RemoveKeyActionPaused(TabInputs, Keys.Tab);
			base.ClearObject();
		}

		public void InitWindow(ViewManager viewManager, UserRegistration userRegistration, Action<string, string> toLogin, Action cancelLogin)
		{
			viewManager.AddViewModal(this);
			_userRegistration = userRegistration;
			SetParams(350, 200, 500, 150, "Регистрация игрока");
			InitTexture("WindowSample", 10);

			AddComponent(ViewLabel.Create(10, 10, System.Drawing.Color.Green, "Ваш код (GUID)"));
			AddComponent(ViewLabel.Create(10, 25, System.Drawing.Color.Green, "Ваш ник"));
			AddComponent(ViewLabel.Create(10, 40, System.Drawing.Color.Green, "Официальное имя"));
			AddComponent(ViewLabel.Create(10, 55, System.Drawing.Color.Green, "Почта"));
			AddComponent(ViewLabel.Create(10, 70, System.Drawing.Color.Green, "Пароль"));

			AddComponent(ViewLabel.Create(160, 10, System.Drawing.Color.ForestGreen, _userRegistration.UserGUID));

			_fieldNick = new ViewInput();
			AddComponent(_fieldNick);
			_fieldNick.SetParams(160, 25, 140, 15, "_fieldNick");
			_fieldNick.InputAction(_userRegistration.NickName ?? "");
			_fieldNick.IsFocused = true;

			_fieldName = new ViewInput();
			AddComponent(_fieldName);
			_fieldName.SetParams(160, 40, 140, 15, "_fieldNick");
			_fieldName.InputAction(_userRegistration.OfficialName ?? "");

			_fieldMail = new ViewInput();
			AddComponent(_fieldMail);
			_fieldMail.SetParams(160, 55, 140, 15, "_fieldNick");
			_fieldMail.InputAction(_userRegistration.Mail ?? "");

			_fieldPass = new ViewInput();
			AddComponent(_fieldPass);
			_fieldPass.SetParams(160, 70, 140, 15, "_fieldNick");
			_fieldPass.InputAction("");

			var btn1 = new ViewButton();
			AddComponent(btn1);
			btn1.InitButton(Entered, "Зарегистрироваться", "Зарегистрироваться", Keys.Enter);
			btn1.SetParams(20, 100, 190, 30, "btnRegistration");
			btn1.InitTexture("textRB", "textRB");

			var btn2 = new ViewButton();
			AddComponent(btn2);
			btn2.InitButton(Cancel, "Отмена", "Отмена", Keys.Escape);
			btn2.SetParams(220, 100, 190, 30, "btnCancel");
			btn2.InitTexture("textRB", "textRB");

			TabInputs();
			_toLogin = toLogin;
			_cancelLogin = cancelLogin;
			_viewManager = viewManager;
		}

		private void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			OnRegistration = null;
			OnClose = null;
			_viewManager = null;
		}

		private void Cancel()
		{
			OnClose?.Invoke();
			CloseWindow();
		}

		private void Entered()
		{
			foreach (var input in _inputs) {
				if (string.IsNullOrEmpty(input.Text))
					return;
			}
			if (!string.IsNullOrEmpty(_fieldNick.Text))
				_userRegistration.NickName = _fieldNick.Text;
			if (!string.IsNullOrEmpty(_fieldName.Text))
				_userRegistration.OfficialName = _fieldName.Text;
			if (!string.IsNullOrEmpty(_fieldMail.Text))
				_userRegistration.Mail = _fieldMail.Text;
			if (!string.IsNullOrEmpty(_fieldPass.Text))
				_userRegistration.HSPassword = CryptoHelper.CalculateHash(_fieldPass.Text);
			OnRegistration?.Invoke(_userRegistration);
			CloseWindow();
		}

		private int _tabNum = -1;
		private void TabInputs()
		{
			_tabNum++;
			if (_tabNum >= _inputs.Count) _tabNum = 0;
			foreach (var input in _inputs) {
				input.IsFocused = false;
			}
			_inputs[_tabNum].IsFocused = true;
		}

		private void PressEnter()
		{
			TabInputs();
			Entered();
		}
	}
}