using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class Chat:EventBase
    {
        public System.DateTime Data { get; set; }
        public long PlayerCode { get; set; }
        public string Text { get; set; }
        public long MiniGameId { get; set; }
        public long AreaId { get; set; }
    }
}
