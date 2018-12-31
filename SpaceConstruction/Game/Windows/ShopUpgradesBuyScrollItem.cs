using Engine;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace SpaceConstruction.Game.Windows
{
	internal class ShopUpgradesBuyScrollItem : ScrollItem
	{
		private ItemManager _mUpgrade;
		private ItemUpgrade _upgradeItem;
		private ViewButton _btnBuy;
		private List<string> _upgrades = null;

		public ShopUpgradesBuyScrollItem(ItemManager upgrade)
		{
			_mUpgrade = upgrade;
			_upgradeItem = upgrade.Item as ItemUpgrade;
			_upgrades = new List<string>();
			for (int i = 0; i < _upgradeItem.Upgrades.Count; i++) {
				var u = _upgradeItem.Upgrades[i];
				var s = u.Name + " " + u.UpName + " " + u.UpValue;
				_upgrades.Add(s);
			}
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			_btnBuy = new ViewButton();
			AddComponent(_btnBuy);
			_btnBuy.InitButton(Buy, "Купить улучшение", "Купить улучшение", Keys.None);
			_btnBuy.SetParams(100, 40, 200, 40, "buy button");
		}

		private void Buy()
		{
			if (!ItemsManager.BuyUpgrade(_mUpgrade))
				StateEngine.Log?.AddLog("нету наличности");
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 100, Y, _upgradeItem.Name);
			//visualizationProvider.Print(X + 100, Y + 20, _researchItem.Description);
			visualizationProvider.Print(X + 100, Y + 20, "цена " + _upgradeItem.Cost.PlayerCount + " ");
			visualizationProvider.PrintTexture(_upgradeItem.Cost.Item.Texture);
			if (!string.IsNullOrEmpty(_upgradeItem.Texture))
				visualizationProvider.DrawTexture(X + 40, Y + 40, _upgradeItem.Texture);
			DrawUpgradeInfo(visualizationProvider, X + 400, Y + 20, _upgradeItem.Quality);
			DrawUpgradesCounters(visualizationProvider, X + 400, Y, _mUpgrade);
			visualizationProvider.SetColor(Color.GreenYellow);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}

		private void DrawUpgradeInfo(VisualizationProvider visualizationProvider, int x, int y, ItemUpgradeQualityEnum quality)
		{
			var color = Color.SandyBrown;
			if (quality == ItemUpgradeQualityEnum.Normal)
				color = Color.Silver;
			if (quality == ItemUpgradeQualityEnum.Extra)
				color = Color.Gold;
			visualizationProvider.SetColor(color);
			for (int i = 0; i < _upgrades.Count; i++) {
				visualizationProvider.Print(x, y + i * 20, _upgrades[i]);
			}
		}

		private void DrawUpgradesCounters(VisualizationProvider visualizationProvider, int x, int y, ItemManager upgrade)
		{
			if (upgrade.PlayerCount <= 0)
				return;

			visualizationProvider.Print(x, y, " ");
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print("куплено: ");
			visualizationProvider.SetColor(Color.Yellow);
			visualizationProvider.Print(upgrade.PlayerCount.ToString());

			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(" установлено: ");
			visualizationProvider.SetColor(Color.Green);
			visualizationProvider.Print(upgrade.SetupCount.ToString());

			if (upgrade.SetupCount < upgrade.PlayerCount) {
				visualizationProvider.SetColor(Color.White);
				visualizationProvider.Print(" в запасе:  ");
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Print((upgrade.PlayerCount - upgrade.SetupCount).ToString());
			}

		}

	}
}