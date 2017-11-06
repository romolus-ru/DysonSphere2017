using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public class AtlasTexture:EventBase
    {
        public long IdAtlasLink { get; set; }
        public long AtlasFileId { get; set; }
        public long P1X { get; set; }
        public long P1Y { get; set; }
        public long P2X { get; set; }
        public long P2Y { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
