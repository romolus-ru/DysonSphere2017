using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game.Windows
{
	internal class ResearchesBuyWindow : FilteredScrollViewWindow
	{
		private List<ItemManager> _researches;
		private Action _onBuy;
		private Action _onClose;

		public void InitWindow(ViewManager viewManager, List<ItemManager> researches, Action onBuy, Action onClose)
		{
			_researches = researches;
			_onBuy = onBuy;
			_onClose = onClose;

			InitWindow("Покупка результатов исследований", viewManager, showOkButton: false, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var items = _researches.Where(x => x.PlayerCount < 1).OrderBy(x => x.Item.Cost.PlayerCount).ToList();
			items.AddRange(_researches.Where(x => x.PlayerCount > 0));
			var i = 1;
			foreach (var item in items) {
				var scrollItem = new ResearchesBuyScrollItem(item);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.OnBuyed = StartBuy;
				scrollItem.SetParams(1, 1, 980, 100, "ri" + i + " " + item.Item.Name);
				scrollItem.ActivateButton();
				i++;
			}
		}
		
		private void StartBuy()
		{
			_onBuy?.Invoke();
			ViewScroll.ClearItems();
			InitScrollItems();
			UpdateScrollViewSize();
		}

		protected override void InitButtonCancel(ViewButton btnCancel)
		{
			base.InitButtonCancel(btnCancel);
			btnCancel.SetCoordinatesRelative(-100, 0, 0);
			btnCancel.Caption = "закрыть";
			btnCancel.Hint = "закрыть исследования";
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_onClose?.Invoke();
		}

		internal void UpdateBuyButtons()
		{
			foreach (var scrollItem in ViewScroll.GetItems()) {
				var item = scrollItem as ResearchesBuyScrollItem;
				item?.UpdateBuyButton();
			}
		}
	}
}