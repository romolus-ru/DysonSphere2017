using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class ShipPlayer:EventBase
    {
        public long IdShip { get; set; }
        public long PlayerId { get; set; }
        public long IdShipType { get; set; }
        public System.DateTime Data { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
