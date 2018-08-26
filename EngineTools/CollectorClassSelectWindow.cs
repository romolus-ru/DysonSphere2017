using Engine;
using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;

namespace EngineTools
{
	/// <summary>
	/// Выбрать из коллекции коллектора нужный тип класса и/или добавить новый
	/// </summary>
	/// <typeparam name="T">Класс-основа с которым будет работать Коллектор (например EventBase или VisualizationProvider)</typeparam>
	public class CollectorClassSelectWindow<T> : FilteredScrollViewWindow where T : class
	{
		private Action<CollectClass> _selectedCollectorClass;
		private Action _cancel;
		private ViewManager _viewManager;
		private DataSupportBase _dataSupport;

		public void InitWindow(ViewManager viewManager, DataSupportBase dataSupport, Action<CollectClass> selectedCollectorClass, Action cancel)
		{
			_selectedCollectorClass = selectedCollectorClass;
			_cancel = cancel;
			_viewManager = viewManager;
			_dataSupport = dataSupport;

			InitWindow("Выбор класса из Коллектора", viewManager, false);
		}

		protected override void InitScrollItems()
		{
			var classes = ToolsCollectorManager.GetCollectClasses<T>(_dataSupport);
			UpdateScroll(classes);
		}

		protected override void InitButtonNew(ViewButton btnNew)
		{
			base.InitButtonNew(btnNew);
			btnNew.Caption = "Scan";
			btnNew.Hint = "Scan";
		}

		protected override void NewCommand()
		{
			var classes = ToolsCollectorManager.GetAllClasses<T>(_dataSupport);
			UpdateScroll(classes);
		}

		private void UpdateScroll(List<CollectClass> classes)
		{
			var i = 2;
			ViewScroll.ClearItems();
			foreach (var collectClass in classes) {
				var scrollItem = new CollectorClassScrollViewItem(collectClass);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, collectClass.Id + " " + collectClass.ClassName);
				scrollItem.OnDelete += DeleteCollectClass;
				scrollItem.OnSelect += SelectCollectClass;
				i++;
			}
			ViewScroll.CalcScrollSize();
		}

		private void SelectCollectClass(CollectorClassScrollViewItem item)
		{
			ToolsCollectorManager.SaveNewCollectorClass(_dataSupport, item.CollectClass);
			_selectedCollectorClass?.Invoke(item.CollectClass);
			CloseWindow();
		}

		private void DeleteCollectClass(CollectorClassScrollViewItem itemToDel)
		{
			_dataSupport.DeleteCollectClasses(itemToDel.CollectClass);
			ViewScroll.RemoveComponent(itemToDel);
			UpdateScrollViewSize();
		}
	}
}