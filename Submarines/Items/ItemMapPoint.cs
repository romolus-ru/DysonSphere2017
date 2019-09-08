namespace Submarines.Items
{
    /// <summary>
    /// Точка на глобальной карте
    /// </summary>
    internal class ItemMapPoint
    {
        public int PointId { get; set; }
        public string PointName { get; set; }
        public Vector Point { get; set; }
        public string MapCode { get; set; }
    }
}