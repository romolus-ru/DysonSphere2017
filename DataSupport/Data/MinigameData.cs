using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MinigameData:EventBase
    {
        public long PlayerId { get; set; }
        public long MiniGameId { get; set; }
        public System.DateTime Data { get; set; }
        public string RowType { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
