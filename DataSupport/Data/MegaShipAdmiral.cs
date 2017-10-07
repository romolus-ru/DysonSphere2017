using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MegaShipAdmiral:EventBase
    {
        public long IdMegaShip { get; set; }
        public long MegaShipTypeId { get; set; }
        public long AdmiralId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
