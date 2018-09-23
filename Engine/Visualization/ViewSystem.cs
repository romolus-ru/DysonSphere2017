namespace Engine.Visualization
{
	/// <summary>
	/// Вершина визуальной иерархии. выводит сообщения об ошибках, управляет модальностью и т.п.
	/// </summary>
	class ViewSystem : ViewComponent
	{
		public ViewSystem() : base()
		{
		}

		public void SetScreenPos(int newScreenPosX, int newScreenPosY)
		{
			_xScreen = newScreenPosX;
			_yScreen = newScreenPosY;
		}

		public void CursorOverClear()
		{
			CursorOverOff();
		}
	}
}
