using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class RewardsPackCondition:EventBase
    {
        public long IdCondition { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NeedTypeValue { get; set; }
        public Nullable<double> NeedCount { get; set; }
    }
}
