using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MegaShipDescription:EventBase
    {
        public long IdMegaShipType { get; set; }
        public long MegaShipClassId { get; set; }
        public long AdmiralLevel { get; set; }
        public long ShipTypeId1 { get; set; }
        public long ShipType1Count { get; set; }
        public long ShipTypeId2 { get; set; }
        public long ShipType2Count { get; set; }
        public long ShipTypeId3 { get; set; }
        public long ShipType3Count { get; set; }
        public long MaxShipTypeLevel { get; set; }
        public long MaxShipsCount { get; set; }
    }
}
