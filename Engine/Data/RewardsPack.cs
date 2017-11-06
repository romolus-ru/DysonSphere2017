using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class RewardsPack:EventBase
    {
        public long IdRewardPack { get; set; }
        public string PackName { get; set; }
        public string PackType { get; set; }
        public string PackDescription { get; set; }
    }
}
