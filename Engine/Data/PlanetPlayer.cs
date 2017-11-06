using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class PlanetPlayer:EventBase
    {
        public long PlanetId { get; set; }
        public long PlayerId { get; set; }
        public long XP { get; set; }
        public long Level { get; set; }
    }
}
