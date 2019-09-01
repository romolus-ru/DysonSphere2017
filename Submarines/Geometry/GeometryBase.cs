using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Submarines.Geometry
{
	internal class GeometryBase
	{
		public string Name { get; set; }
		public Color Color { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public GeometryType GeometryType { get;set; }
        /// <summary>
        /// в перспективе перевести на полилинии - не хранить промежуточные точки
        /// </summary>
		public List<LineInfo> Lines { get; set; } = new List<LineInfo>();
	}
}