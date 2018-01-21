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

namespace DysonSphere
{
	public class DebugView:ViewWindow
	{
		private List<string> _list = new List<string>();
		private ViewComponent _root = null;
		private ViewButton _mainButton = null;
		private List<ViewButton> _buttons = null;
		private Dictionary<string, ViewWindow> _windows = null;
		private const int btnWidth = 80;
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
			_root = GetRoot(Parent);
			SetDebugHeight(1);
		}

		private void ViewButtons()
		{
			var row = 1;
			if (_buttons==null) {
				_buttons = new List<ViewButton>();
				string[] names = { "ComponentsView", "name2" };
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

			if (window != null) {
				if (_windows == null) _windows = new Dictionary<string, ViewWindow>();
				this.Parent.AddComponent(window);
				window.SetParams(700, 100, 200, 200, name);
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

		private ViewComponent GetRoot(ViewComponent parent)
		{

			if (parent != null) {
				if (parent.Parent != null) return GetRoot(parent.Parent);
				return parent;
			}
			return null;
		}

		private void RefreshList()
		{
			if (_root != null)
				_list = _root.GetObjectsView();
		}


		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			RefreshList();
			var y = 0;
			visualizationProvider.SetColor(Color.Coral);
			foreach (var item in _list) {
				visualizationProvider.Print(10, y, item);
				y += 16;
			}
		}
	}
}
