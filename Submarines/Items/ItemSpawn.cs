namespace Submarines.Items
{
	internal class ItemSpawn
	{

		public SpawnType SpawnType { get; private set; }
		/// <summary>
		/// Вспомогательный признак для привязки к конкретной карте
		/// </summary>
		public string MapCode { get; private set; }
		public string SpawnName { get; private set; }

		public Vector Position { get; private set; }

		public float Size { get; private set; }
		/// <summary>
		/// Как выглядит значок на карте (или радаре), может быть проще будет сделать все таки текстурами
		/// </summary>
		public string GeometryName { get; private set; }

		/// <summary>
		/// Текстура для вывода на карте
		/// </summary>
		public string TextureMap { get; private set; }

		/// <summary>
		/// Текстура для вывода на радаре
		/// </summary>
		public string TextureRadar { get; private set; }

	}
}