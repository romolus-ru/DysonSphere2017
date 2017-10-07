using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class RecalculateQueue:EventBase
    {
        public long ShipId { get; set; }
        public long MegaShipId { get; set; }
        public long MModuleId { get; set; }
        public long PModuleId { get; set; }
        public long PlayerId { get; set; }
    }
}
