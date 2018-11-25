using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Orders;
using System.Drawing;

namespace SpaceConstruction.Game.Windows
{
	internal class OrdersInfosScrollIem : ScrollItem
	{
		private OrderInfo _orderInfo;

		public OrdersInfosScrollIem(OrderInfo orderInfo)
		{
			_orderInfo = orderInfo;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 10, Y, _orderInfo.Name + " " + _orderInfo.Level + " GrValues:" + _orderInfo.ResourceGroupValues.Count);
			visualizationProvider.Print(X + 10, Y + 20, _orderInfo.Description);
			if (!string.IsNullOrEmpty(_orderInfo.OrderLogo))
				visualizationProvider.DrawTexture(X + 40, Y + 40, _orderInfo.OrderLogo);
			visualizationProvider.SetColor(Color.GreenYellow);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}

		//public override bool Filtrate(string filter = null)
		//{
		//	if (string.IsNullOrEmpty(filter))
		//		return true;
		//	return _value.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0;
		//}
	}
}