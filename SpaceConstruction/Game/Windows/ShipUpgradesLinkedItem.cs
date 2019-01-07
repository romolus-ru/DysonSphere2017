using Engine;
using Engine.Enums;
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
		private ViewButton _btnRemove = null;
		public Action<ItemUpgrade> OnRemoveItem;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			_btnRemove = new ViewButton();
			AddComponent(_btnRemove);
			_btnRemove.InitButton(RemoveItem, "X", "Удалить улучшение", Keys.None);
			_btnRemove.SetParams(1, 1, 30, 20, "_btnRemove");
			_btnRemove.InitTexture("textRB", "textRB");
			_btnRemove.SetVisible(false);

			_text = new ViewText();
			AddComponent(_text);
			_text.SetParams(35, 1, 300, 20, "ViewText");
			_text.ClearTexts();
		}

		private void RemoveItem()
		{
			OnRemoveItem?.Invoke(_upgrade);
		}

		internal void ActivateButton()
		{
			// что бы на кнопку можно было нажать без перемещения курсора
			_btnRemove.CursorOver = true;
		}

		public void SetUpgrade(ItemUpgrade itemUpgrade)
		{
			_upgrade = itemUpgrade;
			_btnRemove.SetVisible(_upgrade != null);
			_text.ClearTexts();

			if (_upgrade == null) {
				var tr = _text.CreateTextRow();
				_text.AddText(tr, Color.LightGray, null, "EMPTY", textAlign: TextAlign.Center);
			} else {
				var tr = _text.CreateTextRow();
				_text.AddText(tr, ViewUpgradeHelper.GetQualityColor(_upgrade.Quality),
					null, _upgrade.Name, textAlign: TextAlign.Left);
			}
		}
	}
}