using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class MModuleBase:EventBase
    {
        public long IdMModuleBase { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long AddP1 { get; set; }
        public long AddP2 { get; set; }
        public long AddP3 { get; set; }
    }
}
