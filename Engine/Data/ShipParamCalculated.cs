using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class ShipParamCalculated:EventBase
    {
        public long ShipId { get; set; }
        public System.DateTime Data { get; set; }
        public long Hull { get; set; }
        public long Attack { get; set; }
        public long Speed { get; set; }
        public long Armor { get; set; }
        public long Mobility { get; set; }
        public long Precision { get; set; }
        public long Protection { get; set; }
        public long Recharge { get; set; }
        public System.DateTimeOffset RecoveryTime { get; set; }
    }
}
