using Engine.Visualization.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Visualization
{
	/// <summary>
	/// Вывод на экран системных больших сообщений
	/// Для начала просто текст большим шрифтом
	/// Потом это будет компонент который будет выводить переданный компонент
	/// </summary>
	public class ViewBigMessages:ViewComponent
	{
		private Dictionary<DateTime, List<ViewComponent>> _messages = new Dictionary<DateTime, List<ViewComponent>>();

		public void ShowMessage(TimeSpan timeOnScreen, string font, string message)
		{
			var dt = DateTime.Now + timeOnScreen;
			var t = new ViewText();
			AddComponent(t);
			t.SetParams(0, 0, Width, 30, "Header");
			t.CreateSplitedTextAuto(System.Drawing.Color.White, font, message);
			t.CalculateTextPositions();
			if (!_messages.ContainsKey(dt))
				_messages.Add(dt, new List<ViewComponent>());
			_messages[dt].Add(t);
		}

		protected override void DrawComponents(VisualizationProvider visualizationProvider) { }

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var dt = DateTime.Now;
			foreach (var key in _messages.Keys.ToList()) {
				if (key > dt) continue;
				foreach (var msg in _messages[key]) {
					RemoveComponent(msg);
				}
				_messages[key].Clear();
				_messages.Remove(key);
			}

			int counter = 0;
			foreach (var msgs in _messages) {
				foreach (var txt in msgs.Value) {
					visualizationProvider.OffsetAdd(0, 30 * counter);
					txt.DrawObject(visualizationProvider);
					visualizationProvider.OffsetRemove();
					counter++;
				}
			}

		}
	}
}