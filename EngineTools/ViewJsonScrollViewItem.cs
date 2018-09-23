using Engine;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Engine.Visualization.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EngineTools
{
	/// <summary>
	/// Просмотр элемента сохраненного в джисоне. первоначально просто просмотр как текста, но возможно потом надо будет добавить возможность задать отображение
	/// Так же нужно будет добавить что бы была возможность при редактировании обрабатывать Enum
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ViewJsonScrollViewItem<T> : ScrollItem where T : class
	{
		private Action _onItemChanged;// признак что надо сохранить данные
		private Action<ScrollItem, T> _onDelete;// удалить надо и скролайтем и сам элемент
		private T _item;
		private ViewText _viewText;
		private ViewManager _viewManager;

		public ViewJsonScrollViewItem(ViewManager viewManager, T item, Action onItemChanged, Action<ScrollItem, T> onDelete)
		{
			_viewManager = viewManager;
			_item = item;
			_onItemChanged = onItemChanged;
			_onDelete = onDelete;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var btnDelete = new ViewButton();
			AddComponent(btnDelete);
			btnDelete.InitButton(DeleteRow, "delete", "Удалить", Keys.None);
			btnDelete.SetParams(20, 10, 60, 30, "DeleteRow");
			btnDelete.InitTexture("textRB", "textRB");

			var btnEdit = new ViewButton();
			AddComponent(btnEdit);
			btnEdit.InitButton(EditRow, "Edit", "Редактировать", Keys.None);
			btnEdit.SetParams(90, 10, 120, 30, "Редактировать");
			btnEdit.InitTexture("textRB", "textRB");

			_viewText = new ViewText();
			AddComponent(_viewText);
			FillText();
		}

		/// <summary>
		/// Заполнить текст данными
		/// </summary>
		protected virtual void FillText()
		{
			_viewText.SetParams(250, 5, 500, 200, "Class");
			_viewText.ClearTexts();
			var mis = _item.GetType().GetProperties();
			foreach (var mi in mis) {
				var value = mi.GetValue(_item);
				var strValue = value == null ? null : " " + value.ToString();
				var str = mi.Name + strValue;
				var tr = _viewText.CreateTextRow();
				_viewText.AddText(tr, Color.White, null, str);
			}
			_viewText.CalculateTextPositions();
		}

		private void DeleteRow()
		{
			_onDelete?.Invoke(this, _item);
		}

		private void EditRow()
		{
			EditRow(_viewManager, _item, UpdateObject, _onDelete);
		}

		public static void EditRow(ViewManager viewManager, T item, Action<T> update, Action<ScrollItem, T> onDelete = null)
		{
			//new DataEditor<T>().InitWindow(_viewManager, _item, UpdateObject, dataSupport: null);
			var type1 = typeof(T);
			var type2 = typeof(DataEditor<>);
			var windowType = type2.MakeGenericType(new Type[] { type1 });
			var window = Activator.CreateInstance(windowType);
			IEnumerable<MethodInfo> ms = windowType.GetMethods().Where(mv => mv.Name == "InitWindow");
			MethodInfo m = null;
			foreach (var method in ms) {
				var parameters = method.GetParameters();
				if (parameters.Length < 5) continue;
				var p2 = parameters[4];
				if (p2.Name == "dataSupport")
					m = method;
			}
			if (m != null) {
				object[] parametersArray = { viewManager, item, update, onDelete, null };
				m.Invoke(window, parametersArray);
			} else
				StateEngine.Log.AddLog("InitWindow in DataEditor not found");
		}

		private void UpdateObject(T objectToUpdate)
		{
			FillText();
			_onItemChanged?.Invoke();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(
				CursorOver
				? Color.YellowGreen
				: Color.Red);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}

		public override bool Filtrate(string filter = null)
		{
			if (string.IsNullOrEmpty(filter))
				return true;
			var mis = _item.GetType().GetProperties();
			foreach (var mi in mis) {
				var str = mi.GetValue(_item).ToString();
				if (str.Contains(filter))
					return true;
			}
			return false;
		}
	}
}