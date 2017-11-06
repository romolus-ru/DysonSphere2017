using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class RewardsUnit:EventBase
    {
        public long IdUnit { get; set; }
        public long ItemId { get; set; }
        public string Type { get; set; }
        public long Variation { get; set; }
        public double Chance { get; set; }
        public long Capacity { get; set; }
        public long MinCapacity { get; set; }
        public string Blocking { get; set; }
    }
}
