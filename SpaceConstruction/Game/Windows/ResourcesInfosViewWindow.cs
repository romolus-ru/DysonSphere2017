using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Resources;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Windows
{
	public class ResourcesInfosViewWindow : FilteredScrollViewWindow
	{
		private List<ResourceInfo> _resourceInfos;

		public void InitWindow(ViewManager viewManager, List<ResourceInfo> resourceInfos)
		{
			_resourceInfos = resourceInfos;

			InitWindow("Просмотр информации о ресурсах", viewManager, showOkButton: false, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var i = 1;
			foreach (var ri in _resourceInfos) {
				var scrollItem = new ResourcesInfosScrollItem(ri);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(1, 1, 400, 100, "ri" + i + " " + ri.Name);
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