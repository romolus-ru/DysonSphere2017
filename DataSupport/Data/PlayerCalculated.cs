using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class PlayerCalculated:EventBase
    {
        public long PlayerId { get; set; }
        public long HP { get; set; }
        public long Attack { get; set; }
        public long Defance { get; set; }
        public long Dex { get; set; }
        public long Lead { get; set; }
        public long Race { get; set; }
    }
}
