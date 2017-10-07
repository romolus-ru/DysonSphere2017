using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class BattleInfoRace:EventBase
    {
        public long BattleId { get; set; }
        public long RaceId { get; set; }
        public long TotalShipsCount { get; set; }
        public long TotalMegaShipsCount { get; set; }
        public long PlayersGrantedShips { get; set; }
    }
}
