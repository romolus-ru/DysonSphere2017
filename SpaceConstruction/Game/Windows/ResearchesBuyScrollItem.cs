using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System.Drawing;
using Engine;
using System.Windows.Forms;
using System;

namespace SpaceConstruction.Game.Windows
{
	internal class ResearchesBuyScrollItem : ScrollItem
	{
		private Item _researchItem;
		public Action OnBuyed;
		/// <summary>
		/// Уже куплено
		/// </summary>
		private bool _buyed;
		private ViewButton _btnBuy;
		private byte txtAlpha = 100;

		public ResearchesBuyScrollItem(ItemManager research)
		{
			_researchItem = research.Item;
			_buyed = research.PlayerCount > 0;
			if (_buyed)
				txtAlpha = 25;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			if (!_buyed) {
				_btnBuy = new ViewButton();
				AddComponent(_btnBuy);
				_btnBuy.InitButton(Buy, "buy research", "Купить результаты исследований", Keys.B);
				_btnBuy.SetParams(120, 60, 200, 20, "buy button");
			}
		}

		private void Buy()
		{
			if (ItemsManager.BuyItem(_researchItem.Code)) {
				OnBuyed?.Invoke();
				RemoveComponent(_btnBuy);
				_btnBuy = null;
				txtAlpha = 40;
			} else
				StateEngine.Log?.AddLog("нету наличности");
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.DarkGreen);
			visualizationProvider.Print(X + 100, Y, _researchItem.Code);
			visualizationProvider.SetColor(Color.White, txtAlpha);
			visualizationProvider.Print(X + 200, Y, _researchItem.Name);
			visualizationProvider.Print(X + 100, Y + 20, _researchItem.Description);
			visualizationProvider.Print(X + 100, Y + 40, "cost=" + _researchItem.Cost.PlayerCount + " ");
			visualizationProvider.PrintTexture(_researchItem.Cost.Item.Texture);
			if (!string.IsNullOrEmpty(_researchItem.Texture))
				visualizationProvider.DrawTexture(X + 40, Y + 40, _researchItem.Texture);
			visualizationProvider.SetColor(Color.GreenYellow);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}
	}
}