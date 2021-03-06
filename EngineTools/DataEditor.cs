﻿using Engine;
using Engine.DataPlus;
using Engine.EventSystem;
using Engine.EventSystem.Event;
using Engine.Helpers;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Linq;
using System.Reflection;

namespace EngineTools
{
	public class DataEditor<T> : FilteredScrollViewWindow where T : EventBase
	{
		private Action<T> _update;
		private Action _cancel;
		private T _objectToEdit;
		private DataSupportBase _dataSupport;

		public void InitWindow(ViewManager viewManager, T objectToEdit, Action<T> update, Action cancel = null, DataSupportBase dataSupport=null)
		{
			_update = update;
			_cancel = cancel;
			_objectToEdit = objectToEdit;
			_dataSupport = dataSupport;

			InitWindow("Редактор для " + objectToEdit.GetType(), viewManager, showOkButton: true);
		}

		protected override void InitScrollItems()
		{
			var row = 0;
			var t = _objectToEdit.GetType();
			var mi = t.GetMembers().Where(m => m.MemberType == MemberTypes.Property);
			foreach (PropertyInfo item in mi) {
				if (AttributesHelper.IsHasAttribute<SkipEditEditorAttribute>(item))
					continue;
				if (AttributesHelper.IsHasAttribute<MemberCollectorClassEditorAttribute>(item)) {
					var type = AttributesHelper.GetMemberCollectorClassEditorType(item);
					var scrollItem = new MemberCollectorClassScrollViewItem<T>(ViewManager, _dataSupport, type);
					ViewScroll.AddComponent(scrollItem);
					scrollItem.InitValueEditor(_objectToEdit, item);
					scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "item" + item);
				} else if (AttributesHelper.IsHasAttribute<MemberSpecialEditorAttribute>(item)) {
					var type = AttributesHelper.GetAttribute<MemberSpecialEditorAttribute>(item);
					if (type.EditorType == "SelectClassInFile") {// редактируем поля хранящие имя файла и имя класса 
						var scrollItem = new MemberClassInFileScrollView<T>(ViewManager);
						ViewScroll.AddComponent(scrollItem);
						scrollItem.InitValueEditor(_objectToEdit, item);
						scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "item" + item);
					}
				} else {
					var scrollItem = new MemberScrollView<T>();
					ViewScroll.AddComponent(scrollItem);
					scrollItem.InitValueEditor(_objectToEdit, item);
					scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "item" + item);
					if (item.PropertyType.Name == "String")
						scrollItem.SetupMemberEditor(getValue: str => str);
				}
				row++;
			}
		}

		protected override void InitButtonOk(ViewButton btnOk)
		{
			base.InitButtonOk(btnOk);
			btnOk.Caption = "Ok";
			btnOk.Hint = "Сохранить";
		}

		protected override void OkCommand()
		{
			GetValues();
			if (_update == null)
				return;
			var updater = _update;
			Checkers.AddToCheckOnce(() => updater?.Invoke(_objectToEdit));
		}

		/// <summary>
		/// Получить значения переменных
		/// </summary>
		private void GetValues()
		{
			var a = ViewScroll.GetItems();
			foreach (var item in a) {
				var m = item as MemberBaseScrollView<T>;
				if (m == null) continue;

				m.SetValue(_objectToEdit);
			}
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_update = null;
			_cancel = null;
		}

		protected override void CancelCommand()
		{
			if (_cancel == null) return;
			var canceler = _cancel;
			Checkers.AddToCheckOnce(() => canceler?.Invoke());
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}
	}
}