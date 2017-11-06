using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class ShipMegaShip:EventBase
    {
        public long ShipId { get; set; }
        public long AdmiralId { get; set; }
        public Nullable<long> MegaShipId { get; set; }
        public bool AutoConnect { get; set; }
    }
}
