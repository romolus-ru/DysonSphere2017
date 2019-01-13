using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Orders;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Windows
{
	internal class OrdersInfosViewWindow : FilteredScrollViewWindow
	{
		private List<OrderInfo> _orderInfos;

		public void InitWindow(ViewManager viewManager, List<OrderInfo> orderInfos)
		{
			_orderInfos = orderInfos;

			InitWindow("Просмотр информации о заказах", viewManager, showOkButton: false, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var i = 1;
			foreach (var oi in _orderInfos) {
				var scrollItem = new OrdersInfosScrollIem(oi);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(1, 1, 400, 100, "ri" + i + " " + oi.Name);
				i++;
			}
		}

		protected override void InitButtonOk(ViewButton btnOk)
		{
			base.InitButtonOk(btnOk);
			btnOk.Caption = "ok";
			btnOk.Hint = "выбрать";
		}

	}
}