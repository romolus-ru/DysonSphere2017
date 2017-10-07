using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MModuleCalculated:EventBase
    {
        public long MModulePlayer { get; set; }
        public long AddP1 { get; set; }
        public long AddP2 { get; set; }
        public long AddP3 { get; set; }
    }
}
