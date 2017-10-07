using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class RewardsPackBase:EventBase
    {
        public long IdPackBase { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
