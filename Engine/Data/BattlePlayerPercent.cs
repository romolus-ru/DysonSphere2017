using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class BattlePlayerPercent:EventBase
    {
        public long BattleId { get; set; }
        public long PlayerId { get; set; }
        public double PercentV { get; set; }
    }
}
