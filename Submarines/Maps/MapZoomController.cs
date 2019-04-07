namespace Submarines.Maps
{
	/// <summary>
	/// Управление зумом на карте
	/// </summary>
	internal class MapZoomController
	{
		public float ZoomStep { get; private set; }
		public MapBase Map { get; private set; }
		public float Zoom { get; private set; }

		public MapZoomController(MapBase map, float zoomStep = 0.1f)
		{
			Map = map;
			ZoomStep = zoomStep;
		}

		public void ChangeTo(float newValue, bool immediately)
		{

		}

		/// <summary>
		/// Изменение текущего зума пошагово
		/// </summary>
		public void Change()
		{

		}

	}
}