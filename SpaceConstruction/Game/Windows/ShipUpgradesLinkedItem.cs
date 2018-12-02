using Engine;
using Engine.Visualization;
using Engine.Visualization.Text;
using SpaceConstruction.Game.Items;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceConstruction.Game.Windows
{
	/// <summary>
	/// Показывает апгрейд установленный на корабле в виде строки
	/// </summary>
	internal class ShipUpgradesLinkedItem : ViewComponent
	{
		private ItemUpgrade _upgrade;
		private ViewText _text = null;
		private ViewButton _btn = null;
		public Action<ItemUpgrade> OnRemoveItem;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			_btn = new ViewButton();
			AddComponent(_btn);
			_btn.InitButton(RemoveItem, "X", "hint", Keys.S);
			_btn.SetParams(1, 1, 30, 20, "_btn");
			_btn.InitTexture("textRB", "textRB");
			_btn.SetVisible(false);

			_text = new ViewText();
			AddComponent(_text);
			_text.SetParams(35, 1, 300, 20, "ViewText");
			_text.ClearTexts();
		}

		private void RemoveItem()
		{
			OnRemoveItem?.Invoke(_upgrade);
		}

		public void SetUpgrade(ItemUpgrade itemUpgrade)
		{
			_upgrade = itemUpgrade;
			_btn.SetVisible(_upgrade != null);
			_text.ClearTexts();

			if (_upgrade == null) {
				var tr = _text.CreateTextRow();
				_text.AddText(tr, Color.LightGray, null, "EMPTY", textAlign: Engine.Enums.TextAlign.Center);
			} else {
				var tr = _text.CreateTextRow();
				_text.AddText(tr, Color.LightYellow, null, _upgrade.Name, textAlign: Engine.Enums.TextAlign.Left);
			}
		}
	}
}