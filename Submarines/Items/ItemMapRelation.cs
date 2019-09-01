using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Имя точки карты на глобальной карте
        /// </summary>
        public string MapName1 { get; set; }
        /// <summary>
        /// Имя точки карты на глобальной карте
        /// </summary>
        public string MapName2 { get; set; }
        
        /// <summary>
        /// Вес линии для расчёта и измменения кратчайшего пути
        /// </summary>
        public int Weight { get; set; }
    }
}
