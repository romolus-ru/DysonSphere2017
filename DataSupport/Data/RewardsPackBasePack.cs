using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class RewardsPackBasePack:EventBase
    {
        public long PackId { get; set; }
        public long PackBaseId { get; set; }
        public long ConditionId { get; set; }
    }
}
