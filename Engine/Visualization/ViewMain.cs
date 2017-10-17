using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Основной класс - управляет всеми остальными View
	/// </summary>
	public class ViewMain : View
	{
		public ViewMain(VisualizationProvider provider) : base(provider)
		{
		}
	}
}
