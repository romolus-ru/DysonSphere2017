using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class ItemsPlayer:EventBase
    {
        public long ItemId { get; set; }
        public long PlayerId { get; set; }
        public long ModifierId { get; set; }
        public long mod1 { get; set; }
        public long mod2 { get; set; }
        public long count { get; set; }
    }
}
