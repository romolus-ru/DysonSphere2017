using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MedalKeep:EventBase
    {
        public long IdMedal { get; set; }
        public long MedalDescriptionId { get; set; }
        public System.DateTime Date { get; set; }
        public long PlanetId { get; set; }
        public long MainUserId { get; set; }
        public long UserId { get; set; }
        public string ActualName { get; set; }
    }
}
