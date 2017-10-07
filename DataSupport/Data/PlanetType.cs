using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class PlanetType:EventBase
    {
        public long IdPlanetType { get; set; }
        public long RaceId { get; set; }
        public double OceanPercent { get; set; }
        public double MountainPercent { get; set; }
        public double FaunaAggression { get; set; }
        public double Loyality { get; set; }
    }
}
