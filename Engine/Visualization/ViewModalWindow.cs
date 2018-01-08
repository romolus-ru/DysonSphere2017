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
		private Input _input;
		private ViewInput _inputComponent;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			_input = input;
			input.AddCursorAction(this.CursorHandler, true);
			input.AddInputStringAction(InputStringAction);
			input.AddKeyActionPaused(CursorLeft, Keys.Left);
		}

		private void InputStringAction(string str)
		{
			_inputComponent?.InputAction(str);
		}

		public void InitInputDialog(ViewInput inputComponent)
		{
			_inputComponent = inputComponent;
		}

		private void CursorLeft()
		{
			_inputComponent?.InputAction("<");
		}
	}
}
