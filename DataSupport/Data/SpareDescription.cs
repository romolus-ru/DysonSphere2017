using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class SpareDescription:EventBase
    {
        public long IdSpareDescription { get; set; }
        public long SpareType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Hull { get; set; }
        public long Attack { get; set; }
        public long Speed { get; set; }
        public long Armor { get; set; }
        public long Mobility { get; set; }
        public long Precision { get; set; }
        public long Protection { get; set; }
        public long Recharge { get; set; }
        public long AtlasLinkId { get; set; }
    }
}
