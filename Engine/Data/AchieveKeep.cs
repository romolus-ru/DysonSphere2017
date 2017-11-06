using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public class AchieveKeep:EventBase
    {
        public long AchieveId { get; set; }
        public long PlayerId { get; set; }
        public System.DateTime Data { get; set; }
        public bool Recieved { get; set; }
    }
}
