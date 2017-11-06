using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class StarInfo:EventBase
    {
        public long IdStar { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
    }
}
