using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class RewardsCounter:EventBase
    {
        public long UnitId { get; set; }
        public long PlayerId { get; set; }
        public long AttemptsCount { get; set; }
    }
}
