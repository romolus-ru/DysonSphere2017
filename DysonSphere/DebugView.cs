using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Drawing;
using System.Windows.Forms;
using DysonSphere.DebugViewModules;

namespace Engine.Visualization.DebugOld
{
	public class DebugView:ViewWindow
	{
		private List<string> _list = new List<string>();
		private ViewButton _mainButton = null;
		private List<ViewButton> _buttons = null;
		private Dictionary<string, ViewWindow> _windows = null;
		private const int btnWidth = 140;
		private const int btnHeight = 15;
		private const int border = 2;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Width = btnWidth + 2 * border;
			_mainButton = new ViewButton();
			this.AddComponent(_mainButton);
			_mainButton.SetParams(border, btnHeight, btnWidth - border, btnHeight, "MainButton");
			_mainButton.InitButton(ViewButtons, "switch", "", Keys.None);
			SetDebugHeight(1);
		}

		private void ViewButtons()
		{
			var row = 1;
			if (_buttons==null) {
				_buttons = new List<ViewButton>();
				string[] names = { "ComponentsView", "EventsView", "KeysView" };
				ViewButton btn;
				foreach (var name in names) {
					row++;
					btn = new ViewButton();
					AddComponent(btn);
					btn.SetParams(border, row * (btnHeight+2), btnWidth - border, btnHeight, name);
					btn.InitButton(()=>SwitchWindow(name), name, "", Keys.None);
					_buttons.Add(btn);
				}
			}else {
				foreach (var btn in _buttons) {
					RemoveComponent(btn);
				}
				_buttons = null;
			}
			SetDebugHeight(row);
		}

		private void SwitchWindow(string name)
		{
			ViewWindow window = null;
			if (name == "ComponentsView") {
				if (_windows == null || !_windows.ContainsKey(name)) {
					window = new ComponentsView();
				}
			}
			if (name == "EventsView") {
				if (_windows == null || !_windows.ContainsKey(name)) {
					window = new EventsView();
				}
			}
			if (name == "KeysView") {
				if (_windows == null || !_windows.ContainsKey(name)) {
					window = new KeysView();
				}
			}

			if (window != null) {
				if (_windows == null) _windows = new Dictionary<string, ViewWindow>();
				this.Parent.AddComponent(window);
				window.SetParams(700, 100, 400, 400, name);
				window.SetName(name);
				window.InitTexture("WindowSample", 10);
				_windows.Add(name, window);
			} else {
				if (_windows != null&&_windows.ContainsKey(name)) {// удаляем найденное окно
					window = _windows[name];
					this.Parent.RemoveComponent(window);
					_windows.Remove(name);
				}
			}
		}

		private void SetDebugHeight(int rowCount)
		{
			Height = 14 + border + (btnHeight + 2) * rowCount;
		}

	}
}
