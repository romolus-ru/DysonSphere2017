using System.Collections.Generic;
using Submarines.Geometry;
using Submarines.Utils;

namespace Submarines.Items
{
	/// <summary>
	/// Корпус подлодки
	/// </summary>
	internal class ItemHull : ItemBase
	{
		public GeometryBase Geometry { get; private set; }

		internal override void Init(Dictionary<string, string> values)
		{
			base.Init(values);
			Geometry = ItemsManager.GetGeometry(values.GetString("GeometryName"));
		}
	}
}