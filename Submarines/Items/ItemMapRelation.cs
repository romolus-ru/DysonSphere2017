namespace Submarines.Items
{
    /// <summary>
    /// 
    /// </summary>
    internal class ItemMapRelation
    {
        /// <summary>
        /// Имя условия активации
        /// </summary>
        public string ConditionActivateName { get; set; }

        /// <summary>
        /// Условие видимости
        /// </summary>
        public string ConditionViewName { get; set; }

        /// <summary>
        /// Идентификатор карты
        /// </summary>
        public int MapPointId1 { get; set; }

        public int MapSpawnId1 { get; set; }
        /// <summary>
        /// Идентификатор карты
        /// </summary>
        public int MapPointId2 { get; set; }
        public int MapSpawnId2 { get; set; }
        
        /// <summary>
        /// Вес линии для расчёта и измерения кратчайшего пути
        /// </summary>
        public int Weight { get; set; }
    }
}
