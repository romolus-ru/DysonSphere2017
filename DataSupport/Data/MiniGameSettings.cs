using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MiniGameSettings:EventBase
    {
        public long MiniGameId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Hint { get; set; }
    }
}
