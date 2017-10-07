using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class Blog:EventBase
    {
        public long IdBlogRecord { get; set; }
        public long BlogRecordId { get; set; }
        public long PlayerId { get; set; }
        public System.DateTime Data { get; set; }
        public string Text { get; set; }
    }
}
