using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class MiniGamePermission_old:EventBase
    {
        public long PlayerId { get; set; }
        public long MiniGameId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
