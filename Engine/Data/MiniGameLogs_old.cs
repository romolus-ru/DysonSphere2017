using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class MiniGameLogs_old:EventBase
    {
        public long MiniGameId { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public string IdsWinners { get; set; }
        public string IdsOthers { get; set; }
        public string Result { get; set; }
    }
}
