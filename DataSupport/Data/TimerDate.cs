using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class TimerDate:EventBase
    {
        public long IdTimerDate { get; set; }
        public long PlayerId { get; set; }
        public System.DateTime BeginData { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
