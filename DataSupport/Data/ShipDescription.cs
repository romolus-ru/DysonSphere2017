using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class ShipDescription:EventBase
    {
        public long IdShipType { get; set; }
        public long SlotWeapon { get; set; }
        public long SlotDefance { get; set; }
        public long SlotWarehouse { get; set; }
        public long SlotFree { get; set; }
        public long RequiredPlayerLevel { get; set; }
        public long RequiredSpareLevel { get; set; }
        public string ShipTypeName { get; set; }
        public long AtlasLinkId { get; set; }
    }
}
