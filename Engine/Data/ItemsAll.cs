using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class ItemsAll:EventBase
    {
        public long IdItem { get; set; }
        public Nullable<long> MiniGameId { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public bool ShowAll { get; set; }
        public long Limit { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long AtlasLinkBigId { get; set; }
        public long AtlasLinkSmallId { get; set; }
    }
}
