namespace Submarines.Geometry
{
	/// <summary>
	/// Хранит 2 точки
	/// </summary>
	struct LineInfo
	{
		public Vector From;
		public Vector To;

		public LineInfo(Vector from, Vector to)
		{
			From = from;
			To = to;
		}
	}
}