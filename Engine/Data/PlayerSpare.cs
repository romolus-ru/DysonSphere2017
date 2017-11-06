using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class PlayerSpare:EventBase
    {
        public long IdPlayerSpare { get; set; }
        public long SpareDescriptionId { get; set; }
        public long PlayerId { get; set; }
        public long Level { get; set; }
        public Nullable<long> ModId1 { get; set; }
        public Nullable<long> Mod1 { get; set; }
        public Nullable<long> ModId2 { get; set; }
        public Nullable<long> Mod2 { get; set; }
        public Nullable<long> ModId3 { get; set; }
        public Nullable<long> Mod3 { get; set; }
        public long XP { get; set; }
    }
}
