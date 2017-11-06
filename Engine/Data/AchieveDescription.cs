using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public class AchieveDescription:EventBase
    {
        public long IdAchieve { get; set; }
        public string Group { get; set; }
        public long Count { get; set; }
        public string Description { get; set; }
        public string DescriptionReward { get; set; }
        public bool Private { get; set; }
        public string ValuePath { get; set; }
        public long ItemId { get; set; }

    }
}
