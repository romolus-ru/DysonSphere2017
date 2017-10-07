using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class SessionMiniGame:EventBase
    {
        public long MiniGameDescriptionId { get; set; }
        public long Version { get; set; }
    }
}
