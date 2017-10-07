using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class BattleShip:EventBase
    {
        public long ShipId { get; set; }
        public long BattleId { get; set; }
    }
}
