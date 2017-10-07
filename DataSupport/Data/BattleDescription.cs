using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public class BattleDescription:EventBase
    {
        public long IdBattleType { get; set; }
        public long AdmiralLevel { get; set; }
        public long AdmiralBattles { get; set; }
        public long AdmiralWins { get; set; }
        public double AdmiralAvg { get; set; }
        public long MegaShipsCount { get; set; }
        public long ShipsCount { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long MinCountShips { get; set; }
    }
}
