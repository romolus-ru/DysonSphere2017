using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class Player:EventBase
    {
        public long IdPlayer { get; set; }
        public long HP { get; set; }
        public long Attack { get; set; }
        public long Defance { get; set; }
        public long Dex { get; set; }
        public long Lead { get; set; }
        public long Race { get; set; }
        public long RaceBase { get; set; }
        public long ReincarnationCount { get; set; }
        public long PlanetsChanged { get; set; }
        public string Specialization { get; set; }
    }
}
