using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Скроллируемый объект
	/// </summary>
	public class ViewScroll : ViewPanel
	{
		private List<IScrollItem> _items = new List<IScrollItem>();
		public override void AddComponent(ViewComponent component, bool toTop = false)
		{
			if (component is IScrollItem)
				_items.Add(component as IScrollItem);
			else
				base.AddComponent(component, toTop);
		}

		public override void RemoveComponent(ViewComponent component)
		{
			_items.Remove(component as IScrollItem);
			base.RemoveComponent(component);
		}
	}
}
