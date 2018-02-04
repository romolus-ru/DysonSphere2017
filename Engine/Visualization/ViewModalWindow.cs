using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Visualization
{
	/// <summary>
	/// Модальное окно
	/// </summary>
	public class ViewModalWindow : ViewWindow
	{
		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.AddCursorAction(CursorHandler, true);
		}

		protected override void ClearObject()
		{
			Input.RemoveCursorAction(CursorHandler);
			base.ClearObject();
		}
	}
}
