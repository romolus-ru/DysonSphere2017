using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class PlanetCalculated:EventBase
    {
        public long PlanetId { get; set; }
    }
}
