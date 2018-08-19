using Engine;
using Engine.DataPlus;
using Engine.EventSystem.Event;
using Engine.Helpers;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	public class DataEditor<T> : ViewModalWindow where T : EventBase
	{
		private Action<T> _update;
		private Action _cancel;
		private ViewManager _viewManager;
		private ViewScroll _viewScroll;
		private T _objectToEdit;
		private DataSupportBase _dataSupport;

		public void InitWindow(ViewManager viewManager, T objectToEdit, Action<T> update, Action cancel = null, DataSupportBase dataSupport=null)
		{
			viewManager.AddViewModal(this);
			SetParams(150, 150, 1200, 700, "Редактировать");
			InitTexture("textRB", 10);

			var btnSave = new ViewButton();
			AddComponent(btnSave);
			btnSave.InitButton(Entered, "ok", "сохранить", Keys.Enter);
			btnSave.SetParams(50, 670, 80, 25, "btnSave");
			btnSave.InitTexture("textRB", "textRB");

			var btnCancel = new ViewButton();
			AddComponent(btnCancel);
			btnCancel.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btnCancel.SetParams(150, 670, 80, 25, "btnCancel");
			btnCancel.InitTexture("textRB", "textRB");

			_update = update;
			_cancel = cancel;
			_viewManager = viewManager;
			_objectToEdit = objectToEdit;
			_dataSupport = dataSupport;

			_viewScroll = new ViewScroll();
			AddComponent(_viewScroll);
			_viewScroll.SetParams(10, 80, 1000, 560, "Список редактируемых свойств");

			var row = 0;
			var t = objectToEdit.GetType();
			var mi = t.GetMembers().Where(m => m.MemberType == MemberTypes.Property);
			foreach (PropertyInfo item in mi) {
				if (AttributesHelper.IsHasAttribute<MemberEditorAttribute>(item)) {
					var type = AttributesHelper.GetMemberEditorType(item);
					var scrollItem = new MemberCollectorClassScrollViewItem<T>(_viewManager, _dataSupport, type);
					_viewScroll.AddComponent(scrollItem);
					scrollItem.InitValueEditor(objectToEdit, item);
					scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "item" + item);
				} else {
					var scrollItem = new MemberScrollView<T>();
					_viewScroll.AddComponent(scrollItem);
					scrollItem.InitValueEditor(objectToEdit, item);
					scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "item" + item);
					if (item.PropertyType.Name == "String")
						scrollItem.SetupMemberEditor(getValue: str => str);
				}
				row++;
			}

		}

		private void Entered()
		{
			GetValues();
			_update?.Invoke(_objectToEdit);
			CloseWindow();
		}

		/// <summary>
		/// Получить значения переменных
		/// </summary>
		private void GetValues()
		{
			var a = _viewScroll.GetItems();
			foreach (var item in a) {
				var m = item as MemberBaseScrollView<T>;
				if (m == null) continue;

				m.SetValue(_objectToEdit);
			}
		}

		private void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			_update = null;
			_cancel = null;
			_viewManager = null;
		}

		private void Cancel()
		{
			_cancel?.Invoke();
			CloseWindow();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}
	}
}