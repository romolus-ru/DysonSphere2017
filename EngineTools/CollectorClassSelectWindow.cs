using Engine;
using Engine.Data;
using Engine.Visualization;
using EngineTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	/// <summary>
	/// Выбрать из коллекции коллектора нужный тип класса и/или добавить новый
	/// </summary>
	/// <typeparam name="T">Класс-основа с которым будет работать Коллектор (например EventBase или VisualizationProvider)</typeparam>
	public class CollectorClassSelectWindow<T> : ViewModalWindow where T : class
	{
		private Action<CollectClass> _selectedCollectorClass;
		private Action _cancel;
		private ViewManager _viewManager;
		private ViewInput _filter;
		private ViewScroll _viewScroll;
		private DataSupportBase _dataSupport;

		public void InitWindow(ViewManager viewManager, DataSupportBase dataSupport, Action<CollectClass> selectedCollectorClass, Action cancel)
		{
			viewManager.AddViewModal(this);
			SetParams(150, 150, 1200, 700, "Выбор класса из Коллектора");
			InitTexture("textRB", 10);

			var btnCancel = new ViewButton();
			AddComponent(btnCancel);
			btnCancel.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
			btnCancel.SetParams(150, 670, 80, 25, "btnCancel");
			btnCancel.InitTexture("textRB", "textRB");

			var btnNew = new ViewButton();
			AddComponent(btnNew);
			btnNew.InitButton(FullScanForType, "Scan", "Scan", Keys.S);
			btnNew.SetParams(20, 20, 80, 25, "btnScan");
			btnNew.InitTexture("textRB", "textRB");

			_filter = new ViewInput();
			AddComponent(_filter);
			_filter.SetParams(140, 30, 500, 40, "filter");
			_filter.InputAction("");

			_selectedCollectorClass = selectedCollectorClass;
			_cancel = cancel;
			_viewManager = viewManager;
			_dataSupport = dataSupport;

			_viewScroll = new ViewScroll();
			AddComponent(_viewScroll);
			_viewScroll.SetParams(10, 80, 1000, 560, "Список классов");
			ScanForType();
		}

		private void ScanForType()
		{
			var classes = ToolsCollectorManager.GetCollectClasses<T>(_dataSupport);
			UpdateScroll(classes);
		}

		private void FullScanForType()
		{
			var classes = ToolsCollectorManager.GetAllClasses<T>(_dataSupport);
			UpdateScroll(classes);
		}

		private void UpdateScroll(List<CollectClass> classes)
		{
			var i = 2;
			_viewScroll.ClearItems();
			foreach (var collectClass in classes) {
				var scrollItem = new CollectorClassScrollViewItem(collectClass);
				_viewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, collectClass.Id + " " + collectClass.ClassName);
				scrollItem.OnDelete += DeleteCollectClass;
				scrollItem.OnSelect += SelectCollectClass;
				i++;
			}
			_viewScroll.CalcScrollSize();
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
			_viewScroll.RemoveComponent(itemToDel);
			var scrollItems = _viewScroll.GetItems();// лучше бы чтоб это ViewScroll сам умел делать
			var h = itemToDel.Height;
			var y = itemToDel.Y;
			if (itemToDel == null) return;
			_viewScroll.RemoveComponent(itemToDel);// удаляем элемент а остальные ставим выше
			scrollItems = _viewScroll.GetItems();
			foreach (CollectorClassScrollViewItem item in scrollItems) {
				if (item.Y < y) continue;
				item.SetCoordinatesRelative(0, -y, 0);
			}
		}

		private void CloseWindow()
		{
			_viewManager.RemoveViewModal(this);
			_selectedCollectorClass = null;
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