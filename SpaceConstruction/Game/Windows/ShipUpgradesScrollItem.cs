using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;

namespace SpaceConstruction.Game.Windows
{
	internal class ShipUpgradesScrollItem : ScrollItem
	{
		public ItemManager _itemManager { get; private set; }
		private ViewButton btnToShip;
		private ViewButton btnToInventory;
		public Action<ShipUpgradesScrollItem> MoveUpgradeToShip;
		public Action MoveToInventory;

		public ShipUpgradesScrollItem(ItemManager itemManager)
		{
			_itemManager = itemManager;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			btnToShip = new ViewButton();
			AddComponent(btnToShip);
			btnToShip.InitButton(MoveToShip, "Установить", "hint", Keys.S);
			btnToShip.SetParams(10, 55, 140, 20, "btnToShip");
			btnToShip.InitTexture("textRB", "textRB");

			btnToInventory = new ViewButton();
			AddComponent(btnToInventory);// возможно надо будет удалить эту кнопку - она заменена на ShipUpgradesScrollItem
			btnToInventory.InitButton(MoveToInventory, "Установить", "hint", Keys.S);
			btnToInventory.SetParams(10, 55, 140, 20, "btnToInventory");
			btnToInventory.InitTexture("textRB", "textRB");
		}

		private void MoveToShip() => MoveUpgradeToShip?.Invoke(this);

		/// <summary>
		/// Устанавливаем состояние кнопок
		/// </summary>
		/// <param name="active">Активные или не активные</param>
		/// <param name="isInventory">Видима кнопка инвентарь или кнопка корабль</param>
		public void SetCurrentState(bool active, bool isInventory)
		{
			btnToShip.Enabled = active;
			btnToInventory.Enabled = active;

			btnToShip.SetVisible(!isInventory);
			btnToInventory.SetVisible(isInventory);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 10, Y, _itemManager.PlayerCount + " " + _itemManager.Item.Name);
			visualizationProvider.Print(X + 10, Y + 20, _itemManager.Item.Description);
			if (!string.IsNullOrEmpty(_itemManager.Item.Texture))
				visualizationProvider.DrawTexture(X + 40, Y + 40, _itemManager.Item.Texture);
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