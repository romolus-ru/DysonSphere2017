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
		public ItemManager ItemManager { get; }
		private Color _itemColor;
		private ViewButton _btnToShip;
		public Action<ShipUpgradesScrollItem> OnMoveUpgradeToShip { get; set; }

		public ShipUpgradesScrollItem(ItemManager itemManager)
		{
			ItemManager = itemManager;
			var item = itemManager.Item as ItemUpgrade;
			_itemColor = item == null
				? Color.PaleVioletRed
				: ViewUpgradeHelper.GetQualityColor(item.Quality);
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			_btnToShip = new ViewButton();
			AddComponent(_btnToShip);
			_btnToShip.InitButton(MoveToShip, "Установить", "Установить улучшение", Keys.None);
			_btnToShip.SetParams(10, 60, 140, 20, "btnToShip");
			_btnToShip.InitTexture("textRB", "textRB");
		}

		private void MoveToShip() => OnMoveUpgradeToShip?.Invoke(this);

		/// <summary>
		/// Устанавливаем состояние кнопок
		/// </summary>
		/// <param name="active">Активные или не активные</param>
		public void SetCurrentState(bool active)
		{
			_btnToShip.Enabled = active;
		}

		internal void ActivateButton()
		{
			// что бы на кнопку можно было нажать без перемещения курсора
			_btnToShip.CursorOver = true;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 10, Y, "В наличии " + ItemManager.AvailableCount + GameConstants.MeasureUnits);
			visualizationProvider.SetColor(_itemColor);
			visualizationProvider.Print(X + 10, Y + 20, ItemManager.Item.Name);
			visualizationProvider.Print(X + 10, Y + 40, ItemManager.Item.Description);
			if (!string.IsNullOrEmpty(ItemManager.Item.Texture))
				visualizationProvider.DrawTexture(X + 40, Y + 40, ItemManager.Item.Texture);
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