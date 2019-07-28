using System;
using System.Collections.Generic;
using System.Drawing;
using Submarines.Geometry;

namespace Submarines.Items
{
	/// <summary>
	/// Группа точек где создаются какие-либо объекты на карте
	/// </summary>
	/// <remarks>Пока не нужно - точка спауна будет определять что она из себя представляет - точка, тип точки, радиус или геометрия</remarks>
	[Obsolete]
	internal class ItemSpawnGroup : ItemBase
	{
		public SpawnType SpawnType { get; private set; }
		public string GroupName { get; private set; }
		/// <summary>
		/// Вспомогательный признак для привязки к конкретной карте
		/// </summary>
		public string MapCode { get; private set; }
		public Color Color { get; private set; }
		/// <summary>
		/// Как выглядит значок на карте (или радаре), может быть проще будет сделать все таки текстурами
		/// </summary>
		public GeometryBase Geometry { get; private set; }

		/// <summary>
		/// Текстура для вывода на карте
		/// </summary>
		public string TextureMap { get; private set; }

		/// <summary>
		/// Текстура для вывода на радаре
		/// </summary>
		public string TextureRadar { get; private set; }

		/// <summary>
		/// Точки на которых могут появиться объекты
		/// </summary>
		public List<Vector> Points { get; private set; }
	}
}