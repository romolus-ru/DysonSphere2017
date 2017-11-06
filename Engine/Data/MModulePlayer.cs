using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class MModulePlayer:EventBase
    {
        public long IdMModulePlayer { get; set; }
        public long MModuleBaseId { get; set; }
        public long PlayerId { get; set; }
        public long Level { get; set; }
        public long XP { get; set; }
    }
}
