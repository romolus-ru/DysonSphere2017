using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Windows
{
	internal class ResearchesBuyWindow : FilteredScrollViewWindow
	{
		private List<ItemManager> _researches;
		private Action _buyed;

		public void InitWindow(ViewManager viewManager, List<ItemManager> researches, Action buyed)
		{
			_researches = researches;
			_buyed = buyed;

			InitWindow("Покупка результатов исследований", viewManager, showOkButton: true, showNewButton: false, showCancelButton: false);
		}

		protected override void InitScrollItems()
		{
			var i = 1;
			foreach (var ri in _researches) {
				var scrollItem = new ResearchesBuyScrollItem(ri);
				scrollItem.OnBuyed = _buyed;
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(1, 1, 980, 100, "ri" + i + " " + ri.Item.Name);
				i++;
			}
		}
		
		protected override void InitButtonOk(ViewButton btnOk)
		{
			base.InitButtonOk(btnOk);
			btnOk.Caption = "закрыть";
			btnOk.Hint = "закрыть окно";
		}
	}
}