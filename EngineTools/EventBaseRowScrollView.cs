using Engine.EventSystem.Event;
using Engine.Visualization.Scroll;

namespace EngineTools
{
	/// <summary>
	/// Просмотр информации о наследнике EventBase в строку
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EventBaseRowScrollView<T>:ScrollItem where T:EventBase
	{
	}
}