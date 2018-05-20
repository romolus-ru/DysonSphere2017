using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class MiniGameSettings_old:EventBase
    {
        public long MiniGameId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Hint { get; set; }
    }
}
