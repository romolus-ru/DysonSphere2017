using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	// TODO возможно уже лишний. всё разнесено или будет разнесено в объекты иерархии ViewObject
	/// <summary>
	/// Основной класс, отвечает за связь с внешним миром и отображение информации
	/// </summary>
	public class View
	{
		protected VisualizationProvider _provider;
		protected List<View> Components = new List<View>();
		public View(VisualizationProvider provider)
		{
			_provider = provider;
		}

		public void AddView(View view)
		{
			Components.Add(view);
		}

		public void Draw(VisualizationProvider provider)
		{
			DrawObject(provider);
			DrawComponents(provider);
		}

		protected virtual void DrawObject(VisualizationProvider provider)
		{

		}

		protected virtual void DrawComponents(VisualizationProvider provider)
		{
			//provider.OffsetAdd(X, Y);
			for (int index = Components.Count - 1; index >= 0; index--) {
				var control = Components[index];
				control.Draw(provider);
			}
			//provider.OffsetRemove();// восстанавливаем смещение	
		}
	}
}