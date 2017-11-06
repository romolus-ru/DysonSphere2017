using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class SpareActivityLog:EventBase
    {
        public long IdSpareActivity { get; set; }
        public long SpareId { get; set; }
        public long PlayerId { get; set; }
        public string Log { get; set; }
        public string Info { get; set; }
    }
}
