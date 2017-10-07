using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class TimerCounter:EventBase
    {
        public long IdTimerCounter { get; set; }
        public long PlayerId { get; set; }
        public long ItemId { get; set; }
        public long OperationType { get; set; }
        public long CountMax { get; set; }
        public long CountCurrent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
