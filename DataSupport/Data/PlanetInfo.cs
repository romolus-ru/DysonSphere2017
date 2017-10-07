using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class PlanetInfo:EventBase
    {
        public long IdPlanet { get; set; }
        public long PlanetTypeId { get; set; }
        public long StarId { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
    }
}
