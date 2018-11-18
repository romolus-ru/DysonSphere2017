using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Resources;
using System;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Windows
{
	public class ResourcesInfosViewWindow : FilteredScrollViewWindow
	{
		private Action<long> _selectedGame;
		private Action _cancel;
		private List<ResourceInfo> _resourceInfos;

		public void InitWindow(ViewManager viewManager, List<ResourceInfo> resourceInfos)
		{
			_resourceInfos = resourceInfos;

			InitWindow("Просмотр информации о ресурсах", viewManager, showOkButton: true);
		}

		protected override void InitScrollItems()
		{
			//var i = 1;
			//foreach (var game in _dataSupport.GetMinigames()) {
			//	var scrollItem = new GameNameScrollView(game);
			//	ViewScroll.AddComponent(scrollItem);
			//	scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, game.Id + " " + game.Name);
			//	scrollItem.OnEdit += EditMiniGame;
			//	scrollItem.OnSelect += SelectSection;
			//	i++;
			//}
		}

		protected override void InitButtonOk(ViewButton btnOk)
		{
			base.InitButtonOk(btnOk);
			btnOk.Caption = "ok";
			btnOk.Hint = "выбрать";
		}

	}
}