using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class BattleMegaShip:EventBase
    {
        public long MegaShipId { get; set; }
        public long BattleId { get; set; }
    }
}
