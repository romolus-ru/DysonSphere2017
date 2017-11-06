using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public class Battle:EventBase
    {
        public long IdBattle { get; set; }
        public long StarId { get; set; }
        public long BattleTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
